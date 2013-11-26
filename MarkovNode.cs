using System;
using RandomItemExtension;

public class MarkovNode
{
    const var ORDER = 3;

    private String value;
    private int hits = 0;
    private MarkovNode[] children;
    private MarkovNode[] descendents;
    private MarkovNode root;

    // used for creating a starting point
    public MarkovNode()
    {
        this.value = null;
    }

    public MarkovNode(String value)
    {
        this.value = value;
    }

    public MarkovNode(String value, MarkovNode root)
    {
        this.root = root;
        MarkovNode(value);
    }

    public MarkovNode root
    {
        get { return root; }
    }

    public String Value
    {
        get { return value; }
    }

    public int Hits
    {
        get { return hits; }
    }

    public void AddHit()
    {
        hits++;
    }

    public void AddDescendent(MarkovNode n)
    {
        var current = root.descendents.SingleOrDefault(descendent => descendent.Value == n.Value);
        if (current == null)
            descendents.Add(n);
        else
            current.AddHit();
    }

    // adds a child node
    public void AddNode(MarkovNode node)
    {
        children.Add(node);
    }

    // returns a random child based on hits
    public MarkovNode GetRandomNode()
    {
        return children.GetRandomItem(x => x.Hits);
    }
}
