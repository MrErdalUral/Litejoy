using System;
using UniRx;

namespace _Game.Features.HumansState.TrainSlot
{
    public interface ITrainSlotView
    {
        IObservable<Unit> OnUpgradeButtonClickedObservable { get; }
    }
}
