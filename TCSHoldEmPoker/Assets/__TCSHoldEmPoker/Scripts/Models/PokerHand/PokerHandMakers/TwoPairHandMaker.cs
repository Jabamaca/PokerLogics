using System.Collections.Generic;
using TCSHoldEmPoker.Models.Define;

namespace TCSHoldEmPoker.Models {
    public static class TwoPairHandMaker {

        internal static bool MakeStrongestHand (IEnumerable<PokerCard> cards, out PokerHand strongestHand) {
            var sortedCardList = PokerHandUtils.GetHighToLowSortedCardList (cards);

            // Find the 2 highest Pair, if posible.
            if (PokerHandUtils.ExtractStrongestParity (parityCount: 2, sortedCardList, out var highPair) &&
                PokerHandUtils.ExtractStrongestParity (parityCount: 2, sortedCardList, out var lowPair)) {
                List<PokerCard> finalHand = new ();

                finalHand.AddRange (highPair); // Add High Pair.
                finalHand.AddRange (lowPair); // Add Low Pair.
                PokerHandUtils.AddMissingHighCards (finalHand, sortedCardList); // Add Kickers.

                strongestHand = new (PokerHandRankEnum.TWO_PAIR, finalHand);
                return true;
            }

            // ONLY ONE or NO Pair found.
            strongestHand = null;
            return false;
        }

    }
}