using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Mirror;
using Spine.Unity;

public struct PlayerSetupMessage : NetworkMessage
{
    public uint unitNetId;
    public uint playerNetId;
}

public class Player : NetworkBehaviour
{
    public List<PlayerUnit> playerUnits = new List<PlayerUnit>();
    public GameObject playerUnitPrefab;
    public Transform unitTarget;
    public TextMeshProUGUI moneyTextUI;

    [SyncVar] public int money = 0;
    [SyncVar] public int unitDamage = 0;
    [SyncVar] public int unitArmor = 0;
    [SyncVar] public int unitSpawnCount = 1;
    [SyncVar] public int level = 1;

    private int damageUpgrades = 0;
    private int armorUpgrades = 0;
    [Command]
    private void Start()
    {
        if (isLocalPlayer)
        {
            moneyTextUI = GameObject.FindFirstObjectByType<TextMeshProUGUI>();
            unitTarget = GameObject.FindGameObjectWithTag("Ebase").transform;
            CmdStartSpawningUnits();
        }
    }
    [Command]
    private void Update()
    {
        if (isLocalPlayer) {
            moneyTextUI.text = money.ToString();
        }
    }

    [Command]
    public void CmdStartSpawningUnits()
    {
        StartCoroutine(SpawnUnits());
    }

    private IEnumerator SpawnUnits()
    {
        while (true)
        {
            CmdSpawnUnitOnServer();
            yield return new WaitForSeconds(5f);
        }
    }

    [Command]
    public void CmdSpawnUnitOnServer()
    {
        Vector3 spawnPosition = GetSpawnPosition();
        GameObject newUnitObject = Instantiate(playerUnitPrefab, spawnPosition, Quaternion.identity);
        NetworkServer.Spawn(newUnitObject);

        // Отправляем сообщение на клиент
        PlayerSetupMessage msg = new PlayerSetupMessage
        {
            unitNetId = newUnitObject.GetComponent<NetworkIdentity>().netId,
            playerNetId = this.netId
        };
        NetworkServer.SendToAll(msg); // Отправка сообщения всем клиентам
    }

    Vector3 GetSpawnPosition()
    {
        float spawnRadius = 2f;
        Vector3 randomDirection = Random.insideUnitSphere * spawnRadius;
        randomDirection.y = 0;
        return transform.position + randomDirection;
    }
    [Command]
    public void AddMoney(int amount)
    {
        money += amount;
    }

    [Server]
    public void CmdApplyUpgrade(UpgradeType type, int value)
    {
        switch (type)
        {
            case UpgradeType.Damage:
                unitDamage += value;
                damageUpgrades++;
                break;
            case UpgradeType.Armor:
                unitArmor += value;
                armorUpgrades++;
                break;
            case UpgradeType.SpawnCount:
                unitSpawnCount += value;
                break;
        }

        UpdatePlayerLevel();
    }

    void UpdatePlayerLevel()
    {
        int minUpgrades = Mathf.Min(damageUpgrades, armorUpgrades);
        if (minUpgrades > 0)
        {
            level = minUpgrades + 1;
        }
    }
}




