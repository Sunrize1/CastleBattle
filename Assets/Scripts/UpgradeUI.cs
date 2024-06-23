using UnityEngine;

public class UpgradeUI : MonoBehaviour
{
    public UpgradeManager upgradeManager;

    public void OnUpgradeDamage()
    {
        upgradeManager.ApplyUpgrade(UpgradeType.Damage);
    }

    public void OnUpgradeArmor()
    {
        upgradeManager.ApplyUpgrade(UpgradeType.Armor);
    }

    public void OnUpgradeSpawnCount()
    {
        upgradeManager.ApplyUpgrade(UpgradeType.SpawnCount);
    }
}