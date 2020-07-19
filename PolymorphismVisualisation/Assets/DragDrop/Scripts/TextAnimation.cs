using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextAnimation : MonoBehaviour
{

    TextMeshProUGUI textObject;

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(shakeText(0));
    }

    private void Awake()
    {
        textObject = gameObject.GetComponent<TextMeshProUGUI>();
        textObject.ForceMeshUpdate();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator shakeText(int wordPosition)
    {

        int firstCharacter = textObject.textInfo.wordInfo[wordPosition].firstCharacterIndex;
        int lastCharacter = textObject.textInfo.wordInfo[wordPosition].lastCharacterIndex;

        int shakeDirection = 1;
        float shakeOffset = 0;
        float maxOffset = 6f;

        Vector3[] vertices;

        while (maxOffset > 3f)
        {

            textObject.ForceMeshUpdate();
            vertices = textObject.mesh.vertices;

            Vector3 movement = new Vector3(4f * shakeDirection, 0, 0);

            for (int charPos = firstCharacter; charPos <= lastCharacter; charPos++)
            {
                TMP_CharacterInfo charInfo = textObject.textInfo.characterInfo[charPos];

                if (!charInfo.isVisible)
                {
                    continue;
                }

                int materialIndex = charInfo.materialReferenceIndex;
                int vertexIndex = charInfo.vertexIndex;


                for(int i = 0; i<4; i++)
                {
                    textObject.textInfo.meshInfo[materialIndex].vertices[vertexIndex + i] += movement;
                    
                }

                
            }
            shakeOffset += movement.x;

            if (shakeOffset * shakeDirection > maxOffset)
            {
                shakeDirection *= -1;
                maxOffset *= 0.9f;
            }

            textObject.UpdateVertexData();
            yield return new WaitForSeconds(0.02f);

        }
    }
}
