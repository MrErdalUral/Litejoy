using System;
using _Game.Features.Bosses;
using _Game.Features.PlayerWallet;
using _Game.Features.StartScene;
using _Game.Features.Upgrades;
using UniRx;
using UnityEngine;

namespace _Game.Features.Humans
{
    public class HumanView : MonoBehaviour
    {
        public const int BaseHealth = 10;
        public const int BaseDamage = 10;
        
        [SerializeField] private HealthBarView _healthBar; 

        private readonly CompositeDisposable _disposables = new();
        private int _startHealth;
        private int _health;
        private int _damage;

        public bool IsDead() => _health <= 0;

        public void Initialize(int count)
        {
            gameObject.name = $"Human_{count}";
        }

        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }

        public void MoveTo(Vector3 targetPosition, Action<HumanView> onReached)
        {
            var speed = 5;
            var startPosition = transform.position;
            var distance = Vector3.Distance(startPosition, targetPosition);
            var duration = distance / speed;
            var elapsedTime = 0.0f;

            Observable.EveryUpdate()
                .Subscribe(_ =>
                {
                    elapsedTime += Time.deltaTime;
                    transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);

                    if (!(Vector3.Distance(transform.position, targetPosition) <= 0.1f))
                        return;

                    onReached(this);
                    _disposables.Clear();
                })
                .AddTo(_disposables);
        }

        public void Train()
        {
            var healthUpgradeAmount = (int) StartSceneContext.Instance.UpgradesModel.GetUpgradeAmount(StatType.Health).Value;
            var damageUpgradeAmount = (int) StartSceneContext.Instance.UpgradesModel.GetUpgradeAmount(StatType.Damage).Value;
            _health = BaseHealth + healthUpgradeAmount;
            _startHealth = _health;
            _damage = BaseDamage + damageUpgradeAmount;
            
            _healthBar.SetAmount((float)_health / _startHealth);
        }

        public void StartAttacking(BossView bossView)
        {
            Observable.Interval(TimeSpan.FromMilliseconds(1000))
                .Subscribe(_ => AttackBoss(bossView))
                .AddTo(this) //Stop attacking if human dead
                .AddTo(bossView); //Stop attacking if boss dead
        }

        private void AttackBoss(BossView bossView)
        {
            if (bossView.IsAlive())
            {
                Wallet.AddCoins(_damage);
                bossView.TakeDamage(_damage);
            }
        }

        public void TakeDamage(int damage)
        {
            _health -= damage;
            _healthBar.SetAmount((float)_health / _startHealth);
            
            if (IsDead())
            {
                Debug.Log("Human Dead!");
                Destroy(gameObject);
            }
        }
    }
}