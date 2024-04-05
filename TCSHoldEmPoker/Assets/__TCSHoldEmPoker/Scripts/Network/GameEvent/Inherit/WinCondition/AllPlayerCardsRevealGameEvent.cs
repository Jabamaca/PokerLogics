using System;
using System.Collections.Generic;
using TCSHoldEmPoker.Models.Define;
using TCSHoldEmPoker.Network.Define;

namespace TCSHoldEmPoker.Network.GameEvents {
    public sealed class AllPlayerCardsRevealGameEvent : PokerGameEvent {

        public override PokerGameEventType GameEventType => PokerGameEventType.ALL_PLAYER_CARDS_REVEAL;

        public IReadOnlyDictionary<Int32, IReadOnlyList<PokerCard>> revealedHands;
        
    }
}