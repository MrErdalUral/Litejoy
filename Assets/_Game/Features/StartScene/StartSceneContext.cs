using System;
using System.Collections.Generic;
using _Game.Features.Upgrades;
using _Game.Features.Upgrades.Config;
using _Game.Features.UpgradesPopup;
using UniRx;
using UnityEngine;

namespace _Game.Features.StartScene
{
    /// <summary>
    /// StartSceneContext is responsible for managing the context of the start scene,
    /// including initializing and disposing of related components.
    /// </summary>
    public class StartSceneContext : MonoBehaviour, IDisposable
    {
        private static StartSceneContext _instance;
        public static StartSceneContext Instance => _instance;

        [Header("Serialized References")] 
        [SerializeField]
        private UpgradesPopupView _upgradesPopupView;
        [SerializeField]
        private UpgradesConfig _upgradesConfig;

        private CompositeDisposable _disposables;
        private IUpgradesPopupModel _upgradesPopupModel;
        private IUpgradesModel _upgradesModel;

        public ICollection<IDisposable> Disposables => _disposables;
        public IUpgradesPopupModel UpgradesPopupModel => _upgradesPopupModel;

        private void Awake()
        {
            _instance ??= this;
            _disposables = new CompositeDisposable();
            _upgradesPopupModel = new UpgradesPopupModel();
            _upgradesModel = new UpgradesModel(_upgradesConfig,0);

            var upgradePopupPresenter = new UpgradesPopupPresenter(_disposables, _upgradesPopupModel, _upgradesPopupView, _upgradesModel);
            upgradePopupPresenter.Initialize();
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}