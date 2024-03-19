using GameUtils;
using System.Collections.Generic;
using TCSHoldEmPoker.Models.Define;

namespace TCSHoldEmPoker.Models {
    public class CardDeck {

        #region Properties

        private static readonly IReadOnlyList<PokerCard> _cardsInDeck = new List<PokerCard> () {
            // CLUB CARDS
            new (CardSuitEnum.CLUBS, CardValueEnum.TWO),
            new (CardSuitEnum.CLUBS, CardValueEnum.THREE),
            new (CardSuitEnum.CLUBS, CardValueEnum.FOUR),
            new (CardSuitEnum.CLUBS, CardValueEnum.FIVE),
            new (CardSuitEnum.CLUBS, CardValueEnum.SIX),
            new (CardSuitEnum.CLUBS, CardValueEnum.SEVEN),
            new (CardSuitEnum.CLUBS, CardValueEnum.EIGHT),
            new (CardSuitEnum.CLUBS, CardValueEnum.NINE),
            new (CardSuitEnum.CLUBS, CardValueEnum.TEN),
            new (CardSuitEnum.CLUBS, CardValueEnum.JACK),
            new (CardSuitEnum.CLUBS, CardValueEnum.QUEEN),
            new (CardSuitEnum.CLUBS, CardValueEnum.KING),
            new (CardSuitEnum.CLUBS, CardValueEnum.ACE),
            // DIAMOND CARDS
            new (CardSuitEnum.DIAMONDS, CardValueEnum.TWO),
            new (CardSuitEnum.DIAMONDS, CardValueEnum.THREE),
            new (CardSuitEnum.DIAMONDS, CardValueEnum.FOUR),
            new (CardSuitEnum.DIAMONDS, CardValueEnum.FIVE),
            new (CardSuitEnum.DIAMONDS, CardValueEnum.SIX),
            new (CardSuitEnum.DIAMONDS, CardValueEnum.SEVEN),
            new (CardSuitEnum.DIAMONDS, CardValueEnum.EIGHT),
            new (CardSuitEnum.DIAMONDS, CardValueEnum.NINE),
            new (CardSuitEnum.DIAMONDS, CardValueEnum.TEN),
            new (CardSuitEnum.DIAMONDS, CardValueEnum.JACK),
            new (CardSuitEnum.DIAMONDS, CardValueEnum.QUEEN),
            new (CardSuitEnum.DIAMONDS, CardValueEnum.KING),
            new (CardSuitEnum.DIAMONDS, CardValueEnum.ACE),
            // HEART CARDS
            new (CardSuitEnum.HEARTS, CardValueEnum.TWO),
            new (CardSuitEnum.HEARTS, CardValueEnum.THREE),
            new (CardSuitEnum.HEARTS, CardValueEnum.FOUR),
            new (CardSuitEnum.HEARTS, CardValueEnum.FIVE),
            new (CardSuitEnum.HEARTS, CardValueEnum.SIX),
            new (CardSuitEnum.HEARTS, CardValueEnum.SEVEN),
            new (CardSuitEnum.HEARTS, CardValueEnum.EIGHT),
            new (CardSuitEnum.HEARTS, CardValueEnum.NINE),
            new (CardSuitEnum.HEARTS, CardValueEnum.TEN),
            new (CardSuitEnum.HEARTS, CardValueEnum.JACK),
            new (CardSuitEnum.HEARTS, CardValueEnum.QUEEN),
            new (CardSuitEnum.HEARTS, CardValueEnum.KING),
            new (CardSuitEnum.HEARTS, CardValueEnum.ACE),
            // SPADE CARDS
            new (CardSuitEnum.SPADES, CardValueEnum.TWO),
            new (CardSuitEnum.SPADES, CardValueEnum.THREE),
            new (CardSuitEnum.SPADES, CardValueEnum.FOUR),
            new (CardSuitEnum.SPADES, CardValueEnum.FIVE),
            new (CardSuitEnum.SPADES, CardValueEnum.SIX),
            new (CardSuitEnum.SPADES, CardValueEnum.SEVEN),
            new (CardSuitEnum.SPADES, CardValueEnum.EIGHT),
            new (CardSuitEnum.SPADES, CardValueEnum.NINE),
            new (CardSuitEnum.SPADES, CardValueEnum.TEN),
            new (CardSuitEnum.SPADES, CardValueEnum.JACK),
            new (CardSuitEnum.SPADES, CardValueEnum.QUEEN),
            new (CardSuitEnum.SPADES, CardValueEnum.KING),
            new (CardSuitEnum.SPADES, CardValueEnum.ACE),
        };
        private static int DeckCardCount => _cardsInDeck.Count;

        private int _currentDeckIndex;
        private IReadOnlyList<PokerCard> _shuffledDeck;
        public IReadOnlyList<PokerCard> ShuffledDeck => _shuffledDeck;

        #endregion

        #region Methods

        public void Shuffle () {
            _shuffledDeck = ListUtils.Shuffle (_cardsInDeck);
            _currentDeckIndex = 0;
        }

        public PokerCard GetNextCard () {
            if (_currentDeckIndex >= DeckCardCount) {
                Shuffle ();
            }

            PokerCard dealtCard = _shuffledDeck[_currentDeckIndex];
            _currentDeckIndex++;
            return dealtCard;
        }

        #endregion

    }
}