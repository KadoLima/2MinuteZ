using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class MultiplayerManager : MonoBehaviourPunCallbacks
{
    List<Transform> coopPlayers = new List<Transform>();

    public static MultiplayerManager instance;

    float deadPlayers = 0;

    private void Awake()
    {
        instance = this;
    }

    public static void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        StartCoroutine(BackToLoadingScreen());
    }

    IEnumerator BackToLoadingScreen()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("Loading");
    }


    public void CoopPlayers(Transform t)
    {
        coopPlayers.Add(t);
    }

    [PunRPC]
    public void RPC_AddToDeadPlayersCount()
    {
        deadPlayers++;
    }

    public void SetRPC_AddToDeadPlayersCount()
    {
        this.photonView.RPC("RPC_AddToDeadPlayersCount", RpcTarget.All);
    }

    public bool EveryoneDied()
    {
        return deadPlayers >= coopPlayers.Count;
    }

    public List<Transform> GetCoopPlayers()
    {
        return coopPlayers;
    }
}
