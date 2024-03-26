using System.Collections.Generic;
using TCSHoldEmPoker.Models.Define;

namespace TCSHoldEmPoker.Models {
    public static class HighCardHandMaker  {

        internal static bool GetStrongestHand (IEnumerable<PokerCard> cards, out PokerHand strongestHand) {
            var sortedCardList = PokerHandUtils.GetHighToLowSortedCardList (cards);

            List<PokerCard> finalCardList = new ();

            PokerHandUtils.AddMissingHighCards (finalCardList, sortedCardList);

            strongestHand = new (PokerHandRankEnum.HIGH_CARD, finalCardList);
            return true;
        }

    }
}