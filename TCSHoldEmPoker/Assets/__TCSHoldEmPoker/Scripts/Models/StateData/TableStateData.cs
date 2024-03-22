using GameUtils;
using System;
using System.Collections.Generic;
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

        #region Internal Methods

        public override bool Equals (object obj) {
            var other = obj as TableStateData;

            if (this.minimumWager != other.minimumWager ||
                this.currentTableStake != other.currentTableStake ||
                this.cashPot != other.cashPot ||
                this.currentTurnPlayerIndex != other.currentTurnPlayerIndex ||
                !ListUtils.CheckEquals (this.seatStateDataOrder, other.seatStateDataOrder) ||
                this.currentGamePhase != other.currentGamePhase ||
                !ListUtils.CheckEquals (this.communityCardsOrder, other.communityCardsOrder)) {

                return false;
            }

            return true;
        }

        public override int GetHashCode () {
            return base.GetHashCode ();
        }

        #endregion

    }
}