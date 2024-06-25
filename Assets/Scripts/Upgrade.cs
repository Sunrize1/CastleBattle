using Mirror;
using UnityEngine;

public enum UpgradeType
{
    Damage,
    Armor,
    SpawnCount,
 }

[System.Serializable]
public class Upgrade
{
    public UpgradeType type;
    public int level = 0;
    public int maxLevel = 5;
    public int baseCost;
    public int value;

    public int GetCost()
    {
        return baseCost * (level + 1);
    }
 
    public void ApplyUpgrade(Player player)
    {
        if (level < maxLevel)
        {
            level++;
            switch (type)
            {
                case UpgradeType.Damage:
                    player.CmdApplyUpgrade(type, value);
                    break;
                case UpgradeType.Armor:
                    player.CmdApplyUpgrade(type, value);
                    break;
                case UpgradeType.SpawnCount:
                    player.CmdApplyUpgrade(type, value);
                    break;
            }
        }
    }
}

