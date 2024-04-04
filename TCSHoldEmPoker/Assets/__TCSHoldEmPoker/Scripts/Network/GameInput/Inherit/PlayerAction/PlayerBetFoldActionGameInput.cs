using TCSHoldEmPoker.Network.Define;

namespace TCSHoldEmPoker.Network.Inputs {
    public sealed class PlayerBetFoldActionGameInput : PokerGameInput {

        public override PokerGameInputType GameInputType => PokerGameInputType.PLAYER_ACTION_BET_FOLD;

    }
}