using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSCompiler
{
    class Program
    {

        static void Main(string[] args)
        {
            string path = "c:/test.txt";
            string outPath = "e:/out.txt";

            //Console.WriteLine("Enter the path of the file to be read:");
            //path = Console.ReadLine();
            //Console.WriteLine("Enter the path of the output file to be created:");
            //outPath = Console.ReadLine();
            try
            {
                if (File.Exists(outPath))
                {
                    File.Delete(outPath);
                }

                var watch = Stopwatch.StartNew();

                fileReader fr = new fileReader(path);

                string[] lineArr = fr.readFile();

                lexAnalyser la = new lexAnalyser(lineArr, outPath);

                TokenReader tr = new TokenReader(outPath);

                SynAnalize sa = new SynAnalize(tr.readFile());

                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;

                Console.WriteLine("Execution Time: " + elapsedMs + "ms.");
                Process.Start(outPath);
            }
            catch (Exception e) {
                Console.WriteLine("Oops!! Something went wrong.");
                Console.WriteLine(e.Message);
            }
            Console.ReadLine();
        }

    }
}
