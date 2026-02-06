using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Batches.Generic.Enums
{
    internal enum BatchState
    {
        NotStarted,
        Initialized,
        Running,
        Terminated,
        Failed,
        Cancelled,
        Succeeded
    }
}
