using System.Collections.Generic;
using TCSHoldEmPoker.Models.Define;

namespace TCSHoldEmPoker.Models {
    public sealed class FullHousePokerHand : PokerHand {

        #region Properties

        public override PokerHandRankEnum HandRank => PokerHandRankEnum.FULL_HOUSE;

        #endregion

        #region Constructors

        private FullHousePokerHand (List<PokerCard> cards) : base (cards) { }

        #endregion

        #region Methods

        internal static bool GetStrongestHand (IEnumerable<PokerCard> cards, out FullHousePokerHand strongestHand) {
            var sortedCardList = GetSortedCardList (cards, HighestToLowestValue);

            // Find Trio and Pair, if posible.
            if (ExtractStrongestParity (parityCount: 3, sortedCardList, out var highTrio) &&
                ExtractStrongestParity (parityCount: 2, sortedCardList, out var highPair)) {
                List<PokerCard> finalHand = new ();

                finalHand.AddRange (highTrio); // Add Trio.
                finalHand.AddRange (highPair); // Add Pair.
                AddMissingHighCards (finalHand, sortedCardList); // Add Kickers.

                strongestHand = new (finalHand);
                return true;
            }

            // Missing Trio or Pair.
            strongestHand = null;
            return false;
        }

        #endregion

    }
}