
using System.Collections.Generic;
using static CodeTextFormatter;

public abstract class QuestionFactory
{
    protected string question;
    protected static Language codeLanguage;

    protected string codeText;
    protected string questionText;
    protected Dictionary<int, List<Entity>> entities;
    protected System.Random randomGen = new System.Random();

    public abstract Question getQuestion();

    protected abstract string getQuestionText();

    protected abstract string getCodeText();

    protected virtual Entity generateVariable(int gen1Odds, int gen2Odds, int gen3Odds)
    {
        int total = gen1Odds + gen2Odds + gen3Odds;
        int weightingResult = randomGen.Next(total);
        int chosenGeneration = 0;

        chosenGeneration = weightingResult < gen1Odds ? 1 : (weightingResult < gen1Odds + gen2Odds) ? 2 : 3;

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
        int methodCount = entity.identity.selectedMethods.Count;
        int selectedIndex = randomGen.Next(methodCount);
        string method = entity.identity.selectedMethods[selectedIndex];
        return method;
    }

    public static void setCodeLanguage(Language language)
    {
        QuestionFactory.codeLanguage = language;
    }
}
