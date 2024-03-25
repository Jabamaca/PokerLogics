using TCSHoldEmPoker.Network.Define;

namespace TCSHoldEmPoker.Network.Events {
    public sealed class PlayerBetCheckGameEvent : PokerGameEvent {

        public override PokerGameEventType GameEventType => PokerGameEventType.PLAYER_BET_CHECK;

        public int playerID;

    }
}