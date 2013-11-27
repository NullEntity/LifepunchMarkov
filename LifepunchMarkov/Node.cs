using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RandomItemExtension;

namespace LifepunchMarkov
{
    class Node
    {
        public String[] Value { get; private set; }
        //public HashSet<Link> Links { get; private set; }
        public Hashtable Links { get; private set; }

        public Node(String[] values)
        {
            this.Value = values;
            //this.Links = new HashSet<Link>();
            this.Links = new Hashtable();
        }

        public bool Equals(Node n)
        {
            return this.Value.SequenceEqual(n.Value);
        }

        public String BuildString()
        {
            return BuildString(0);
        }

        public String BuildString(int depth)
        {
            if (depth > 25)
                return Value[0];  // abort after too many recursions

            if (Links.Count == 0)
                return String.Join(" ", Value);

            var randomLink = ItemExtension.GetRandomItem<Link>(Links.Values.OfType<Link>().ToList(), x => x.Score);
            if (randomLink == null || randomLink.Target == null)
                return ""; // end the sentence

            return Value[0] + " " + randomLink.Target.BuildString(depth + 1);
        }
    }
}
