using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Spine.Unity;
using Drawinguy;



public class Enemy : MonoBehaviour
{
    public List<EnemyUnit> enemyUnits = new List<EnemyUnit>();
    public GameObject playerUnitPrefab;
    public Transform unitTarget;
    public TextMeshProUGUI moneyTextUI;
    

    public int money = 0;
    public int unitDamage = 0;
    public int unitArmor = 0;
    public int unitSpawnCount = 1;
    public int level = 1;

    private int damageUpgrades = 0;
    private int armorUpgrades = 0;
    
    private void Start()
    {
        moneyTextUI = GameObject.FindFirstObjectByType<TextMeshProUGUI>();
        StartCoroutine(SpawnUnits());
    }
    
    private void Update()
    {
        unitTarget = GameObject.FindGameObjectWithTag("Pbase").transform;
    }

   
        
    private IEnumerator SpawnUnits()
    {
        while (unitTarget != null)
        {
            SpawnUnit();
            yield return new WaitForSeconds(5f);
        }
    }
    
    public void SpawnUnit()
    {
        Vector3 spawnPosition = GetSpawnPosition();
        GameObject newUnitObject = Instantiate(playerUnitPrefab, spawnPosition, Quaternion.identity);
        EnemyUnit newUnit = newUnitObject.GetComponent<EnemyUnit>();
        newUnit.SetMainTarget(unitTarget);
        newUnit.SetEnemy(this);
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

    
    public void AddMoney(int amount)
    {
        money += amount;
    }

    
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
