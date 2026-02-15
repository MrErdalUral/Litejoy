using UniRx;

namespace _Game.Features.PlayerWallet
{
    public static class Wallet
    {
        private static readonly IReactiveProperty<int> _coins;
        public static IReadOnlyReactiveProperty<int> Coins => _coins;

        static Wallet()
        {
            _coins = new ReactiveProperty<int>(0);
        }

        public static void AddCoins(int amount)
        {
            _coins.Value += amount;
        }

        public static int GetCoins()
        {
            return _coins.Value;
        }

    }
}