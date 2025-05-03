using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OctoWhirl.Services.Data.DataBaseModels
{
    public class DBSpot
    {
        public string ticker { get; set; }
        public string currency { get; set; }    
        public DateTime timestamp {  get; set; }
        public double open { get; set; }
        public double high { get; set; }
        public double low { get; set; }
        public double close { get; set; }
        public int volume { get; set; }    
    }
}
