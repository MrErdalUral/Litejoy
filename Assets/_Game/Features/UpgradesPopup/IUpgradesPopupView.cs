using System;
using UniRx;

namespace _Game.Features.UpgradesPopup
{
    public  interface IUpgradesPopupView
    {
        public void Show();
        public void Hide();
        public IObservable<Unit> OnCloseButtonClickedObservable { get; }
    }
}