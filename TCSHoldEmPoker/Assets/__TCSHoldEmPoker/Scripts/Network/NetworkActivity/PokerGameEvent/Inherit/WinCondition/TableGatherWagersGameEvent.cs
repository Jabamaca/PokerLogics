using System;
using TCSHoldEmPoker.Network.Define;

namespace TCSHoldEmPoker.Network.Activity.PokerGameEvents {
    public sealed class TableGatherWagersGameEvent : PokerGameEvent {

        public override PokerGameEventType GameEventType => PokerGameEventType.TABLE_GATHER_WAGERS;

        public Int32 newCashPot;

    }
}