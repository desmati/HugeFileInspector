using System.CommandLine;
using System.Text.RegularExpressions;

namespace HugeFileInspector
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            var rootCommand = new RootCommand("HugeFileInspector - Search and replace text patterns in large files.");

            var inputOption = new Option<FileInfo>(
                aliases: ["--input", "-i"],
                description: "The input file to process.")
            { IsRequired = true };

            var outputOption = new Option<string>(
                aliases: ["--output", "-o"],
                description: "The output file to log results instead of the console. \r\n In replace mode, it is mandatory.");

            var patternOption = new Option<string>(
                aliases: ["--find", "-f"],
                description: "The text pattern or regular expression to search for.")
            { IsRequired = true };

            var replaceOption = new Option<string>(
                aliases: ["--replace", "-r"],
                description: "The replacement text (optional).");

            var distinctOption = new Option<bool>(
                aliases: ["--distinct", "-d"],
                description: "Only logs distinct texts found in the file.");

            rootCommand.AddOption(inputOption);
            rootCommand.AddOption(outputOption);
            rootCommand.AddOption(patternOption);
            rootCommand.AddOption(replaceOption);
            rootCommand.AddOption(distinctOption);

            rootCommand.SetHandler(ProcessFileAsync, inputOption, outputOption, patternOption, replaceOption, distinctOption);

            return await rootCommand.InvokeAsync(args);
        }

        static async Task ProcessFileAsync(FileInfo inputFile, string? outputFile, string pattern, string replaceText, bool distinctMode)
        {
            if (!inputFile.Exists)
            {
                Console.WriteLine($"Error: The file '{inputFile.FullName}' does not exist.");
                return;
            }

            bool outputFileProvided = !string.IsNullOrEmpty(outputFile);
            bool isReplacing = !string.IsNullOrEmpty(replaceText);
            if (isReplacing && !outputFileProvided)
            {
                Console.WriteLine($"Error: The output file path is not provided. \r\nIt is optional in find mode but mandatory in replace mode");
                return;
            }

            if (isReplacing && distinctMode)
            {
                Console.WriteLine($"Error: --replace and --distinct options can not be used at the same time");
                return;
            }

            long totalMatches = 0;
            long totalDistinctMatches = 0;
            var distinctMatches = new List<string>();
            try
            {
                using var reader = new StreamReader(inputFile.FullName);
                StreamWriter? writer = null;

                if (outputFileProvided)
                {
                    writer = new StreamWriter(outputFile!);
                }

                string? line;
                int lineNumber = 0;
                var regex = new Regex(pattern, RegexOptions.Compiled);

                while ((line = await reader.ReadLineAsync()) != null)
                {
                    lineNumber++;
                    var matches = regex.Matches(line);
                    var matchFound = matches.Count > 0;
                    var distinctModeAndNewMatchFound = distinctMode && !distinctMatches.Contains(line);

                    if (matchFound)
                    {
                        totalMatches += matches.Count;

                        if (!distinctMode || distinctModeAndNewMatchFound)
                        {
                            Console.WriteLine($"Line {lineNumber}:\t {line}");

                            if (distinctModeAndNewMatchFound)
                            {
                                distinctMatches.Add(line);
                                totalDistinctMatches += matches.Count;
                            }
                        }
                    }

                    if (matchFound && !isReplacing && outputFileProvided && (!distinctMode || distinctModeAndNewMatchFound))
                    {
                        await writer!.WriteLineAsync($"Line {lineNumber}:\t {line}");
                    }

                    if (isReplacing)
                    {
                        if (matchFound)
                        {
                            line = regex.Replace(line, replaceText);
                        }

                        await writer!.WriteLineAsync(line);
                    }
                }

                Console.WriteLine($"\nTotal matches: {totalMatches}");

                if (outputFileProvided)
                {
                    await writer!.FlushAsync();
                    writer.Dispose();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing the file: {ex.Message}");
            }
        }
    }
}
