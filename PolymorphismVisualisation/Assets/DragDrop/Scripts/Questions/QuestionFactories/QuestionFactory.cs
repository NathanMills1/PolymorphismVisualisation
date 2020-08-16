
using System.Collections.Generic;
using static CodeTextFormatter;

public abstract class QuestionFactory
{
    protected string question;
    protected static Language codeLanguage;

    protected string codeText;
    protected string questionText;
    protected int[] generationWeighting;
    protected Dictionary<int, List<Entity>> entities;
    protected System.Random randomGen = new System.Random();

    public abstract Question getQuestion();

    protected abstract string getQuestionText();

    protected abstract string getCodeText();

    protected virtual Entity generateVariable(int[] weighting)
    {
        int total = weighting[0] + weighting[1] + weighting[2];
        int weightingResult = randomGen.Next(total);

        int chosenGeneration = weightingResult < weighting[0] ? 1 : (weightingResult < weighting[0] + weighting[1]) ? 2 : 3;

        int entityCount = entities[chosenGeneration].Count;
        int chosenEntityIndex = randomGen.Next(entityCount);

        return entities[chosenGeneration][chosenEntityIndex];
    }

    protected Entity selectChild(Entity entity)
    {
        int childrenCount = entity.children.Count;
        int childIndex = randomGen.Next(childrenCount);
        return entity.children[childIndex];
    }

    protected string selectMethod(Entity entity)
    {
        int methodCount = entity.identity.methods.Length;
        int selectedIndex = randomGen.Next(methodCount);
        string method = entity.identity.methods[selectedIndex];
        return method;
    }

    public static void setCodeLanguage(Language language)
    {
        QuestionFactory.codeLanguage = language;
    }
}
