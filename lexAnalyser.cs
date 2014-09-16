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
        string outPath;
        char[] chArr;

        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        int lineNum = 0;

        char[] sep = { ' ', '.', '\t',',',';' };
        char[] num_sep = { ' ','\t', ',', ';' };
        string[] id = {"var","break",",while","do","if","for","Number","String"};

        int[,] tt_id ={{1,1,2},{1,1,1},{2,2,2}} ;
        int[,] tt_num = { {1,2,3,6}, {6,2,3,6}, {6,2,3,5}, {6,4,6,5}, {6,4,6,5}, {3,4,6,6}, {6,6,6,6} };
        int[,] tt_str = { {2,2,2,1,2},{3,3,3,4,5},{2,2,2,2,2},{3,3,3,4,5},{2,2,2,1,2},{3,3,3,3,3} };


        public lexAnalyser(string[] lines,string outPath) {
            this.lines = lines;
            this.outPath = outPath;
            this.checkStr();
        }

        void checkStr() {
            while (lineNum < lines.Length){
                //chArr = lines[lineNum].ToCharArray();
                string[] s = lines[lineNum].Split(num_sep);
                int i=0;
                while (i < s.Length) {
                    if (id.Contains(s[i]))           //check if the string contains a keyword //
                    {
                        writeToFile(s[i], "", lineNum);    // if keyword found write to file //
                    }
                    else
                    {

                        chArr = s[i].ToCharArray();
                        if (s[i] != "")
                        {
                            if (dfa_id(chArr,null))
                            {
                                
                            }
                            else if (dfa_num(chArr))
                            {
                                writeToFile("NUM", s[i], lineNum);
                            }
                            else
                            {
                                writeToFile("ERR", s[i], lineNum);
                            }
                        }
                    }
                    i++;
                }
               
                
                lineNum++;

            }
            
        }


        //bool dfa_str(char[] chArr) {

        //    int state = 0, i = 0;

        //    while (i < chArr.Length) { 
        //        state = trans_str(state,char[i]);
        //            i++;
        //    }
        
        //}

        //int trans_str(int st, char ch)
        //{ 
        
        //}

        bool dfa_num(char[] chArr)
        {
            int state = 0, i = 0;
            sb = new System.Text.StringBuilder();
            while (i < chArr.Length)
            {

                    state = trans_num(state, chArr[i]);
                    if (state > 3 && chArr[i] == '.')
                    {
                        writeToFile("DOT", ".", lineNum);
                        dfa_id(chArr, i + 1);
                    }
                i++;
            }
            if (state == 2 || state == 4)
            {
                //temp = new string(chArr);      //Still buggy
                return true;
            }
            else
            {
                return false;
            }
        }

        int trans_num(int st, char ch)
        {
            if (ch == '.')
            {
                //sb.Append(ch);
                return tt_num[st, 2];
            }
            else if (ch == 'e')
            {
                //sb.Append(ch);
                return tt_num[st, 3];
            }
            else if ((ch == '+' || ch == '-'))
            {
                //sb.Append(ch);
                return tt_num[st, 0];
            }
            else if ((ch >= '0' && ch <= '9'))
            {
                //sb.Append(ch);
                return tt_num[st, 1];
            }
            else
            {
                //sb.Append(ch);
                return 0;
            }
        }


        bool dfa_id(char[] chArr,int ? index) {
            int state = 0, i = 0;
            if (index.HasValue) {
                i = index.Value;
            }

            sb = new System.Text.StringBuilder();
            while (i < chArr.Length)
            {
                
                state = trans_id(state, chArr[i]);
                if (state == 3) {
                    writeToFile("ID", sb.ToString(), lineNum);
                    writeToFile("DOT", ".", lineNum);
                    sb = new StringBuilder();
                    state = 0;
                }
                i++;
            }
            if (state == 1)
            {
                writeToFile("ID", sb.ToString(), lineNum);
                //temp = new string(chArr);      //Still buggy
                return true;
            }
            else
            {
                return false;
            }
        }

        int trans_id(int st,char ch) {
            if (ch == '_') {
                sb.Append(ch);
                return tt_id[st,0];
            }
            else if ((ch >= 'A' && ch <= 'Z') || (ch >= 'a' && ch <= 'z')) {
                sb.Append(ch);
                return tt_id[st, 1];
            }
            else if ((ch >= '0' && ch <= '9'))
            {
                sb.Append(ch);
                return tt_id[st, 2];
            }
            else if (ch == '.' && st!=2)
            {
                return 3;
            }
            else {
                return 2;
            }
        }

        void writeToFile(string cp,string vp,int lineNum) {
            try
            {
                System.IO.StreamWriter file = new System.IO.StreamWriter(outPath, true);
                file.Write("("+cp+"," + vp + "," + lineNum + "),");
                file.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be created:");
                Console.WriteLine(e.Message);

            }
        }

    }
}
