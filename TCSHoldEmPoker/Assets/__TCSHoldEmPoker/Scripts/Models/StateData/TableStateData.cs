using System;
using System.Collections.Generic;
using TCSHoldEmPoker.Models;
using TCSHoldEmPoker.Models.Define;

namespace TCSHoldEmPoker.Data {
    [Serializable]
    public class TableStateData {

        #region Properties

        // Wagering Data
        public int minimumWager;
        public int currentTableStake;
        public int cashPot;

        // Turning Data
        public int currentTurnPlayerIndex;
        public List<SeatStateData> seatStateDataOrder;

        // Game Phase Data
        public PokerGamePhase currentGamePhase;
        public List<PokerCard> communityCardsOrder;

        #endregion

    }
}