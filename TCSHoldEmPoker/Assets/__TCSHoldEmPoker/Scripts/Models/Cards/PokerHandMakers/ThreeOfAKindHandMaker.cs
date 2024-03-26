using System.Collections.Generic;
using TCSHoldEmPoker.Models.Define;

namespace TCSHoldEmPoker.Models {
    public static class ThreeOfAKindHandMaker {

        internal static bool MakeStrongestHand (IEnumerable<PokerCard> cards, out PokerHand strongestHand) {
            var sortedCardList = PokerHandUtils.GetHighToLowSortedCardList (cards);

            // Find the highest Trio, if posible.
            if (PokerHandUtils.ExtractStrongestParity (parityCount: 3, sortedCardList, out var highTrio)) {
                List<PokerCard> finalHand = new ();

                finalHand.AddRange (highTrio); // Add Trio.
                PokerHandUtils.AddMissingHighCards (finalHand, sortedCardList); // Add Kickers.

                strongestHand = new (PokerHandRankEnum.THREE_OF_A_KIND, finalHand);
                return true;
            }

            // NO Trio found.
            strongestHand = null;
            return false;
        }

    }
}