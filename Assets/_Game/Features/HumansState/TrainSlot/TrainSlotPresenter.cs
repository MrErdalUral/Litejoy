using System;
using System.Collections.Generic;
using _Game.Features.UpgradesPopup;
using UniRx;
using Unity.VisualScripting;

public class TrainSlotPresenter : IInitializable, IDisposable
{
    private readonly CompositeDisposable _disposables;
    private readonly TrainSlotView _view;
    private readonly IUpgradesPopupModel _model;
    
    public TrainSlotPresenter(ICollection<IDisposable> contextDisposable, TrainSlotView view, IUpgradesPopupModel model)
    {
        _disposables = new CompositeDisposable();
        _disposables.AddTo(contextDisposable);
        
        _view = view;
        _model = model;
    }

    public void Initialize()
    {
        _view.OnUpgradeButtonClickedObservable
            .Subscribe(_ => _model.IsShowing.Value = true)
            .AddTo(_disposables);
    }

    public void Dispose()
    {
        _disposables.Dispose();
    }
}