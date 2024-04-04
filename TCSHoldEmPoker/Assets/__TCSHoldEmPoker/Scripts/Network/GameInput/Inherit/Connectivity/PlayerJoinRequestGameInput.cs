using System;
using TCSHoldEmPoker.Network.Define;

namespace TCSHoldEmPoker.Network.Input {
    public sealed class PlayerJoinRequestGameInput : PokerGameInput {

        public override PokerGameInputType GameSignalType => PokerGameInputType.PLAYER_REQUEST_JOIN;

        public Int32 buyInChips;

    }
}