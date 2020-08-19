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
    public Identity parent { get; set; }
    public List<Identity> children { get; private set; }
    public string name { get; private set; }
    public string[] methods { get; set; }
    public string field { get; private set; }
    public string fieldValues { get; private set; }

    private static System.Random randomGen = new System.Random();

    public Identity(Identity parent, string name, string fieldsString, string methodsString) : this(name, fieldsString, methodsString)
    {
        this.parent = parent;
        parent.addChild(this);
    }

    public Identity(string name, string fieldsString, string methodsString)
    {
        int numberOfMethods = 2;
        this.name = name;
        children = new List<Identity>();

        selectFieldAndValues(fieldsString);
        selectMethods(methodsString, numberOfMethods);
    }

    private void selectMethods(string methodString, int numberOfMethods)
    {
        this.methods = new string[numberOfMethods];
        string[] allMethods = methodString.Split(',');
        for(int i = 0; i<numberOfMethods; i++)
        {
            int methodPos = randomGen.Next(allMethods.Length);
            while (this.methods.Contains(allMethods[methodPos]))
            {
                methodPos = randomGen.Next(allMethods.Length);
            }
            methods[i] = allMethods[methodPos];
        }

    }

    private void selectFieldAndValues(string fieldsString)
    {
        string[] fields = fieldsString.Split(',');
        int selectedFieldPos = randomGen.Next(fields.Length);
        string[] selectedField = fields[selectedFieldPos].Split(':');
        this.field = selectedField[0];
        this.fieldValues = selectedField[1];
    }

    public string generateFieldValue()
    {
        //selectFromOptions
        if (fieldValues.Contains('/'))
        {
            string[] potentialValues = fieldValues.Split('/');
            int selectedPos = randomGen.Next(potentialValues.Length);
            return potentialValues[selectedPos];
        }
        //selectFromRange
        else
        {
            string[] minAndMax = fieldValues.Split('-');
            int value = randomGen.Next(int.Parse(minAndMax[0]), int.Parse(minAndMax[1]) + 1);
            return value.ToString();
        }
    }

    public void addChild(Identity child)
    {
        children.Add(child);
    }


}
