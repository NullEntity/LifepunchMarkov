using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RandomItemExtension;

namespace LifepunchMarkov
{
    class Chain
    {
        public static readonly int ORDER = 3;

       // private List<Node> nodes = new List<Node>();
        private Hashtable nodes = new Hashtable(StructuralComparisons.StructuralEqualityComparer);
        private List<Node> starts = new List<Node>();
        private List<String> messages = new List<String>();
        
        public void AddText(String s)
        {
            messages.Add(s);

            List<String> tokens = new List<String>(s.Split(' '));

            if (StringOrDefault(tokens.ElementAt(0)) == "")
                return; // abort if it doesn't have anything

            String[] lastWord = null;
            for(int i = 0; i < tokens.Count - (ORDER - 1); i++)
            {
                String[] currentWord = new String[ORDER];
                for (int j = 0; j < ORDER; j++)
                    currentWord[j] = StringOrDefault(tokens.ElementAt(i + j));

                if (lastWord != null)
                    AddPair(lastWord, currentWord);
                else
                {
                    Node node = (Node) nodes[currentWord];
                    if (node == null)
                    {
                        node = new Node(currentWord);
                        starts.Add(node);
                        nodes.Add(currentWord, node);
                    }
                }

                lastWord = currentWord;
            }
        }

        public void AddPair(String[] foo, String[] bar, int score = 0)
        {
            // find if nodes[foo] exists and initialize it if it doesn't
            Node fooNode = (Node) nodes[foo];
            HashSet<Link> fooLinks;
            if (fooNode != null)
                fooLinks = fooNode.Links;
            else
            {
                fooNode = new Node(foo);
                nodes.Add(fooNode.Value, fooNode);
                fooLinks = fooNode.Links;
            }

            // find if nodes[foo] exists and initialize it if it doesn't
            var barNode = (Node) nodes[bar];
            if (barNode == null)
            {
                barNode = new Node(bar);
                nodes.Add(barNode.Value, barNode);
            }

            var link = barNode.Links.FirstOrDefault(x => x.Target != null && x.Target.Value != null && x.Target.Value.SequenceEqual(bar));
            //var link = barNode.Links[barNode];
            if (link == null)
            {
                link = new Link(fooNode, barNode);
                fooLinks.Add(link);
            }
            link.Hit();
        }

        private String StringOrDefault(String s)
        {
            return s == null ? "" : s;
        }

        public String BuildString()
        {
            var rng = new Random();
            var start = starts.ElementAt(rng.Next(starts.Count));
            String message = start.BuildString();
            if (messages.Contains(message))
                message = "*EXACT MATCH!!!* " + message;

            return message;
        }
    }
}
