using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSection : MonoBehaviour
{
    public string[] dialogue;
    public bool nextSectionOnDialogueComplete;
    public GameObject[] highlightedComponents;

    public Transform[] componentParents { get;  set; }
    public int[] originalPositions { get; set; }

    public void Start()
    {
        componentParents = new Transform[highlightedComponents.Length];
        originalPositions = new int[highlightedComponents.Length];
    }
}
