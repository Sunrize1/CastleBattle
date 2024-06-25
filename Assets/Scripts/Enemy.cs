using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Mirror;
using Spine.Unity;

public class Enemy : NetworkBehaviour
{
    public List<EnemyUnit> enemyUnits = new List<EnemyUnit>();
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
    [Server]
    private void Start()
    {
        if (!isLocalPlayer)
        {
            moneyTextUI = GameObject.FindFirstObjectByType<TextMeshProUGUI>();
            unitTarget = GameObject.FindGameObjectWithTag("Pbase").transform;
            StartSpawningUnits();
        }
    }
    [Server]
    private void Update()
    {
        if (moneyTextUI != null)
        {
            moneyTextUI.text = money.ToString();
        }
    }

    [Server]
    public void StartSpawningUnits()
    {
        StartCoroutine(SpawnUnits());
    }

    [Server]
    private IEnumerator SpawnUnits()
    {
        while (true)
        {
            CmdSpawnUnitOnServer();
            yield return new WaitForSeconds(5f);
        }
    }

    [Server]
    public void CmdSpawnUnitOnServer()
    {
        Vector3 spawnPosition = GetSpawnPosition();
        GameObject newUnitObject = Instantiate(playerUnitPrefab, spawnPosition, Quaternion.identity);
        EnemyUnit newUnit = newUnitObject.GetComponent<EnemyUnit>();
        newUnit.SetEnemy(this);
        NetworkServer.Spawn(newUnitObject);
        RpcSetupUnit(newUnitObject);
    }

    [Server]
    public void RpcSetupUnit(GameObject newUnitObject)
    {
        EnemyUnit newUnit = newUnitObject.GetComponent<EnemyUnit>();
        newUnit.SetMainTarget(unitTarget);
        newUnit.attackEnemyFirst += unitDamage;
        newUnit.attackEnemySecond += unitDamage;
        newUnit.HP += unitArmor;
        enemyUnits.Add(newUnit);
    }

    Vector3 GetSpawnPosition()
    {
        float spawnRadius = 2f;
        Vector3 randomDirection = Random.insideUnitSphere * spawnRadius;
        randomDirection.y = 0;
        return transform.position + randomDirection;
    }

    [Server]
    public void AddMoney(int amount)
    {
        money += amount;
    }

    [Command]
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
