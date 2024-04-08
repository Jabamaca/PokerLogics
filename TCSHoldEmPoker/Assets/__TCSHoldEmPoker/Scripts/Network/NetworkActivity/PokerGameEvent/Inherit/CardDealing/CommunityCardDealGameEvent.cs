using System;
using TCSHoldEmPoker.Models.Define;
using TCSHoldEmPoker.Network.Define;

namespace TCSHoldEmPoker.Network.Activity.PokerGameEvents {
    public sealed class CommunityCardDealGameEvent : PokerGameEvent {

        public override PokerGameEventType GameEventType => PokerGameEventType.COMMUNITY_CARD_DEAL;

        public PokerCard pokerCard;
        public Int16 cardIndex;

    }
}