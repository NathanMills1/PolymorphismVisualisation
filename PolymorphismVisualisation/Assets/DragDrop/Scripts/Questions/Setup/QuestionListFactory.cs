using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class QuestionListFactory
{
    public static List<QuestionFactory> generateQuestionList(int activitySection)
    {
        switch (activitySection)
        {
            case 1:
                return generateBasicQuestions();
            case 2:
                return generateInheritanceQuestions();
            default:
                return generateMultiInheritanceQuestions();
        }
    }

    private static List<QuestionFactory> generateBasicQuestions()
    {
        List<QuestionFactory> basicQuestions = new List<QuestionFactory>();
        basicQuestions.Add(new BasicQuestionFactory(new int[] { 1, 0, 0 }));
        basicQuestions.Add(new ValidInsertionQuestionFactory(new int[] { 1, 0, 0 }, new int[] { 1, 0, 0 }));
        basicQuestions.Add(new ValidAssignmentQuestionFactory(new int[] { 1, 0, 0 }, new int[] { 1, 0, 0 }));
        return basicQuestions;
    }

    private static List<QuestionFactory> generateInheritanceQuestions()
    {
        List<QuestionFactory> inheritanceQuestions = new List<QuestionFactory>();

        return inheritanceQuestions;
    }

    private static List<QuestionFactory> generateMultiInheritanceQuestions()
    {
        List<QuestionFactory> multiInheritanceQuestions = new List<QuestionFactory>();
        multiInheritanceQuestions.Add(new BasicQuestionFactory(new int[] { 1, 1, 1 }));
        multiInheritanceQuestions.Add(new BasicInheritanceQuestionFactory(new int[] { 1, 1, 0 }));
        multiInheritanceQuestions.Add(new ValidMethodCallQuestionFactory(new int[] { 1, 2, 0 }));
        multiInheritanceQuestions.Add(new ValidAssignmentQuestionFactory(new int[] { 4, 1, 2 }, new int[] { 1, 2, 4 }));
        multiInheritanceQuestions.Add(new ValidInsertionQuestionFactory(new int[] { 3, 2, 1 }, new int[] { 1, 2, 3 }));
        multiInheritanceQuestions.Add(new CollectionCreationQuestionFactory(new int[] { 1, 2, 0 }));
        return multiInheritanceQuestions;
    }
}
