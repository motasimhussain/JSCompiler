﻿using System;
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
        List<SymTbl> symArr = new List<SymTbl>();
        SymTbl symTbl;
        int scope = 0;
        Stack<int> stack = new Stack<int>();

        

        public SynAnalyze(Token[] tkn) {
            this.tkn = tkn;
            stack.Push(scope);
            analyze();
            
        }

        public void analyze() {
            
            while (pos+1 < tkn.Length)
            {
                stmnt();

            }
        }


        public void stmnt() {
            switch (tkn[pos].CP)
            {
                case "var":
                    dec_st();
                    break;
                case "class":
                    cl();
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
                case "else":
                    else_if();
                    o_else();
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

        /* =============================== CLASS START ============================ */


        public void cl() {
            if (pos + 1 < tkn.Length)
            {
                pos++;
                if (tkn[pos].CP == "ID")
                {
                    body_c();
                }
            }
        }

        public void body_c() {
            if (pos + 1 < tkn.Length)
            {
                pos++;
                if (tkn[pos].CP == "{")
                {
                    scope++;
                    if (pos + 1 < tkn.Length)
                    {
                        pos++;
                        attr();
                        methods();
                        if (tkn[pos].CP == "}")
                        {
                            stack.Pop();
                            if (pos + 1 < tkn.Length)
                            {
                                pos++;
                                return;
                            }
                        }
                    }
                }
            }
        }

        public void attr() {
            dec_st();
        }
        public void methods() {
            fn_def();
            if (pos + 1 < tkn.Length && tkn[pos].CP != "}")
            {
                pos++;
                attr();
            }
        }


        /* =============================== CLASS END ============================== */


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
            else if (tkn[pos].CP == "default")
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
                }else if(tkn[pos].CP == "break"){
                    if (pos + 1 < tkn.Length)
                    {
                        pos++;
                        if (tkn[pos].CP == ";")
                        {
                            if (pos + 1 < tkn.Length)
                            {
                                pos++;
                                break;
                            }
                        }
                    }
                }
            }
        }
        /* =============================== SWITCH END ============================== */

        /* =============================== ASSIGNMENT START ============================ */
        public void assign()
        {
            if (pos < tkn.Length)
            {
                if(!lookup(tkn[pos])){
                    Console.WriteLine("Undeclared variable at: " + tkn[pos].LN);
                }

                if (pos + 1 < tkn.Length)
                {
                    if (tkn[pos + 1].CP == "OP")
                    {
                        e();
                    }
                    pos++;
                    if ((tkn[pos].CP == "AS_OP"))
                    {
                        if (pos + 1 < tkn.Length)
                        {
                            pos++;
                            oe();
                        }
                    }
                    else if (tkn[pos].CP == "(")
                    {
                        if (pos + 1 < tkn.Length)
                        {
                            pos++;
                            p_l();
                            
                                if (tkn[pos].CP == ";") {
                                    pos++;
                                }
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
                                re();
                                if (tkn[pos].CP == ")")
                                {
                                    if (pos + 1 < tkn.Length)
                                    {
                                        pos++;
                                        body();
                                    }
                                    if (pos + 1 < tkn.Length)
                                    {
                                        pos++;
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
                                            scope++;
                                            stack.Push(scope);
                                            if (pos + 1 < tkn.Length)
                                            {
                                                pos++;
                                                if (tkn[pos].CP == "}")
                                                {
                                                    stack.Pop();
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
                                                        if_body();
                                                        if (pos + 1 < tkn.Length && tkn[pos].CP == "}")
                                                        {
                                                            stack.Pop();
                                                            pos++;
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

        public void if_body() {
                stmnt();
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
                    if (tkn[pos].CP == "if")
                    {
                        if_else();
                    }
                    else {
                        pos--;
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
                        if (!lookup(tkn[pos]))
                        {
                            insert(tkn[pos]);
                        }
                        else {
                            Console.WriteLine("Function Redeclaration at line: " + tkn[pos].LN);
                        }
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
                                        insert_param(p_count);
                                        p_count = 0;
                                        if (pos + 1 < tkn.Length)
                                        {
                                            pos++;
                                            body();
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

        int p_count = 0;

        public void param() {
            
            if(tkn[pos].CP == "ID" ){
                p_count++;
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
                insert_param(p_count);
                p_count = 0;
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
                                        body();
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
            if (tkn[pos].CP == "{")
            {
                scope++;
                stack.Push(scope);
                if (pos + 1 < tkn.Length)
                {
                    pos++;
                    if (tkn[pos].CP == "}")
                    {
                        stack.Pop();
                        if (pos + 1 < tkn.Length)
                        {
                            pos++;

                        }
                    }
                    else
                    {
                        while (tkn[pos].CP != "}" && pos + 1 < tkn.Length)
                        {
                            if (pos < tkn.Length) {
                                stmnt();
                            }
                            if (pos + 1 < tkn.Length)
                            {
                                pos++;

                            }
                        }
                        stack.Pop();
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
                            if (!lookup(tkn[pos]))
                            {
                                insert(tkn[pos]);
                            }
                            else {
                                Console.WriteLine("Redeclaration at: " + tkn[pos].LN);
                            }
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
                    //Console.WriteLine("Incorrect statement or missing a terminator at line: "+tkn[pos].LN);
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
                if (tkn[pos].CP == "ID")
                {
                    if (pos + 1 < tkn.Length)
                    {
                        if (tkn[pos + 1].CP == "OP")
                        {
                            e();
                        }
                        else
                        {
                            if (lookup(tkn[pos]))
                            {
                                if (pos + 1 < tkn.Length)
                                {
                                    if (!compat(tkn[pos], tkn[pos - 2], tkn[pos - 1]) && tkn[pos - 3].CP != "var")
                                    {
                                        Console.WriteLine("Incompatible Types at: " + tkn[pos].LN);
                                    }
                                }
                            }
                            else
                            {
                                Console.WriteLine("Undeclared variable at: " + tkn[pos].LN);
                            }

                            if (pos + 1 < tkn.Length)
                            {
                                pos++;
                                init();
                            }
                        }
                    }
                }
                else if (tkn[pos].CP == "STR" || tkn[pos].CP == "NUM")
                {
                    insertType(tkn[pos]);

                    if (!compat(tkn[pos], tkn[pos - 2], tkn[pos - 1]) && tkn[pos - 3].CP != "var")
                    {
                        Console.WriteLine("Incompatible Types at: " + tkn[pos].LN);
                    }

                    if (pos + 1 < tkn.Length)
                    {
                        pos++;
                        return;
                    }
                }
                else
                {
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
                            if (!lookup(tkn[pos]))
                            {
                                insert(tkn[pos]);
                            }
                            else
                            {
                                Console.WriteLine("Redeclaration at: " + tkn[pos].LN);
                            }

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
                        if ((tkn[pos - 2].CP == "ID" || tkn[pos - 2].CP == "STR" || tkn[pos - 2].CP == "NUM")&&tkn[pos+1].CP!="ID_OP")
                        {
                            if (lookup(tkn[pos]))
                            {
                                if (!compat(tkn[pos], tkn[pos - 2], tkn[pos - 1]) && tkn[pos - 3].CP != "var")
                                {
                                    Console.WriteLine("Incompatible Types at: " + tkn[pos].LN);
                                }
                            }
                            else
                            {
                                Console.WriteLine("Undeclared variable at: " + tkn[pos].LN);
                            }
                        }
                        else if (tkn[pos].CP == "ID" && tkn[pos + 1].CP == "CMP_OP")
                        {
                            if (!lookup(tkn[pos]))
                            {
                                Console.WriteLine("Undeclared variable at: " + tkn[pos].LN);
                            }
                        }
                    }

                    if (pos + 1 < tkn.Length)
                    {
                        pos++;
                        if (tkn[pos].CP == "ID_OP")
                        {
                            f_();
                        }
                        else if (tkn[pos].CP == "AS_OP")
                        {
                            if (pos + 1 < tkn.Length)
                            {
                                pos++;
                                e();
                                if (tkn[pos].CP == ";")
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
                        else {
                            return;
                        }
                    }
                }
                else {
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
                    }else if(tkn[pos].CP == ")"){
                        return;
                    }
                    else {
                        Console.WriteLine("Err: Missing Terminator At Line: " + tkn[pos].LN);
                    }
                }
            }
        }

        public void p_l() {
            if (tkn[pos].CP == "ID" || tkn[pos].CP == "STR" || tkn[pos].CP == "NUM" || tkn[pos].CP == "," || tkn[pos].CP == ")")
            {
                if (tkn[pos].CP == "ID" || tkn[pos].CP == "STR" || tkn[pos].CP == "NUM")
                {
                    p_count++;
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
                            lookParam(tkn[pos],p_count);
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
                else if (tkn[pos].CP == ")")
                {
                    if (pos + 1 < tkn.Length)
                    {
                        pos++;
                        if (tkn[pos].CP == ";")
                        {
                            if (pos + 1 < tkn.Length)
                            {
                                pos++;
                            }
                            return;
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

        public void inc_pos() {
            if (pos + 1 < tkn.Length)
            {
                pos++;
            }
        }


        public void insert(Token tkn)
        {
            symTbl = new SymTbl();
            symArr.Add(symTbl);

            int loc = symArr.Count - 1;
            if (loc > -1)
            {
                symArr[loc].N = tkn.VP;
                symArr[loc].S = stack.First();
            }
            else {
                symArr[0].N = tkn.VP;
                symArr[loc].S = stack.First();
            }
        }

        public void insertType(Token tkn)
        {
            int loc = symArr.Count - 1;
            if (tkn.CP == "NUM" || tkn.CP == "STR")
            {
                if (loc > -1)
                {
                    symArr[loc].T = tkn.CP;
                }
                else
                {
                    symArr[0].T = tkn.CP;
                }
            }
        }

        public bool lookup(Token tkn)
        {
            int fl = 0;
            for (int i = 0; i < symArr.Count; i++)
            {
                if (symArr[i].N == tkn.VP && (symArr[i].S == stack.First()||symArr[i].S == 0))
                {
                    fl++;
                }
            }
            if (fl >= 1)
            {
                return true;
            }
            else {
                return false;
            }
        }

        public bool compat(Token tkn1,Token tkn2,Token op)
        {
            string a="",b="";

            for (int i = 0; i < symArr.Count; i++)
            {
                if (symArr[i].N == tkn1.VP && (symArr[i].S == stack.First() ||symArr[i].S == 0))
                {
                    a = symArr[i].T; 
                }
                if (symArr[i].N == tkn2.VP && (symArr[i].S == stack.First() || symArr[i].S == 0))
                {
                    b = symArr[i].T;
                }
            }

            switch (op.CP) { 
                case "CMP_OP":
                    if (a == b)
                    {
                        return true;
                    }
                    else {
                        return false;
                    }
                    break;
                case "AS_OP":
                    if (a == b)
                    {
                        return true;
                    }
                    else {
                        return false;
                    }
                    break;
                case "OP":
                    if (a == b && a=="STR" && op.VP == "+")
                    {
                        return true;
                    }
                    else if (a == b && a != "STR")
                    {
                        return true;
                    }
                    else {
                        return false;
                    }
                    break;
                default:
                    return false;
            }
        }

        public bool compat(Token tkn,Token op)
        {
            string a = "";

            for (int i = 0; i < symArr.Count; i++)
            {
                if (symArr[i].N == tkn.VP && (symArr[i].S == stack.First() || symArr[i].S == 0))
                {
                    a = symArr[i].T;
                }
            }

            switch (op.CP)
            {
                case "ID_OP":
                    if (a == "NUM")
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                    break;
                default:
                    return false;
            }
        }

        public void insert_param(int param) {
            int loc = symArr.Count - 1;
                if (loc > -1)
                {
                    symArr[loc].T = "param->"+param;
                }
                else
                {
                    symArr[0].T = "param->" + param;
                }
        }

    }


}
