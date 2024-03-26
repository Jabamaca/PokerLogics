using System.Collections.Generic;
using TCSHoldEmPoker.Models.Define;

namespace TCSHoldEmPoker.Models {
    public static class OnePairHandMaker {

        internal static bool MakeStrongestHand (IEnumerable<PokerCard> cards, out PokerHand strongestHand) {
            var sortedCardList = PokerHandUtils.GetHighToLowSortedCardList (cards);

            // Find the highest Pair, if posible.
            if (PokerHandUtils.ExtractStrongestParity (parityCount: 2, sortedCardList, out var highPair)) {
                List<PokerCard> finalHand = new ();

                finalHand.AddRange (highPair); // Add Pair.
                PokerHandUtils.AddMissingHighCards (finalHand, sortedCardList); // Add Kickers.

                strongestHand = new (PokerHandRankEnum.ONE_PAIR, finalHand);
                return true;
            }

            // NO Pair found.
            strongestHand = null;
            return false;
        }

    }
}