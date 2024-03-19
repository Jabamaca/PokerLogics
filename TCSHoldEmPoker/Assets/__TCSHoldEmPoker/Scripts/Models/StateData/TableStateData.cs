using System;
using System.Collections.Generic;
using TCSHoldEmPoker.Models;
using TCSHoldEmPoker.Models.Define;

namespace TCSHoldEmPoker.Data {
    [Serializable]
    public class TableStateData {

        #region Properties

        public int minimumWager;
        public int currentTurnPlayerIndex;
        public List<SeatStateData> seatStateDataList;
        public PokerGamePhase currentGamePhase;
        public int currentTableStake;
        public int cashPot;
        public List<PokerCard> communityCards;

        #endregion

    }
}