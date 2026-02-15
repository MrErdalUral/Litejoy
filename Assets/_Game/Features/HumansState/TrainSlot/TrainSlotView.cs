using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Unit = UniRx.Unit;

public class TrainSlotView : MonoBehaviour, ITrainSlotView
{
    [SerializeField] private Button _upgradesButton;
    
    public IObservable<Unit> OnUpgradeButtonClickedObservable => _upgradesButton.OnClickAsObservable();
}