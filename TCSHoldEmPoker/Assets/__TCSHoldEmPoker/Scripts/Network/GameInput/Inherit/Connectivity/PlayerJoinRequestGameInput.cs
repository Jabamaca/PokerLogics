using System;
using TCSHoldEmPoker.Network.Define;

namespace TCSHoldEmPoker.Network.Inputs {
    public sealed class PlayerJoinRequestGameInput : PokerGameInput {

        public override PokerGameInputType GameInputType => PokerGameInputType.PLAYER_REQUEST_JOIN;

        public Int32 buyInChips;

    }
}