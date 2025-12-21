namespace OctoWhirl.Core.Tools.Maths.Generators.Interfaces
{
    public interface ISimpleGenerator<T> : IGenerator
    {
        T GetNext();
    }
}