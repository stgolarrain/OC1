using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OC1
{
    class Program
    {
        static void Main(string[] args)
        {
            DataParse data = new DataParse("dataset/test.txt");
            AxisParallelSplit axis = new AxisParallelSplit(data._data, data.Dimension);
            Console.WriteLine(axis.SameClass());
            Console.WriteLine(axis.SameAtributtes(new int[]{0,1,2,3,4,6,7}));
            foreach(int i in axis.getClases())
                Console.Write(i);
            Console.Write("\n");
            foreach (double p in axis.getClasesProbabilities())
                Console.Write(p + " - ");
            Console.Read();
        }
    }
}
