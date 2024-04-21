using System;
using System.Diagnostics;
using System.IO;
using System.Text;
// MPM for AdjacencyList
// MPMMatrix for Matrix  
namespace MPMAlgorithm
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var graphList = new AdjacencyListGraph();
            var graphMatrix = new MatrixGraph();
            var vertexNum = 20;
            var possibility = 0.3;
            var maxWeight = 100;
            var swList = new Stopwatch();
            var swMatrix = new Stopwatch();

            for (var i = 0; i < 5; i++)
            {
                for (var j = 0; j < 10; j++)
                {
                    for (var n = 1; n < 11; n++)
                    {
                        graphList.GenerationGraph(possibility, vertexNum, maxWeight);
                        swList.Start();
                        var algorithmList = new MPM(graphList, 0, vertexNum - 1);
                        var l = algorithmList.MaxFlow();
                        swList.Stop();
                        var resultList = $"{n}, {vertexNum}, {possibility.ToString().Replace(",", ".")}, {swList.Elapsed}\n";
                        File.AppendAllText("C:\\Users\\kvita\\RiderProjects\\discrete-math-project\\MPMAlgorithm\\MPMAlgorithm\\outputList.csv", resultList);

                        graphMatrix.GenerateGraph(vertexNum, possibility, maxWeight);
                        swMatrix.Start();
                        var algorithmMatrix = new MPMmatrix(graphMatrix, 0, vertexNum - 1);
                        var m = algorithmMatrix.MaxFlow();
                        swMatrix.Stop();
                        var resultMatrix = $"{n}, {vertexNum}, {possibility.ToString().Replace(",", ".")}, {swMatrix.Elapsed}\n";
                        File.AppendAllText("C:\\Users\\kvita\\RiderProjects\\discrete-math-project\\MPMAlgorithm\\MPMAlgorithm\\outputMatrix.csv", resultMatrix);
                        Console.WriteLine($"Iterration {i}, {j}, {n}");
                    }

                    possibility += 0.1;
                }

                possibility = 0.3;
                vertexNum += 20;
            }
        }
    }
}
