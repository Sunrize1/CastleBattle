using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    private GameObject player;
    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject enemyPrefab;
    void Start()
    {
        if(PhotonNetwork.CountOfPlayers == 1)
        {
            player = PhotonNetwork.Instantiate(playerPrefab.name, playerPrefab.transform.position, Quaternion.identity);
        } else
        {
            player = PhotonNetwork.Instantiate(enemyPrefab.name, enemyPrefab.transform.position, Quaternion.identity);
        }
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.Destroy(player.gameObject);
    }
}
