namespace OctoWhirl.Core.Tools.Maths.Generators
{
    public interface IGenerator<T>
    {
        T GetNext();

        void Reset() 
        { 
        }
    }
}