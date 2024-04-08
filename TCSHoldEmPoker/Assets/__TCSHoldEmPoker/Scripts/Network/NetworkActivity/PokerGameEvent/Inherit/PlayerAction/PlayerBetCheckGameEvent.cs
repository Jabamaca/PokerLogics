using System;
using TCSHoldEmPoker.Network.Define;

namespace TCSHoldEmPoker.Network.Activity.PokerGameEvents {
    public sealed class PlayerBetCheckGameEvent : PokerGameEvent {

        public override PokerGameEventType GameEventType => PokerGameEventType.PLAYER_BET_CHECK;

        public Int32 playerID;

    }
}