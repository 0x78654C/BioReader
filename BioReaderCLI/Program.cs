using BioLib;

// Run if any arguments;
if (args.Any())
    ConvertToBioReader(args);
else
    ColorConsoleTextLineError("Need to provite a text for bionic reader conversion!");


// Get paramas from args as a string.
void ConvertToBioReader(string[] args)
{
    Console.WriteLine("\n\n\n--------------\n");
    foreach (var arg in args)
        if (!string.IsNullOrEmpty(arg))
        {
            var halfWords = Reader.GetHalfChars(arg);
            foreach (var bioWrod in halfWords)
            {
                var secondHalf = bioWrod.Key.Substring(bioWrod.Value.Length);
                ColorConsoleText(ConsoleColor.DarkGreen, bioWrod.Value);
                ColorConsoleText(ConsoleColor.White, $"{secondHalf} ");
            }
        }
    Console.WriteLine("\n\n--------------\nPress any key to exit...");
    Console.ReadLine();
}



/// <summary>
/// Change color of a specific text in console.
/// </summary>
/// <param name="color"></param>
/// <param name="text"></param>
void ColorConsoleText(ConsoleColor color, string text)
{
    ConsoleColor currentForeground = Console.ForegroundColor;
    Console.ForegroundColor = color;
    Console.Write(text);
    Console.ForegroundColor = currentForeground;
}

/// <summary>
/// Change color of a specific text in console.
/// </summary>
/// <param name="color"></param>
/// <param name="text"></param>
void ColorConsoleTextLineError(string errorMessage)
{
    ConsoleColor currentForeground = Console.ForegroundColor;
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine(errorMessage);
    Console.ForegroundColor = currentForeground;
}
