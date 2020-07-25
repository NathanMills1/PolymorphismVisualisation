using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;

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

        
        int materialIndex = textObject.textInfo.characterInfo[firstCharacter].materialReferenceIndex;
        Vector3[] startPositions = (Vector3[])(textObject.textInfo.meshInfo[materialIndex].vertices.Clone());

        while (maxOffset > 3f)
        {

            textObject.ForceMeshUpdate();

            Vector3 movement = new Vector3(4f * shakeDirection, 0, 0);

            for (int charPos = firstCharacter; charPos <= lastCharacter; charPos++)
            {
                TMP_CharacterInfo charInfo = textObject.textInfo.characterInfo[charPos];

                if (!charInfo.isVisible)
                {
                    continue;
                }

                materialIndex = charInfo.materialReferenceIndex;
                int vertexIndex = charInfo.vertexIndex;

                for (int i = 0; i<4; i++)
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

        for (int charPos = firstCharacter; charPos <= lastCharacter; charPos++)
        {
            TMP_CharacterInfo charInfo = textObject.textInfo.characterInfo[charPos];

            if (!charInfo.isVisible)
            {
                continue;
            }

            int vertexIndex = charInfo.vertexIndex;

            for (int i = 0; i < 4; i++)
            {
                textObject.textInfo.meshInfo[materialIndex].vertices[vertexIndex + i] = startPositions[vertexIndex + i];
            }
        }

        textObject.UpdateVertexData();
    }

    public IEnumerator glowText(int wordPosition)
    {
        const float TRANSITION_TIME = 0.8f;
        const float STEP_SIZE = TRANSITION_TIME / 20;
        const float HORIZONTAL_INCREASE = 15.0f;
        const float VERTICAL_INCREASE = 5f;


        int firstCharacter = textObject.textInfo.wordInfo[wordPosition].firstCharacterIndex;
        int lastCharacter = textObject.textInfo.wordInfo[wordPosition].lastCharacterIndex;


        Vector3 topRight = textObject.textInfo.characterInfo[lastCharacter].topRight;
        Vector3 bottomLeft = textObject.textInfo.characterInfo[firstCharacter].bottomLeft;
        Vector3 centerPoint = (bottomLeft + topRight) / 2;
        Vector3 radius = topRight - centerPoint;
        Vector3[] vertices;
        Vector3[] initialVertices = (Vector3[])(textObject.textInfo.meshInfo[textObject.textInfo.characterInfo[firstCharacter].materialReferenceIndex].vertices.Clone());
        Vector3[] transformedVertices = (Vector3[])initialVertices.Clone();



        Color32 glowColour = new Color32(72, 255, 0, 255);
        Color32 initialColour = textObject.textInfo.characterInfo[firstCharacter].color;
        initialColour = new Color32(initialColour.r, initialColour.g, initialColour.b, initialColour.a);

        Color32[] newVertexColors;


        //Calculate the transformed vertices
        for (int charPos = firstCharacter; charPos <= lastCharacter; charPos++)
        {
            TMP_CharacterInfo charInfo = textObject.textInfo.characterInfo[charPos];
            if (!charInfo.isVisible)
            {
                continue;
            }

            int materialIndex = charInfo.materialReferenceIndex;
            int vertexIndex = charInfo.vertexIndex;

            for (int i = 0; i < 4; i++)
            {
                Vector3 corner = transformedVertices[vertexIndex + i];
                Vector3 translation = new Vector3(((corner.x - centerPoint.x) / radius.x) * HORIZONTAL_INCREASE, ((corner.y - centerPoint.y) / radius.y) * VERTICAL_INCREASE, 0);
                transformedVertices[vertexIndex + i] += translation;

            }
        }


        float currentTime = 0;

        while (currentTime <= TRANSITION_TIME)
        {
            textObject.ForceMeshUpdate();
            float interpolatePosition = (TRANSITION_TIME - 2 * Math.Abs(TRANSITION_TIME / 2 - currentTime)) / TRANSITION_TIME;
            Color32 currentColour = Color32.Lerp(initialColour, glowColour, interpolatePosition);


            for (int charPos = firstCharacter; charPos <= lastCharacter; charPos++)
            {
                TMP_CharacterInfo charInfo = textObject.textInfo.characterInfo[charPos];
                if (!charInfo.isVisible)
                {
                    continue;
                }

                int materialIndex = charInfo.materialReferenceIndex;
                int vertexIndex = charInfo.vertexIndex;

                newVertexColors = textObject.textInfo.meshInfo[materialIndex].colors32;
                vertices = textObject.textInfo.meshInfo[materialIndex].vertices;

                for (int i = 0; i < 4; i++)
                {
                    newVertexColors[vertexIndex + i] = currentColour;
                    vertices[vertexIndex + i] = (1 - interpolatePosition) * initialVertices[vertexIndex + i] + interpolatePosition * transformedVertices[vertexIndex + i];
                    
                }

            }

            currentTime += STEP_SIZE;

            textObject.UpdateVertexData();

            yield return new WaitForSeconds(STEP_SIZE);
        }

        //Ensure values back to original
        for (int charPos = firstCharacter; charPos <= lastCharacter; charPos++)
        {
            TMP_CharacterInfo charInfo = textObject.textInfo.characterInfo[charPos];
            if (!charInfo.isVisible)
            {
                continue;
            }

            int materialIndex = charInfo.materialReferenceIndex;
            int vertexIndex = charInfo.vertexIndex;

            newVertexColors = textObject.textInfo.meshInfo[materialIndex].colors32;
            vertices = textObject.textInfo.meshInfo[materialIndex].vertices;

            for (int i = 0; i < 4; i++)
            {
                newVertexColors[vertexIndex + i] = initialColour;
                vertices[vertexIndex + i] = initialVertices[vertexIndex + i];

            }

        }


    }
}
