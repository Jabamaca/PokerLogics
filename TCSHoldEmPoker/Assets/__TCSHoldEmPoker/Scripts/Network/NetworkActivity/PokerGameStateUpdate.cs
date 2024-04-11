using GameUtils.Observing;
using TCSHoldEmPoker.Data;

namespace TCSHoldEmPoker.Network.Activity {
    public sealed class PokerGameStateUpdate : Observable {

        public int gameTableID;
        public TableStateData updatedTableStateData;

    }
}