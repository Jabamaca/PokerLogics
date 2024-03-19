using System.Collections.Generic;
using TCSHoldEmPoker.Models.Define;

namespace TCSHoldEmPoker.Models {
    public class OnePairPokerHand : PokerHand {

        #region Properties

        public override PokerHandRankEnum HandRank => PokerHandRankEnum.ONE_PAIR;

        #endregion

        #region Constructors

        private OnePairPokerHand (List<PokerCard> cards) : base (cards) { }

        #endregion

        #region Methods

        internal static bool GetStrongestHand (IEnumerable<PokerCard> cards, out OnePairPokerHand strongestHand) {
            var sortedCardList = GetSortedCardList (cards, HighestToLowestValue);

            // Find the highest Pair, if posible.
            if (ExtractStrongestParity (parityCount: 2, sortedCardList, out var highPair)) {
                List<PokerCard> finalHand = new ();

                finalHand.AddRange (highPair); // Add Pair.
                AddMissingHighCards (finalHand, sortedCardList); // Add Kickers.

                strongestHand = new (finalHand);
                return true;
            }

            // NO Pair found.
            strongestHand = null;
            return false;
        }

        #endregion

    }
}