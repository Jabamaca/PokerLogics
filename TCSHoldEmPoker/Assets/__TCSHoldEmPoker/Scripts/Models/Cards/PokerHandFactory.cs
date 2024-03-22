using System.Collections.Generic;
using TCSHoldEmPoker.Models.Define;

namespace TCSHoldEmPoker.Models {
    public static class PokerHandFactory {

        private const int POKER_HAND_SIZE = HoldEmPokerDefines.POKER_HAND_SIZE;

        #region Methods

        public static PokerHand GetHighestPokerHandWithCardSets (params IEnumerable<PokerCard>[] cardSets) {
            List<PokerCard> compiledCards = new ();

            foreach (var cardSet in cardSets) {
                compiledCards.AddRange (cardSet);
            }

            return GetHighestPokerHandWithCardSet (compiledCards);
        }

        public static PokerHand GetHighestPokerHandWithCardSet (IEnumerable<PokerCard> cards) {
            if (cards == null)
                return null; // NO Card List to check.

            if (!FilterNullCards (cards, out var filteredCards))
                return null;

            // Check for highest to lowest rank hand.
            if (RoyalFlushPokerHand.GetStrongestHand (filteredCards, out var royalFlush))
                return royalFlush;
            else if (StraightFlushPokerHand.GetStrongestHand (filteredCards, out var straightFlush))
                return straightFlush;
            else if (FourOfAKindPokerHand.GetStrongestHand (filteredCards, out var fourOfAKind))
                return fourOfAKind;
            else if (FullHousePokerHand.GetStrongestHand (filteredCards, out var fullHouse))
                return fullHouse;
            else if (FlushPokerHand.GetStrongestHand (filteredCards, out var flush))
                return flush;
            else if (StraightPokerHand.GetStrongestHand (filteredCards, out var straight))
                return straight;
            else if (ThreeOfAKindPokerHand.GetStrongestHand (filteredCards, out var threeOfAKind))
                return threeOfAKind;
            else if (TwoPairPokerHand.GetStrongestHand (filteredCards, out var twoPair))
                return twoPair;
            else if (OnePairPokerHand.GetStrongestHand (filteredCards, out var onePair))
                return onePair;

            // Worst rank scenario, settle for High Card.
            HighCardPokerHand.GetStrongestHand (filteredCards, out var highCard);
            return highCard;
        }

        private static bool FilterNullCards (IEnumerable<PokerCard> cards, out IReadOnlyList<PokerCard> filteredList) {
            List<PokerCard> newCardList = new ();

            foreach (var card in cards) {
                if (card.Suit == CardSuitEnum.NULL || card.Value == CardValueEnum.NULL) {
                    continue;
                }

                newCardList.Add (card);
            }

            filteredList = newCardList;
            return newCardList.Count >= POKER_HAND_SIZE; // Return whether filtered Card List has enough cards.
        }

        #endregion

    }
}