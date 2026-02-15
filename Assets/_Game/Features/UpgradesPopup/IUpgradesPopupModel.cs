using UniRx;

namespace _Game.Features.UpgradesPopup
{
    public interface IUpgradesPopupModel
    {
        IReadOnlyReactiveProperty<bool> IsShowing { get; }
    }
}