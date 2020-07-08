using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Permissions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Identity
{
    public Identity parent;
    public List<Identity> children;
    public string name;
    public string[] methods;
    public int generation;

    public Identity(Identity parent, string name, string[] methods) : this(name, methods)
    {
        this.parent = parent;
        parent.addChild(this);
    }

    public Identity(string name, string[] methods)
    {
        this.name = name;
        this.methods = methods;
        children = new List<Identity>();
    }

    public void addChild(Identity child)
    {
        children.Add(child);
    }


}
