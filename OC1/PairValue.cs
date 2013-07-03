using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OC1
{
    class PairValue
    {
        public double Key{get; set;}
        public double Value{get; set;}

        public PairValue(double key, double value)
        {
            Key = key;
            Value = value;
        }
    }
}
