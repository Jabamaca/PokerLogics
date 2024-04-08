using System;
using TCSHoldEmPoker.Network.Define;

namespace TCSHoldEmPoker.Network.Activity.PokerGameEvents {
    public sealed class PlayerBetCallGameEvent : PokerGameEvent {

        public override PokerGameEventType GameEventType => isAllIn ? 
            PokerGameEventType.PLAYER_BET_CALL_ALL_IN :
            PokerGameEventType.PLAYER_BET_CALL_BASIC;

        public Int32 playerID;
        public Int32 chipsSpent;
        public bool isAllIn;

    }
}