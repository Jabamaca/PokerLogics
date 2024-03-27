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
            if (RoyalFlushHandMaker.MakeStrongestHand (filteredCards, out var royalFlush))
                return royalFlush;
            else if (StraightFlushHandMaker.MakeStrongestHand (filteredCards, out var straightFlush))
                return straightFlush;
            else if (FourOfAKindHandMaker.MakeStrongestHand (filteredCards, out var fourOfAKind))
                return fourOfAKind;
            else if (FullHouseHandMaker.MakeStrongestHand (filteredCards, out var fullHouse))
                return fullHouse;
            else if (FlushHandMaker.MakeStrongestHand (filteredCards, out var flush))
                return flush;
            else if (StraightHandMaker.MakeStrongestHand (filteredCards, out var straight))
                return straight;
            else if (ThreeOfAKindHandMaker.MakeStrongestHand (filteredCards, out var threeOfAKind))
                return threeOfAKind;
            else if (TwoPairHandMaker.MakeStrongestHand (filteredCards, out var twoPair))
                return twoPair;
            else if (OnePairHandMaker.MakeStrongestHand (filteredCards, out var onePair))
                return onePair;

            // Worst rank scenario, settle for High Card.
            HighCardHandMaker.GetStrongestHand (filteredCards, out var highCard);
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