using System;
using UniRx;

namespace _Game.Features.UpgradesPopup
{
    public interface IUpgradesPopupView
    {
        IObservable<Unit> OnCloseButtonClickedObservable { get; }
        IObservable<Unit> OnUpgradeButtonClickedObservable { get; }
        bool UpgradeButtonInteractable { get; set; }
        void Show();
        void Hide();
        void UpdateHealthUpgradeText(float amount);
        void UpdateLevelText(int level);
        void UpdateUpgradeButtonText(bool isMaxLevel, int cost);
    }
}