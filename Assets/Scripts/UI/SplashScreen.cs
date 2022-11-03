using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour
{
    [SerializeField]TMP_Text[] splashTexts;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < splashTexts.Length; i++)
            splashTexts[i].color = new Color(1, 1, 1, 0);
        
        StartCoroutine(SplashSequence());
    }

    IEnumerator SplashSequence()
    {
        yield return new WaitForSeconds(0.5f);
        float alpha1 = splashTexts[0].color.a; //a game by

        while (alpha1 < 1)
        {
            alpha1 += Time.deltaTime;
            splashTexts[0].color = new Color(1, 1, 1, alpha1);
            yield return null;
        }

        yield return new WaitForSeconds(.25f);

        float alpha2 = splashTexts[1].color.a; //ricardo lima

        while (alpha2 < 1)
        {
            alpha2 += Time.deltaTime * 1.2f ;
            splashTexts[1].color = new Color(1, 1, 1, alpha2);
            yield return null;
        }

        yield return new WaitForSeconds(1.5f);

        float alpha3 = 1;

        while (alpha3>0)
        {
            alpha3 -= Time.deltaTime;

            for (int i = 0; i < splashTexts.Length; i++)
                splashTexts[i].color = new Color(1, 1, 1, alpha3);

            yield return null;
        }

        for (int i = 0; i < splashTexts.Length; i++)
            splashTexts[i].color = new Color(1, 1, 1, 0);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
