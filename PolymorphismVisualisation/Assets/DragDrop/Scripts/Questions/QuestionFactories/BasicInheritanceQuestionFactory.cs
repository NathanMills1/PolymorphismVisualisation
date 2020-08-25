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
        return "Place the shown variable type, along with an instance that inherits from said variable type";
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
    }
}
