using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Unit = UniRx.Unit;

namespace _Game.Features.HumansState.TrainSlot
{
    public class TrainSlotView : MonoBehaviour, ITrainSlotView
    {
        [SerializeField] private Button _upgradesButton;

        public IObservable<Unit> OnUpgradeButtonClickedObservable => _upgradesButton.OnClickAsObservable();
    }
}