public interface IWeaponUpgrade
{
    bool UpgradeRange { get; set; }
    bool UpgradeDamage { get; set; }
    bool UpgradeAccuracy { get; set; }
    bool IsFullyUpgraded { get; }
}