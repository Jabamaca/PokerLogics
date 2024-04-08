using System;
using GameUtils.Observing;
using TCSHoldEmPoker.Network.Define;

namespace TCSHoldEmPoker.Network.Activity.PokerGameEvents {
    public abstract class PokerGameEvent : Observable {

        public virtual PokerGameEventType GameEventType => PokerGameEventType.NULL;

        public Int32 gameTableID;

    }
}