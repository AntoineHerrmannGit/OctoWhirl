namespace OctoWhirl.Core.Tools.Generators
{
    public interface IGenerator<T>
    {
        T GetNext();

        void Reset() 
        { 
        }
    }
}