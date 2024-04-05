using TCSHoldEmPoker.Network.Define;

namespace TCSHoldEmPoker.Network.GameEvents {
    public sealed class AnteStartGameEvent : PokerGameEvent {

        public override PokerGameEventType GameEventType => PokerGameEventType.ANTE_START;

    }
}