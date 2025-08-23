namespace OctoWhirl.Core.Tools.Maths.Generators.Interfaces
{
    public interface IMultiGenerator<T> : IGenerator
    {
        T[] GetNext();
    }
}
