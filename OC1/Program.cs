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
            DataParse data = new DataParse("dataset/glass_mm.txt");
            Node.setSplitConstant(2);
            Node.SetPmode(0.5);
            OC1Algorithm oc1 = new OC1Algorithm(data.Data, 2);
            oc1.ConstructTree();

            Console.Read();
        }
    }
}
