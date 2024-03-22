using System.Collections.Generic;
using TCSHoldEmPoker.Models.Define;
using TCSHoldEmPoker.Network.Define;

namespace TCSHoldEmPoker.Network.Events {
    public sealed class AllPlayerCardsRevealGameEvent : PokerGameEvent {

        public override PokerGameEventType GameEventType => PokerGameEventType.PLAYER_CARDS_REVEAL;

        public IReadOnlyDictionary<int, IReadOnlyList<PokerCard>> revealedHands;
        
    }
}