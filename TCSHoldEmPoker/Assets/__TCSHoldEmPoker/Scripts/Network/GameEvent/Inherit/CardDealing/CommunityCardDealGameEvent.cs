using TCSHoldEmPoker.Models.Define;
using TCSHoldEmPoker.Network.Define;

namespace TCSHoldEmPoker.Network.Events {
    public sealed class CommunityCardDealGameEvent : PokerGameEvent {

        public override PokerGameEventType GameEventType => PokerGameEventType.COMMUNITY_CARD_REVEAL;

        public PokerCard pokerCard;
        public int cardIndex;

    }
}