using System;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;

namespace _Game.Features.Bosses
{
    public class BossPresenter : IInitializable, IDisposable
    {
        private readonly BossView _bossView;
        private readonly BossModel _model;
        private readonly CompositeDisposable _disposables;

        public BossModel Model => _model;
        public BossView View => _bossView;

        public BossPresenter(BossView bossView, BossModel model)
        {
            _bossView = bossView;
            _model = model;
            _disposables = new();
        }

        public void Initialize()
        {
            _bossView.AddTo(_disposables);
            
            Observable.EveryUpdate().Subscribe(_ =>
            {
                if (!_model.IsAlive())
                    return;

                _model.AttackHumans();
            }).AddTo(_disposables);
            
            _model.OnAttack.Subscribe(_ =>
            {
                _bossView.Shake();
            }).AddTo(_disposables);
            
            _model
                .OnDefeated
                .Subscribe(_ => _disposables.Dispose())
                .AddTo(_disposables);
        }
        
        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}