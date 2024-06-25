using Mirror;
using Spine.Unity;
using UnityEngine;

public class ClientMessageHandler : MonoBehaviour
{
    private void OnEnable()
    {
        NetworkClient.RegisterHandler<PlayerSetupMessage>(OnPlayerSetupMessage);
    }

    private void OnDisable()
    {
        NetworkClient.UnregisterHandler<PlayerSetupMessage>();
    }

    private void OnPlayerSetupMessage(PlayerSetupMessage msg)
    {
        if (NetworkClient.spawned.TryGetValue(msg.unitNetId, out NetworkIdentity unitIdentity) &&
            NetworkClient.spawned.TryGetValue(msg.playerNetId, out NetworkIdentity playerIdentity))
        {
            GameObject unitObject = unitIdentity.gameObject;
            GameObject playerObject = playerIdentity.gameObject;

            PlayerUnit newUnit = unitObject.GetComponent<PlayerUnit>();
            Player player = playerObject.GetComponent<Player>();

            if (newUnit != null && player != null)
            {
                newUnit.SetMainTarget(player.unitTarget);
                newUnit.SetPlayer(player);
                newUnit.attackEnemyFirst += player.unitDamage;
                newUnit.attackEnemySecond += player.unitDamage;
                newUnit.HP += player.unitArmor;
                player.playerUnits.Add(newUnit);
            }
        }
    }
}
