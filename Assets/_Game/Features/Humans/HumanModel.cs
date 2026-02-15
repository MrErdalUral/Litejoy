using _Game.Features.Bosses;
using _Game.Features.PlayerWallet;
using _Game.Features.StartScene;
using _Game.Features.Upgrades;
using UniRx;
using UnityEngine;

namespace _Game.Features.Humans
{
    public class HumanModel
    {
        public const int BaseHealth = 10;
        public const int BaseDamage = 10;
     
        private readonly IReactiveProperty<int> _health = new ReactiveProperty<int>(BaseHealth);
        private int _startHealth;
        private int _damage;
        
        public IReadOnlyReactiveProperty<float> HealthPercent => _health
            .Select(x=>(float)x/_startHealth)
            .ToReadOnlyReactiveProperty();
        
        public Subject<Unit> OnDeadSubject { get; } = new();
        
        public bool IsDead() => _health.Value <= 0;
        
        public void Train()
        {
            var healthUpgradeAmount = (int) StartSceneContext.Instance.UpgradesModel.GetUpgradeAmount(StatType.Health).Value;
            var damageUpgradeAmount = (int) StartSceneContext.Instance.UpgradesModel.GetUpgradeAmount(StatType.Damage).Value;
            _health.Value = BaseHealth + healthUpgradeAmount;
            _startHealth = _health.Value;
            _damage = BaseDamage + damageUpgradeAmount;
            
        }

        public void AttackBoss(BossModel bossModel)
        {
            if (bossModel.IsAlive())
            {
                Wallet.AddCoins(_damage);
                bossModel.TakeDamage(_damage);
            }
        }

        public void TakeDamage(int damage)
        {
            _health.Value -= damage;
            
            if (IsDead())
            {
                Debug.Log("Human Dead!");
                OnDeadSubject.OnNext(Unit.Default);
            }
        }
    }
}