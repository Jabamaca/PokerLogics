using System.Collections.Generic;
using TCSHoldEmPoker.Models.Define;

namespace TCSHoldEmPoker.Models {
    public static class FourOfAKindHandMaker {

        internal static bool MakeStrongestHand (IEnumerable<PokerCard> cards, out PokerHand strongestHand) {
            var sortedCardList = PokerHandUtils.GetHighToLowSortedCardList (cards);

            // Find the highest Quad, if posible.
            if (PokerHandUtils.ExtractStrongestParity (parityCount: 4, sortedCardList, out var highQuad)) {
                List<PokerCard> finalHand = new ();

                finalHand.AddRange (highQuad); // Add Quad.
                PokerHandUtils.AddMissingHighCards (finalHand, sortedCardList); // Add kickers.

                strongestHand = new (PokerHandRankEnum.FOUR_OF_A_KIND, finalHand);
                return true;
            }

            // NO Quad found.
            strongestHand = null;
            return false;
        }

    }
}