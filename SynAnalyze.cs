using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSCompiler
{
    class SynAnalyze
    {
        Token[] tkn;
        int pos = 0;
        public SynAnalyze(Token[] tkn) {
            this.tkn = tkn;
            analyze();
        }

        public void analyze() {
            dec_st();
        }

        public void dec_st() {
            while (pos < tkn.Length){
                if (tkn[pos].CP == "var")
                {
                    if (pos + 1 < tkn.Length)
                    {
                        pos++;
                        if (tkn[pos].CP == "ID")
                        {
                            if (pos + 1 < tkn.Length)
                            {
                                pos++;
                                init();
                                list();
                            }
                        }
                    }
                }
                else {
                    /* Error goes here*/
                    Console.WriteLine("Incorrect statement or missing a terminator at line: "+tkn[pos].LN);
                    break;
                }
                pos++;
            }
        }

        public void init() {
    
            if (pos < tkn.Length) {
                if (tkn[pos].CP == "AS_OP")
                {
                    if (pos + 1 < tkn.Length)
                    {
                        pos++;
                        init2();
                    } 
                }
                else {
                    return;
                }
            }
        }

        public void init2() {
            if (pos < tkn.Length) {
                if (tkn[pos].CP == "ID") {
                    if (pos + 1 < tkn.Length)
                    {
                        pos++;
                        init();
                    }
                }
                else if (tkn[pos].CP == "STR" || tkn[pos].CP == "NUM")
                {
                    if (pos + 1 < tkn.Length)
                    {
                        pos++;
                        return;
                    }
                }
            }
        }

        public void list() {
            if (pos < tkn.Length) {
                if (tkn[pos].CP == ";"){
                    if (pos + 1 < tkn.Length)
                    {
                        pos++;
                        analyze();
                    }
                }
                else if (tkn[pos].CP == ",")
                {
                    if (pos + 1 < tkn.Length)
                    {
                        pos++;
                        if (tkn[pos].CP == "ID")
                        {
                            if (pos + 1 < tkn.Length)
                            {
                                pos++;
                                init();
                                list();
                            }
                        }
                    }
                }
                else {
                    Console.WriteLine("Err at line: "+tkn[pos].LN);
                    return;
                }
            }
        }

    }
}
