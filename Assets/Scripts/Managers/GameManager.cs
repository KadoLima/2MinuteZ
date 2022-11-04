using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;


[System.Serializable]
public class PersistentData
{
    public float bestScore;
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    float currentScore = 0;
    float bestSavedScore = 0;

    [Header("Game Rules")]
    [SerializeField] float timeToSurvive = 120f;
    [SerializeField] bool isHackMode;
    [SerializeField] GameObject helicopterEffect;


    //[Header("Joysticks")]
    //[SerializeField] FixedJoystick moveJoystick;
    //[SerializeField] FixedJoystick rotationFireJoystick;

    float remainingTime;

    bool isPlayable;
    bool endGame;

    [SerializeField] PlayerBehaviour player;

    public PlayerBehaviour Player
    {
        get => player;
        set => player = value;
    }

    public float CurrentScore
    {
        get => currentScore;
        set => currentScore = value;
    }

    public float BestSavedScore
    {
        get => bestSavedScore;
        set => bestSavedScore = value;
    }

    public bool IsPlayable
    {
        get => isPlayable;
        set => isPlayable = value;
    }

    public float RemainingTime
    {
        get => remainingTime;
    }

    public bool IsHackMode
    {
        get => isHackMode;
    }

    private void Awake()
    {
        Time.timeScale = 1;

        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        LoadGame();

        helicopterEffect.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
#if UNITY_EDITOR
        Debug.unityLogger.logEnabled = true;
#else
        Debug.unityLogger.logEnabled = false;
#endif

        remainingTime = timeToSurvive;

        Show_MissionStartSequence();

    }

    private void Update()
    {
        if (remainingTime <= 0 && !endGame)
        {
            EventsManager.OnMissionComplete?.Invoke();
            endGame = true;
        }
    }

    private void OnEnable()
    {
        EventsManager.OnPlayerDied += SaveGame;
        EventsManager.OnMissionIsPlayable += StartMission;
        EventsManager.OnMissionComplete += Show_MissionCompleteSequence;
    }

    private void OnDisable()
    {
        EventsManager.OnPlayerDied -= SaveGame;
        EventsManager.OnMissionIsPlayable -= StartMission;
        EventsManager.OnMissionComplete += Show_MissionCompleteSequence;
    }

    //public FixedJoystick GetJoystick(int a)
    //{
    //    if (a == 0)
    //        return moveJoystick;
    //    if (a == 1)
    //        return rotationFireJoystick;

    //    Debug.LogError("JOYSTICK NOT FOUND!");
    //    return null;
    //}

    #region Gameflow Controlling

    public void ReduceTimeScale()
    {
        Time.timeScale = 0.6f;
    }

    public void StartMission()
    {
        IsPlayable = true;
        StartCountdown();
    }

    public void StartCountdown()
    {
        StartCoroutine(Countdown());
    }

    IEnumerator Countdown()
    {
        while (remainingTime>0)
        {
           remainingTime -= Time.deltaTime;
            yield return null;
        }
        remainingTime = 0f;
    }

    public bool IsMissionComplete()
    {
        return RemainingTime == 0;
    }
    
    void Show_MissionStartSequence()
    {
        DialogueManager.instance.ShowFirstOpenSequence();
    }

    void Show_MissionCompleteSequence()
    {
        helicopterEffect.SetActive(true);
        DialogueManager.instance.ShowFinalDialogSequence();

        var audioSource = helicopterEffect.GetComponent<AudioSource>();
        SFXManager.instance.PlayHeliSound(audioSource);
    }
    #endregion

    #region Score Management
    public void AddPoints(float p)
    {
        float lastScore = currentScore;
        currentScore += p;
        HUDManager.instance.UpdateScore(lastScore);
    }
    #endregion

    #region Restart/Quit
    public void ReloadGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    #endregion

    #region Save/Load Data

    public float CheckForBestScore()
    {
        if (currentScore > bestSavedScore)
            return currentScore;

        return bestSavedScore;
    }

    public void SaveGame(int a=0)
    {
        PersistentData persistentData = new PersistentData();

        persistentData.bestScore = CheckForBestScore();


        string json = JsonUtility.ToJson(persistentData, true);
        try
        {
            File.WriteAllText(Application.persistentDataPath + "/PersistentData.dat", json);

        }
        catch (Exception e)
        {
            Debug.LogError($"FAILED TO SAVE ON {Application.persistentDataPath + "/PersistentData.dat"}, exception: {e}");
        }
    }

    public void LoadGame()
    {
        if (File.Exists(Application.persistentDataPath + "/PersistentData.dat"))
        {
            string json = File.ReadAllText(Application.persistentDataPath + "/PersistentData.dat");
            PersistentData persistentData = JsonUtility.FromJson<PersistentData>(json);
            bestSavedScore = persistentData.bestScore;
        }
    }

    public static void DeleteData()
    {
        if (File.Exists(Application.persistentDataPath + "/PersistentData.dat"))
        {
            File.Delete(Application.persistentDataPath + "/PersistentData.dat");
        }
    }
    #endregion
}
