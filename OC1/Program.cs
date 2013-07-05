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
            string db = "pima_indians_diabetes.txt";
            DataParse data = new DataParse("dataset/" + db);

            string result = "";

            for (int k = 1; k < 5; k++)
            {
                Console.WriteLine("\n========================");
                Console.WriteLine("K = " + k);
                result += "\nk = " + k.ToString();
                for (int maxLeaf = 2; maxLeaf < 6; maxLeaf++)
                {
                    Console.WriteLine("Max Leaf = " + maxLeaf);
                    result += "\nmax leaf = " + maxLeaf.ToString();
                    double[] accuracy = new double[10];
                    int[] node = new int[10];
                    double[] leafLastNode = new double[10];
                    for (int crossValidation = 0; crossValidation < 10; crossValidation++)
                    {
                        Console.WriteLine("Cross Validation = " + crossValidation);
                        // Creating Cross Validation data.
                        List<double[]> trainData = new List<double[]>();
                        List<double[]> validationData = new List<double[]>();
                        int segment = data.Data.Length / 10;
                        int dimension = data.Data[0].Length;
                        for (int i = 0; i < data.Data.Length; i++)
                        {
                            if (i > crossValidation * segment && i < (crossValidation + 1) * segment)
                                validationData.Add(data.Data[i]);
                            else
                                trainData.Add(data.Data[i]);
                        }

                        // Training
                        Train trainVal = new Train(trainData.ToArray(), k);
                        trainVal.Algorithm();
                        DesicionTree tree = trainVal.tree;
                        tree.Prunning(maxLeaf);

                        // Testing and taking accuracy
                        foreach (double[] line in validationData)
                        {
                            if (tree.Evaluate(line) == line[dimension - 1])
                                accuracy[crossValidation]++;
                        }
                        node[crossValidation] = tree.CountNodes();
                        accuracy[crossValidation] /= validationData.Count;
                        leafLastNode[crossValidation] = tree.LeafAvg();
                        result += "\nNnode = " + tree.CountNodes();
                        result += "\nAcuraccy = " + accuracy.Average();
                    }
                    result += "\nAcuraccyAvg = " + accuracy.Average();
                    result += "\nNodes Number Avg = " + node.Average();
                    result += "\nLeaf Avg last node = " + leafLastNode.Average();
                }
            }
            System.IO.File.WriteAllText("results/" + db, result);
            Console.WriteLine("FINISH");
            Console.Read();
        }
    }
}
