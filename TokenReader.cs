using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace JSCompiler
{
    class TokenReader{

        public string path;

        public TokenReader(string path)
        {
            this.path = path;
        }

        public Token[] readFile(){
            int counter = 0;
            string line;
            char[] chArr;
            StringBuilder sb = new StringBuilder();
            string cp = null,vp = null,ln = null;
            int sep = 0;

            try{
                System.IO.StreamReader file = new System.IO.StreamReader(path);
                int count = File.ReadLines(path).Count();     // Counting the number of lines in a file
                Token[] tokenArr = new Token[count];
                for (int i = 0; i < tokenArr.Length; i++)
                {
                    tokenArr[i] = new Token();
                }

                while ((line = file.ReadLine()) != null){     // Storing individual lines in an array.
                    chArr = line.ToCharArray();
                    int i = 0;
                        while(i<chArr.Length){
                            if ((chArr[i] != '`') && (i + 1 != chArr.Length))
                            {
                                sb.Append(chArr[i]);
                            }
                            else if(chArr[i] == '`'){
                                sep++;
                                if (sep == 1) {
                                    cp = sb.ToString();
                                    sb.Clear();
                                }
                                else if (sep == 2) {
                                    vp = sb.ToString();
                                    sb.Clear();
                                }
                            }
                            else if (i + 1 == chArr.Length)
                            {
                                sb.Append(chArr[i]);
                                ln = sb.ToString();
                                sb.Clear();
                            }
                            i++;
                        }
                    sep = 0;
                    tokenArr[counter].CP = cp;
                    tokenArr[counter].VP = vp;
                    tokenArr[counter].LN = ln;

                    counter++;
                }

                file.Close();
                return tokenArr;                               //returning an array

            }catch (Exception e){
                Console.WriteLine("The token file could not be read:");
                Console.WriteLine(e.Message);

            }
             Token[] tok =new Token[0];
             return tok;                    //returning empty array if file read fails!
        }

    }
}
