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
            AxisParallelSplit axis = new AxisParallelSplit(data.Data, data.Dimension, 2);
            Console.WriteLine(axis.SameClass());
            Console.WriteLine(axis.SameAtributtes(new int[]{0,1,2,3}));
            foreach(int i in axis.getClases())
                Console.Write(i);
            Console.Write("\n");
            foreach (double p in axis.getClasesProbabilities(data.Data))
                Console.Write(p + " - ");
            Console.Write("\n");

            Console.WriteLine(axis.Entropy(data.Data));
            Console.Write("\n");

            Console.WriteLine(axis.Gain(0, data.Data, 3));
            Console.WriteLine(axis.Gain(1, data.Data, 3));
            Console.WriteLine(axis.Gain(2, data.Data, 2));
            Console.WriteLine(axis.Gain(3, data.Data, 2));

            Console.Read();
        }
    }
}
