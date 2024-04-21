using System.IO;

namespace MPMAlgorithm
{
    public static class FileWork
    {
        public static void WriteToCSV(string filePath, string data)
        {
                using (var sw = new StreamWriter(filePath, true))
                {
                    if (sw.BaseStream.Length == 0)
                    {
                        sw.WriteLine("#, Number of vertexes, Number of Edges, Elapsed Time");
                    }
                    
                    sw.WriteLine(data);
                }
        }
    }
}