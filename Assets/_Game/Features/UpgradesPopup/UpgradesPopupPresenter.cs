using System;
using UniRx;
using Unity.VisualScripting;

namespace _Game.Features.UpgradesPopup
{
    public class UpgradesPopupPresenter : IInitializable,IDisposable
    {
        private readonly CompositeDisposable _disposables;
        private readonly IUpgradesPopupView _view;
        private readonly IUpgradesPopupModel _model;

        public UpgradesPopupPresenter(CompositeDisposable disposables,IUpgradesPopupView view, IUpgradesPopupModel model)
        {
            _disposables = disposables;
            _view = view;
            _model = model;
        }

        public void Initialize()
        {
            _model.IsShowing.Subscribe(OnIsShowingChanged).AddTo(_disposables);
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