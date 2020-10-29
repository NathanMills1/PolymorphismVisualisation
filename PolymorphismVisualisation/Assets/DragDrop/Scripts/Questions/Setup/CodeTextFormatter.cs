using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeTextFormatter { 
    public static Color variableTypeColour { get; } = new Color32(204,120,50, 255);
    public static Color codeColour { get; } = new Color32(152, 118, 159, 255);
    public static Color objectColour { get; } = new Color32(86, 156, 214, 255);

    public static Color methodColour { get; } = new Color32(255, 255, 255, 255);

    public string formattedString { get; private set; }
    public CodeTextFormatter()
    {
        formattedString = "";
    }

    public CodeTextFormatter addCodeSection(Color colour, string text)
    {
        formattedString += "<color=#" + ColorUtility.ToHtmlStringRGB(colour) + ">" + text + "</color>";
        return this;
    }
}
