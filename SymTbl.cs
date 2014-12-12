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
        private int scope;
        public List<SymTbl> child = new List<SymTbl>();

        public string N
        {
            get { return name; }
            set { name = value; }
        }

        public string T
        {
            get { return type; }
            set { type = value; }
        }

        public int S
        {
            get { return scope; }
            set { scope = value; }
        }
    }
}
