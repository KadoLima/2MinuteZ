using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;

public class WaitingRoom : MonoBehaviour
{
    [SerializeField] GameObject uiToHide;
    [SerializeField] GameObject startingText;
    [SerializeField]TMP_Text countdownText;
    [SerializeField] float timeToWait = 20f;
    [SerializeField] GameObject playButton;

    // Start is called before the first frame update
    void Start()
    {
        startingText.SetActive(false);
        uiToHide.SetActive(true);
        playButton.SetActive(PhotonNetwork.IsMasterClient);

        StartCoroutine(StartMultiplayerGame());
    }

    // Update is called once per frame
    void Update()
    {
        timeToWait -= Time.deltaTime;
        countdownText.text = "Game starting in " + timeToWait.ToString("F0");
    }

    IEnumerator StartMultiplayerGame()
    {
        yield return new WaitUntil(() => PhotonNetwork.CurrentRoom.PlayerCount > 1 || timeToWait <=0);
        uiToHide.SetActive(false);
        startingText.SetActive(true);
        yield return new WaitForSeconds(2);
        LoadGame();
    }

    public void LoadGame() //botão PLAY
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;
        SceneManager.LoadScene("Game");
    }
}
