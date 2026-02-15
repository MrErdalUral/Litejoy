using System;
using System.Collections.Generic;
using UniRx;
using Unity.VisualScripting;

namespace _Game.Features.UpgradesPopup
{
    public class UpgradesPopupPresenter : IInitializable, IDisposable
    {
        private readonly CompositeDisposable _disposables;
        private readonly IUpgradesPopupView _view;
        private readonly IUpgradesPopupModel _model;

        public UpgradesPopupPresenter(ICollection<IDisposable>  contextDisposable, IUpgradesPopupModel model, IUpgradesPopupView view)
        {
            _disposables = new CompositeDisposable();
            _disposables.AddTo(contextDisposable);
            
            _view = view;
            _model = model;
        }

        public void Initialize()
        {
            _model.IsShowing.Subscribe(OnIsShowingChanged).AddTo(_disposables);
            _view.OnCloseButtonClickedObservable.Subscribe(_ => _model.IsShowing.Value = false).AddTo(_disposables);
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }

        private void OnIsShowingChanged(bool isShowing)
        {
            if(isShowing)
                _view.Show();
            else
                _view.Hide();
        }
    }
}