using System;
using _Game.Features.Humans;
using _Game.Features.HumansState.Scripts.Combat;
using _Game.Features.HumansState.Scripts.Core;
using _Game.Features.HumansState.TrainSlot;
using _Game.Features.StartScene;
using UniRx;
using Unity.Mathematics;
using UnityEngine;

namespace _Game.Features.HumansState.Scripts.Training
{
    public class TrainingState : HumanState
    {
        private readonly Vector3 _startingPosition = new(0, -2.72f, 0);

        public override bool HasFreeSlot() => _isFree;

        private bool _isFree = true;

        public TrainingState(HumanStateController humanStateController, TrainSlotView trainSlotPrefab) : base(
            humanStateController)
        {
            var instance = GameObject.Instantiate(trainSlotPrefab, _startingPosition, quaternion.identity);
            SetupTrainSlotPresenter(trainSlotPrefab);
        }

        public void SetupTrainSlotPresenter(ITrainSlotView trainSlotView)
        {
            var trainSlotPresenter = new TrainSlotPresenter(StartSceneContext.Instance.Disposables, trainSlotView, StartSceneContext.Instance.UpgradesPopupModel);
            trainSlotPresenter.Initialize();
        }

        protected override void Enter(HumanView humanView)
        {
            _isFree = false;
            humanView.MoveTo(_startingPosition, _ => PrepareForCombatCurrentHuman(humanView));
        }

        private void PrepareForCombatCurrentHuman(HumanView humanView)
        {
            Observable.Timer(TimeSpan.FromMilliseconds(1000))
                .Subscribe(_ => OnFinished(humanView));
        }

        private void OnFinished(HumanView humanView)
        {
            humanView.Train();
            _isFree = true;
            humanStateController.TransitionTo<CombatState>(humanView);
        }
    }
}