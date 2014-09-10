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
        char[] chArr;
        int lineNum = 0;

        int[,] tt_id ={{1,2,3},{3,2,2},{3,2,2},{3,3,3}} ;


        public lexAnalyser(string[] lines) {
            this.lines = lines;
            this.convertChar();
        }

        void convertChar() {
            while (lineNum < lines.Length)
            {
                chArr = lines[lineNum].ToCharArray();
                lineNum++;
                bool isId = dfa_id(chArr);
            }
           
        }

        bool dfa_id(char[] chArr) {
            int state = 0,f_state=2,i=0;
            while (i < chArr.Length) {
                state = trans_id(state, chArr[i]);
                i++;
            }
            if (state == f_state)
            {
                return true;
            }
            else {
                return false;
            }
        }

        int trans_id(int st,char ch) {
            if (ch == '_') {
                return tt_id[st,0];
            }
            else if ((ch >= 'A' && ch <= 'Z') || (ch >= 'a' && ch <= 'z')) {
                return tt_id[st, 1];
            }
            return 0;
        }

    }
}
