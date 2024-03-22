using TCSHoldEmPoker.Network.Define;

namespace TCSHoldEmPoker.Network.Events {
    public sealed class TableGatherWagersGameEvent : PokerGameEvent {

        public override PokerGameEventType GameEventType => PokerGameEventType.TABLE_GATHER_WAGERS;

        public int newCashPot;

    }
}