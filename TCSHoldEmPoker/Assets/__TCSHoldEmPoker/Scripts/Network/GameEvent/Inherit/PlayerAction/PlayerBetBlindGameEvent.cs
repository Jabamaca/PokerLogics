using TCSHoldEmPoker.Network.Define;

namespace TCSHoldEmPoker.Network.Events {
    public sealed class PlayerBetBlindGameEvent : PokerGameEvent{

        public override PokerGameEventType GameEventType => PokerGameEventType.PLAYER_BET_BLIND;

        public int playerID;
        public int chipsSpent;

    }
}