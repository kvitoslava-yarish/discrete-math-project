using System;
using System.Diagnostics;
using System.Text;


namespace MPMAlgorithm
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var graphList = new AdjacencyListGraph();
            var vertexNum = 20;
            var possibility = 0.3;
            var maxWeight = 100;
            var resultList = new StringBuilder();
            var resultMatrix = new StringBuilder();
            var swList = new Stopwatch();
            var swMatrix = new Stopwatch();
            
            for (var i = 0; i < 5; i++)
            {
                for (var j = 0; j < 10; j++)
                {
                    for (var n = 1; n < 21; n++)
                    {
                        graphList.GenerationGraph(possibility, vertexNum, maxWeight);
                        swList.Start();
                        var algorithmList = new MPM(graphList, 0, vertexNum - 1);
                        var l = algorithmList.MaxFlow();
                        swList.Stop();
                        resultList.Append($"{n}, {vertexNum}, {possibility.ToString().Replace(",", ".")}, {swList.Elapsed}\n");
                        
                        var graphMatrix = new MatrixGraph(vertexNum, possibility, maxWeight);
                        swMatrix.Start();
                        var algorithmMatrix = new MPMmatrix(graphMatrix, 0, vertexNum - 1);
                        var m = algorithmMatrix.MaxFlow();
                        swMatrix.Stop();
                        resultMatrix.Append($"{n}, {vertexNum}, {possibility.ToString().Replace(",", ".")}, {swMatrix.Elapsed}\n");
                    }

                    possibility += 0.1;
                }

                possibility = 0.3;
                vertexNum += 20;
            }
            FileWork.WriteToCSV(
                "C:\\Users\\pavlo\\RiderProjects\\discrete-math-project\\MPMAlgorithm\\MPMAlgorithm\\outputList.csv",
                resultList.ToString());
            FileWork.WriteToCSV(
                "C:\\Users\\pavlo\\RiderProjects\\discrete-math-project\\MPMAlgorithm\\MPMAlgorithm\\outputMatrix.csv",
                resultMatrix.ToString());
        }
        }
    }

