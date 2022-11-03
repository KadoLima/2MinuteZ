using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Realtime;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    public TMP_InputField hostRoom_InputField;
    public TMP_InputField joinRoom_InputField;

    [Space(10)]

    public Button createButton;
    public Button joinButton;

    private void Start()
    {
        createButton.gameObject.SetActive(false);
        joinButton.gameObject.SetActive(false);
    }

    #region Photon Multiplayer

    public void HostRoom()
    {
        PhotonNetwork.CreateRoom(hostRoom_InputField.text);
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(joinRoom_InputField.text);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("WaitingRoom");
    }

    public void EnableCreateButton()
    {
        createButton.gameObject.SetActive(hostRoom_InputField.text != string.Empty);
    }

    public void EnableJoinButton()
    {
        joinButton.gameObject.SetActive(joinRoom_InputField.text != string.Empty);
    }

    #endregion
    #region Play Solo
    public void PlaySolo()
    {
        SceneManager.LoadScene("GameOffline");
    }
    #endregion
}
