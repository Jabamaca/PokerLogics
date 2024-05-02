using System;
using TCSHoldEmPoker.Network.Define;

namespace TCSHoldEmPoker.Network.Activity.PokerGameEvents {
    public sealed class CreateSidePrizePotGameEvent : PokerGameEvent {

        public override PokerGameEventType GameEventType => PokerGameEventType.CREATE_SIDE_PRIZE_POT;

        public Int32 wagerPerPlayer;

    }
}