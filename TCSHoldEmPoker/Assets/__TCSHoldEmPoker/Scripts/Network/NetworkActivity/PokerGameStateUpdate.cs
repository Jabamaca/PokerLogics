using GameUtils.Observing;
using System.Collections.Generic;
using TCSHoldEmPoker.Data;
using TCSHoldEmPoker.Models.Define;

namespace TCSHoldEmPoker.Network.Activity {
    public sealed class PokerGameStateUpdate : Observable {

        public int gameTableID;
        public TableStateData updatedTableStateData;

    }
}