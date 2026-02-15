using UniRx;

namespace _Game.Features.UpgradesPopup
{
    public class UpgradesPopupModel : IUpgradesPopupModel
    {
        public IReadOnlyReactiveProperty<bool> IsShowing { get; } = new ReactiveProperty<bool>(false);
    }
}