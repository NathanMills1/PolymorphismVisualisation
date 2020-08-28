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
        Entity child1 = descendants[RandomGen.next(descendants.Count)];
        Entity child2 = descendants[RandomGen.next(descendants.Count)];
        while (child2.Equals(child1))
        {
            child2 = descendants[RandomGen.next(descendants.Count)];
        }

        int variablePosition = -1;
        int objectPosition = GameManager.codingLanguage == Language.CPlusPlus ? 7 : 9;
        int object2Position = GameManager.codingLanguage == Language.CPlusPlus ? 10 : 13;





        return new CollectionCreationQuestion(getCodeText(), getQuestionText(), 3, selectedEntity, child1, child2, variablePosition, objectPosition, object2Position);
    }
    protected override string getQuestionText()
    {
        return "Add the correct variable type for the container to allow both instances to be added";
    }

    protected override string getCodeText()
    {
        CodeTextFormatter formattedText = new CodeTextFormatter();
        switch (codeLanguage)
        {
            case Language.CPlusPlus:

                formattedText.addCodeSection(CodeTextFormatter.codeColour, "vector<")
                    .addCodeSection(CodeTextFormatter.variableTypeColour, "ContainerType*")
                    .addCodeSection(CodeTextFormatter.codeColour, "> container;");

                for(int i = 1; i<3; i++)
                {
                    formattedText.addCodeSection(CodeTextFormatter.codeColour, "\ncontainer.push_back(new ")
                        .addCodeSection(CodeTextFormatter.objectColour, "Child" + i + "Type()")
                        .addCodeSection(CodeTextFormatter.codeColour, ");");
                }

                return formattedText.formattedString;

            case Language.CSharp:

                formattedText.addCodeSection(CodeTextFormatter.codeColour, "ArrayList<")
                    .addCodeSection(CodeTextFormatter.variableTypeColour, "ContainerType*")
                    .addCodeSection(CodeTextFormatter.codeColour, "> container = new ArrayList<ContainerType>();");

                for (int i = 1; i < 3; i++)
                {
                    formattedText.addCodeSection(CodeTextFormatter.codeColour, "\ncontainer.Add(new ")
                        .addCodeSection(CodeTextFormatter.objectColour, "Child" + i + "Type()")
                        .addCodeSection(CodeTextFormatter.codeColour, ");");
                }

                return formattedText.formattedString;

            case Language.Java:

                formattedText.addCodeSection(CodeTextFormatter.codeColour, "ArrayList<")
                    .addCodeSection(CodeTextFormatter.variableTypeColour, "ContainerType*")
                    .addCodeSection(CodeTextFormatter.codeColour, "> container = new ArrayList<ContainerType>();");

                for (int i = 1; i < 3; i++)
                {
                    formattedText.addCodeSection(CodeTextFormatter.codeColour, "\ncontainer.add(new ")
                        .addCodeSection(CodeTextFormatter.objectColour, "Child" + i + "Type()")
                        .addCodeSection(CodeTextFormatter.codeColour, ");");
                }

                return formattedText.formattedString;

            default:
                return "";
        }
        
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
