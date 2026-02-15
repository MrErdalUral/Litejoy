using System.Collections.Generic;
using UnityEngine;

namespace _Game.Features.Upgrades.Config
{
    [CreateAssetMenu(fileName = "UpgradesConfig", menuName = "Config/UpgradesConfig")]
    public class UpgradesConfig : ScriptableObject
    {
        [SerializeField] 
        private List<UpgradeStep> _upgradeStepsList;
        
        public List<UpgradeStep> UpgradeStepsList => _upgradeStepsList;
    }

    [System.Serializable]
    public struct UpgradeStep
    {
        public int UpgradeCost;
        public float UpgradeAmount;
        public StatType statType;
    }
}