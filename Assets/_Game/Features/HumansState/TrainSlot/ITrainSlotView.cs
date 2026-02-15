using System;
using UniRx;

public interface ITrainSlotView
{
    IObservable<Unit> OnUpgradeButtonClickedObservable { get; }
}