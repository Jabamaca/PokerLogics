using System.Collections.Generic;
using TCSHoldEmPoker.Models.Define;

namespace TCSHoldEmPoker.Models {
    public class StraightFlushPokerHand : PokerHand {

        #region Properties

        public override PokerHandRankEnum HandRank => PokerHandRankEnum.STRAIGHT_FLUSH;

        #endregion

        #region Constructors

        private StraightFlushPokerHand (List<PokerCard> cards) : base (cards) { }

        #endregion

        #region Methods

        internal static bool GetStrongestHand (IEnumerable<PokerCard> cards, out StraightFlushPokerHand strongestHand) {
            var sortedCardList = GetSortedCardList (cards, SegregatedHighestToLowestValue);
            var cardsBySuits = SeparateBySuit (sortedCardList);

            // Find a Flush, if posible.
            List<PokerCard> highFlush = new () { PokerCard.BLANK };
            foreach (var suitKVP in cardsBySuits) {
                var suitCards = suitKVP.Value;
                if (GetStrongestStreak (streakLength: POKER_HAND_SIZE, suitCards, out var currentFlush)) {
                    if (currentFlush[0].Value > highFlush[0].Value) {
                        highFlush = currentFlush;
                    }
                }
            }

            if (highFlush.Count == POKER_HAND_SIZE) {
                // The Flush as Final Hand.
                strongestHand = new (highFlush);
                return true;
            }

            // NO Flush found.
            strongestHand = null;
            return false;
        }

        #endregion

    }
}