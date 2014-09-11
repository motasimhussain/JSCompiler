using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSCompiler
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("Enter the path of the file to be read:");
            string path = Console.ReadLine();
            Console.WriteLine("Enter the path of the output file to be created:");
            string outPath = Console.ReadLine();
            fileReader fr = new fileReader(path);
   
            string[] lineArr = fr.readFile();
            lexAnalyser la = new lexAnalyser(lineArr,outPath);

            Console.ReadLine();
        }

    }
}
