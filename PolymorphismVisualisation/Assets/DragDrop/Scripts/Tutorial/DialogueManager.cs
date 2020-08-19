using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{

	public Text dialogueText;
	public Animator animator;
	public TutorialManager tutorialManager;
	public GameObject continueButton;


	private bool nextSectionOnCompletion;
	private Queue<string> sentences;

	

	// Use this for initialization
	void Start()
	{
		sentences = new Queue<string>();
	}

	public void StartDialogue(string[] dialogue, bool nextSectionOnCompletion)
	{
		//animator.SetBool("IsOpen", true);

		this.nextSectionOnCompletion = nextSectionOnCompletion;
		sentences.Clear();
		continueButton.SetActive(true);

		foreach (string sentence in dialogue)
		{
			sentences.Enqueue(sentence);
		}

		DisplayNextSentence();
	}

	public void DisplayNextSentence()
	{
		if (sentences.Count == 0)
		{
			EndDialogue();
			return;
		}

		string sentence = sentences.Dequeue();
		StopAllCoroutines();
		StartCoroutine(TypeSentence(sentence));

		if (sentences.Count == 0 && !nextSectionOnCompletion)
        {
			continueButton.SetActive(false);
        }
	}

	IEnumerator TypeSentence(string sentence)
	{
		dialogueText.text = "";
		foreach (char letter in sentence.ToCharArray())
		{
			dialogueText.text += letter;
			yield return null;
		}
	}

	void EndDialogue()
	{
        //animator.SetBool("IsOpen", false);
        if (nextSectionOnCompletion)
        {
			tutorialManager.loadNextSection();
        }
	}

}