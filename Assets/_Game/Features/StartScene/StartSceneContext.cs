using System;
using _Game.Features.UpgradesPopup;
using UniRx;
using UnityEngine;

/// <summary>
/// StartSceneContext is responsible for managing the context of the start scene,
/// including initializing and disposing of related components.
/// </summary>
public class StartSceneContext : MonoBehaviour,IDisposable
{
    private static StartSceneContext _instance;
    public static StartSceneContext Instance => _instance;

    [Header("UI References")]
    [SerializeField] private UpgradesPopupView _upgradesPopupView;
    
    private IUpgradesPopupModel _upgradesPopupModel;
    private CompositeDisposable _disposables;
    
    private void Awake()
    {
        _instance ??= this;
        _disposables = new CompositeDisposable();
        _upgradesPopupModel = new UpgradesPopupModel();
        
        var upgradePopupPresenter = new UpgradesPopupPresenter(_disposables, _upgradesPopupModel, _upgradesPopupView);
        upgradePopupPresenter.Initialize();
    }

    public void SetupTrainSlotPresenter(ITrainSlotView trainSlotView)
    {
        var trainSlotPresenter = new TrainSlotPresenter(_disposables, trainSlotView, _upgradesPopupModel);
        trainSlotPresenter.Initialize();
    }
    
    public void Dispose()
    {
        _disposables.Dispose();
    }
}