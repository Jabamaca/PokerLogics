using System;
using TCSHoldEmPoker.Network.Define;

namespace TCSHoldEmPoker.Network.GameEvents {
    public sealed class PlayerBetBlindGameEvent : PokerGameEvent{

        public override PokerGameEventType GameEventType => PokerGameEventType.PLAYER_BET_BLIND;

        public Int32 playerID;
        public Int32 chipsSpent;

    }
}