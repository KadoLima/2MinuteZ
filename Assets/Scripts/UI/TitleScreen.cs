using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{

    [SerializeField] Image transitionImage;
    [SerializeField] GameObject tapAnywhereText;
    [SerializeField] GameObject button;
    //[SerializeField] GameObject loginButtons;

    [SerializeField] float fadeInVelocity;
    [SerializeField] float fadeOutVelocity;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        tapAnywhereText.SetActive(false);
        transitionImage.gameObject.SetActive(true);
        transitionImage.color = new Color(0, 0, 0, 1);
        button.SetActive(false);
        //,loginButtons.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FadeIn());
    }

    private void Update()
    {
        if (Input.anyKeyDown && button.activeSelf)
        {
            TappedAnywhere();
        }
    }

    public void GoToNextScene()
    {
        StartCoroutine(FadeOut(true));
        button.SetActive(false);
    }

    public void TappedAnywhere() //tb é chamado pelo inspector dentro do botão "Button"
    {
        button.SetActive(false);
        tapAnywhereText.SetActive(false);
        GoToNextScene();
        //loginButtons.SetActive(true);
    }

    public void PlayWithoutLogin()
    {
        //loginButtons.SetActive(false);
        GoToNextScene();
    }

    IEnumerator FadeIn()
    {
        float alpha = 1;

        while (alpha>0)
        {
            alpha -= Time.deltaTime * fadeInVelocity;
            transitionImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        tapAnywhereText.SetActive(true);
        button.SetActive(true);
    }

    IEnumerator FadeOut(bool hasLoadScene)
    {
        float alpha = 0;

        while (alpha <1)
        {
            alpha += Time.deltaTime * fadeOutVelocity;
            transitionImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        if (hasLoadScene)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
