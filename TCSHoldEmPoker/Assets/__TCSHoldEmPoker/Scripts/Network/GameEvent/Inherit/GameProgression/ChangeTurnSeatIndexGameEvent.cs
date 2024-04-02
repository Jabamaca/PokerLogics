using System;
using TCSHoldEmPoker.Network.Define;

namespace TCSHoldEmPoker.Network.Events {
    public sealed class ChangeTurnSeatIndexGameEvent : PokerGameEvent {

        public override PokerGameEventType GameEventType => PokerGameEventType.ANTE_TURN_CHANGE;

        public Int16 seatIndex;

    }
}