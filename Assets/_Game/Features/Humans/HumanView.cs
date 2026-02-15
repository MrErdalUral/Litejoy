using System;
using DG.Tweening;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;

namespace _Game.Features.Humans
{
    public class HumanView : MonoBehaviour, IDisposable
    {
        [SerializeField] private HealthBarView _healthBar; 
        
        private readonly CompositeDisposable _disposables = new();
        private Tweener _shakeSequence;
        public HealthBarView HealthBar => _healthBar;

        public void Initialize(int count)
        {
            gameObject.name = $"Human_{count}";
            _healthBar.SetAmount(1);
        }

        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }

        public void Shake()
        {
            _shakeSequence ??= transform.DOShakeRotation(0.3f,strength:20f).SetAutoKill(false);
            _shakeSequence.Restart();
        }
        public void Dispose()
        {
            Destroy(gameObject);
        }
    }
}