using TCSHoldEmPoker.Network.Define;

namespace TCSHoldEmPoker.Network.Input {
    public sealed class PlayerBetFoldActionGameInput : PokerGameInput {

        public override PokerGameInputType GameSignalType => PokerGameInputType.PLAYER_ACTION_BET_FOLD;

    }
}