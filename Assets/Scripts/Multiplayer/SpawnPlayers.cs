using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPlayers : MonoBehaviour
{
    public GameObject playerPrefab;
    public Transform[] playerStartPos;
    Vector3 chosenPos;
    float fixedY;

    private void Start()
    {
        fixedY = playerStartPos[0].transform.position.y;


        Vector3 p1Pos = new Vector3(playerStartPos[0].position.x, fixedY, playerStartPos[0].position.z);
        Vector3 p2Pos = new Vector3(playerStartPos[1].position.x, fixedY, playerStartPos[1].position.z);


        if (PhotonNetwork.InRoom)
        {
            if (PhotonNetwork.IsMasterClient)
                chosenPos = p1Pos;
            else chosenPos = p2Pos;
        }

        PhotonNetwork.Instantiate(playerPrefab.name, chosenPos, Quaternion.identity);
    }
}
