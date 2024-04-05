using System;
using TCSHoldEmPoker.Network.Define;

namespace TCSHoldEmPoker.Network.GameEvents {
    public sealed class PlayerJoinGameEvent : PokerGameEvent {

        public override PokerGameEventType GameEventType => PokerGameEventType.PLAYER_JOIN;

        public Int32 playerID;
        public Int32 buyInChips;

    }
}