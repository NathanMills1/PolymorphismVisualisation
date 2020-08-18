using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicQuestionFactory : QuestionFactory
{

    public BasicQuestionFactory(int[] generationWeighting)
    {
        this.entities = InheritanceGenerator.selectedEntitiesByGeneration;
        this.codeText = getCodeText();
        this.questionText = getQuestionText();
        this.generationWeighting = generationWeighting;
    }

    public override Question getQuestion()
    {

        Entity selectedEntity = generateVariable(generationWeighting);

        return new BasicQuestion(codeText, questionText, 1, selectedEntity);
    }
    protected override string getQuestionText()
    {
        return "Place a screen and object to make this statement a valid call";
    }

    protected override string getCodeText()
    {
        switch (codeLanguage)
        {
            case Language.CPlusPlus:
                return new CodeTextFormatter().addCodeSection(CodeTextFormatter.variableTypeColour, "VariableType ")
                    .addCodeSection(CodeTextFormatter.codeColour, "*VariableName = new ")
                    .addCodeSection(CodeTextFormatter.objectColour, "ObjectType();")
                    .formattedString;
            case Language.Java:
                return new CodeTextFormatter().addCodeSection(CodeTextFormatter.variableTypeColour, "VariableType ")
                    .addCodeSection(CodeTextFormatter.codeColour, "VariableName = new ")
                    .addCodeSection(CodeTextFormatter.objectColour, "ObjectType();")
                    .formattedString;
            case Language.CSharp:
                //Question doesnt work in python as can't set typing
                return "";
            default:
                return "";
        }
        

        //snippet = codeColour + "VariableType VariableName = New </color>" + objectColour + "VariableType();</color>\n" + codeColour + "List<</color>" + variableTypeColour + "ParentType</color>" + codeColour + "> ParentTypes.add(VariableName);</color>";
        //question = "Place the correct screen and object to represent this situation";
    }
}
