using System;
using _Game.Features.Bosses;
using UniRx;
using UnityEngine;

namespace _Game.Features.Humans
{
    public class HumanPresenter : IDisposable
    {
        private readonly CompositeDisposable _disposables = new();
        private readonly CompositeDisposable _stateDisposables = new();
        private readonly HumanView _humanView;
        private readonly HumanModel _humanModel;

        public HumanPresenter(HumanView humanView, HumanModel humanModel)
        {
            _humanView = humanView;
            _humanModel = humanModel;
        }

        public HumanModel Model => _humanModel;
        public HumanView View => _humanView;

        public void Initialize(int count)
        {
            _humanView.Initialize(count);
            _humanView.AddTo(_disposables);
            _stateDisposables.AddTo(_disposables);

            _humanModel.HealthPercent.Subscribe(_humanView.HealthBar.SetAmount).AddTo(_disposables);
            _humanModel.OnDeadSubject.Subscribe(_ => Dispose()).AddTo(_disposables);
        }

        public void StartAttacking(BossView bossView)
        {
            Observable.Interval(TimeSpan.FromMilliseconds(1000)).Subscribe(_ =>
                {
                    _humanModel.AttackBoss(bossView);
                    _humanView.Shake();
                })
                .AddTo(_stateDisposables) //Stop attacking if human dead
                .AddTo(bossView); //Stop attacking if boss dead
        }

        public void MoveTo(Vector3 targetPosition, Action<HumanPresenter> onReached)
        {
            var speed = 5;
            var startPosition = _humanView.transform.position;
            var distance = Vector3.Distance(startPosition, targetPosition);
            var duration = distance / speed;
            var elapsedTime = 0.0f;

            Observable.EveryUpdate().Subscribe(_ =>
            {
                elapsedTime += Time.deltaTime;
                _humanView.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);

                if (!(Vector3.Distance(_humanView.transform.position, targetPosition) <= 0.1f))
                    return;

                _stateDisposables.Clear();
                onReached(this);
            }).AddTo(_stateDisposables);
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}