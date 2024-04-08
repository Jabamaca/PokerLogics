using TCSHoldEmPoker.Network.Define;

namespace TCSHoldEmPoker.Network.Activity.PokerGameInputs {
    public sealed class PlayerLeaveRequestGameInput : PokerGameInput {

        public override PokerGameInputType GameInputType => PokerGameInputType.PLAYER_REQUEST_LEAVE;

    }
}