using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OC1
{
    class DataParse
    {

        private int _dimension;
        private double[][] _data { get; set; }

        public double[][] Data
        {
            get { return _data; }
        }

        public int Dimension
        {
            get
            {
                return _data[0].Count();
            }
        }

        public DataParse(string input)
        {
            string[] splitArray = new string[] { "  ", " " };
            var inputValues = File.ReadAllLines(input).Select(a => a.Split(splitArray, StringSplitOptions.RemoveEmptyEntries));

            _data = new double[inputValues.Count()][];
            int i = 0;
            foreach (var line in inputValues)
            {
                _data[i] = new double[line.Count()];
                int j = 0;
                foreach (var value in line)
                {
                    _data[i][j] = Double.Parse(value, NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign);
                    j++;
                }
                i++;
            }
        }
    }
}
