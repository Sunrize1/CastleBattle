using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public List<Upgrade> upgrades;
    public Player player;

    private void Start()
    {
        // Инициализация улучшений с 0 уровнем
        upgrades = new List<Upgrade>
        {
            new Upgrade { type = UpgradeType.Damage, value = 5, baseCost = 20 },
            new Upgrade { type = UpgradeType.Armor, value = 10, baseCost = 20 },
            new Upgrade { type = UpgradeType.SpawnCount, value = 1, baseCost = 100 },
        };
    }

    public void ApplyUpgrade(UpgradeType type)
    {
        Upgrade upgrade = upgrades.Find(u => u.type == type && u.level < u.maxLevel);
        if (upgrade != null && player.money >= upgrade.GetCost())
        {
            player.money -= upgrade.GetCost();
            upgrade.ApplyUpgrade(player);
        }
    }
}

