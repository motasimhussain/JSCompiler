using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSCompiler
{
    class Program
    {

        static void Main(string[] args)
        {
            //Console.WriteLine("Enter the path of the file to be read:");
            //string path = Console.ReadLine();
            //Console.WriteLine("Enter the path of the output file to be created:");
            //string outPath = Console.ReadLine();

            var watch = Stopwatch.StartNew();

            // the code that you want to measure comes here
            
            fileReader fr = new fileReader("c:/test.txt");
   
            string[] lineArr = fr.readFile();
            lexAnalyser la = new lexAnalyser(lineArr,"e:/out.txt");

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;

            Console.WriteLine("Execution Time: " + elapsedMs+"ms.");
            Console.ReadLine();
        }

    }
}
