using TCSHoldEmPoker.Network.Define;

namespace TCSHoldEmPoker.Network.Inputs {
    public sealed class PlayerLeaveRequestGameInput : PokerGameInput {

        public override PokerGameInputType GameInputType => PokerGameInputType.PLAYER_REQUEST_LEAVE;

    }
}