using System;
using System.Collections.Generic;
using TCSHoldEmPoker.Models.Define;
using TCSHoldEmPoker.Network.Define;

namespace TCSHoldEmPoker.Network.Activity.PokerGameEvents {
    public sealed class PlayerCardsDealGameEvent : PokerGameEvent {

        public override PokerGameEventType GameEventType => PokerGameEventType.PLAYER_CARD_DEAL;

        public Int32 playerID;
        public IReadOnlyList<PokerCard> cards;

    }
}