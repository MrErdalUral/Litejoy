using _Game.Features.Upgrades.Config;
using UniRx;

namespace _Game.Features.Upgrades
{
    public interface IUpgradesModel
    {
        IReadOnlyReactiveProperty<float> GetUpgradeAmount(StatType statType);
        UpgradeStep GetNextUpgradeStep();
        IReadOnlyReactiveProperty<int> CurrentUpgradeIndex { get; }
        bool IsMaxLevel();
        int GetUpgradeCost();
    }
}