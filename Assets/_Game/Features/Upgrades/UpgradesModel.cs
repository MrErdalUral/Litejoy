using System;
using System.Collections.Generic;
using _Game.Features.Upgrades.Config;
using UniRx;
using Unity.VisualScripting;

namespace _Game.Features.Upgrades
{
    public enum StatType
    {
        Health = 0,
        Damage = 1,
    }
    
    public class UpgradesModel : IUpgradesModel
    {
        private readonly UpgradesConfig _upgradesConfig;
        private readonly IReactiveProperty<int> _currentUpgradeIndex;
        private readonly Dictionary<StatType, IReactiveProperty<float>> _upgradeAmounts;
        
        public IReadOnlyReactiveProperty<int> CurrentUpgradeIndex => _currentUpgradeIndex;
        
        public UpgradesModel(UpgradesConfig upgradesConfig, int currentUpgradeIndex)
        {
            _upgradesConfig = upgradesConfig;
            _currentUpgradeIndex = new ReactiveProperty<int>();
            _currentUpgradeIndex.Value = currentUpgradeIndex;
            
            _upgradeAmounts = new Dictionary<StatType, IReactiveProperty<float>>();
            foreach (StatType type in Enum.GetValues(typeof(StatType)))
            {
                _upgradeAmounts.Add(type, new ReactiveProperty<float>(0));
            }

            for (int i = 0; i < _upgradesConfig.UpgradeStepsList.Count && i < currentUpgradeIndex; i++)
            {
                var step = _upgradesConfig.UpgradeStepsList[i];
                _upgradeAmounts[step.statType].Value = step.UpgradeAmount;
            }
        }
        
        public IReadOnlyReactiveProperty<float> GetUpgradeAmount(StatType statType) => _upgradeAmounts[statType];
        public UpgradeStep GetUpgradeStep() => _upgradesConfig.UpgradeStepsList[CurrentUpgradeIndex.Value];
        public bool IsMaxLevel() => CurrentUpgradeIndex.Value >= _upgradesConfig.UpgradeStepsList.Count - 1;
        public int GetUpgradeCost() => IsMaxLevel() ? 0 : GetUpgradeStep().UpgradeCost;
        public void ApplyUpgrade()
        {
            var step = GetUpgradeStep();
            _upgradeAmounts[step.statType].Value = step.UpgradeAmount;
            _currentUpgradeIndex.Value++;
        }
    }
}