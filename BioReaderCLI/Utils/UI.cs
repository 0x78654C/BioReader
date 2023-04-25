using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BioReaderCLI.Utils
{
    public class UI
    {
        /// <summary>
        /// Change color of a specific text in console.
        /// </summary>
        /// <param name="color"></param>
        /// <param name="text"></param>
        public void ColorConsoleText(ConsoleColor color, string text)
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
        public void ColorConsoleTextLineError(string errorMessage)
        {
            ConsoleColor currentForeground = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(errorMessage);
            Console.ForegroundColor = currentForeground;
        }
    }
}
