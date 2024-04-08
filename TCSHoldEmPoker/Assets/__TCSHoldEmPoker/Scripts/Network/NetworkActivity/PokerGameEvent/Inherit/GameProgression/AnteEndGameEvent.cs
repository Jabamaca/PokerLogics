using TCSHoldEmPoker.Network.Define;

namespace TCSHoldEmPoker.Network.Activity.PokerGameEvents {
    public sealed class AnteEndGameEvent : PokerGameEvent {

        public override PokerGameEventType GameEventType => PokerGameEventType.ANTE_END;

    }
}