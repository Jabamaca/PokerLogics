using GameUtils.Observing;
using TCSHoldEmPoker.Network.Define;

namespace TCSHoldEmPoker.Network.Events {
    public abstract class PokerGameEvent : Observable {

        public virtual PokerGameEventType GameEventType => PokerGameEventType.NULL;

    }
}