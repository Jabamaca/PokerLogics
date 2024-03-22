using TCSHoldEmPoker.Network.Define;

namespace TCSHoldEmPoker.Network.Events {
    public sealed class AnteEndGameEvent : PokerGameEvent {

        public override PokerGameEventType GameEventType => PokerGameEventType.ANTE_END;

    }
}