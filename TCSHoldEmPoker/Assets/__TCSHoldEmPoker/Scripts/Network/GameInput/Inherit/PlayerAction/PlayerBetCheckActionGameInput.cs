using TCSHoldEmPoker.Network.Define;

namespace TCSHoldEmPoker.Network.GameInputs {
    public sealed class PlayerBetCheckActionGameInput : PokerGameInput {

        public override PokerGameInputType GameInputType => PokerGameInputType.PLAYER_ACTION_BET_CHECK;

    }
}