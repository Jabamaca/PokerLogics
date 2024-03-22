using GameUtils.Observing;
using TCSHoldEmPoker.Network.Define;

namespace TCSHoldEmPoker.Network.Signals {
    public abstract class PokerGameSignal : Observable {

        public virtual PokerGameSignalType GameSignalType => PokerGameSignalType.NULL;

    }
}
