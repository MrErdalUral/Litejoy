using System;
using UniRx;

namespace _Game.Features.UpgradesPopup
{
    public interface IUpgradesPopupView
    {
        IObservable<Unit> OnCloseButtonClickedObservable { get; }
        IObservable<Unit> OnUpgradeButtonClickedObservable { get; }
        IObservable<bool> OnPointerHoverUpgradeButtonObservable { get; }
        void Show();
        void Hide();
        void UpdateHealthUpgradeText(float amount,bool isGreen=false );
        void UpdateLevelText(int level);
        void UpdateUpgradeButtonText(bool isMaxLevel, int cost);
    }
}