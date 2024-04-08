using System;
using TCSHoldEmPoker.Network.Define;

namespace TCSHoldEmPoker.Network.Activity.PokerGameInputs {
    public sealed class PlayerBetRaiseActionGameInput : PokerGameInput {

        public override PokerGameInputType GameInputType => PokerGameInputType.PLAYER_ACTION_BET_RAISE;

        public Int32 newStake;

    }
}