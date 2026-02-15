using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TrainSlotView : MonoBehaviour
{
    [SerializeField] private Button _upgradesButton;
    
    public IObservable<Unit> OnUpgradeButtonClickedObservable => _upgradesButton.OnClickAsObservable();
}
