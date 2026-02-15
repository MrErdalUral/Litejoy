using _Game.Features.Humans;

namespace _Game.Features.HumansState.Scripts.Core
{
    public abstract class HumanState
    {
        protected readonly HumanStateController humanStateController;

        protected abstract void Enter(HumanPresenter humanPresenter);
        public abstract bool HasFreeSlot();

        protected HumanState(HumanStateController humanStateController)
        {
            this.humanStateController = humanStateController;
        }

        public void OnEnter(HumanPresenter humanView)
        {
            Enter(humanView);
        }
    }
}