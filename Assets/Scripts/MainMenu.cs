using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class MainMenu : MonoBehaviourPunCallbacks
{
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.ConnectToRegion("ru");
    }

    public void QuickMatch()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            Debug.LogError("Client is not connected to MasterServer yet.");
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master Server");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        PhotonNetwork.CreateRoom(null, roomOptions, null);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("GameScene");
    }

    // Метод для выхода из игры
    public void QuitGame()
    {
        Debug.Log("Quit Game!");
        Application.Quit();
    }
}

