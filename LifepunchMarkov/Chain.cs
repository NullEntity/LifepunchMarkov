﻿using System;
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
        public static readonly int ORDER = 1;

       // private List<Node> nodes = new List<Node>();
        private Dictionary<String[], Node> nodes = new Dictionary<String[], Node>();
        private List<Node> starts = new List<Node>();
        private List<String> messages = new List<String>();
        
        public void AddText(String s)
        {
            messages.Add(s);

            List<String> tokens = new List<String>(s.Split(' '));

            if (StringOrDefault(tokens.ElementAt(0)) == "")
                return; // abort if it doesn't have anything

            Node lastNode = null;
            String[] currentWord = new String[ORDER];
            for(int i = 0; i < tokens.Count - (ORDER - 1); i++)
            {
                for (int j = 0; j < ORDER; j++)
                    currentWord[j] = StringOrDefault(tokens.ElementAt(i + j));

                var currentNode = nodes.ContainsKey(currentWord) ? nodes[currentWord] : null;
                if (currentNode == null)
                {
                    currentNode = new Node(currentWord);
                    nodes.Add(currentWord, currentNode);
                }


                if (lastNode == null)
                    starts.Add(currentNode);
                else
                    AddPair(lastNode, currentNode);

                lastNode = new Node(currentNode.Value.Clone() as String[]);
            }
        }

        // point foo to bar
        public void AddPair(Node fooNode, Node barNode, int score = 0)
        {
            Link link = fooNode.Links.ContainsKey(barNode.Value) ? fooNode.Links[barNode.Value] : null;
            if (link == null)
            {
                link = new Link(fooNode, barNode);
                fooNode.Links.Add(barNode.Value, link);
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
