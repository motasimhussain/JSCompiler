using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSCompiler
{
    class SymTbl
    {
        private string name;
        private string type;
        private string scope;

        public string CP
        {
            get { return name; }
            set { name = value; }
        }

        public string VP
        {
            get { return type; }
            set { type = value; }
        }

        public string LN
        {
            get { return scope; }
            set { scope = value; }
        }
    }
}
