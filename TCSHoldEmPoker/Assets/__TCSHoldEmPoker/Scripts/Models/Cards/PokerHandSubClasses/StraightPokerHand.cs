using System.Collections.Generic;
using TCSHoldEmPoker.Models.Define;

namespace TCSHoldEmPoker.Models {
    public class StraightPokerHand : PokerHand {

        #region Properties

        public override PokerHandRankEnum HandRank => PokerHandRankEnum.STRAIGHT;

        #endregion

        #region Constructors

        private StraightPokerHand (List<PokerCard> cards) : base (cards) { }

        #endregion

        #region Methods

        internal static bool GetStrongestHand (IEnumerable<PokerCard> cards, out StraightPokerHand strongestHand) {
            var sortedCardList = GetSortedCardList (cards, HighestToLowestValue);

            if (GetStrongestStreak (streakLength: POKER_HAND_SIZE, sortedCardList, out var highStreak)) {
                // The Straight as Final Hand.
                strongestHand = new (highStreak);
                return true;
            }

            // NO Straight found.
            strongestHand = null;
            return false;
        }

        #endregion

    }
}