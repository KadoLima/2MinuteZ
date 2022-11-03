using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TypewriterEffect : MonoBehaviour
{
    TMP_Text uiText;
    string textToWrite;
    int characterIndex;
    float timePerCharacter;
    float timer;
    bool invisibleCharacters;

    public void AddWriter(TMP_Text uiText, string textToWrite,float timePerCharacter, bool invisibleCharacters)
    {
        this.uiText = uiText;
        this.textToWrite = textToWrite;
        this.timePerCharacter = timePerCharacter;
        characterIndex = 0;
    }

    private void Update()
    {
        if (uiText != null)
        {
            timer -= Time.deltaTime;
            while (timer < 0)
            {
                //show next char
                timer += timePerCharacter;
                characterIndex++;
                string text = textToWrite.Substring(0, characterIndex);
                if (invisibleCharacters)
                {
                    text += "<color=#00000000>" + textToWrite.Substring(characterIndex) +"</color>";
                }
                uiText.text = text;

                if (characterIndex >= textToWrite.Length)
                {
                    uiText = null;
                    return;
                }
            }
        }
    }

}
