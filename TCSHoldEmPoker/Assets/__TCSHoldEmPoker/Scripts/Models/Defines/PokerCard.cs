using System;

namespace TCSHoldEmPoker.Models.Define {
    [Serializable]
    public readonly struct PokerCard {

        #region Properties

        public CardSuitEnum Suit { get; }
        public CardValueEnum Value { get; }

        #endregion

        #region Constructors

        public PokerCard (CardSuitEnum s, CardValueEnum v) {
            Suit = s;
            Value = v;
        }

        #endregion

        #region Overrides

        public override bool Equals (object obj) {
            if (obj is not PokerCard)
                return false;

            PokerCard other = (PokerCard)obj;

            return this.Suit == other.Suit && this.Value == other.Value;
        }

        public override int GetHashCode () {
            return base.GetHashCode ();
        }

        public override string ToString () {
            string print = "";

            print += Value switch {
                CardValueEnum.LOW_ACE => "A",
                CardValueEnum.TWO => "2",
                CardValueEnum.THREE => "3",
                CardValueEnum.FOUR => "4",
                CardValueEnum.FIVE => "5",
                CardValueEnum.SIX => "6",
                CardValueEnum.SEVEN => "7",
                CardValueEnum.EIGHT => "8",
                CardValueEnum.NINE => "9",
                CardValueEnum.TEN => "X",
                CardValueEnum.JACK => "J",
                CardValueEnum.QUEEN => "Q",
                CardValueEnum.KING => "K",
                CardValueEnum.ACE => "A",
                _ => "?",
            };

            print += Suit switch {
                CardSuitEnum.CLUBS => "C",
                CardSuitEnum.DIAMONDS => "D",
                CardSuitEnum.HEARTS => "H",
                CardSuitEnum.SPADES => "S",
                _ => "?",
            };

            return print;
        }

        #endregion

        #region Enumerated Cards

        public static PokerCard BLANK = new (CardSuitEnum.NULL, CardValueEnum.NULL);

        public static PokerCard CARD_A_C = new (CardSuitEnum.CLUBS, CardValueEnum.ACE);
        public static PokerCard CARD_2_C = new (CardSuitEnum.CLUBS, CardValueEnum.TWO);
        public static PokerCard CARD_3_C = new (CardSuitEnum.CLUBS, CardValueEnum.THREE);
        public static PokerCard CARD_4_C = new (CardSuitEnum.CLUBS, CardValueEnum.FOUR);
        public static PokerCard CARD_5_C = new (CardSuitEnum.CLUBS, CardValueEnum.FIVE);
        public static PokerCard CARD_6_C = new (CardSuitEnum.CLUBS, CardValueEnum.SIX);
        public static PokerCard CARD_7_C = new (CardSuitEnum.CLUBS, CardValueEnum.SEVEN);
        public static PokerCard CARD_8_C = new (CardSuitEnum.CLUBS, CardValueEnum.EIGHT);
        public static PokerCard CARD_9_C = new (CardSuitEnum.CLUBS, CardValueEnum.NINE);
        public static PokerCard CARD_X_C = new (CardSuitEnum.CLUBS, CardValueEnum.TEN);
        public static PokerCard CARD_J_C = new (CardSuitEnum.CLUBS, CardValueEnum.JACK);
        public static PokerCard CARD_Q_C = new (CardSuitEnum.CLUBS, CardValueEnum.QUEEN);
        public static PokerCard CARD_K_C = new (CardSuitEnum.CLUBS, CardValueEnum.KING);

        public static PokerCard CARD_A_D = new (CardSuitEnum.DIAMONDS, CardValueEnum.ACE);
        public static PokerCard CARD_2_D = new (CardSuitEnum.DIAMONDS, CardValueEnum.TWO);
        public static PokerCard CARD_3_D = new (CardSuitEnum.DIAMONDS, CardValueEnum.THREE);
        public static PokerCard CARD_4_D = new (CardSuitEnum.DIAMONDS, CardValueEnum.FOUR);
        public static PokerCard CARD_5_D = new (CardSuitEnum.DIAMONDS, CardValueEnum.FIVE);
        public static PokerCard CARD_6_D = new (CardSuitEnum.DIAMONDS, CardValueEnum.SIX);
        public static PokerCard CARD_7_D = new (CardSuitEnum.DIAMONDS, CardValueEnum.SEVEN);
        public static PokerCard CARD_8_D = new (CardSuitEnum.DIAMONDS, CardValueEnum.EIGHT);
        public static PokerCard CARD_9_D = new (CardSuitEnum.DIAMONDS, CardValueEnum.NINE);
        public static PokerCard CARD_X_D = new (CardSuitEnum.DIAMONDS, CardValueEnum.TEN);
        public static PokerCard CARD_J_D = new (CardSuitEnum.DIAMONDS, CardValueEnum.JACK);
        public static PokerCard CARD_Q_D = new (CardSuitEnum.DIAMONDS, CardValueEnum.QUEEN);
        public static PokerCard CARD_K_D = new (CardSuitEnum.DIAMONDS, CardValueEnum.KING);

        public static PokerCard CARD_A_H = new (CardSuitEnum.HEARTS, CardValueEnum.ACE);
        public static PokerCard CARD_2_H = new (CardSuitEnum.HEARTS, CardValueEnum.TWO);
        public static PokerCard CARD_3_H = new (CardSuitEnum.HEARTS, CardValueEnum.THREE);
        public static PokerCard CARD_4_H = new (CardSuitEnum.HEARTS, CardValueEnum.FOUR);
        public static PokerCard CARD_5_H = new (CardSuitEnum.HEARTS, CardValueEnum.FIVE);
        public static PokerCard CARD_6_H = new (CardSuitEnum.HEARTS, CardValueEnum.SIX);
        public static PokerCard CARD_7_H = new (CardSuitEnum.HEARTS, CardValueEnum.SEVEN);
        public static PokerCard CARD_8_H = new (CardSuitEnum.HEARTS, CardValueEnum.EIGHT);
        public static PokerCard CARD_9_H = new (CardSuitEnum.HEARTS, CardValueEnum.NINE);
        public static PokerCard CARD_X_H = new (CardSuitEnum.HEARTS, CardValueEnum.TEN);
        public static PokerCard CARD_J_H = new (CardSuitEnum.HEARTS, CardValueEnum.JACK);
        public static PokerCard CARD_Q_H = new (CardSuitEnum.HEARTS, CardValueEnum.QUEEN);
        public static PokerCard CARD_K_H = new (CardSuitEnum.HEARTS, CardValueEnum.KING);

        public static PokerCard CARD_A_S = new (CardSuitEnum.SPADES, CardValueEnum.ACE);
        public static PokerCard CARD_2_S = new (CardSuitEnum.SPADES, CardValueEnum.TWO);
        public static PokerCard CARD_3_S = new (CardSuitEnum.SPADES, CardValueEnum.THREE);
        public static PokerCard CARD_4_S = new (CardSuitEnum.SPADES, CardValueEnum.FOUR);
        public static PokerCard CARD_5_S = new (CardSuitEnum.SPADES, CardValueEnum.FIVE);
        public static PokerCard CARD_6_S = new (CardSuitEnum.SPADES, CardValueEnum.SIX);
        public static PokerCard CARD_7_S = new (CardSuitEnum.SPADES, CardValueEnum.SEVEN);
        public static PokerCard CARD_8_S = new (CardSuitEnum.SPADES, CardValueEnum.EIGHT);
        public static PokerCard CARD_9_S = new (CardSuitEnum.SPADES, CardValueEnum.NINE);
        public static PokerCard CARD_X_S = new (CardSuitEnum.SPADES, CardValueEnum.TEN);
        public static PokerCard CARD_J_S = new (CardSuitEnum.SPADES, CardValueEnum.JACK);
        public static PokerCard CARD_Q_S = new (CardSuitEnum.SPADES, CardValueEnum.QUEEN);
        public static PokerCard CARD_K_S = new (CardSuitEnum.SPADES, CardValueEnum.KING);

        #endregion

    }
}