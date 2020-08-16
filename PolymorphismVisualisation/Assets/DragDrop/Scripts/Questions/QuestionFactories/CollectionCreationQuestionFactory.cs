using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionCreationQuestionFactory : QuestionFactory
{

    public CollectionCreationQuestionFactory(int[] generationWeighting)
    {
        this.entities = InheritanceGenerator.selectedEntitiesByGeneration;
        this.generationWeighting = generationWeighting;
    }

    public override Question getQuestion()
    {

        Entity selectedEntity = generateVariable(generationWeighting);
        List<Entity> descendants = new List<Entity>();
        addChildrenToList(descendants, selectedEntity);
        Entity child1 = descendants[randomGen.Next(descendants.Count)];
        Entity child2 = descendants[randomGen.Next(descendants.Count)];
        while (child2.Equals(child1))
        {
            child2 = descendants[randomGen.Next(descendants.Count)];
        }

        int containerType = randomGen.Next(2);
        int variablePosition = -1;
        int objectPosition = -1;
        

        


        return new CollectionCreationQuestion(getCodeText(containerType), getQuestionText(), 3, child1, child2, variablePosition, objectPosition);
    }
    protected override string getQuestionText()
    {
        return "Add the correct variable type for the container to allow both objects to be added";
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
                formattedText.addCodeSection(CodeTextFormatter.variableTypeColour, "ContainerType");

                variablePart = containerType == 0 ? ">" : "[]";
                formattedText.addCodeSection(CodeTextFormatter.codeColour, variablePart + " container;");

                for(int i = 1; i<3; i++)
                {
                    variablePart = containerType == 0 ? ".push_back(" : "[" + i + "] = ";
                    formattedText.addCodeSection(CodeTextFormatter.codeColour, "\ncontainer" + variablePart)
                        .addCodeSection(CodeTextFormatter.objectColour, "Child" + i + "Type ")
                        .addCodeSection(CodeTextFormatter.codeColour, "Child" + i + "Name()");

                    variablePart = containerType == 0 ? ");" : ";";
                    formattedText.addCodeSection(CodeTextFormatter.codeColour, variablePart);
                }

                return formattedText.formattedString;

            case Language.Java:
                formattedText = new CodeTextFormatter();
                if (containerType == 0)
                {
                    formattedText.addCodeSection(CodeTextFormatter.codeColour, "List<");
                }
                formattedText.addCodeSection(CodeTextFormatter.variableTypeColour, "ContainerType");

                variablePart = containerType == 0 ? ">" : "[]";
                formattedText.addCodeSection(CodeTextFormatter.codeColour, variablePart + " = new ");

                variablePart = containerType == 0 ? "List<" : "";
                formattedText.addCodeSection(CodeTextFormatter.codeColour, variablePart);

                variablePart = containerType == 0 ? ">()" : "[]";
                formattedText.addCodeSection(CodeTextFormatter.codeColour, "ContainerType" + variablePart + ";");

                for(int i = 1; i<3; i++)
                {
                    variablePart = containerType == 0 ? ".add(" : "[" + i + "] = ";
                    string variablePart2 = containerType == 0 ? ");" : ";";
                    formattedText.addCodeSection(CodeTextFormatter.codeColour, "\ncontainer" + variablePart + "new ")
                        .addCodeSection(CodeTextFormatter.objectColour, "Child" + i + "Type()")
                        .addCodeSection(CodeTextFormatter.codeColour, variablePart2);
                }
                
                return formattedText.formattedString;

            case Language.Python:
                formattedText = new CodeTextFormatter().addCodeSection(CodeTextFormatter.codeColour, "container = []");
                for(int i = 1; i<3; i++)
                {
                    formattedText.addCodeSection(CodeTextFormatter.codeColour, "\ncontainer.append(")
                    .addCodeSection(CodeTextFormatter.objectColour, "Child" + i + "Type()")
                    .addCodeSection(CodeTextFormatter.codeColour, ")");
                }
                return formattedText.formattedString;

            default:
                return "";
        }
        
    }

    protected override string getCodeText() {
        return getCodeText(0);
    }

    private void addChildrenToList(List<Entity> descendants, Entity parent)
    {
        if(parent.children == null || parent.children.Count == 0)
        {
            descendants.Add(parent);
        }
        else
        {
            foreach(Entity child in parent.children)
            {
                addChildrenToList(descendants, child);
            }
            descendants.Add(parent);
        }
    }
}
