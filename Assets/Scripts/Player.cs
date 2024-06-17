using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class Player : MonoBehaviour
{
    public List<PlayerUnit> playerUnits = new List<PlayerUnit>();
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
        // Запуск корутины для добавления юнитов каждые 5 секунд
        StartCoroutine(AddUnitEveryFiveSeconds());
    }

    private void Update()
    {
        moneyTextUI.text = money.ToString();  
    }

    IEnumerator AddUnitEveryFiveSeconds()
    {
        while (true)
        {
            for (int i = 0; i < unitSpawnCount; i++)
            {
                AddNewUnit();
            }
            yield return new WaitForSeconds(5f);
        }
    }

    void AddNewUnit()
    {
        // Создание нового юнита и добавление его в массив и сцену
        GameObject newUnitObject = Instantiate(playerUnitPrefab, GetSpawnPosition(), Quaternion.identity);
        PlayerUnit newUnit = newUnitObject.GetComponent<PlayerUnit>();
        newUnit.SetMainTarget(unitTarget);
        newUnit.SetPlayer(this);
        newUnit.attackEnemyFirst += unitDamage;
        newUnit.attackEnemySecond += unitDamage;
        newUnit.HP += unitArmor;
        playerUnits.Add(newUnit);
    }

    Vector3 GetSpawnPosition()
    {
        // Возвращает позицию рядом с объектом Player
        float spawnRadius = 2f; // Радиус спавна вокруг объекта Player
        Vector3 randomDirection = Random.insideUnitSphere * spawnRadius;
        randomDirection.y = 0; // Оставляем на той же высоте
        return transform.position + randomDirection;
    }

    public void AddMoney(int amount)
    {
        money += amount;
        Debug.Log("Money added. Total money: " + money);
    }

    public void ApplyUpgrade(UpgradeType type, int value, GameObject newSkin = null)
    {
        switch (type)
        {
            case UpgradeType.Damage:
                unitDamage += value;
                damageUpgrades++;
                UpdatePlayerLevel();
                break;
            case UpgradeType.Armor:
                unitArmor += (int)value;
                armorUpgrades++;
                UpdatePlayerLevel();
                break;
            case UpgradeType.SpawnCount:
                unitSpawnCount += (int)value;
                break;
        }
    }

    private void UpdatePlayerLevel()
    {
        int minUpgrades = Mathf.Min(damageUpgrades, armorUpgrades);
        if (minUpgrades > 0)
        {
            level = minUpgrades + 1;
        }
    }
}