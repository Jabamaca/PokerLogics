using System;
using System.Collections.Generic;
using TCSHoldEmPoker.Models.Define;

namespace TCSHoldEmPoker.Models {
    public class PokerHand : IComparable {

        protected const int POKER_HAND_SIZE = HoldEmPokerDefines.POKER_HAND_SIZE;

        #region Properties

        private readonly PokerHandRankEnum _handRank = PokerHandRankEnum.NULL;
        public PokerHandRankEnum HandRank => _handRank;

        protected readonly PokerCard[] _cardOrder = new PokerCard[POKER_HAND_SIZE];
        public IReadOnlyList<PokerCard> CardOrder => _cardOrder;

        #endregion

        #region Constructors

        public PokerHand (PokerHandRankEnum rank, IEnumerable<PokerCard> cards) {
            _handRank = rank;

            int i = 0;
            foreach (var card in cards) {
                _cardOrder[i] = card;
                i++;
                if (i >= POKER_HAND_SIZE)
                    break;
            }
        }

        #endregion

        #region Overrides

        public override string ToString () {
            string print = PokerHandUtils.RankString (HandRank);

            print += " - ";

            string split = "  ";
            foreach (var card in CardOrder) {
                print += card + split;
            }

            print = print.Remove (print.Length - split.Length);

            return print;
        }

        #endregion

        #region Implement IComparable

        public int CompareTo (object obj) {
            if (obj == null)
                return 1;

            PokerHand other = obj as PokerHand;

            int handRankComp = this.HandRank.CompareTo (other.HandRank);

            if (handRankComp == 0) { // If Hand Ranks are equal.
                for (int i = 0; i < POKER_HAND_SIZE; i++) {
                    int currentCardValueComp = this.CardOrder[i].Value.CompareTo (other.CardOrder[i].Value);

                    if (currentCardValueComp == 0) // If current priority Card Values are equal.
                        continue; // Proceed to next priority Card Value.

                    return currentCardValueComp;
                }
                
                return 0; // Hands are absolutely equal in Rank and Value.
            } else {
                return handRankComp;
            }
        }

        #endregion

    }
}