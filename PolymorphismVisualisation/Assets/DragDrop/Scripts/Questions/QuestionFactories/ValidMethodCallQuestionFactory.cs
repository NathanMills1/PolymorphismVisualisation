using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValidMethodCallQuestionFactory : QuestionFactory
{

    public ValidMethodCallQuestionFactory(int[] generationWeighting)
    {
        this.entities = InheritanceGenerator.selectedEntitiesByGeneration;
        this.codeText = getCodeText();
        this.questionText = getQuestionText();
        this.generationWeighting = generationWeighting;
    }

    public override Question getQuestion()
    {

        Entity selectedEntity = generateVariable(generationWeighting);
        Entity selectedChild = selectChild(selectedEntity);

        int methodCallGeneration = RandomGen.next(2);
        Entity entityToGetMethodFor;
        bool correctAnswer;

        if(methodCallGeneration == 0)
        {
            entityToGetMethodFor = selectedEntity;
            correctAnswer = true;
        }
        else
        {
            entityToGetMethodFor = selectedChild;
            correctAnswer = false;
        }

        string method = selectMethod(entityToGetMethodFor);

        return new ValidMethodCallQuestion(codeText, questionText, 2, selectedEntity, selectedChild, method, correctAnswer);
    }
    protected override string getQuestionText()
    {
        return "Is the method call shown in the code snippet valid?";
    }

    protected override string getCodeText()
    {
        switch (codeLanguage)
        {
            case Language.CPlusPlus:
                return new CodeTextFormatter().addCodeSection(CodeTextFormatter.variableTypeColour, "VariableType ")
                    .addCodeSection(CodeTextFormatter.codeColour, "*VariableName = new ")
                    .addCodeSection(CodeTextFormatter.objectColour, "ChildType();\n")
                    .addCodeSection(CodeTextFormatter.codeColour, "VariableName->SelectedMethod();")
                    .formattedString;
            case Language.Java:
                return new CodeTextFormatter().addCodeSection(CodeTextFormatter.variableTypeColour, "VariableType ")
                    .addCodeSection(CodeTextFormatter.codeColour, "VariableName = new ")
                    .addCodeSection(CodeTextFormatter.objectColour, "ChildType();\n")
                    .addCodeSection(CodeTextFormatter.codeColour, "VariableName.SelectedMethod();")
                    .formattedString;
            case Language.CSharp:
                return new CodeTextFormatter().addCodeSection(CodeTextFormatter.codeColour, "VariableName = ")
                    .addCodeSection(CodeTextFormatter.objectColour, "ChildType()\n")
                    .addCodeSection(CodeTextFormatter.codeColour, "VariableName.SelectedMethod()")
                    .formattedString;
            default:
                return "";
        }
        
    }
}
