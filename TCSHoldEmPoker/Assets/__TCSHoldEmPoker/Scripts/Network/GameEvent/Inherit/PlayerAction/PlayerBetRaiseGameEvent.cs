using TCSHoldEmPoker.Network.Define;

namespace TCSHoldEmPoker.Network.Events {
    public sealed class PlayerBetRaiseGameEvent : PokerGameEvent {

        public override PokerGameEventType GameEventType => isAllIn ? 
            PokerGameEventType.PLAYER_BET_RAISE_ALL_IN : 
            PokerGameEventType.PLAYER_BET_RAISE;

        public int playerID;
        public int chipsSpent;
        public bool isAllIn;

    }
}