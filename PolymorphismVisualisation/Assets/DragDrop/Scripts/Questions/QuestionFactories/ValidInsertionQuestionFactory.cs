using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValidInsertionQuestionFactory : QuestionFactory
{
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

        int variablePosition;
        int objectPosition = -1;

        variablePosition = codeLanguage == Language.CSharp ? -1 : 1;
        objectPosition = codeLanguage == Language.CSharp ? 6 : 8;
        

        


        return new ValidInsertionQuestion(getCodeText(), getQuestionText(), 2, containerEntity, selectedEntity, correctAnswer, variablePosition, objectPosition);
    }
    protected override string getQuestionText()
    {
        string type = codeLanguage == Language.CPlusPlus ? "vector" : "ArrayList";
        return "Can the object type shown be added to the given " + type + "?";
    }

    protected override string getCodeText()
    {
        CodeTextFormatter formattedText = new CodeTextFormatter();
        switch (codeLanguage)
        {
            case Language.CPlusPlus:

                formattedText.addCodeSection(CodeTextFormatter.codeColour, "vector<")
                    .addCodeSection(CodeTextFormatter.variableTypeColour, "VariableType*")
                    .addCodeSection(CodeTextFormatter.codeColour, "> VariableNames;")
                    .addCodeSection(CodeTextFormatter.objectColour, "\nInsertedType ")
                    .addCodeSection(CodeTextFormatter.codeColour, "*insertedName = new ")
                    .addCodeSection(CodeTextFormatter.objectColour, "InsertedType")
                    .addCodeSection(CodeTextFormatter.codeColour, "();\nVariableNames.push_back(insertedName);");

                return formattedText.formattedString;


            case Language.Java:

                formattedText.addCodeSection(CodeTextFormatter.codeColour, "ArrayList<")
                    .addCodeSection(CodeTextFormatter.variableTypeColour, "VariableType")
                    .addCodeSection(CodeTextFormatter.codeColour, "> VariableNames = new ArrayList<")
                    .addCodeSection(CodeTextFormatter.variableTypeColour, "VariableType")
                    .addCodeSection(CodeTextFormatter.codeColour, ">();")
                    .addCodeSection(CodeTextFormatter.objectColour, "\nInsertedType ")
                    .addCodeSection(CodeTextFormatter.codeColour, "insertedName = new ")
                    .addCodeSection(CodeTextFormatter.objectColour, "InsertedType")
                    .addCodeSection(CodeTextFormatter.codeColour, "();\nVariableNames.add(insertedName);");

                return formattedText.formattedString;

            case Language.CSharp:

                formattedText.addCodeSection(CodeTextFormatter.codeColour, "ArrayList<")
                    .addCodeSection(CodeTextFormatter.variableTypeColour, "VariableType")
                    .addCodeSection(CodeTextFormatter.codeColour, "> VariableNames = new ArrayList<")
                    .addCodeSection(CodeTextFormatter.variableTypeColour, "VariableType")
                    .addCodeSection(CodeTextFormatter.codeColour, ">();")
                    .addCodeSection(CodeTextFormatter.objectColour, "\nInsertedType ")
                    .addCodeSection(CodeTextFormatter.codeColour, "insertedName = new ")
                    .addCodeSection(CodeTextFormatter.objectColour, "InsertedType")
                    .addCodeSection(CodeTextFormatter.codeColour, "();\nVariableNames.Add(insertedName);");

                return formattedText.formattedString;

            default:
                return "";
        }
        
    }

}
