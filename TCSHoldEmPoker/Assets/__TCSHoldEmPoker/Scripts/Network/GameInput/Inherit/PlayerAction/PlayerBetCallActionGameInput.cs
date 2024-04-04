using TCSHoldEmPoker.Network.Define;

namespace TCSHoldEmPoker.Network.Input {
    public sealed class PlayerBetCallActionGameInput : PokerGameInput {

        public override PokerGameInputType GameSignalType => PokerGameInputType.PLAYER_ACTION_BET_CALL;

    }
}