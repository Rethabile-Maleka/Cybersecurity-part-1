using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;

internal static class CyberSecurityBotHelpers2
{
    // Tries Azure TTS when AZURE_SPEECH_KEY and AZURE_SPEECH_REGION are set.
    // Otherwise falls back to playing a local WAV file named "greeting.wav".
    public static async Task PlayVoiceGreetingAsync()
    {
        const string wavPath = "greeting.wav";
        const string greetingText = "Hello, welcome to the Cybersecurity Awareness Bot.";

        string key = Environment.GetEnvironmentVariable("AZURE_SPEECH_KEY");
        string region = Environment.GetEnvironmentVariable("AZURE_SPEECH_REGION");

        if (!string.IsNullOrWhiteSpace(key) && !string.IsNullOrWhiteSpace(region))
        {
            try
            {
                var config = SpeechConfig.FromSubscription(key, region);
                using var synthesizer = new SpeechSynthesizer(config);
                await synthesizer.SpeakTextAsync(greetingText);
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Azure TTS failed: {ex.Message}. Falling back to WAV if available.");
            }
        }

        // Fallback: play local WAV
        if (File.Exists(wavPath))
        {
            try
            {
                if (OperatingSystem.IsWindows())
                {
                    Process.Start(new ProcessStartInfo("cmd", $"/c start /min wmplayer \"{wavPath}\"") { CreateNoWindow = true });
                }
                else if (OperatingSystem.IsLinux())
                {
                    Process.Start("aplay", $"\"{wavPath}\"");
                }
                else if (OperatingSystem.IsMacOS())
                {
                    Process.Start("afplay", $"\"{wavPath}\"");
                }
                else
                {
                    Console.WriteLine("Voice greeting not supported on this OS.");
                }
            }
            catch
            {
                Console.WriteLine("Voice greeting could not be played.");
            }
        }
        else
        {
            Console.WriteLine("Voice greeting file not found and Azure keys not configured.");
        }
    }
}