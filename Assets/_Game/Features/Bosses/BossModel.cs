using System;
using System.Collections.Generic;
using _Game.Features.Humans;
using UniRx;
using UnityEngine;

namespace _Game.Features.Bosses
{
    public class BossModel: IDisposable
    {
        public Action<List<HumanPresenter>> BossDefeatedCallback { get; set; }

        private readonly List<HumanPresenter> _attackers = new();
        public readonly Subject<Unit> OnAttack = new Subject<Unit>();
        public readonly Subject<Unit> OnDefeated = new Subject<Unit>();

        private float _lastAttackTime;
        private double _currentHp;
        private float _attackInterval;
        private int _targetsPerAttack;
        private int _damage;
        
        public BossModel(double hp, float attackInterval, int targetsPerAttack, double damage)
        {
            _currentHp = hp;
            _attackInterval = attackInterval;
            _targetsPerAttack = targetsPerAttack;
            _damage = (int)damage;
        }
        
        public bool IsAlive() => _currentHp > 0;
        
        public void TakeDamage(double damage)
        {
            _currentHp = Math.Max(0, _currentHp - damage);
            
            if (!(_currentHp <= 0))
                return;

            Debug.Log("Boss Defeated!");
            BossDefeatedCallback.Invoke(_attackers);
            OnDefeated.OnNext(Unit.Default);
        }

        public void RegisterAttacker(HumanPresenter human)
        {
            if (!_attackers.Contains(human))
            {
                _attackers.Add(human);
            }
        }

        public void AttackHumans()
        {
            if (!(Time.time - _lastAttackTime >= _attackInterval) || _attackers.Count <= 0) return;
            
            OnAttack.OnNext(Unit.Default);
                
            var defeatedHumans = new List<HumanPresenter>();
            for (var i = 0; i < Mathf.Min(_targetsPerAttack, _attackers.Count); i++)
            {
                var target = _attackers[i];
                target.Model.TakeDamage(_damage);
                if (target.Model.IsDead())
                {
                    defeatedHumans.Add(target);
                }
            }

            foreach (var defeatedHuman in defeatedHumans)
            {
                _attackers.Remove(defeatedHuman);
            }

            _lastAttackTime = Time.time;
        }

        public void Dispose()
        {
            
        }
    }
}