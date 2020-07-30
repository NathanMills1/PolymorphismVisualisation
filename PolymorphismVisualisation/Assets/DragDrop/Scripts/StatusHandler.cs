using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatusHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator statusUpdate(string status, Color textColour)
    {
        TextMeshProUGUI textBox = this.gameObject.GetComponentInChildren<TextMeshProUGUI>();
        for (int i = 0; i < 2; i++)
        {
            if (i == 0)
            {
                textBox.text = status;
                textBox.color = textColour;
                textBox.ForceMeshUpdate();
                yield return new WaitForSeconds(4.0f);
            }
            else
            {
                textBox.text = "Status: ";
                textBox.color = Color.black;
                textBox.ForceMeshUpdate();
                yield return new WaitForSeconds(4.0f);
            }
        }
    }

    public void updateStatus(string status, Color textColour)
    {
        StartCoroutine(statusUpdate(status, textColour));
    }
}
