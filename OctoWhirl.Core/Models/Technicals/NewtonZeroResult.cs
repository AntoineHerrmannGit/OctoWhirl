using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OctoWhirl.Core.Models.Technicals
{
    public class NewtonZeroResult
    {
        public double Value { get; set; }   
        public int Iterations { get; set; }
        public bool HasConverged { get; set; }
        public double Distance { get; set; }
        public string Error { get; set; }
    }
}
