using System;
using System.Collections.Generic;
using _Game.Features.Humans;
using _Game.Features.HumansState.Scripts.Core;
using _Game.Features.HumansState.Scripts.Training;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Game.Features.HumansState.Scripts.Waiting
{
    public class WaitingState : HumanState
    {
        private readonly Dictionary<int, CompositeDisposable> _disposables = new();

        public override bool HasFreeSlot() => true;

        public WaitingState(HumanStateController humanStateController) : base(humanStateController)
        {
        }

        protected override void Enter(HumanPresenter humanPresenter)
        {
            var position = new Vector3(Random.Range(-1f, 1f), -4, 0);
            humanPresenter.MoveTo(position, OnReachedWaitingSlot);
        }

        private void OnReachedWaitingSlot(HumanPresenter humanPresenter)
        {
            _disposables.Add(humanPresenter.View.GetInstanceID(), new CompositeDisposable());
            Observable.Timer(TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(0.5f))
                .Where(_ => humanStateController.FreeSlotIn<TrainingState>())
                .Subscribe(_ => FreeGridSlotAndMoveHumanToCombatPreparation(humanPresenter))
                .AddTo(_disposables[humanPresenter.View.GetInstanceID()]);
        }

        private void FreeGridSlotAndMoveHumanToCombatPreparation(HumanPresenter humanPresenter)
        {
            _disposables[humanPresenter.View.GetInstanceID()].Dispose();
            _disposables.Remove(humanPresenter.View.GetInstanceID());
            humanStateController.TransitionTo<TrainingState>(humanPresenter);
        }
    }
}