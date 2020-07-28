using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValidInsertionQuestionFactory : QuestionFactory
{

    public ValidInsertionQuestionFactory(Dictionary<int, List<Entity>> entities)
    {
        this.entities = entities;
    }

    public override Question getQuestion()
    {

        Entity selectedEntity = generateVariable(1, 1, 1);
        Entity containerEntity = generateVariable(1, 1, 1);

        bool correctAnswer = selectedEntity.determineIfChildOf(containerEntity);

        int containerType = randomGen.Next(2);
        int variablePosition;
        int objectPosition;
        if (containerType.Equals(0))
        {
            variablePosition = codeLanguage == Language.Python ? -1 : 1;
            objectPosition = codeLanguage == Language.Java ? 8 : codeLanguage == Language.CSharp ? 6 : 3;
        }
        else
        {
            variablePosition = codeLanguage == Language.Python ? -1 : 0;
            objectPosition = codeLanguage == Language.Java ? 7 : codeLanguage == Language.CSharp ? 4 : 3;
        }
        

        


        return new ValidInsertionQuestion(getCodeText(containerType), getQuestionText(), 2, containerEntity, selectedEntity, correctAnswer, variablePosition, objectPosition);
    }
    protected override string getQuestionText()
    {
        return "Is the method call shown in the code snippet valid?";
    }

    protected string getCodeText(int containerType)
    {
        string variablePart = "";
        CodeTextFormatter formattedText;
        switch (codeLanguage)
        {
            case Language.CSharp:
                
                formattedText = new CodeTextFormatter();
                if(containerType == 0)
                {
                    formattedText.addCodeSection(CodeTextFormatter.codeColour, "list<");
                }
                formattedText.addCodeSection(CodeTextFormatter.variableTypeColour, "VariableType");

                variablePart = containerType == 0 ? ">" : "[]";
                formattedText.addCodeSection(CodeTextFormatter.codeColour, variablePart + " VariableName;\nVariableName");

                variablePart = containerType == 0 ? ".push_back(" : "[0] = ";
                formattedText.addCodeSection(CodeTextFormatter.codeColour, variablePart)
                    .addCodeSection(CodeTextFormatter.objectColour, "InsertedType ")
                    .addCodeSection(CodeTextFormatter.codeColour, "InsertedName()");

                variablePart = containerType == 0 ? ");" : ";";
                formattedText.addCodeSection(CodeTextFormatter.codeColour, variablePart);
                return formattedText.formattedString;

            case Language.Java:
                formattedText = new CodeTextFormatter();
                if (containerType == 0)
                {
                    formattedText.addCodeSection(CodeTextFormatter.codeColour, "List<");
                }
                formattedText.addCodeSection(CodeTextFormatter.variableTypeColour, "VariableType");

                variablePart = containerType == 0 ? ">" : "[]";
                formattedText.addCodeSection(CodeTextFormatter.codeColour, variablePart + " = new ");

                variablePart = containerType == 0 ? "List<" : "";
                formattedText.addCodeSection(CodeTextFormatter.codeColour, variablePart);

                variablePart = containerType == 0 ? ">()" : "[]";
                string variablePart2 = containerType == 0 ? ".add(" : "[0] = ";
                formattedText.addCodeSection(CodeTextFormatter.codeColour, "VariableType" + variablePart + ";\nVariableName" + variablePart2);

                 variablePart = containerType == 0 ? ");" : ";";
                formattedText.addCodeSection(CodeTextFormatter.codeColour, "new ")
                    .addCodeSection(CodeTextFormatter.objectColour, "InsertedType")
                    .addCodeSection(CodeTextFormatter.codeColour, variablePart);
                return formattedText.formattedString;

            case Language.Python:
                return new CodeTextFormatter().addCodeSection(CodeTextFormatter.codeColour, "VariableName = []\nVariableName.append(")
                    .addCodeSection(CodeTextFormatter.objectColour, "InsertedType()")
                    .addCodeSection(CodeTextFormatter.codeColour, ")")
                    .formattedString;

            default:
                return "";
        }
        
    }

    protected override string getCodeText() {
        return getCodeText(0);
    }
}
