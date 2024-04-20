using System.IO;

namespace MPMAlgorithm
{
    public static class FileWork
    {
        public static void WriteToCSV(string filePath, string[] data)
        {
                using (var sw = new StreamWriter(filePath, true))
                {
                    if (sw.BaseStream.Length == 0)
                    {
                        sw.WriteLine("Header");
                    }

                    // Запис даних у файл
                    sw.WriteLine(string.Join(", ", data));
                }
        }
    }
}