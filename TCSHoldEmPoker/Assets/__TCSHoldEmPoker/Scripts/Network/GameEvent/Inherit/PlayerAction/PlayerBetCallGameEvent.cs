using TCSHoldEmPoker.Network.Define;

namespace TCSHoldEmPoker.Network.Events {
    public sealed class PlayerBetCallGameEvent : PokerGameEvent {

        public override PokerGameEventType GameEventType => isAllIn ? 
            PokerGameEventType.PLAYER_BET_CALL_ALL_IN :
            PokerGameEventType.PLAYER_BET_CALL;

        public int playerID;
        public int chipsSpent;
        public bool isAllIn;

    }
}