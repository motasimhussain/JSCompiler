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
        bool[] isRec = { false, false, false, false, false, false, false, false };


        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        int lineNum = 0;
        int j = 0;

        char[] sep = { ' ', '.', '\t', ',', ';' ,':', '"', '}', '{', '(', ')', '[', ']' };
        char[] op = { '+', '-', '*', '/', '=', '%', '&', '|', '!', '>', '<' };
        char[] num_sep = { ' ', '\t', ',', ';' };
        string[] id = { "var", "switch", "case", "default", "function", "new", "else", "array", "void", "return", "in", "finally", "break", "while", "do", "if", "for", "Number", "String" };
        int[,] tt_op = { { 1 }, { 2 }, { 2 } };
        int[,] tt_id = { { 1, 1, 2 }, { 1, 1, 1 }, { 2, 2, 2 } };
        int[,] tt_num = { { 1, 2, 3, 6 }, { 6, 2, 3, 6 }, { 6, 2, 3, 5 }, { 6, 4, 6, 5 }, { 6, 4, 6, 5 }, { 3, 4, 6, 6 }, { 6, 6, 6, 6 } };
        int[,] tt_str = { { 2, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1 }, { 5, 3, 4, 4, 4 }, { 4, 4, 4, 4, 4 }, { 5, 3, 4, 4, 4 }, { 5, 5, 5, 5, 5 } };
        int[,] tt_ass = { { 1, 2 }, { 3, 2 }, { 3, 3 }, { 3, 3 } };
        int[,] tt_ido = { { 1, 3 }, { 2, 4 }, { 4, 4 }, { 4, 2 }, { 4, 4 } };
        int[,] tt_lop = { { 1,2,3}, {3, 4,4 }, { 4, 3, 4 }, { 4, 4, 4 },{4,4,4} };
        int[,] tt_cmp = { { 1, 2, 2 }, { 4, 4, 3 }, { 4, 4, 3 }, { 4, 4, 4 }, { 4, 4, 4 } };

        public lexAnalyser(string[] lines, string outPath)
        {
            this.lines = lines;
            this.outPath = outPath;
            this.checkStr();

        }

        void checkStr()
        {
            while (lineNum < lines.Length)
            {
                chArr = lines[lineNum].ToCharArray();
                sb.Clear();
                int DOT = 0;
                int ddotflag = 0;
                int i = 0, separatorflag = 0, opFlag = 0, incomm = 0; // MAKE ALL = 0 Bewoqoofi nahi dikhana koi
                while (j < chArr.Length)
                {

                        if (j < chArr.Length)
                        {

                            if (sep.Contains(chArr[j]))
                            {
                                isRecognized(sb.ToString());
                                sb.Clear();
                                separatorflag = 1;
                                if (chArr[j] == '.')
                                {
                                    if (DOT >= 1)
                                    {
                                        ddotflag++;
                                        isRecognized(sb.ToString());
                                        sb = new StringBuilder();
                                        DOT = 0;
                                    }
                                    if (j > 0 && ddotflag == 0)
                                    {
                                        if (Char.IsNumber(chArr[j - 1]) && Char.IsNumber(chArr[j + 1]) && DOT == 0)
                                        {
                                            sb.Append(chArr[j]);    //JO KUCH BHI HE USKO APPEND KARO
                                            DOT++;         //THIS IS DOT FLAG
                                        }
                                        else if (!(Char.IsNumber(chArr[j - 1])) && Char.IsNumber(chArr[j + 1]) && DOT == 0)
                                        {
                                            isRecognized(sb.ToString());
                                            sb = new StringBuilder();     //CLEAR SB HERE AND SEND IT TO DFAS
                                            DOT = 0;
                                            sb.Append(chArr[j]);
                                            DOT++;
                                        }
                                        else if (!(Char.IsNumber(chArr[j - 1])) && !(Char.IsNumber(chArr[j + 1])) && DOT == 0)
                                        {
                                            isRecognized(sb.ToString());
                                            sb = new StringBuilder();     //CLEAR SB HERE AND SEND IT TO DFAS
                                            sb.Append(chArr[j]);
                                            writeToFile("DOT", ".", lineNum);
                                            sb = new StringBuilder();// YES CLEAR SB AGAIN ONLY SEND DOT TO DFAS AS TOKENe.g. token(DOT,.,1)
                                        }
                                        else if (Char.IsNumber(chArr[j - 1]) && !(Char.IsNumber(chArr[j + 1])) && DOT == 0)
                                        {
                                            isRecognized(sb.ToString());
                                            sb = new StringBuilder();     //CLEAR SB HERE AND SEND IT TO DFAS
                                            sb.Append(chArr[j]);
                                            writeToFile("DOT", ".", lineNum);
                                            sb = new StringBuilder();    // YES CLEAR SB AGAIN ONLY SEND DOT TO DFAS AS TOKENe.g. token(DOT,.,1)
                                        }
                                    }
                                    else if (j == 0 || ddotflag == 1)
                                    {
                                        ddotflag = 0;
                                        if (Char.IsNumber(chArr[j + 1]))
                                        {
                                            sb.Append(chArr[j]);
                                            DOT++;
                                        }
                                        else
                                        {
                                            sb.Append(chArr[j]);
                                            writeToFile("DOT", ".", lineNum);
                                            sb = new StringBuilder();	//SEND DOT AS SEPARATE TOKEN
                                        }
                                    }


                                }
                                else if (chArr[j] == '"')
                                {
                                    incomm = 1;
                                    while (incomm == 1)
                                    {
                                        if (j + 1 < chArr.Length)
                                        {
                                            j++;
                                            if (chArr[j] == '\\' && (j + 1 < chArr.Length))
                                            {
                                                j++;
                                                sb.Append(chArr[j]);
                                            }
                                            else if (chArr[j] == '"' || j >= chArr.Length)
                                            {
                                                incomm = 0;
                                                //j++;
                                                string str = sb.ToString();
                                                writeToFile("STR", str, lineNum);
                                                sb = new StringBuilder();
                                            }
                                            else
                                            {
                                                sb.Append(chArr[j]);
                                            }
                                        }
                                        else {
                                            writeToFile("ERR", sb.ToString(), lineNum);
                                            sb.Clear();
                                            incomm = 0;
                                            break;
                                        }
                                    }

                                }
                                else if (chArr[j] == ',' || chArr[j] == ';' || chArr[j] == '(' || chArr[j] == ')' || chArr[j] == '[' || chArr[j] == ']' || chArr[j] == '{' || chArr[j] == '}' || chArr[j] == ':')
                                {
                                    isRecognized(sb.ToString());
                                    sb.Clear();
                                    writeToFile(chArr[j].ToString(),chArr[j].ToString(), lineNum);
                                }
                                else
                                {
                                    isRecognized(sb.ToString());
                                    sb.Clear();
                                }
                            }
                        }

                        if (j < chArr.Length)
                        {
                            if (op.Contains(chArr[j]))
                            {
                                opFlag = 1;

                                if (chArr[j] == '+')
                                {
                                    isRecognized(sb.ToString());
                                    sb.Clear();
                                    if (chArr[j + 1] == '+' || chArr[j + 1] == '=')
                                    {
                                        sb.Append(chArr[j]);
                                        j++;
                                        sb.Append(chArr[j]);
                                        isRecognized(sb.ToString());
                                        sb.Clear();
                                    }
                                    else if (!(chArr[j + 1] == '+' || chArr[j + 1] == '='))
                                    {
                                        sb.Append(chArr[j]);
                                        isRecognized(sb.ToString());
                                        sb.Clear();
                                    }
                                }
                                else if (chArr[j] == '-')
                                {
                                    isRecognized(sb.ToString());
                                    sb.Clear();
                                    if (chArr[j + 1] == '-' || chArr[j + 1] == '=')
                                    {
                                        sb.Append(chArr[j]);
                                        j++;
                                        sb.Append(chArr[j]);
                                        isRecognized(sb.ToString());
                                        sb.Clear();
                                    }
                                    else if (!(chArr[j + 1] == '-' || chArr[j + 1] == '='))
                                    {
                                        sb.Append(chArr[j]);
                                        isRecognized(sb.ToString());
                                        sb.Clear();
                                    }
                                }
                                else if (chArr[j] == '*')
                                {
                                    isRecognized(sb.ToString());
                                    sb.Clear();
                                    if (chArr[j + 1] == '=')
                                    {
                                        sb.Append(chArr[j]);
                                        j++;
                                        sb.Append(chArr[j]);
                                        isRecognized(sb.ToString());
                                        sb.Clear();
                                    }
                                    else if (!(chArr[j + 1] == '='))
                                    {
                                        sb.Append(chArr[j]);
                                        isRecognized(sb.ToString());
                                        sb.Clear();
                                    }
                                }
                                else if (chArr[j] == '/')
                                {
                                    isRecognized(sb.ToString());
                                    sb.Clear();
                                    if (chArr[j + 1] == '=')
                                    {
                                        sb.Append(chArr[j]);
                                        j++;
                                        sb.Append(chArr[j]);
                                        isRecognized(sb.ToString());
                                        sb.Clear();
                                    }
                                    else if (chArr[j + 1] == '*')
                                    {
                                        j++;
                                        j++;
                                        mlCmnt();
                                        j++;
                                    }
                                    else if (chArr[j + 1] == '/')
                                    {
                                        j = chArr.Length - 1;
                                    }
                                    else if (!(chArr[j + 1] == '='))
                                    {
                                        sb.Append(chArr[j]);
                                        isRecognized(sb.ToString());
                                        sb.Clear();
                                    }

                                }
                                else if (chArr[j] == '%')
                                {
                                    isRecognized(sb.ToString());
                                    sb.Clear();
                                    if (chArr[j + 1] == '=')
                                    {
                                        sb.Append(chArr[j]);
                                        j++;
                                        sb.Append(chArr[j]);
                                        isRecognized(sb.ToString());
                                        sb.Clear();
                                    }
                                    else if (!(chArr[j + 1] == '='))
                                    {
                                        sb.Append(chArr[j]);
                                        isRecognized(sb.ToString());
                                        sb.Clear();
                                    }
                                }
                                else if (chArr[j] == '&')
                                {
                                    if (chArr[j + 1] == '&')
                                    {
                                        sb.Append(chArr[j]);
                                        j++;
                                        sb.Append(chArr[j]);
                                        isRecognized(sb.ToString());
                                        sb.Clear();
                                    }
                                    else if (!(chArr[j + 1] == '&'))
                                    {
                                        sb.Append(chArr[j]);
                                    }
                                }
                                else if (chArr[j] == '|')
                                {
                                    if (chArr[j + 1] == '|')
                                    {
                                        sb.Append(chArr[j]);
                                        j++;
                                        sb.Append(chArr[j]);
                                        isRecognized(sb.ToString());
                                        sb.Clear();
                                    }
                                    else if (!(chArr[j + 1] == '|'))
                                    {
                                        sb.Append(chArr[j]);
                                    }
                                }
                                else if (chArr[j] == '!')
                                {
                                    isRecognized(sb.ToString());
                                    sb.Clear();
                                    if (chArr[j + 1] == '=')
                                    {
                                        sb.Append(chArr[j]);
                                        j++;
                                        sb.Append(chArr[j]);
                                        isRecognized(sb.ToString());
                                        sb.Clear();
                                    }
                                    else if (!(chArr[j + 1] == '='))
                                    {
                                        sb.Append(chArr[j]);
                                        isRecognized(sb.ToString());
                                        sb.Clear();
                                    }
                                }
                                else if (chArr[j] == '=')
                                {
                                    isRecognized(sb.ToString());
                                    sb.Clear();
                                    if (chArr[j + 1] == '=')
                                    {
                                        sb.Append(chArr[j]);
                                        j++;
                                        sb.Append(chArr[j]);
                                        isRecognized(sb.ToString());
                                        sb.Clear();
                                    }
                                    else if (!(chArr[j + 1] == '='))
                                    {
                                        sb.Append(chArr[j]);
                                        isRecognized(sb.ToString());
                                        sb.Clear();
                                    }
                                }
                                else if (chArr[j] == '<')
                                {
                                    isRecognized(sb.ToString());
                                    sb.Clear();
                                    if (chArr[j + 1] == '=')
                                    {
                                        sb.Append(chArr[j]);
                                        j++;
                                        sb.Append(chArr[j]);
                                        isRecognized(sb.ToString());
                                        sb.Clear();
                                    }
                                    else if (!(chArr[j + 1] == '='))
                                    {
                                        sb.Append(chArr[j]);
                                        isRecognized(sb.ToString());
                                        sb.Clear();
                                    }
                                }
                                else if (chArr[j] == '>')
                                {
                                    isRecognized(sb.ToString());
                                    sb.Clear();
                                    if (chArr[j + 1] == '=')
                                    {
                                        sb.Append(chArr[j]);
                                        j++;
                                        sb.Append(chArr[j]);
                                        isRecognized(sb.ToString());
                                        sb.Clear();
                                    }
                                    else if (!(chArr[j + 1] == '='))
                                    {
                                        sb.Append(chArr[j]);
                                        isRecognized(sb.ToString());
                                        sb.Clear();
                                    }
                                }

                            }
                        }

                    if (separatorflag == 0 && opFlag == 0)
                    {
                        sb.Append(chArr[j]);
                    }
                    j++;
                    separatorflag = 0;
                    opFlag = 0;
                }
                i++;
                lineNum++;
                j = 0;
                isRecognized(sb.ToString());
                sb.Clear();
            }
        }


        bool dfa_ido(char[] chArr)
        {

            int state = 0, i = 0;

            while (i < chArr.Length)
            {
                state = trans_ido(state, chArr[i]);
                i++;
            }

            if (state == 2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        int trans_ido(int st, char ch)
        {
            if (ch == '+')
            {
                return tt_ido[st, 0];
            }
            else if (ch == '-')
            {
                return tt_ido[st, 1];
            }
            else
            {
                return 4;
            }

        }

        bool dfa_lop(char[] chArr)
        {

            int state = 0, i = 0;

            while (i < chArr.Length)
            {
                state = trans_lop(state, chArr[i]);
                i++;
            }

            if (state == 3)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        int trans_lop(int st, char ch)
        {
            if (ch == '&')
            {
                return tt_lop[st, 0];
            }
            else if (ch == '|')
            {
                return tt_lop[st, 1];
            }
            else if (ch == '!')
            {
                return tt_lop[st, 2];
            }
            else
            {
                return 4;
            }

        }

        bool dfa_cmp(char[] chArr)
        {

            int state = 0, i = 0;

            while (i < chArr.Length)
            {
                state = trans_cmp(state, chArr[i]);
                i++;
            }

            if (state == 1 || state == 3)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        int trans_cmp(int st, char ch)
        {
            if (ch == '>' || ch == '<')
            {
                return tt_cmp[st, 0];
            }
            else if (ch == '!') {
                return tt_cmp[st, 1];
            }
            else if (ch == '=')
            {
                return tt_cmp[st, 2];
            }
            else
            {
                return 4;
            }

        }

        bool dfa_op(char[] chArr)
        {

            int state = 0, i = 0;

            while (i < chArr.Length)
            {
                state = trans_op(state, chArr[i]);
                i++;
            }

            if (state == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        int trans_op(int st, char ch)
        {
            if (ch == '+' || ch == '-' || ch == '*' || ch == '/' || ch == '%')
            {
                return tt_op[st, 0];
            }
            else
            {
                return 2;
            }

        }

        bool dfa_ass(char[] chArr)
        {

            int state = 0, i = 0;

            while (i < chArr.Length)
            {
                state = trans_ass(state, chArr[i]);
                i++;
            }

            if (state == 2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        int trans_ass(int st, char ch)
        {
            if (ch == '+' || ch == '-' || ch == '*' || ch == '/' || ch == '%')
            {
                return tt_ass[st, 0];
            }
            else if (ch == '=')
            {
                return tt_ass[st, 1];
            }
            else
            {
                return 3;
            }

        }

        bool dfa_str(char[] chArr)
        {

            int state = 0, i = 0;

            while (i < chArr.Length)
            {
                state = trans_str(state, chArr[i]);
                i++;
            }

            if (state == 5)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        int trans_str(int st, char ch)
        {
            if (ch == '"')
            {
                return tt_str[st, 0];
            }
            else if (ch == '\\')
            {
                return tt_str[st, 1];
            }
            else if ((ch >= 'A' && ch <= 'Z') || (ch >= 'a' && ch <= 'z'))
            {
                return tt_str[st, 2];
            }
            else if ((ch >= '0' && ch <= '9'))
            {
                return tt_str[st, 3];
            }
            else if (ch >= ' ' && ch <= '~' && ch != '\\')
            {
                return tt_str[st, 4];
            }
            else
            {
                return 2;
            }

        }

        bool dfa_num(char[] chArr)
        {
            int state = 0, i = 0;
            sb = new System.Text.StringBuilder();
            while (i < chArr.Length)
            {
                state = trans_num(state, chArr[i]);
                i++;
            }
            if (state == 2 || state == 4)
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
                return tt_num[st, 2];
            }
            else if (ch == 'e')
            {
                return tt_num[st, 3];
            }
            else if ((ch == '+' || ch == '-'))
            {
                return tt_num[st, 0];
            }
            else if ((ch >= '0' && ch <= '9'))
            {
                return tt_num[st, 1];
            }
            else
            {
                return 6;
            }
        }


        bool dfa_id(char[] chArr)
        {
            int state = 0, i = 0;

            sb = new System.Text.StringBuilder();
            while (i < chArr.Length)
            {
                state = trans_id(state, chArr[i]);
                i++;
            }
            if (state == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        int trans_id(int st, char ch)
        {
            if (ch == '_')
            {
                return tt_id[st, 0];
            }
            else if ((ch >= 'A' && ch <= 'Z') || (ch >= 'a' && ch <= 'z'))
            {
                return tt_id[st, 1];
            }
            else if ((ch >= '0' && ch <= '9'))
            {
                return tt_id[st, 2];
            }
            else
            {
                return 2;
            }
        }

        void writeToFile(string cp, string vp, int lineNum)
        {
            try
            {
                System.IO.StreamWriter file = new System.IO.StreamWriter(outPath, true);
                file.WriteLine(cp + "`" + vp + "`" + lineNum);
                file.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be created:");
                Console.WriteLine(e.Message);

            }
        }
        void mlCmnt()
        {
            int fl = 0;
            while (fl != 1)
            {
                if (chArr.Length > 0)
                {
                    if (chArr[j] == '*' && chArr[j + 1] == '/')
                    {
                        fl = 1;
                    }
                    else
                    {
                        j++;
                        if (j >= chArr.Length)
                        {
                            if (lineNum + 1 < lines.Length)
                            {
                                lineNum++;
                                chArr = lines[lineNum].ToCharArray();
                                j = 0;
                            }
                            else {
                                break;
                            }
                        }
                    }
                }
                else
                {
                    if (lineNum + 1 < lines.Length)
                    {
                        lineNum++;
                        chArr = lines[lineNum].ToCharArray();
                        j = 0;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        void isRecognized(string str)
        {
            if (str != "")
            {
                char[] ch = str.ToCharArray();
                isRec[0] = dfa_str(ch);
                isRec[1] = dfa_ass(ch);
                isRec[2] = dfa_ido(ch);
                if (!id.Contains(str))
                {
                    isRec[3] = dfa_id(ch);
                }
                else if (id.Contains(str))
                {
                    writeToFile(str, str, lineNum);
                    return;
                }
                isRec[4] = dfa_num(ch);
                isRec[5] = dfa_lop(ch);
                isRec[6] = dfa_cmp(ch);
                isRec[7] = dfa_op(ch);

                if (isRec[0])
                {
                    writeToFile("STR", str, lineNum);
                }
                else if (isRec[1])
                {
                    writeToFile("AS_OP", str, lineNum);
                }
                else if (isRec[2])
                {
                    writeToFile("ID_OP", str, lineNum);
                }
                else if (isRec[3])
                {
                    writeToFile("ID", str, lineNum);
                }
                else if (isRec[4])
                {
                    writeToFile("NUM", str, lineNum);
                }
                else if (isRec[5])
                {
                    writeToFile("LOG_OP", str, lineNum);

                }
                else if (isRec[6])
                {
                    writeToFile("CMP_OP", str, lineNum);
                }
                else if (isRec[7])
                {
                    writeToFile("OP", str, lineNum);
                }
                else
                {
                    writeToFile("LEXICAL_ERROR", str, lineNum);
                }
                int i = 0;
                while (i < isRec.Length)
                {
                    isRec[i] = false;
                    i++;
                }

            }
        }

    }
}
