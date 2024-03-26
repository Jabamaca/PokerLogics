using System;
using System.Collections.Generic;
using TCSHoldEmPoker.Models.Define;

namespace TCSHoldEmPoker.Models {
    internal static class PokerHandUtils {

        #region Methods

        internal static Dictionary<CardSuitEnum, List<PokerCard>> SeparateBySuit (IEnumerable<PokerCard> sortedCardList) {
            Dictionary<CardSuitEnum, List<PokerCard>> returnDict = new ();

            foreach (var card in sortedCardList) {
                if (returnDict.TryGetValue (card.Suit, out var cardList)) {
                    cardList.Add (card);
                } else {
                    returnDict.Add (card.Suit, new ());
                    returnDict[card.Suit].Add (card);
                }
            }

            return returnDict;
        }

        internal static void AddMissingHighCards (List<PokerCard> listToFill, IEnumerable<PokerCard> sortedCardList) {
            foreach (PokerCard card in sortedCardList) {
                if (listToFill.Count >= HoldEmPokerDefines.POKER_HAND_SIZE)
                    break; // Stop adding cards.

                listToFill.Add (card);
            }
        }

        internal static List<PokerCard> GetHighToLowSortedCardList (IEnumerable<PokerCard> cards) {
            return GetSortedCardList (cards, HighestToLowestValue);
        }

        internal static List<PokerCard> GetSegregatedHighToLowSortedCardList (IEnumerable<PokerCard> cards) {
            return GetSortedCardList (cards, SegregatedHighestToLowestValue);
        }

        private static List<PokerCard> GetSortedCardList (IEnumerable<PokerCard> cards, Comparison<PokerCard> comp) {
            List<PokerCard> sortedCardList = new ();
            sortedCardList.AddRange (cards);
            sortedCardList.Sort (comp);

            return sortedCardList;
        }

        private static int HighestToLowestValue (PokerCard card1, PokerCard card2) {
            return card2.Value.CompareTo (card1.Value);
        }

        private static int SegregatedHighestToLowestValue (PokerCard card1, PokerCard card2) {
            int suitComp = card1.Suit.CompareTo (card2.Suit);

            if (suitComp == 0)
                return HighestToLowestValue (card1, card2);
            else
                return suitComp;
        }

        internal static bool ExtractStrongestParity (int parityCount, List<PokerCard> sortedCardList, out List<PokerCard> highParity) {
            List<PokerCard> parity = new () { PokerCard.BLANK };
            foreach (PokerCard card in sortedCardList) {
                // Check Value parity.
                if (card.Value == parity[0].Value) {
                    // Same Value found.
                    parity.Add (card);

                    if (parity.Count == parityCount) {
                        // Highest parity found. Stop enum.
                        break;
                    }
                } else {
                    // Different Value found. Start new parity.
                    parity.Clear ();
                    parity.Add (card);
                }
            }

            if (parity.Count == parityCount) {
                highParity = new ();

                // Add the Trio to Final Hand.
                foreach (var card in parity) {
                    highParity.Add (card);
                    sortedCardList.Remove (card);
                }

                return true;
            }

            // Parity count not reached.
            highParity = null;
            return false;
        }

        internal static bool GetStrongestStreak (int streakLength, List<PokerCard> sortedCardList, out List<PokerCard> highStreak) {
            // For Low-Ace Streak
            bool foundAce = false;
            PokerCard firstAce = PokerCard.BLANK;

            // Find a Streak, if posible.
            CardValueEnum currentCount = CardValueEnum.NULL;
            List<PokerCard> streak = new ();
            foreach (PokerCard card in sortedCardList) {
                if (currentCount == card.Value)
                    // Same Value found, skip to next card.
                    continue;

                // Reset countdown if a Value is skipped.
                if ((int)currentCount - (int)card.Value != 0x10) { // Counting byte difference
                    streak.Clear ();
                }

                streak.Add (card);
                currentCount = card.Value;

                if (streak.Count == streakLength)
                    break; // Streak length reached, stop enum.

                // Save the Ace for Low-Ace Straights.
                if (!foundAce && card.Value == CardValueEnum.ACE) {
                    foundAce = true;
                    firstAce = card;
                }
            }

            // Try utilizing Low-Ace.
            if (streak.Count < streakLength &&
                currentCount == CardValueEnum.TWO &&
                foundAce) {

                streak.Add (firstAce);
            }

            if (streak.Count == streakLength) {
                // The Straight as Final Hand.
                highStreak = new (streak);
                return true;
            }

            // NO Straight found.
            highStreak = null;
            return false;
        }

        public static string RankString (PokerHandRankEnum rank) {
            return rank switch {
                PokerHandRankEnum.HIGH_CARD => "High Card",
                PokerHandRankEnum.ONE_PAIR => "One Pair",
                PokerHandRankEnum.TWO_PAIR => "Two Pair",
                PokerHandRankEnum.THREE_OF_A_KIND => "3 of a Kind",
                PokerHandRankEnum.STRAIGHT => "Straight",
                PokerHandRankEnum.FLUSH => "Flush",
                PokerHandRankEnum.FULL_HOUSE => "Full House",
                PokerHandRankEnum.FOUR_OF_A_KIND => "4 of a Kind",
                PokerHandRankEnum.STRAIGHT_FLUSH => "Straight Flush",
                PokerHandRankEnum.ROYAL_FLUSH => "Royal Flush",
                _ => "INVALID",
            };
        }

        #endregion

    }
}