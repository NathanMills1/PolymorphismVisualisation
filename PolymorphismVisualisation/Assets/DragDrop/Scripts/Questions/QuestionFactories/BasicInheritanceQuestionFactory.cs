using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicInheritanceQuestionFactory : QuestionFactory
{

    public BasicInheritanceQuestionFactory(int[] generationWeighting)
    {
        this.entities = InheritanceGenerator.selectedEntitiesByGeneration;
        this.codeText = getCodeText();
        this.questionText = getQuestionText();
        this.generationWeighting = generationWeighting;
    }

    public override Question getQuestion()
    {

        Entity selectedEntity = generateVariable(generationWeighting);

        return new BasicInheritanceQuestion(codeText, questionText, 1, selectedEntity);
    }
    protected override string getQuestionText()
    {
        return "Place an object that inherits from the variable type, along with the screen for that variable type";
    }

    protected override string getCodeText()
    {
        switch (codeLanguage)
        {
            case Language.CSharp:
                return new CodeTextFormatter().addCodeSection(CodeTextFormatter.variableTypeColour, "VariableType ")
                    .addCodeSection(CodeTextFormatter.codeColour, "*VariableName = new ")
                    .addCodeSection(CodeTextFormatter.objectColour, "ObjectType();")
                    .formattedString;
            case Language.Java:
                return new CodeTextFormatter().addCodeSection(CodeTextFormatter.variableTypeColour, "VariableType ")
                    .addCodeSection(CodeTextFormatter.codeColour, "VariableName = new ")
                    .addCodeSection(CodeTextFormatter.objectColour, "ObjectType();")
                    .formattedString;
            case Language.Python:
                //Question doesnt work in python as can't set typing
                return "";
            default:
                return "";
        }
    }
}
