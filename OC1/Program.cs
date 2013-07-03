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
            DataParse data = new DataParse("dataset/heart_mm.txt");

            Train trainVal = new Train(data.Data, 2);
            trainVal.Algorithm();
            DesicionTree tree = trainVal.tree;
            tree.Prunning(5);

            for(int i = 0; i < data.Data.Length; i++)
                Console.WriteLine(tree.Evaluate(data.Data[i]));

            Console.Read();
        }
    }
}
