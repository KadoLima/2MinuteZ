using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] float delayToStart;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadAfterDelay(delayToStart));
    }

    IEnumerator LoadAfterDelay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
