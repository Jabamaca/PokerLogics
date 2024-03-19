using System.Collections.Generic;
using TCSHoldEmPoker.Models.Define;

namespace TCSHoldEmPoker.Models {
    public class FourOfAKindPokerHand : PokerHand {

        #region Properties

        public override PokerHandRankEnum HandRank => PokerHandRankEnum.FOUR_OF_A_KIND;

        #endregion

        #region Constructors

        private FourOfAKindPokerHand (List<PokerCard> cards) : base (cards) { }

        #endregion

        #region Methods

        internal static bool GetStrongestHand (IEnumerable<PokerCard> cards, out FourOfAKindPokerHand strongestHand) {
            var sortedCardList = GetSortedCardList (cards, HighestToLowestValue);

            // Find the highest Quad, if posible.
            if (ExtractStrongestParity (parityCount: 4, sortedCardList, out var highQuad)) {
                List<PokerCard> finalHand = new ();

                finalHand.AddRange (highQuad); // Add Quad.
                AddMissingHighCards (finalHand, sortedCardList); // Add kickers.

                strongestHand = new (finalHand);
                return true;
            }

            // NO Quad found.
            strongestHand = null;
            return false;
        }

        #endregion

    }
}