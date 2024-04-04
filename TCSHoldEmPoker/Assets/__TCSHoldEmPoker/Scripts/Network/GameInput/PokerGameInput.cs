using System;
using GameUtils.Observing;
using TCSHoldEmPoker.Network.Define;

namespace TCSHoldEmPoker.Network.Input {
    public abstract class PokerGameInput : Observable {

        public virtual PokerGameInputType GameSignalType => PokerGameInputType.NULL;

        public Int32 gameTableID;
        public Int32 playerID;

    }
}
