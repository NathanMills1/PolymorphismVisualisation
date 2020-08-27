﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LoggingController : MonoBehaviour
{

    // private readonly string baseURL = "https://oop-visualisation.herokuapp.com/stats";
    private readonly string baseURL = "localhost:3000/stats";
    private string token = "";

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetToken());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator GetToken()
    {
        // string URL = "https://oop-visualisation.herokuapp.com/token";
        string URL = "localhost:3000/token";

        using (UnityWebRequest www = UnityWebRequest.Get(URL))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError)
            {
                Debug.Log(www.error);
            }
            else
            {
                if (www.isDone)
                {
                    // handle the result
                    token = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);
                    Debug.Log(token);
                }
                else
                {
                    //handle the problem
                    Debug.Log("Error! data couldn't get.");
                }
            }
        }
    }

    //public void AttemptReq(int section, int questionId, string userAnswer, string correctAnswer)
    public void AttemptReq(int section, int questionId, string userAnswer, string correctAnswer)
    {
        //StartCoroutine(Attempt(section, questionId, userAnswer, correctAnswer));
        StartCoroutine(Attempt(section, questionId, userAnswer, correctAnswer));
    }

    IEnumerator Attempt(int section, int questionId, string userAnswer, string correctAnswer)
    {
        string URL = baseURL + "/attempt?token=" + token;

        WWWForm form = new WWWForm();
        form.AddField("section", section.ToString());
        form.AddField("questionId", questionId.ToString());
        form.AddField("userAnswer", userAnswer);
        form.AddField("correctAnswer", correctAnswer);

        UnityWebRequest www = UnityWebRequest.Post(URL, form);

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.LogError(www.error);
            yield break;
        }
    }

    //public void QuestionReq(int section, int questionId, int timeTaken)
    public void QuestionReq(int section, int questionId, int timeTaken)
    {
        //StartCoroutine(Question(section, questionId, timeTaken));
        StartCoroutine(Question(section, questionId, timeTaken));
    }

    IEnumerator Question(int section, int questionId, int timeTaken)
    {
        string URL = baseURL + "/question?token=" + token;

        WWWForm form = new WWWForm();
        form.AddField("section", section.ToString());
        form.AddField("questionId", questionId.ToString());
        form.AddField("timeTaken", timeTaken.ToString());

        UnityWebRequest www = UnityWebRequest.Post(URL, form);

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.LogError(www.error);
            yield break;
        }
    }
}
