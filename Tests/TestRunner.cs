using System.Collections.Generic;
using System.Threading.Tasks;

namespace OctoWhirl.Tests
{
    public class TestRunner
    {
        private ILogger _logger;
        private List<ITest> _tests;
        public TestRunner()
        {
            _tests = new List<ITest>();
            _logger = new Logger("testLogs.log");
            RegisterTests();
        }

        private void RegisterTests()
        {
        }

        public void RunTests()
        {
            Parallel.ForEach(_tests, test => 
            {
                _logger.Log($"Running Test : {test.Name}");
                test.Init();
                test.Run();
                
                if (test.Failed)
                    _logger.Log($"Test : {test.Name} FAILED : {test.Error}");
                
                _logger.Log($"Test : {test.Name} SUCCEEDED");
            });
        }
    }
}