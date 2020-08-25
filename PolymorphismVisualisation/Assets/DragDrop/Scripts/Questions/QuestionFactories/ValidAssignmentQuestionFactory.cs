using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValidAssignmentQuestionFactory : QuestionFactory
{
    private int containerType { get; set; }
    private int[] variableTypeWeighting;

    public ValidAssignmentQuestionFactory(int[] generationWeighting, int[] variableTypeWeighting)
    {
        this.entities = InheritanceGenerator.selectedEntitiesByGeneration;
        this.generationWeighting = generationWeighting;
        this.variableTypeWeighting = variableTypeWeighting;
    }

    public override Question getQuestion()
    {

        Entity selectedEntity = generateVariable(generationWeighting);
        Entity variableTypeEntity = generateVariable(variableTypeWeighting);

        bool correctAnswer = selectedEntity.determineIfChildOf(variableTypeEntity);

        containerType = RandomGen.next(2);

        int variablePosition = 0;
        int objectPosition = -1;
        

        


        return new ValidAssignmentQuestion(getCodeText(), getQuestionText(), 1, variableTypeEntity, selectedEntity, correctAnswer, variablePosition, objectPosition);
    }
    protected override string getQuestionText()
    {
        return "Can the object type shown be assigned to the variable type shown?";
    }

    protected override string getCodeText()
    {
        switch (codeLanguage)
        {
            case Language.CPlusPlus:
                return new CodeTextFormatter().addCodeSection(CodeTextFormatter.variableTypeColour, "VariableType ")
                    .addCodeSection(CodeTextFormatter.codeColour, "*VariableName = new ")
                    .addCodeSection(CodeTextFormatter.objectColour, "selectedType();")
                    .formattedString;

            case Language.CSharp:
            case Language.Java:
                return new CodeTextFormatter().addCodeSection(CodeTextFormatter.variableTypeColour, "VariableType ")
                    .addCodeSection(CodeTextFormatter.codeColour, "VariableName = new ")
                    .addCodeSection(CodeTextFormatter.objectColour, "selectedType();")
                    .formattedString;

            default:
                return "";
        }
        
    }
}
