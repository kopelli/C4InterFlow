using System.Collections;
using System.Reflection;
using C4InterFlow.Automation.Readers;
using TechTalk.SpecFlow;

[assembly: Microsoft.VisualStudio.TestTools.UnitTesting.Parallelize(Scope = ExecutionScope.MethodLevel)]

namespace C4Interflow.Specs
{
    [Binding]
    public sealed class EnvironmentStepDefinitions
    {
        private readonly ScenarioContext _scenarioContext;
        private CliDriverManager _drivers;
        private string _tempPath;
        private int _cliExitCode;

        public EnvironmentStepDefinitions(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            _drivers = new();
        }

        [BeforeScenario]
        public void UpdateScenarioRun(ITestRunnerManager testRunnerManager)
        {
            _tempPath = Path.Combine(Path.GetTempPath(), Assembly.GetExecutingAssembly().GetName().Name, Guid.NewGuid().ToString("N"));
            Environment.CurrentDirectory = Path.GetFullPath("../../../../../Samples", Assembly.GetExecutingAssembly().Location);
            Console.WriteLine("Before scenario...");
        }

        [AfterScenario]
        public void AfterScenario()
        {
            Assert.AreEqual(0, _cliExitCode, "The command exited with a non-zero value.");
        }

        [Given(@"the '(.+)' command")]
        public void ForCommand(string command)
        {
            _drivers.ForContext(_scenarioContext.Description()).ForCommand(command);
        }

        [Given(@"the '(.+)' example")]
        public void GivenTheExample(string example)
        {
            _drivers.ForContext(_scenarioContext.Description()).WorkingDirectory =
                Path.GetFullPath("../../../../../Samples", Assembly.GetExecutingAssembly().Location);
            _drivers.ForContext(_scenarioContext.Description()).SampleRootName = example;
        }

        [Given(@"the path '(.+)'")]
        public void GivenThePath(string path)
        {
            _drivers.ForContext(_scenarioContext.Description()).WithAaCInputPath(path);
        }

        [Given(@"the reader strategy is '(.+)'")]
        public void GivenTheReaderStrategy(string strategy)
        {
            switch (strategy)
            {
                case "Yaml":
                    _drivers.ForContext(_scenarioContext.Description()).WithAaCReaderStrategy(typeof(YamlAaCReaderStrategy));
                    break;
                case "Json":
                    _drivers.ForContext(_scenarioContext.Description()).WithAaCReaderStrategy(typeof(JsonAaCReaderStrategy));
                    break;
                default:
                    Assert.Fail($"Unknown strategy: '{strategy}'");
                    break;
            }
        }

        [Given(@"the interfaces are '(.*)'")]
        public void GivenTheInterfacesAre(string interfaceQuery)
        {
            _drivers.ForContext(_scenarioContext.Description()).WithInterfaces(interfaceQuery);
        }

        [Given(@"the business processes are '(.*)'")]
        public void GivenTheBusinessProcessesAre(string businessQuery)
        {
            _drivers.ForContext(_scenarioContext.Description()).WithBusinessProcesses(businessQuery);
        }


        [Given(@"the level of details is '(.*)'")]
        public void GivenTheLevelOfDetailsIs(string levelOfDetails)
        {
            _drivers.ForContext(_scenarioContext.Description()).WithLevelOfDetails(levelOfDetails);
        }

        [Given(@"send the output to '(.*)'")]
        public void GivenSendTheOutputTo(string outputPath)
        {
            if (!Path.IsPathRooted(outputPath))
            {
                outputPath = Path.Combine(_tempPath, outputPath);
            }

            if (!Directory.Exists(outputPath))
            {
                Directory.CreateDirectory(outputPath);
            }
            _drivers.ForContext(_scenarioContext.Description()).OutputTo(outputPath);
        }

        [When(@"invoking the commandline for those arguments")]
        public void WhenInvokingTheCommandlineForThoseArguments()
        {
            _cliExitCode = _drivers.ForContext(_scenarioContext.Description()).BuildAndInvoke().Result;
        }

        [Then(@"all files under '(.*)' should match example path '(.*)'")]
        public void ThenAllFilesUnderShouldMatchExamplePath(string actualPath, string expectedPath)
        {
            if (!Path.IsPathRooted(actualPath))
            {
                actualPath = Path.Combine(_tempPath, actualPath);
            }

            expectedPath = Path.Combine(Directory.GetCurrentDirectory(), expectedPath);

            var getFileList = (string path) => Directory.GetFiles(path, "*", SearchOption.AllDirectories)
                .Select(p => new { FullPath = p, RelativePath = Path.GetRelativePath(path, p) }).ToArray();

            Console.WriteLine(expectedPath);
            Console.WriteLine(actualPath);
            var expectedFiles = getFileList(expectedPath);
            var actualFiles = getFileList(actualPath);

            Assert.AreNotEqual(0, actualFiles.Length, $"No files were generated under '{actualPath}'.");

            // We cannot compare the raw file count because what's in the Sample directory will always have more files.
            // Instead, we have to compare based on the Relative paths.

            var actualRelativePaths = actualFiles.ToDictionary(x => x.RelativePath, x => x);
            var expectedRelativePaths = expectedFiles.ToDictionary(x => x.RelativePath, x => x);

            var extraFiles = actualRelativePaths.Keys.Except(expectedRelativePaths.Keys).ToArray();
            Assert.AreEqual(0, extraFiles.Length, "There are extra files in the output.");

            foreach (var kvp in actualRelativePaths)
            {
                var actualFullPath = kvp.Value.FullPath;
                var expectedFullPath = expectedRelativePaths[kvp.Key].FullPath;

                var actualContents = File.ReadAllText(actualFullPath);
                var expectedContents = File.ReadAllText(expectedFullPath);
                Assert.AreEqual(expectedContents, actualContents, $"The contents of '{actualFullPath}' did not match the contents of '{expectedFullPath}'");
            }
        }
    }

    internal static class ScenarioContextExtensions
    {
        public static string Description(this ScenarioContext scenarioContext)
        {
            return scenarioContext.ScenarioInfo?.Arguments["Description"]?.ToString() ?? string.Empty;
        }
    }
}

