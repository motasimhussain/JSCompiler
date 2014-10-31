using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSCompiler
{
    class Token{
        private string cp;
        private string vp;
        private string ln;

        public string CP { 
            get{return cp;}
            set { cp = value; } 
        }

        public string VP
        {
            get { return vp; }
            set { vp = value; }
        }

        public string LN
        {
            get { return ln; }
            set { ln = value; }
        }
    }
}
