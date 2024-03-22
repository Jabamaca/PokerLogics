using System.Collections.Generic;
using TCSHoldEmPoker.Models.Define;
using TCSHoldEmPoker.Network.Define;

namespace TCSHoldEmPoker.Network.Events {
    public sealed class PlayerCardsDealGameEvent : PokerGameEvent {

        public override PokerGameEventType GameEventType => PokerGameEventType.PLAYER_CARD_DEAL;

        public int playerID;
        public IReadOnlyList<PokerCard> cards;

    }
}