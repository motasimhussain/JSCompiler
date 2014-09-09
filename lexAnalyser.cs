using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSCompiler
{

    class lexAnalyser
    {
        string[] lines;
        char[] textArr;
        int lineNum = 0;

        public lexAnalyser(string[] lines) {
            this.lines = lines;
            this.convertChar();
        }

        void convertChar() {
            while (lineNum < lines.Length)
            {
                textArr = lines[lineNum].ToCharArray();
                lineNum++;
            }
        }

    }
}
