using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValidMethodCallQuestionFactory : QuestionFactory
{

    public ValidMethodCallQuestionFactory(Dictionary<int, List<Entity>> entities)
    {
        this.entities = entities;
        this.codeText = getCodeText();
        this.questionText = getQuestionText();
    }

    public override Question getQuestion()
    {

        Entity selectedEntity = generateVariable(1, 3, 0);
        Entity selectedChild = selectChild(selectedEntity);

        int methodCallGeneration = randomGen.Next(2);
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
            case Language.CSharp:
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
            case Language.Python:
                return new CodeTextFormatter().addCodeSection(CodeTextFormatter.codeColour, "VariableName = ")
                    .addCodeSection(CodeTextFormatter.objectColour, "ChildType()\n")
                    .addCodeSection(CodeTextFormatter.codeColour, "VariableName.SelectedMethod()")
                    .formattedString;
            default:
                return "";
        }
        
    }
}
