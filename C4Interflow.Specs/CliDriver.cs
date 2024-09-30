using System.Collections.Concurrent;
using System.CommandLine.Builder;
using System.CommandLine.Parsing;
using C4InterFlow.Cli.Commands;
using C4InterFlow.Cli.Extensions;
using C4InterFlow.Cli.Root;

namespace C4Interflow.Specs
{
    public class CliDriverManager
    {
        private readonly ConcurrentDictionary<string, CliDriver> drivers = [];

        public CliDriver ForContext(string context)
        {
            Console.WriteLine($"For context '{context}'");
            drivers.AddOrUpdate(context, new CliDriver(), (c, d) => d);
            return drivers[context];
        }
    }
    public class CliDriver
    {
        public CliDriver()
        {
            cliArgs = [];
        }

        private readonly List<string> cliArgs;

        public string SampleRootName { get; set; }
        public string WorkingDirectory { get; set; }

        public async Task<int> BuildAndInvoke()
        {
            var args = cliArgs.ToArray();
            Console.WriteLine(string.Join(" ", args));
            var rootCommandBuilder = RootCommandBuilder
                .CreateDefaultBuilder(args)
                .Configure(context =>
                {
                    context.Add<DrawDiagramsCommand>();
                    context.Add<QueryUseFlowsCommand>();
                    context.Add<QueryByInputCommand>();
                    context.Add<ExecuteAaCStrategyCommand>();
                    context.Add<GenerateDocumentationCommand>();
                    context.Add<PublishSiteCommand>();
                });

            return await new CommandLineBuilder(rootCommandBuilder.Build())
                .UseDefaults().UseLogging().Build().InvokeAsync(args);
        }

        public void WithAaCInputPath(string path)
        {
            cliArgs.Add("--aac-input-paths");
            cliArgs.Add($"{WorkingDirectory}\\{SampleRootName}\\{path}");
        }

        public void WithAaCReaderStrategy(Type readerType)
        {
            cliArgs.Add("--aac-reader-strategy");
            cliArgs.Add(readerType.AssemblyQualifiedName);
        }

        public void WithInterfaces(string interfaceQuery)
        {
            cliArgs.Add("--interfaces");
            cliArgs.AddRange(interfaceQuery.Split(' '));
        }

        public void WithBusinessProcesses(string businessQuery)
        {
            cliArgs.Add("--business-processes");
            cliArgs.AddRange(businessQuery.Split(' '));
        }

        public void WithLevelOfDetails(string levelOfDetails)
        {
            cliArgs.Add("--levels-of-details");
            cliArgs.AddRange(levelOfDetails.Split(' '));
        }

        public void OutputTo(string outputPath)
        {
            cliArgs.Add("--output-dir");
            cliArgs.Add(outputPath);
        }

        public void ForCommand(string rootCommand)
        {
            cliArgs.Clear();
            cliArgs.Add(rootCommand);
        }
    }
}