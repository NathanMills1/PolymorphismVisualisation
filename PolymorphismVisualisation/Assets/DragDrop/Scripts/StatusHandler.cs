using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatusHandler : MonoBehaviour  
{
    private Coroutine statusCoroutine;

    public AudioSource correctSound;
    public AudioSource incorrectSound;

    private IEnumerator statusUpdate(string status, bool correct)
    {

        if (!GameManager.muted)
        {
            if (correct)
            {
                correctSound.Play();
            }
            else
            {
                incorrectSound.Play();
            }
            
        }

        TextMeshProUGUI textBox = this.gameObject.GetComponent<TextMeshProUGUI>();

        textBox.text = status;
        textBox.color = correct ? new Color32(0, 225, 0, 255) : new Color32(255, 0, 0, 255);
        
        textBox.ForceMeshUpdate();
        yield return new WaitForSeconds(2.0f);

        if (correct)
        {
            FindObjectOfType<QuestionManager>().correctAnswerProcedure();
        }
        
    }

    public void updateStatus(string status, bool correct)
    {
        if(statusCoroutine != null)
        {
            StopCoroutine(statusCoroutine);
        }
        
        statusCoroutine = StartCoroutine(statusUpdate(status, correct));
    }
}
