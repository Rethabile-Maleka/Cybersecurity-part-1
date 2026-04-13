using System;
using System.IO;
using System.Media;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;

class CyberSecurityBot
{
    public CyberSecurityBot()
    {
    }

    static void Main()
    {
        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "greeting.wav");
#if WINDOWS
        var player = new SoundPlayer(path);
        player.Play();
        Console.WriteLine("Playing greeting.wav...");
#else
        Console.WriteLine("Audio playback is only supported on Windows. Skipping greeting.wav...");
#endif
        Console.ReadLine(); 
        Console.Title = "Cybersecurity Awareness Bot";

        // Play voice greeting (tries Azure TTS when keys are set, otherwise plays `greeting.wav`)
        CyberSecurityBotHelpers.PlayVoiceGreeting();

        // Display ASCII Art Logo
        ShowLogo();

        
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write("\nEnter your name: ");
        string name = Console.ReadLine();

        while (string.IsNullOrWhiteSpace(name))
        {
            Console.Write("Name cannot be empty. Please enter your name: ");
            name = Console.ReadLine();
        }

        Console.Clear();
        ShowLogo();

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"\nHello, {name}! Welcome to the Cybersecurity Awareness Bot.\n");

        
        ChatLoop(name);
    }

    static void ShowLogo()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;

        Console.WriteLine("=======================================");
        Console.WriteLine("   CYBERSECURITY AWARENESS BOT");
        Console.WriteLine("=======================================\n");

        Console.WriteLine("        [***]");
        Console.WriteLine("       ( o o )");
        Console.WriteLine("  ----oOO-(_)-OOo----");
        Console.WriteLine("      Stay Safe Online!");
        Console.WriteLine("=======================================\n");

        Console.ResetColor();
    }

    static void ChatLoop(string name)
    {
        while (true)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"{name}: ");
            string input = Console.ReadLine()?.ToLower() ?? string.Empty;

            // Input validation
            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Bot: I didn’t quite understand that. Could you rephrase?");
                continue;
            }

            Console.ForegroundColor = ConsoleColor.Green;

            
            if (input == "exit")
            {
                Console.WriteLine("Bot: Goodbye! Stay safe online 👋");
                break;
            }

            // Basic responses
            else if (input.Contains("how are you"))
            {
                Console.WriteLine("Bot: I'm just code, but I'm here to help you stay safe online!");
            }
            else if (input.Contains("purpose"))
            {
                Console.WriteLine("Bot: My purpose is to educate you about cybersecurity and help you stay safe online.");
            }
            else if (input.Contains("what can i ask"))
            {
                Console.WriteLine("Bot: You can ask me about passwords, phishing, and safe browsing.");
            }

            // Cybersecurity topics
            else if (input.Contains("password"))
            {
                Console.WriteLine("Bot: Use strong passwords with letters, numbers, and symbols. Never share your password!");
            }
            else if (input.Contains("phishing"))
            {
                Console.WriteLine("Bot: Phishing is when attackers trick you into giving personal info. Always check email sources!");
            }
            else if (input.Contains("safe browsing"))
            {
                Console.WriteLine("Bot: Only visit secure websites (https) and avoid clicking suspicious links.");
            }

            // Default response
            else
            {
                Console.WriteLine("Bot: I didn’t quite understand that. Could you rephrase?");
            }

            // Typing effect
            Thread.Sleep(500);
        }
    }
}

internal static class CyberSecurityBotHelpers
{
    public static void PlayVoiceGreeting()
    {
        try
        {
            string subscriptionKey = "YOUR_SUBSCRIPTION_KEY";
            string region = "YOUR_REGION"; // e.g., "eastus"

            var config = SpeechConfig.FromSubscription(subscriptionKey, region);
            using (var synthesizer = new SpeechSynthesizer(config))
            {
                string greetingText = "Hello, welcome to the Cybersecurity Awareness Bot.";
                // Use the async method and wait for completion
                synthesizer.SpeakTextAsync(greetingText).GetAwaiter().GetResult();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error playing voice greeting: {ex.Message}");
        }
    }
}