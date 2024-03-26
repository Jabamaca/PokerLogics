using System.Collections.Generic;
using TCSHoldEmPoker.Models.Define;

namespace TCSHoldEmPoker.Models {
    public static class StraightHandMaker {

        internal static bool MakeStrongestHand (IEnumerable<PokerCard> cards, out PokerHand strongestHand) {
            var sortedCardList = PokerHandUtils.GetHighToLowSortedCardList (cards);

            if (PokerHandUtils.GetStrongestStreak (streakLength: HoldEmPokerDefines.POKER_HAND_SIZE, sortedCardList, out var highStreak)) {
                // The Straight as Final Hand.
                strongestHand = new (PokerHandRankEnum.STRAIGHT, highStreak);
                return true;
            }

            // NO Straight found.
            strongestHand = null;
            return false;
        }

    }
}