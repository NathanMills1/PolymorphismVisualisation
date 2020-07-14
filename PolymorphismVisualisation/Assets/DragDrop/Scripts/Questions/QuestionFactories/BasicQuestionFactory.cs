using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicQuestionFactory : QuestionFactory
{
    private Dictionary<int, List<Entity>> entities;
    private Dictionary<string, string> codeSnippetsByQuestion;
    private Dictionary<string, int> codeLinesBySnippet;

    private System.Random randomGen = new System.Random();
    public BasicQuestionFactory(Dictionary<int, List<Entity>> entities)
    {
        this.entities = entities;
        this.codeSnippetsByQuestion = new Dictionary<string, string>();
        this.codeLinesBySnippet = new Dictionary<string, int>();
        loadCodeSnippets();
    }

    public override Question getQuestion()
    {
        int generationToUse = randomGen.Next(1,5);
        generationToUse = generationToUse > 2 ? 2 : generationToUse;
        List<Entity> potentialEntities = entities[generationToUse];
        int entityToUse = randomGen.Next(potentialEntities.Count);
        Entity selectedEntity = potentialEntities[entityToUse];

        int numberOfQuestions = codeSnippetsByQuestion.Keys.Count;
        string[] questions = new string[numberOfQuestions];
        codeSnippetsByQuestion.Keys.CopyTo(questions, 0);
        string questionText = questions[randomGen.Next(numberOfQuestions)];
        string codeText = codeSnippetsByQuestion[questionText];
        int numberOfCodeLines = codeLinesBySnippet[codeText];

        return new BasicQuestion(codeText, questionText, numberOfCodeLines, selectedEntity);
    }

    public void loadCodeSnippets()
    {
        string snippet = variableTypeColour + "VariableType</color>" + codeColour + " VariableName = New </color>" + objectColour + "ObjectType</color>";
        string question = "Place a screen and object to make this statement a valid call";
        codeSnippetsByQuestion.Add(question, snippet);
        codeLinesBySnippet.Add(snippet, 1);

        snippet = codeColour + "VariableType VariableName = New </color>" + objectColour + "VariableType();</color>\n" + codeColour + "List<</color>" + variableTypeColour + "ParentType</color>> " + codeColour + "ParentTypes.add(VariableName);</color>";
        question = "Place the correct screen and object to represent this situation";
        codeSnippetsByQuestion.Add(question, snippet);
        codeLinesBySnippet.Add(snippet, 2);
    }
}
