using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class HUDManager : MonoBehaviour
{
    [Header ("Health Bar")]
    [SerializeField] Image healthBarImage;
    //[SerializeField] Color greenC;
    //[SerializeField] Color orangeC;
    //[SerializeField] Color redC;
    [SerializeField]Animator healthBarAnim;
    [SerializeField] Image bloodBorder;

    [Header("Score")]
    [SerializeField] TMP_Text scoreText;
    [SerializeField] Animator skullAnim;

    [Header("Game Over")]
    [SerializeField]CanvasGroup finalPanel;
    [SerializeField] TMP_Text finalText;
    [SerializeField] TMP_Text finalScoreText;
    [SerializeField] TMP_Text bestScoreText;
    [SerializeField] GameObject newBest;
    [SerializeField] TMP_Text timeRemainingText;

    public static HUDManager instance;

    float playerInitialHealth;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        bloodBorder.color = new Color(1, 1, 1, 0);
        finalPanel.gameObject.SetActive(false);
        newBest.SetActive(false);

        playerInitialHealth = GameManager.instance.Player.Health;

#if !UNITY_EDITOR
        timeRemainingText.gameObject.SetActive(false);
#endif
    }

    private void Update()
    {
        if (timeRemainingText.gameObject.activeSelf)
            timeRemainingText.text = GameManager.instance.RemainingTime.ToString("F0");
    }

    private void OnEnable()
    {
        EventsManager.OnPlayerDied += ShowGameOverScreen;
    }
    private void OnDisable()
    {
        EventsManager.OnPlayerDied -= ShowGameOverScreen;
    }

#region HealthBar
    public void UpdateHealthBar(float maxHealth, float damageTaken)
    {
        float r = damageTaken / maxHealth;

        healthBarImage.fillAmount -= r;

        //UpdateColor();

        UpdateBloodBorder();

        healthBarAnim.SetTrigger("update");

        if (healthBarImage.fillAmount < 0) healthBarImage.fillAmount = 0;
    }

    //void UpdateColor()
    //{
    //    if (healthBarImage.fillAmount <= 0.4f)
    //        healthBarImage.color = redC;
    //    else if (healthBarImage.fillAmount > 0.4f && healthBarImage.fillAmount <= 0.7f)
    //        healthBarImage.color = orangeC;
    //    else healthBarImage.color = greenC;
    //}

    void UpdateBloodBorder()
    {
        if (bloodBorder.color.a < 0.6f)
        {
            float a = bloodBorder.color.a;
            bloodBorder.color = new Color(1, 1, 1, a + (0.125f / playerInitialHealth));
        }
        else bloodBorder.color = new Color(1, 1, 1, 0.125f);
    }
    #endregion

    #region Score
    public void UpdateScore(float lastScore)
    {
        skullAnim.SetTrigger("pointsAdded");
        StartCoroutine(UpdateScoreText(lastScore));
    }

    IEnumerator UpdateScoreText(float lastScore)
    {
        while (lastScore < GameManager.instance.CurrentScore)
        {
            lastScore += Time.fixedDeltaTime * 150f;
            if (lastScore > GameManager.instance.CurrentScore) lastScore = GameManager.instance.CurrentScore;
            scoreText.text = lastScore.ToString("F0");
            yield return null;
        }
    }
#endregion

#region GameOver/MissionComplete
    public void ShowGameOverScreen(int id=0)
    {
         StartCoroutine(ShowPanel("GAME OVER", Color.red,2));
    }

    public void ShowMissionCompleteScreen()
    {
        StartCoroutine(ShowPanel("MISSION COMPLETE", Color.green,0));
    }

    IEnumerator ShowPanel(string t, Color c, int delay)
    {
        finalText.text = t;
        finalText.color = c;
        if (t.Contains("GAME OVER"))
        {
            if (PhotonNetwork.InRoom)
                yield return new WaitUntil(() => MultiplayerManager.instance.EveryoneDied());
            else yield return null;
        }
        finalPanel.gameObject.SetActive(true);
        finalPanel.alpha = 0;
        //GameManager.instance.CheckForBestScore();
        finalScoreText.text = GameManager.instance.CurrentScore.ToString();
        bestScoreText.text = GameManager.instance.CheckForBestScore().ToString();
        StartCoroutine(panelAlphaIncrease(delay));
    }

    IEnumerator panelAlphaIncrease(int delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        while (finalPanel.alpha < 1)
        {
            finalPanel.alpha += Time.unscaledDeltaTime;
            yield return null;
        }
        yield return new WaitForSecondsRealtime(0.25f);
        newBest.SetActive(GameManager.instance.CurrentScore >= GameManager.instance.BestSavedScore &&
                         GameManager.instance.CurrentScore != 0);
    }
#endregion
}
