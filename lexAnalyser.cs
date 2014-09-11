﻿using System;
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
        string temp;
        int lineNum = 0;

        int[,] tt_id ={{1,1,2},{1,1,1},{2,2,2}} ;
        int[,] tt_num = { {1 , 3}, {1 , 2}, {2 , 4},{3 , 4},{4 , 4} };


        public lexAnalyser(string[] lines,string outPath) {
            this.lines = lines;
            this.outPath = outPath;
            this.checkStr();
        }

        void checkStr() {
            while (lineNum < lines.Length){
                chArr = lines[lineNum].ToCharArray();
                
                bool isId = dfa_id(chArr);
                bool isNum = dfa_num(chArr);

                if (isId){
                    try{
                        System.IO.StreamWriter file = new System.IO.StreamWriter(outPath, true);
                        file.Write("(ID,"+temp+ ",line),");
                        file.Close();
                    }
                    catch (Exception e){
                        Console.WriteLine("The file could not be created:");
                        Console.WriteLine(e.Message);

                    }
                }

                lineNum++;

            }
            
        }


        bool dfa_num(char[] chArr)
        {
            int state = 0, i = 0;
            while (i < chArr.Length)
            {
                state = trans_num(state, chArr[i]);
                i++;
            }
            if ((state == 1) || (state == 2) || (state == 3))
            {
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
                return tt_num[st, 1];
            }
            else if ((ch >= '0' && ch <= '9'))
            {
                return tt_num[st, 0];
            }
            return 0;
        }


        bool dfa_id(char[] chArr) {
            int state = 0,f_state=1,i=0;
            while (i < chArr.Length) {
                state = trans_id(state, chArr[i]);
                i++;
            }
            if (state == f_state)
            {
                temp = new string(chArr);
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
            else if ((ch >= '0' && ch <= '9')){
                return tt_id[st, 2];
            }
            return 0;
        }

    }
}