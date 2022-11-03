using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FPSCounter : MonoBehaviour
{
    [SerializeField] TMP_Text fpsCounterText;

    int lastFrameIndex;
    float[] frameDeltaTimeArray;
   

    private void Start()
    {
        //fpsCounterText.gameObject.SetActive(GameManager.instance.IsHackMode);

        //if (!GameManager.instance.IsHackMode)
        //    return;

        frameDeltaTimeArray = new float[50];
    }

    private void Update()
    {
        //if (!GameManager.instance.IsHackMode)
        //    return;

        frameDeltaTimeArray[lastFrameIndex] = Time.deltaTime;
        lastFrameIndex = (lastFrameIndex + 1) % frameDeltaTimeArray.Length;

        fpsCounterText.text = Mathf.RoundToInt(CalculateFPS()).ToString();
    }

    float CalculateFPS()
    {
        float total = 0f;
        foreach (float deltaTime in frameDeltaTimeArray)
        {
            total += deltaTime;
        }

        return frameDeltaTimeArray.Length / total;
    }
}

