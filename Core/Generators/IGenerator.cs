namespace OctoWhirl.Core.Generators
{
    public interface IGenerator<T>
    {
        T GetNext();
    }
}