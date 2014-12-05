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
            while (pos+1 < tkn.Length)
            {
                stmnt();
                pos++;
            }
        }


        public void stmnt() {
            switch (tkn[pos].CP)
            {
                case "var":
                    dec_st();
                    break;
                case "while":
                    while_st();
                    break;
                case "do":
                    do_while();
                    break;
                case "if":
                    if_else();
                    break;
                case "function":
                    fn_def();
                    break;
                case "for":
                    for_stmnt();
                    break;
                case "ID":
                    assign();
                    break;
                case "ID_OP":
                    e();
                    break;
                case "switch":
                    switch_case();
                    break;
            }
        }

        /* =============================== SWITCH START ============================ */
        public void switch_case()
        {
            if (pos < tkn.Length)
            {
                if (pos + 1 < tkn.Length)
                {
                    pos++;
                    if (tkn[pos].CP == "(")
                    {
                        if (pos + 1 < tkn.Length)
                        {
                            pos++;
                            if (tkn[pos].CP != ")")
                            {
                                e();

                                if (pos + 1 < tkn.Length)
                                {
                                    if (tkn[pos].CP == ")")
                                    {
                                        if (pos + 1 < tkn.Length)
                                        {
                                            pos++;
                                            if (tkn[pos].CP == "{")
                                            {
                                                if (pos + 1 < tkn.Length)
                                                {
                                                    pos++;
                                                    if (tkn[pos].CP != "}")
                                                    {
                                                        s_body();
                                                    }
                                                    else {
                                                        return;
                                                    }
                                                }
                                            }
                                        }

                                    }
                                }

                            }else{
                                Console.WriteLine("Err at line: " + tkn[pos].LN);
                            }
                        }
                    }
                }
            }
        }

        public void s_body() {
            if (tkn[pos].CP == "case")
            {
                if (pos + 1 < tkn.Length)
                {
                    pos++;
                    init2();
                    if (tkn[pos].CP == ":")
                    {
                        if (pos + 1 < tkn.Length)
                        {
                            pos++;
                            code();
                        }
                    }
                    else
                    {
                        Console.WriteLine("Err at line: " + tkn[pos].LN);
                    }
                }
                else {
                    Console.WriteLine("Err at line: " + tkn[pos].LN);
                }
            }
            if (tkn[pos].CP == "default")
            {
                if (pos + 1 < tkn.Length)
                {
                    pos++;
                    if (tkn[pos].CP == ":")
                    {
                        if (pos + 1 < tkn.Length)
                        {
                            pos++;
                            code();
                        }
                    }
                    else
                    {
                        Console.WriteLine("Err at line: " + tkn[pos].LN);
                    }
                }
                else
                {
                    Console.WriteLine("Err at line: " + tkn[pos].LN);
                }
            }
        }

        public void code() {
            while (tkn[pos].CP != "}") {
                if (pos + 1 < tkn.Length)
                {
                    stmnt();
                }
            }
        }
        /* =============================== SWITCH END ============================== */

        /* =============================== ASSIGNMENT START ============================ */
        public void assign()
        {
            if (pos < tkn.Length)
            {
                if (pos + 1 < tkn.Length)
                {
                    pos++;
                    if ((tkn[pos].CP == "AS_OP"))
                    {
                        if (pos + 1 < tkn.Length)
                        {
                            pos++;
                            e();
                        }
                    }
                    else if (tkn[pos].CP == "(")
                    {
                        if (pos + 1 < tkn.Length)
                        {
                            pos++;
                            p_l();
                        }
                    }
                    else if ((tkn[pos].CP == "ID_OP"))
                    {
                        pos--;
                        e();
                    }
                    else {
                        Console.WriteLine("Err at line: " + tkn[pos].LN);
                    }
                }
            }
        }
        /* =============================== ASSIGNMENT END ============================== */


        /* =============================== FOR START ============================ */
        public void for_stmnt() {
            if (pos < tkn.Length)
            {
                if (pos + 1 < tkn.Length)
                {
                    pos++;
                    if (tkn[pos].CP == "(")
                    {
                        if (pos + 1 < tkn.Length)
                        {
                            pos++;
                            dec_st();

                            if (tkn[pos].CP == "ID")
                            {
                                oe();
                                if (pos + 1 < tkn.Length)
                                {
                                    pos++;
                                    if (tkn[pos].CP == ";")
                                    {
                                        if (pos + 1 < tkn.Length)
                                        {
                                            pos++;
                                            re();
                                            if (pos + 1 < tkn.Length)
                                            {
                                                pos++;
                                                if (tkn[pos].CP == ")")
                                                {
                                                    if (pos + 1 < tkn.Length)
                                                    {
                                                        pos++;
                                                        if (tkn[pos].CP == "{")
                                                        {
                                                            if (pos + 1 < tkn.Length)
                                                            {
                                                                pos++;
                                                                if (tkn[pos].CP == "}")
                                                                {
                                                                    if (pos + 1 < tkn.Length)
                                                                    {
                                                                        pos++;

                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    while (tkn[pos].CP != "}" && pos + 1 < tkn.Length)
                                                                    {
                                                                        body();
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                Console.WriteLine("Unexpected End of file at line: " + tkn[pos].LN);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            Console.WriteLine("Err at line: " + tkn[pos].LN);
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    Console.WriteLine("Err at line: " + tkn[pos].LN);
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Err at line: " + tkn[pos].LN);
                                    }
                                }
                            }
                            else
                            {
                                Console.WriteLine("Err at line: " + tkn[pos].LN);
                            }
                        }
                       
                    }
                }
            }
        }
        /* =============================== FOR END ============================== */

        /* =============================== IF ELSE START ============================ */
        public void if_else() {
            if (pos < tkn.Length)
            {
                if (pos + 1 < tkn.Length)
                {
                    pos++;
                    if (tkn[pos].CP == "(")
                    {
                        if (pos + 1 < tkn.Length)
                        {
                            pos++;
                            if (tkn[pos].CP != ")")
                            {
                                oe();

                                if (tkn[pos].CP == ")")
                                {
                                    if (pos + 1 < tkn.Length)
                                    {
                                        pos++;
                                        if (tkn[pos].CP == "{")
                                        {
                                            if (pos + 1 < tkn.Length)
                                            {
                                                pos++;
                                                if (tkn[pos].CP == "}")
                                                {
                                                    if (pos + 1 < tkn.Length)
                                                    {
                                                        pos++;
                                                        if (tkn[pos].CP == "else")
                                                        {
                                                            else_if();
                                                            o_else();
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    while (tkn[pos].CP != "}" && pos + 1 < tkn.Length)
                                                    {
                                                        body();
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                Console.WriteLine("Unexpected End of file at line: " + tkn[pos].LN);
                                            }
                                        }
                                        else {
                                            Console.WriteLine("Err at line: " + tkn[pos].LN);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                Console.WriteLine("Unexpected ) at line: " + tkn[pos].LN);
                            }

                        }
                        else {
                            Console.WriteLine("Err at line: " + tkn[pos].LN);
                        }
                    }
                    else {
                        Console.WriteLine("Err at line: " + tkn[pos].LN);
                    }
                }
            }
        }

        public void o_else() {
            if (pos < tkn.Length && tkn[pos].CP == "else")
            {
                if (pos + 1 < tkn.Length)
                {
                    pos++;
                    body();
                }
            }
        }

        public void else_if() {
            if (pos < tkn.Length && tkn[pos].CP == "else")
            {
                if (pos + 1 < tkn.Length)
                {
                    pos++;
                    if (tkn[pos].CP == "if") {
                        if_else();
                    }
                }
            }
        }
        /* =============================== IF ELSE END =============================== */


        /* =============================== DO WHILE START ============================ */
        public void do_while() {
            if (pos < tkn.Length)
            {
                if (pos + 1 < tkn.Length)
                {
                    pos++;
                    if (tkn[pos].CP == "{")
                    {
                        if (pos + 1 < tkn.Length)
                        {
                            pos++;
                            if (tkn[pos].CP == "}")
                            {
                                if (pos + 1 < tkn.Length)
                                {
                                    pos++;
                                }
                            }
                            else
                            {
                                while (tkn[pos].CP != "}" && pos + 1 < tkn.Length)
                                {
                                    body();
                                }
                                if (pos + 1 < tkn.Length)
                                {
                                    pos++;
                                    if (tkn[pos].CP == "while")
                                    {
                                        if (pos + 1 < tkn.Length)
                                        {
                                            pos++;
                                            oe();
                                        }
                                    }
                                    else {
                                        Console.WriteLine("Err at line: " + tkn[pos].LN);
                                    }

                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("Unexpected End of file at line: " + tkn[pos].LN);
                        }
                    }
                    else {
                        Console.WriteLine("Err at line: " + tkn[pos].LN);
                    }
                }
            }
        }

        /* =============================== DO WHILE END ============================== */


        /* =============================== FUNCTION START ============================ */
        public void fn_def() {
            if (pos < tkn.Length)
            {
                if (pos + 1 < tkn.Length)
                {
                    pos++;
                    if (tkn[pos].CP == "ID") {
                        if (pos + 1 < tkn.Length)
                        {
                            pos++;
                            if (tkn[pos].CP == "(")
                            {
                                if (pos + 1 < tkn.Length)
                                {
                                    pos++;
                                    param();
                                    if (tkn[pos].CP == ")")
                                    {
                                        if (pos + 1 < tkn.Length)
                                        {
                                            pos++;
                                            if (tkn[pos].CP == "{")
                                            {
                                                if (pos + 1 < tkn.Length)
                                                {
                                                    pos++;
                                                    if (tkn[pos].CP == "}")
                                                    {
                                                        if (pos + 1 < tkn.Length)
                                                        {
                                                            pos++;

                                                        }
                                                    }
                                                    else
                                                    {
                                                        while (tkn[pos].CP != "}" && pos + 1 < tkn.Length)
                                                        {
                                                            body();
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    Console.WriteLine("Unexpected End of file at line: " + tkn[pos].LN);
                                                }
                                            }
                                            else {
                                                Console.WriteLine("Err at line: " + tkn[pos].LN);
                                            }
                                        }
                                    }
                                    else {
                                        Console.WriteLine("Err at line: " + tkn[pos].LN);
                                    }

                                }
                            }
                            else {
                                Console.WriteLine("Err at line: " + tkn[pos].LN);
                            }
                        }
                    }
                }
            }
        }

        public void param() { 
            if(tkn[pos].CP == "ID" ){
                if (pos + 1 < tkn.Length) {
                    pos++;
                    if (tkn[pos].CP == ",")
                    {
                        if (pos + 1 < tkn.Length)
                        {
                            pos++;
                            param();
                        }
                    }
                    else if (tkn[pos].CP == "ID")
                    {
                        Console.WriteLine("Invalid Parameter Declaration at Line: " + tkn[pos].LN);
                    }
                }
            }
            else if ((tkn[pos].CP == ")"))
            {
                return;
            }
            else {
                Console.WriteLine("Invalid Parameter Declaration at Line: " + tkn[pos].LN);
            }
        }
        /* =============================== FUNCTION END ============================== */


        /* =============================== WHILE STATEMENT START ============================ */

        public void while_st() {
            if (pos < tkn.Length)
            {
                if (tkn[pos].CP == "while")
                {
                    if (pos + 1 < tkn.Length)
                    {
                        pos++;
                        if (tkn[pos].CP == "(")
                        {
                            if (pos + 1 < tkn.Length)
                            {
                                pos++;
                                oe();
                                if (tkn[pos].CP == ")")
                                {
                                    if (pos + 1 < tkn.Length)
                                    {
                                        pos++;
                                        if (tkn[pos].CP == "{")
                                        {
                                            if (pos + 1 < tkn.Length)
                                            {
                                                pos++;
                                                if (tkn[pos].CP == "}")
                                                {
                                                    if (pos + 1 < tkn.Length)
                                                    {
                                                        pos++;

                                                    }
                                                }
                                                else
                                                {
                                                    while (tkn[pos].CP != "}" && pos + 1 < tkn.Length)
                                                    {
                                                        body();
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                Console.WriteLine("Unexpected End of file at line: " + tkn[pos].LN);
                                            }
                                        }
                                        else
                                        {
                                            Console.WriteLine("Err at line: " + tkn[pos].LN);
                                        }
                                    }
                                    else {
                                        Console.WriteLine("Err at line: " + tkn[pos].LN);
                                    }
                                }

                            }
                        }
                        else {
                            Console.WriteLine("Err at line: " + tkn[pos].LN);
                        }
                    }
                }
            }
        }

        public void body() {
            if (pos < tkn.Length) {
                stmnt();
                if (tkn[pos].CP != "}")
                {
                    oe();
                }
            }
        }

        /* =============================== WHILE STATEMENT START ============================ */
        /* ====================DECLARATION STATEMENT ANALYZER START======================== */

        public void dec_st() {
            if(pos < tkn.Length){
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
                        else {
                            if (pos + 1 < tkn.Length)
                            {
                                pos++;
                                Console.WriteLine("Invalid idientifier on line: " + tkn[pos].LN);
                            }
                        }
                    }
                }
                else {
                    /* Error goes here*/
                    Console.WriteLine("Incorrect statement or missing a terminator at line: "+tkn[pos].LN);
                }
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
                else {
                    Console.WriteLine("Err at line: " + tkn[pos].LN);
                }
            }
        }

        public void list() {
            if (pos < tkn.Length) {
                if (tkn[pos].CP == ";"){
                    if (pos + 1 < tkn.Length)
                    {
                        pos++;
                        return;
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
                    pos--;
                    return;
                }
            }
        }

        /* ====================DECLARATION STATEMENT ANALYZER END======================== */

        /* ==============================EXPRESSION START=================================*/

        public void oe() { ae(); oe_(); }
        public void oe_() {
            if (tkn[pos].VP == "||")
            {
                if(pos + 1 < tkn.Length){
                    pos++;
                    ae();
                    oe_();
                }
                
            }
            else {return;}
        }
        public void ae() { re(); ae_(); }
        public void ae_() {
            if (tkn[pos].VP == "&&")
            {
                if (pos + 1 < tkn.Length)
                {
                    pos++;
                    re();
                    ae_();
                }
            }
            else {return;}
        }
        public void re() {e(); re_();}
        public void re_() {
            if (tkn[pos].CP == "CMP_OP")
            {
                if (pos + 1 < tkn.Length)
                {
                    pos++;
                    e();
                    re_();
                }
            }
            else { return; }
        }
        public void e() { t(); e_(); }
        public void e_() {
            if (tkn[pos].VP == "+" || tkn[pos].VP == "-")
            {
                if (pos + 1 < tkn.Length)
                {
                    pos++;
                    t();
                    e_();
                }
            }
            else { return; }
        }
        public void t() { f(); t_(); }
        public void t_() {
            if (tkn[pos].VP == "*" || tkn[pos].VP == "/" || tkn[pos].VP == "%")
            {
                if (pos + 1 < tkn.Length)
                {
                    pos++;
                    f();
                    t_();
                }
            }
            else { return; }
        }
        public void f() {
            if (tkn[pos].CP == "ID" || tkn[pos].CP == "STR" || tkn[pos].CP == "NUM")
            {
                if (tkn[pos].CP == "ID")
                {
                    if (pos + 1 < tkn.Length)
                    {
                        pos++;
                        if (tkn[pos].CP == "ID_OP")
                        {
                            f_();
                        }
                        else {
                            return;
                        }
                    }
                }
                else {
                    if (pos + 1 < tkn.Length)
                    {
                        pos++;
                        return;
                    }
                }

            }
            else if (tkn[pos].CP == "ID_OP")
            {
                if (pos + 1 < tkn.Length)
                {
                    pos++;
                    if (tkn[pos].CP == "ID")
                    {
                        if (pos + 1 < tkn.Length)
                        {
                            pos++;
                            if (tkn[pos].CP == ";")
                            {
                                if (pos + 1 < tkn.Length)
                                {
                                    pos++;
                                    return;
                                }
                            }
                        }
                    }
                    else {
                        Console.WriteLine("Err at line: " + tkn[pos].LN);
                    }
                }
            }
            else if (tkn[pos].VP == "!")
            {
                if (pos + 1 < tkn.Length)
                {
                    pos++;
                    f();
                }
            }
            else
            {
                Console.WriteLine("Err at line: " + tkn[pos].LN);
                return;
            }
        }
        public void f_() {
            if (tkn[pos].CP == "(") {
                if (pos + 1 < tkn.Length)
                {
                    pos++;
                    if (tkn[pos].CP == ")")
                    {
                        if (pos + 1 < tkn.Length)
                        {
                            pos++;
                            if (tkn[pos].CP == ";")
                            {
                                return;
                            }
                            else {
                                Console.WriteLine("Err: Missing Terminator At Line: "+tkn[pos].LN);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Err: Unexpected End Of file At Line: " + tkn[pos].LN);
                        }   
                    }
                    else{
                        p_l();
                    }
                }
            }
            else if(tkn[pos].CP == "ID_OP"){
                if (pos + 1 < tkn.Length)
                {
                    pos++;
                    if (tkn[pos].CP == ";")
                    {
                        if (pos + 1 < tkn.Length)
                        {
                            pos++;
                            return;
                        }
                    }
                }
            }
        }

        public void p_l() {
            if (tkn[pos].CP == "ID" || tkn[pos].CP == "STR" || tkn[pos].CP == "NUM" || tkn[pos].CP == ",")
            {
                if (tkn[pos].CP == "ID" || tkn[pos].CP == "STR" || tkn[pos].CP == "NUM")
                {
                    if (pos + 1 < tkn.Length)
                    {
                        pos++;
                        if (tkn[pos].CP == ",")
                        {
                            if (pos + 1 < tkn.Length)
                            {
                                pos++;
                                p_l();
                            }
                        }
                        else if (tkn[pos].CP == ")")
                        {
                            if (pos + 1 < tkn.Length)
                            {
                                pos++;
                                if (tkn[pos].CP == ";")
                                {
                                    return;
                                }
                                else
                                {
                                    Console.WriteLine("Err: Missing Terminator at line: " + tkn[pos].LN);
                                }
                            }
                        }
                    }
                }
                else {
                    Console.WriteLine("ERR: Invalid Argument At Line: " + tkn[pos].LN);
                }
            }
            else {
                Console.WriteLine("ERR: Invalid Argument At Line: "+tkn[pos].LN);
            }
                            
        }
        /* ================================EXPRESSION END=================================*/

    }
}
