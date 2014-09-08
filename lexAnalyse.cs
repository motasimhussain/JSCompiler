using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace JSCompiler
{
    class lexAnalyse
    {
        public string path;

        public lexAnalyse(string path) {
            this.path = path;
        }

        public string[] readFile(){
            int counter = 0;
            string line;
            string[] lineArr;

            try{
                System.IO.StreamReader file = new System.IO.StreamReader(path);
                int count = File.ReadLines(path).Count();     // Counting the number of lines in a file
                lineArr = new string[count];

                while ((line = file.ReadLine()) != null){     // Storing individual lines in an array
                    Console.WriteLine(line);
                    lineArr[counter] = line;
                    counter++;
                }

                file.Close();
                return lineArr;                               //returning an array

            }catch (Exception e){
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);

            }

            return lineArr=new string[0];                     //returning empy array if file read fails!
        }
    }
}
