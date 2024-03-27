using System.Collections.Generic;
using TCSHoldEmPoker.Models.Define;

namespace TCSHoldEmPoker.Models {
    public static class FullHouseHandMaker {

        internal static bool MakeStrongestHand (IEnumerable<PokerCard> cards, out PokerHand strongestHand) {
            var sortedCardList = PokerHandUtils.GetHighToLowSortedCardList (cards);

            // Find Trio and Pair, if posible.
            if (PokerHandUtils.ExtractStrongestParity (parityCount: 3, sortedCardList, out var highTrio) &&
                PokerHandUtils.ExtractStrongestParity (parityCount: 2, sortedCardList, out var highPair)) {
                List<PokerCard> finalHand = new ();

                finalHand.AddRange (highTrio); // Add Trio.
                finalHand.AddRange (highPair); // Add Pair.
                PokerHandUtils.AddMissingHighCards (finalHand, sortedCardList); // Add Kickers.

                strongestHand = new (PokerHandRankEnum.FULL_HOUSE, finalHand);
                return true;
            }

            // Missing Trio or Pair.
            strongestHand = null;
            return false;
        }

    }
}