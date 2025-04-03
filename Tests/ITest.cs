namespace OctoWhirl.Tests
{
    public interface ITest
    {
        string Name;
        bool Failed;
        string Error;

        void Init();
        void Run();
    }
}