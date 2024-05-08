using System;
using TCSHoldEmPoker.Network.Define;

namespace TCSHoldEmPoker.Network.Activity.PokerGameEvents {
    public sealed class PlayerGetKickedGameEvent : PokerGameEvent {

        public override PokerGameEventType GameEventType => PokerGameEventType.PLAYER_GET_KICKED;

        public Int32 playerID;

    }
}