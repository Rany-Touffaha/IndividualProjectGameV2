using System.Collections.Generic;
using Gamekit2D;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WordBoardDisplay : MonoBehaviour
{
    public WordBoard wordBoard;
    public List<TextMeshPro> consonantLetterList;
    public List<TextMeshPro> vowelLetterList;
    public List<TextMeshPro> outputLetterList;
    public Image wordImage;

    public PressurePad pressurePad;
    public InventoryItem key;
    public TextDialogue incorrectWordTextDialogue;

    private int currentIndex = 0;

    private void Start()
    {
        UpdateWordBoard();
    }

    private void UpdateWordBoard()
    {
        wordBoard.Print();

        for (int i = 0; i < wordBoard.consonantLetters.Count && i < consonantLetterList.Count; i++)
        {
            consonantLetterList[i].text = wordBoard.consonantLetters[i].ToString();
        }

        for (int i = 0; i < wordBoard.vowelLetters.Count && i < vowelLetterList.Count; i++)
        {
            vowelLetterList[i].text = wordBoard.vowelLetters[i].ToString();
        }

        wordImage.sprite = wordBoard.wordImage;
    }

    private void AddLetter(string letter)
    {
        if (currentIndex < outputLetterList.Count)
        {
            outputLetterList[currentIndex].text = letter;
            MoveToNextEmptyLetter();
        }
        else
        {
            CheckWord();
        }
    }

    private void MoveToNextEmptyLetter()
    {
        currentIndex = outputLetterList.FindIndex(letter => string.IsNullOrEmpty(letter.text));
        if (currentIndex == -1)
        {
            currentIndex = outputLetterList.Count;
            AddLetter("");
        }
    }

    private void CheckWord()
    {
        string word = wordBoard.word.ToUpper();
        bool isWordCorrect = true;

        for (int i = 0; i < word.Length && i < outputLetterList.Count; i++)
        {
            TextMeshPro outputLetter = outputLetterList[i];
            char wordChar = word[i];
            if (wordChar != outputLetter.text[0]) 
            {
                isWordCorrect = false;
                outputLetter.text = "";
            }
        }

        if (isWordCorrect)
        {
            TextDialogue textDialogue = new TextDialogue
            {
                sentences = new[]
                {
                "Well Done!",
                }
            };
            TextTrigger.TriggerText(textDialogue);
            DisablePressurePad();
            EnableKey();

            GameObject ellen = GameObject.Find("Ellen");
            ellen.GetComponent<CharacterControllerTransition>().EnableCharacterController();
            ellen.GetComponent<CameraTransition>().TransitionToMainCamera();
        }
        else
        {
            TextTrigger.TriggerText(incorrectWordTextDialogue);
            MoveToNextEmptyLetter();
        }
    }

    void DisablePressurePad()
    {
        pressurePad.gameObject.SetActive(false); 
    }

    void EnableKey()
    {
        key.gameObject.SetActive(true);
    }
}
