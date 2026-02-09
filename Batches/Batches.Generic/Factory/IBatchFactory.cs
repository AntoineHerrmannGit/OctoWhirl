using Batches.Generic.Interfaces;

namespace Batches.Generic.Factory
{
    public interface IBatchFactory
    {
        IBatch Create(string name);
    }
}
