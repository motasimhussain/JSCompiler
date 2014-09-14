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


        public lexAnalyser(string[] lines,string outPath) {
            this.lines = lines;
            this.outPath = outPath;
            this.checkStr();
        }

        void checkStr() {
            while (lineNum < lines.Length){
                chArr = lines[lineNum].ToCharArray();
                
                bool isId = dfa_id(chArr);
                //bool isNum = dfa_num(chArr);

                //if (isId){
                //    writeToFile("ID", sb.ToString(), lineNum);
                //}
                //else 
                //if(isNum){
                //    writeToFile("NUM", sb.ToString(), lineNum);      //buggy
                //}

                lineNum++;

            }
            
        }


        bool dfa_num(char[] chArr)
        {
            int state = 0, fstate = 0, i = 0;
            sb = new System.Text.StringBuilder();
            while (i < chArr.Length)
            {
                if (!num_sep.Contains(chArr[i])){
                    state = trans_num(state, chArr[i]);
                }
                if (num_sep.Contains(chArr[i]) || (i + 1 == chArr.Length) || (state == 6))                  ///// if seperator dump previously read char as ID and clear sb (string builder) /////
                {
                    if ((sb.ToString() != "") && (state != 2 && state != 4))
                    {
                        writeToFile("ERR", sb.ToString(), lineNum);
                        sb = new System.Text.StringBuilder();
                        fstate = 0;
                    }
                    else if ((sb.ToString() != "") && (state == 2 || state == 4))
                    {
                        writeToFile("NUM", sb.ToString(), lineNum);
                        sb = new System.Text.StringBuilder();
                        fstate = 1;
                    }
                    state = 0;
                }
                i++;
            }
            if (fstate == 1)
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
                sb.Append(ch);
                return tt_num[st, 2];
            }
            else if (ch == 'e')
            {
                sb.Append(ch);
                return tt_num[st, 3];
            }
            else if ((ch == '+' || ch == '-'))
            {
                sb.Append(ch);
                return tt_num[st, 0];
            }
            else if ((ch >= '0' && ch <= '9'))
            {
                sb.Append(ch);
                return tt_num[st, 1];
            }
            else
            {
                sb.Append(ch);
                return 0;
            }
        }


        bool dfa_id(char[] chArr) {
            int state = 0, i = 0;
            sb = new System.Text.StringBuilder();
            while (i < chArr.Length)
            {
                if (!sep.Contains(chArr[i]))
                {
                    state = trans_id(state, chArr[i]);
                }
                if (sep.Contains(chArr[i]) || (i + 1 == chArr.Length))                                   ///// if seperator dump previously read char as ID and clear sb (string builder) /////
                {
                    if ((sb.ToString() != "") && (state != 1))
                    {

                        if (!dfa_num(sb.ToString().ToCharArray()))
                        {
                            //writeToFile("ERR", sb.ToString(), lineNum);
                            sb = new System.Text.StringBuilder();
                        }
                        else
                        {
                            sb = new System.Text.StringBuilder();
                        }

                    }
                    else if ((sb.ToString() != "") && (state == 1))
                    {
                        if (!id.Contains(sb.ToString()))
                        {
                            writeToFile("ID", sb.ToString(), lineNum);
                            sb = new System.Text.StringBuilder();
                        }
                        else
                        {
                            writeToFile(sb.ToString(), "", lineNum);
                            sb = new System.Text.StringBuilder();
                        }
                    }
                    state = 0;
                }
                i++;
            }
            if (state == 1)
            {
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
            else
            {
                return 0;
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
