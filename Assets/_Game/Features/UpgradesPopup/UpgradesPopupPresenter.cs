using System;
using System.Collections.Generic;
using _Game.Features.Upgrades;
using UniRx;
using Unity.VisualScripting;

namespace _Game.Features.UpgradesPopup
{
    public class UpgradesPopupPresenter : IInitializable, IDisposable
    {
        private readonly CompositeDisposable _disposables;
        private readonly IUpgradesPopupView _view;
        private readonly IUpgradesPopupModel _popupModel;
        private readonly IUpgradesModel _upgradesModel;

        public UpgradesPopupPresenter(ICollection<IDisposable> contextDisposable, IUpgradesPopupModel popupModel, IUpgradesPopupView view, IUpgradesModel upgradesModel)
        {
            _disposables = new CompositeDisposable();
            _disposables.AddTo(contextDisposable);

            _view = view;
            _upgradesModel = upgradesModel;
            _popupModel = popupModel;
        }

        public void Initialize()
        {
            _popupModel.IsShowing.Subscribe(OnIsShowingChanged).AddTo(_disposables);

            _view.OnCloseButtonClickedObservable.Subscribe(_ => _popupModel.IsShowing.Value = false).AddTo(_disposables);

            _view.OnUpgradeButtonClickedObservable.Subscribe(_ => OnUpgrade()).AddTo(_disposables);
            
            var pointerHoverNextUpgrade = _view
                .OnPointerHoverUpgradeButtonObservable
                .Where(_=>_upgradesModel.IsMaxLevel())
                .Select(isHover => (isHover, nextStep: _upgradesModel.GetNextUpgradeStep()))
                .Where(x=>x.nextStep.statType == StatType.Health);

            _upgradesModel
                .GetUpgradeAmount(StatType.Health)
                .CombineLatest(pointerHoverNextUpgrade, 
                    (currentAmount,y)=>(currentAmount, y.isHover, y.nextStep))
                .Subscribe(x =>_view
                    .UpdateHealthUpgradeText(x.isHover ? x.nextStep.UpgradeAmount : x.currentAmount, x.isHover))
                .AddTo(_disposables);

            _upgradesModel.CurrentUpgradeIndex.Select(i => i + 1) //index starts from 0
                .Subscribe(_view.UpdateLevelText).AddTo(_disposables);

            _upgradesModel.CurrentUpgradeIndex.Select(i => (isMax: _upgradesModel.IsMaxLevel(), cost: _upgradesModel.GetUpgradeCost())) //index starts from 0
                .Subscribe(x => _view.UpdateUpgradeButtonText(x.isMax, x.cost)).AddTo(_disposables);
        }

        public void OnUpgrade()
        {
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }

        private void OnIsShowingChanged(bool isShowing)
        {
            if (isShowing)
                _view.Show();
            else
                _view.Hide();
        }
    }
}