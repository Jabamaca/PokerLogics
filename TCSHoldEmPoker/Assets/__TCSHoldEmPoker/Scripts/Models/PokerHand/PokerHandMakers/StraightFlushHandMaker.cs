using System.Collections.Generic;
using TCSHoldEmPoker.Models.Define;

namespace TCSHoldEmPoker.Models {
    public static class StraightFlushHandMaker {

        internal static bool MakeStrongestHand (IEnumerable<PokerCard> cards, out PokerHand strongestHand) {
            var sortedCardList = PokerHandUtils.GetSegregatedHighToLowSortedCardList (cards);
            var cardsBySuits = PokerHandUtils.SeparateBySuit (sortedCardList);

            // Find a Flush, if posible.
            List<PokerCard> highFlush = new () { PokerCard.BLANK };
            foreach (var suitKVP in cardsBySuits) {
                var suitCards = suitKVP.Value;
                if (PokerHandUtils.GetStrongestStreak (streakLength: HoldEmPokerDefines.POKER_HAND_SIZE, suitCards, out var currentFlush)) {
                    if (currentFlush[0].Value > highFlush[0].Value) {
                        highFlush = currentFlush;
                    }
                }
            }

            if (highFlush.Count == HoldEmPokerDefines.POKER_HAND_SIZE) {
                // The Flush as Final Hand.
                strongestHand = new (PokerHandRankEnum.STRAIGHT_FLUSH, highFlush);
                return true;
            }

            // NO Flush found.
            strongestHand = null;
            return false;
        }

    }
}