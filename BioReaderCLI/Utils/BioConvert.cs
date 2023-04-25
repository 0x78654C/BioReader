using BioLib;

namespace BioReaderCLI.Utils
{
    public class BioConvert
    {
        private string[] Args { get; set; }
        UI ui = new UI();

        /// <summary>
        /// Raad text from arguments path.
        /// </summary>
        /// <param name="args"></param>
        public BioConvert(string[] args)
        {
            Args = args;
        }

        /// <summary>
        /// Read text from path.
        /// </summary>
        /// <returns></returns>
        public void BioConverter()
        {
            try
            {
                Console.WriteLine("\n\n\n--------------\n");
                string firstArg = Args[0];
                if (firstArg == "-f")
                {
                    foreach (var arg in Args)
                    {
                        if (arg == "-f")
                            continue;

                        if (!File.Exists(arg))
                        {
                            ui.ColorConsoleTextLineError($"File does not exist: {arg}");
                            continue;
                        }

                        if (!string.IsNullOrEmpty(arg))
                        {
                            var data = File.ReadAllText(arg);
                            var line = string.Empty;
                            using (var reader = new StringReader(data))
                                while ((line = reader.ReadLine()) != null)
                                    ConvertToBioReader(line, true);
                            break;
                        }

                    }
                    Console.WriteLine("\n\n\n--------------\n");
                    return;
                }

                foreach (var arg in Args)
                    ConvertToBioReader(arg, false);
                Console.WriteLine("\n\n\n--------------\n");
            }
            catch (Exception ex)
            {
                ui.ColorConsoleTextLineError($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Convert text to bionic reader text.
        /// </summary>
        /// <param name="data"></param>
        private void ConvertToBioReader(string data, bool isNewLine)
        {
            if (!string.IsNullOrEmpty(data))
            {
                var halfWords = Reader.GetHalfChars(data);
                foreach (var bioWrod in halfWords)
                {
                    var secondHalf = bioWrod.Key.Substring(bioWrod.Value.Length);
                    ui.ColorConsoleText(ConsoleColor.DarkGreen, bioWrod.Value);
                    ui.ColorConsoleText(ConsoleColor.White, $"{secondHalf} ");
                    if (isNewLine) Console.WriteLine();
                }
            }
        }
    }
}
