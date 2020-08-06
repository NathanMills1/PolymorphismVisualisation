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
    public Identity parent { get; private set; }
    public List<Identity> children { get; private set; }
    public string name { get; private set; }
    public List<string> selectedMethods;
    public Dictionary<string, string> fieldsAndValues { get; private set; }
    public int totalFields { get; private set; }

    private static System.Random randomGen = new System.Random();

    public Identity(Identity parent, string name, string fields, int numberOfFieldsToUse) : this(name, fields, numberOfFieldsToUse)
    {
        this.parent = parent;
        parent.addChild(this);

        this.totalFields = parent.totalFields + numberOfFieldsToUse;
    }

    public Identity(string name, string fields, int numberOfFieldsToUse)
    {
        this.name = name;
        children = new List<Identity>();

        selectFieldsAndValues(fields, numberOfFieldsToUse);

        this.totalFields = numberOfFieldsToUse;
    }

    private void selectFieldsAndValues(string fieldsString, int numberOfFieldsToUse)
    {
        string[] fields = fieldsString.Split(',');
        fieldsAndValues = new Dictionary<string, string>();

        for(int i = 0; i < numberOfFieldsToUse; i++)
        {
            string field = fields[i];
            string[] fieldAndValueRange = field.Split(':');
            string fieldName = fieldAndValueRange[0];
            string combinedValues = fieldAndValueRange[1];

            //selectFromOptions
            if (combinedValues.Contains('/'))
            {
                string[] potentialValues = combinedValues.Split('/');
                int selectedPos = randomGen.Next(potentialValues.Length);
                fieldsAndValues.Add(fieldName, potentialValues[selectedPos]);
            }
            //selectFromRange
            else
            {
                string[] minAndMax = combinedValues.Split('-');
                int value = randomGen.Next(int.Parse(minAndMax[0]), int.Parse(minAndMax[1]) + 1);
                fieldsAndValues.Add(fieldName, value.ToString());
            }
        }
    }

    public void addChild(Identity child)
    {
        children.Add(child);
    }


}
