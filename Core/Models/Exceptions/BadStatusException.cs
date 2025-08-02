using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OctoWhirl.Core.Models.Exceptions
{
    public class BadStatusException : Exception
    {
        public BadStatusException() { }

        public BadStatusException(string message) : base(message) { }
    }
}
