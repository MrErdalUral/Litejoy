using UniRx;

namespace _Game.Features.UpgradesPopup
{
    public class UpgradesPopupModel : IUpgradesPopupModel
    {
        public IReactiveProperty<bool> IsShowing { get; } = new ReactiveProperty<bool>(false);
    }
}