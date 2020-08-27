using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LoggingController : MonoBehaviour
{

    private readonly string baseURL = "https://oop-visualisation.herokuapp.com/stats";
    private string token = "";

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
        string URL = "https://oop-visualisation.herokuapp.com/token";

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

    public void MakeWebReq()
    {
        StartCoroutine(Inc());
    }

    IEnumerator Inc()
    {
        string URL = baseURL + "/correct?token=" + token;

        UnityWebRequest req = UnityWebRequest.Get(URL);

        yield return req.SendWebRequest();

        if (req.isNetworkError || req.isHttpError)
        {
            Debug.LogError(req.error);
            yield break;
        }
    }
}
