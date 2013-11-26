using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifepunchMarkov
{
    class Nodetable : Hashtable
    {
        public int GetHashCode(Node key)
        {
            return key.Value.GetHashCode();
        }
    }
}
