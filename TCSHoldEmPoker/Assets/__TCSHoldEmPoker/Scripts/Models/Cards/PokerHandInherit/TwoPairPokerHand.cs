using System.Collections.Generic;
using TCSHoldEmPoker.Models.Define;

namespace TCSHoldEmPoker.Models {
    public sealed class TwoPairPokerHand : PokerHand {

        #region Properties

        public override PokerHandRankEnum HandRank => PokerHandRankEnum.TWO_PAIR;

        #endregion

        #region Constructors

        private TwoPairPokerHand (List<PokerCard> cards) : base (cards) { }

        #endregion

        #region Methods

        internal static bool GetStrongestHand (IEnumerable<PokerCard> cards, out TwoPairPokerHand strongestHand) {
            var sortedCardList = GetSortedCardList (cards, HighestToLowestValue);

            // Find the 2 highest Pair, if posible.
            if (ExtractStrongestParity (parityCount: 2, sortedCardList, out var highPair) &&
                ExtractStrongestParity (parityCount: 2, sortedCardList, out var lowPair)) {
                List<PokerCard> finalHand = new ();

                finalHand.AddRange (highPair); // Add High Pair.
                finalHand.AddRange (lowPair); // Add Low Pair.
                AddMissingHighCards (finalHand, sortedCardList); // Add Kickers.

                strongestHand = new (finalHand);
                return true;
            }

            // ONLY ONE or NO Pair found.
            strongestHand = null;
            return false;
        }

        #endregion

    }
}