namespace OctoWhirl.Tests
{
    public interface ITest
    {
        string Name { get; set; };
        bool Failed { get; set; };
        string Error { get; set; };

        void Init();
        void Run();
    }
}