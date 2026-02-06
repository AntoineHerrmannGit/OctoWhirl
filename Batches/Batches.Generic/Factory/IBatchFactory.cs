using Batches.Generic.Interfaces;

namespace Batches.Generic.Factory
{
    internal interface IBatchFactory
    {
        IBatch Create(string name);
    }
}
