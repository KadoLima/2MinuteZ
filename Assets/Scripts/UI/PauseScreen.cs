using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScreen : MonoBehaviour
{
    [SerializeField] GameObject screen;
    bool canPause = false;
    // Start is called before the first frame update
    void Start()
    {
        screen.SetActive(false);
    }

    private void OnEnable()
    {
        EventsManager.OnMissionIsPlayable += SetPausable;
    }

    private void OnDisable()
    {
        EventsManager.OnMissionIsPlayable -= SetPausable;
    }

    public void SetPausable()
    {
        canPause = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        if (!canPause)
            return;

        screen.SetActive(!screen.activeSelf);
        

        if (screen.activeSelf)
        {
            GameManager.instance.IsPlayable = false;
            Time.timeScale = 0;
        }
        else
        {
            GameManager.instance.IsPlayable = true;
            Time.timeScale = 1;
        }

        CursorManager.instance.ChangeCursor();
            
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit()
    {
        Time.timeScale = 1;
        Application.Quit();
        //SceneManager.LoadScene(0);
    }

}
