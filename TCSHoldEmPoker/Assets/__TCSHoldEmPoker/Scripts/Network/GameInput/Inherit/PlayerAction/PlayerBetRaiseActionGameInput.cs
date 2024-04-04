using System;
using TCSHoldEmPoker.Network.Define;

namespace TCSHoldEmPoker.Network.Input {
    public sealed class PlayerBetRaiseActionGameInput : PokerGameInput {

        public override PokerGameInputType GameSignalType => PokerGameInputType.PLAYER_ACTION_BET_RAISE;

        public Int32 newStake;

    }
}