using UniRx;

namespace _Game.Features.UpgradesPopup
{
    public interface IUpgradesPopupModel
    {
        IReactiveProperty<bool> IsShowing { get; }
    }
}