using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifepunchMarkov
{
    class Link
    {
        public Node Parent { get; private set; }
        public Node Target { get; private set; }
        public int Score { get; private set; }

        public Link(Node parent, Node target, int score = 0)
        {
            this.Parent = parent;
            this.Target = target;
            this.Score = score;
        }

        public void Hit()
        {
            Score++;
        }

        public bool Equals(Link l)
        {
            return this.Parent == l.Parent
                && this.Target == l.Target
                && this.Score == l.Score;
        }
    }
}
