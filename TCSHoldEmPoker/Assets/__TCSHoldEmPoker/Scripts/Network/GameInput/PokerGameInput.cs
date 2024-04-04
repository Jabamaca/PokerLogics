using System;
using GameUtils.Observing;
using TCSHoldEmPoker.Network.Define;

namespace TCSHoldEmPoker.Network.Inputs {
    public abstract class PokerGameInput : Observable {

        public virtual PokerGameInputType GameInputType => PokerGameInputType.NULL;

        public Int32 gameTableID;
        public Int32 playerID;

    }
}
