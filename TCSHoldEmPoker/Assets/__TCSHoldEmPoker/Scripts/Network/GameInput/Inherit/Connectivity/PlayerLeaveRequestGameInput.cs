using TCSHoldEmPoker.Network.Define;

namespace TCSHoldEmPoker.Network.Input {
    public sealed class PlayerLeaveRequestGameInput : PokerGameInput {

        public override PokerGameInputType GameSignalType => PokerGameInputType.PLAYER_REQUEST_LEAVE;

    }
}