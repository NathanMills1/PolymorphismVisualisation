using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValidInsertionQuestionFactory : QuestionFactory
{
    private int containerType { get; set; }
    private int[] containerWeighting;

    public ValidInsertionQuestionFactory(int[] generationWeighting, int[] containerWeighting)
    {
        this.entities = InheritanceGenerator.selectedEntitiesByGeneration;
        this.generationWeighting = generationWeighting;
        this.containerWeighting = containerWeighting;
    }

    public override Question getQuestion()
    {

        Entity selectedEntity = generateVariable(generationWeighting);
        Entity containerEntity = generateVariable(containerWeighting);

        bool correctAnswer = selectedEntity.determineIfChildOf(containerEntity);

        containerType = randomGen.Next(2);

        int variablePosition;
        int objectPosition = -1;
        if (containerType.Equals(0))
        {
            variablePosition = codeLanguage == Language.Python ? -1 : 1;
            //objectPosition = codeLanguage == Language.Java ? 8 : codeLanguage == Language.CSharp ? 6 : 3;
        }
        else
        {
            variablePosition = codeLanguage == Language.Python ? -1 : 0;
            //objectPosition = codeLanguage == Language.Java ? 7 : codeLanguage == Language.CSharp ? 4 : 3;
        }
        

        


        return new ValidInsertionQuestion(getCodeText(containerType), getQuestionText(), 2, containerEntity, selectedEntity, correctAnswer, variablePosition, objectPosition);
    }
    protected override string getQuestionText()
    {
        string type = containerType == 0 ? "list" : "array";
        return "Can the object type shown be added to the given " + type + " type";
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
                formattedText.addCodeSection(CodeTextFormatter.codeColour, variablePart + " VariableNames;\nVariableNames");

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
                formattedText.addCodeSection(CodeTextFormatter.codeColour, "VariableType" + variablePart + ";\nVariableNames" + variablePart2);

                 variablePart = containerType == 0 ? ");" : ";";
                formattedText.addCodeSection(CodeTextFormatter.codeColour, "new ")
                    .addCodeSection(CodeTextFormatter.objectColour, "InsertedType()")
                    .addCodeSection(CodeTextFormatter.codeColour, variablePart);
                return formattedText.formattedString;

            case Language.Python:
                return new CodeTextFormatter().addCodeSection(CodeTextFormatter.codeColour, "VariableNames = []\nVariableNames.append(")
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
