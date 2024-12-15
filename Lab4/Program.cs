using McMaster.Extensions.CommandLineUtils;
using ClassLibraryLab4;

namespace Lab4
{
    [Command(Name = "Lab4", Description = "Utility for executing tasks from labs 1-3")]
    [Subcommand(typeof(InfoCommand), typeof(ExecuteCommand), typeof(ConfigurePathCommand))]
    class Application
    {
        public static int Main(string[] args)
        {
            var app = new CommandLineApplication<Application>();
            app.Conventions.UseDefaultConventions();

            try
            {
                return app.Execute(args);
            }
            catch (CommandParsingException)
            {
                return 0;
            }
        }

        private void OnExecute(CommandLineApplication app)
        {
            ShowAvailableCommands();
        }

        private static void ShowAvailableCommands()
        {
            Console.WriteLine("Available commands:");
            Console.WriteLine("  version     - Shows application details and author info");
            Console.WriteLine("  run         - Executes the specified lab solution");
            Console.WriteLine("                Examples:");
            Console.WriteLine("                Lab4 run lab1 -i input.txt -o output.txt");
            Console.WriteLine("  set-path    - Configures the default directory for input/output files");
            Console.WriteLine("                Example:");
            Console.WriteLine("                Lab4 set-path -p /path/to/folder");
            Console.WriteLine("  help        - Displays this help information");
        }

        [Command("version", Description = "Shows application details and author info")]
        class InfoCommand
        {
            private void OnExecute()
            {
                Console.WriteLine("Developer: Dmytro Mazur");
                Console.WriteLine("Software Version: 1.0.0");
            }
        }

        [Command("run", Description = "Executes the specified lab solution")]
        class ExecuteCommand
        {
            [Argument(0, "lab", "Specifies the lab to run (lab1, lab2, lab3)")]
            public string LabIdentifier { get; set; }

            [Option("-i|--input", "Path to the input file", CommandOptionType.SingleValue)]
            public string InputFilePath { get; set; }

            [Option("-o|--output", "Path to the output file", CommandOptionType.SingleValue)]
            public string OutputFilePath { get; set; }

            private void OnExecute()
            {
                Console.WriteLine(Environment.GetEnvironmentVariable("DEFAULT_PATH"));
                string resolvedInputPath = InputFilePath ?? Environment.GetEnvironmentVariable("DEFAULT_PATH") ?? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));
                string resolvedOutputPath = OutputFilePath ?? Environment.GetEnvironmentVariable("DEFAULT_PATH") ?? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));

                resolvedInputPath = Path.Combine(resolvedInputPath, "INPUT.txt");
                resolvedOutputPath = Path.Combine(resolvedOutputPath, "OUTPUT.txt");

                if (!File.Exists(resolvedInputPath))
                {
                    Console.WriteLine($"Error: File {resolvedInputPath} could not be found.");
                }

                switch (LabIdentifier?.ToLower())
                {
                    case "lab1":
                        Lab1.ExecuteLab1(resolvedInputPath, resolvedOutputPath);
                        break;
                    case "lab2":
                        Lab2.ExecuteLab2(resolvedInputPath, resolvedOutputPath);
                        break;
                    case "lab3":
                        Lab3.ExecuteLab3(resolvedInputPath, resolvedOutputPath);
                        break;
                    default:
                        Console.WriteLine("Invalid lab specified. Please use lab1, lab2, or lab3.");
                        break;
                }

                Console.WriteLine($"Task completed successfully. Results are saved in {resolvedOutputPath}");
            }
        }

        [Command("set-path", Description = "Configures the default directory for input/output files")]
        class ConfigurePathCommand
        {
            [Option("-p|--path", "Specifies the directory to use for input/output files", CommandOptionType.SingleValue)]
            public string DirectoryPath { get; set; }

            private void OnExecute()
            {
                if (string.IsNullOrWhiteSpace(DirectoryPath))
                {
                    Console.WriteLine("Error: Directory path is empty.");
                    return;
                }

                try
                {
                    UpdateEnvironmentVariable("DEFAULT_PATH", DirectoryPath);
                    Console.WriteLine($"Default path updated to: {DirectoryPath}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred while updating the default path: {ex.Message}");
                }
            }

            private void UpdateEnvironmentVariable(string variableName, string value)
            {
                if (OperatingSystem.IsLinux() || OperatingSystem.IsMacOS())
                {
                    string envConfigFile = OperatingSystem.IsLinux() ? "/etc/environment" : "/etc/paths";

                    if (File.Exists(envConfigFile))
                    {
                        using (StreamWriter writer = File.AppendText(envConfigFile))
                        {
                            writer.WriteLine($"{variableName}={value}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("System configuration file not found.");
                        throw new InvalidOperationException("Failed to set environment variable.");
                    }
                }
                else if (OperatingSystem.IsWindows())
                {
                    Environment.SetEnvironmentVariable(variableName, value, EnvironmentVariableTarget.Machine);
                }
            }
        }
    }
}
