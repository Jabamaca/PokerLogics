using System;
using TCSHoldEmPoker.Network.Define;

namespace TCSHoldEmPoker.Network.Events {
    public sealed class PlayerLeaveGameEvent : PokerGameEvent {

        public override PokerGameEventType GameEventType => PokerGameEventType.PLAYER_LEAVE;

        public Int32 playerID;

    }
}