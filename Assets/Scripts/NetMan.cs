using Mirror;
using UnityEngine;

public class NetMan : NetworkManager
{
    public GameObject enemyPrefab;
    private int playersConnected = 0;

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        GameObject prefab = playersConnected % 2 == 0 ? playerPrefab : enemyPrefab;
        GameObject gameobject = Instantiate(prefab);
        NetworkServer.AddPlayerForConnection(conn, gameobject);

        playersConnected++;
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        base.OnServerDisconnect(conn);
        playersConnected--;
    }
}








