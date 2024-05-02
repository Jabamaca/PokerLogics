using System;
using TCSHoldEmPoker.Network.Define;

namespace TCSHoldEmPoker.Network.Activity.PokerGameEvents {
    public sealed class UpdateMainPrizePotGameEvent : PokerGameEvent {

        public override PokerGameEventType GameEventType => PokerGameEventType.UPDATE_MAIN_PRIZE_POT;

        public Int32 wagerPerPlayer;

    }
}