//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using TMPro;

//public class Score : MonoBehaviour
//{
//    [SerializeField] TMP_Text scoreText;
//    [SerializeField] Animator skullAnim;
//    float currentScore=0;
//    float lastScore = 0;

//    public float CurrentScore
//    {
//        get => currentScore;
//        set => currentScore = value;
//    }

//    public static Score instance;

//    private void Awake()
//    {
//        instance = this;
//    }

//    // Start is called before the first frame update
//    void Start()
//    {
        
//    }

//    // Update is called once per frame
//    void Update()
//    {
//    }

//    IEnumerator RefreshScore()
//    {
//        while (lastScore < currentScore)
//        {
//            lastScore += Time.fixedDeltaTime*150f;
//            if (lastScore > currentScore) lastScore = currentScore;
//            scoreText.text = lastScore.ToString("F0");
//            yield return null;
//        }
//    }

//    public void AddPoints(float p)
//    {
//        skullAnim.SetTrigger("pointsAdded");
//        lastScore = currentScore;
//        currentScore += p;
//        StartCoroutine(RefreshScore());
//    }
//}
