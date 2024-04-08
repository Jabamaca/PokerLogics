using System;
using TCSHoldEmPoker.Network.Define;

namespace TCSHoldEmPoker.Network.Activity.PokerGameInputs {
    public sealed class PlayerJoinRequestGameInput : PokerGameInput {

        public override PokerGameInputType GameInputType => PokerGameInputType.PLAYER_REQUEST_JOIN;

        public Int32 buyInChips;

    }
}