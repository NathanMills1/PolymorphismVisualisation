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
    public List<string> selectedMethods;
    public int generation;

    public int totalMethods;

    public Identity(Identity parent, string name, string[] methods, int numberOfMethodsToUse) : this(name, methods, numberOfMethodsToUse)
    {
        this.parent = parent;
        parent.addChild(this);

        this.totalMethods = parent.totalMethods + numberOfMethodsToUse;
    }

    public Identity(string name, string[] methods, int numberOfMethodsToUse)
    {
        this.name = name;
        this.methods = methods;
        children = new List<Identity>();

        selectedMethods = new List<string>();

        //select methods to use from list of methods
        for(int i = 0;i< numberOfMethodsToUse; i++)
        {
            int nextMethod = new System.Random().Next(methods.Length);
            while (selectedMethods.Contains(methods[nextMethod])){
                nextMethod = new System.Random().Next(methods.Length);
            }
            selectedMethods.Add(methods[nextMethod]);
        }

        this.totalMethods = numberOfMethodsToUse;
    }

    public void addChild(Identity child)
    {
        children.Add(child);
    }


}
