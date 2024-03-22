using TCSHoldEmPoker.Network.Define;

namespace TCSHoldEmPoker.Network.Events {
    public sealed class PlayerJoinGameEvent : PokerGameEvent {

        public override PokerGameEventType GameEventType => PokerGameEventType.PLAYER_JOIN;

        public int playerID;
        public int buyInChips;

    }
}