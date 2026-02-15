using System;
using System.Collections.Generic;
using _Game.Features.PlayerWallet;
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

            _view.OnCloseButtonClickedObservable
                .Subscribe(_ => _popupModel.IsShowing.Value = false)
                .AddTo(_disposables);
            
            Wallet.Coins.Select(x=> x >= _upgradesModel.GetUpgradeCost())
                .Subscribe(x=> _view.UpgradeButtonInteractable = x)
                .AddTo(_disposables);
            
            _view.OnUpgradeButtonClickedObservable
                .Where(_=>!_upgradesModel.IsMaxLevel() && Wallet.GetCoins() >= _upgradesModel.GetUpgradeCost())
                .Subscribe(_ => OnUpgrade())
                .AddTo(_disposables);

            _upgradesModel
                .GetUpgradeAmount(StatType.Health)
                .Subscribe(_view.UpdateHealthUpgradeText)
                .AddTo(_disposables);
            
            _upgradesModel
                .GetUpgradeAmount(StatType.Damage)
                .Subscribe(_view.UpdateDamageUpgradeText)
                .AddTo(_disposables);

            _upgradesModel.CurrentUpgradeIndex.Select(i => i + 1) //index starts from 0
                .Subscribe(_view.UpdateLevelText).AddTo(_disposables);

            _upgradesModel.CurrentUpgradeIndex.Select(i => (isMax: _upgradesModel.IsMaxLevel(), cost: _upgradesModel.GetUpgradeCost())) //index starts from 0
                .Subscribe(x => _view.UpdateUpgradeButtonText(x.isMax, x.cost)).AddTo(_disposables);
            
        }

        public void OnUpgrade()
        {
            Wallet.AddCoins(-_upgradesModel.GetUpgradeCost());//Subtract upgrade cost from wallet
            _upgradesModel.ApplyUpgrade();
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