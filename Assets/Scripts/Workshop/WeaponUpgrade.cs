public interface IWeaponUpgrade
{
    void OnUpgradeSight();
    void OnUpgradeBarrel();
    void OnUpgradeLaser();
    bool IsSightUpgraded { get; }
    bool IsBarrelUpgraded { get; }
    bool IsLaserUpgraded { get; }
    bool IsFullyUpgraded { get; }
}