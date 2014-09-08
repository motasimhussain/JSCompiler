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
            Console.WriteLine("Enter the path of the file to be read!!");
            string path = Console.ReadLine();
            lexAnalyse la = new lexAnalyse(path);

            string[] lineArr = la.readFile();

            Console.ReadLine();
        }

    }
}
