using System.Collections.Generic;
using TCSHoldEmPoker.Models.Define;

namespace TCSHoldEmPoker.Models {
    public class ThreeOfAKindPokerHand : PokerHand {

        #region Properties

        public override PokerHandRankEnum HandRank => PokerHandRankEnum.THREE_OF_A_KIND;

        #endregion

        #region Constructors

        private ThreeOfAKindPokerHand (List<PokerCard> cards) : base (cards) { }

        #endregion

        #region Methods

        internal static bool GetStrongestHand (IEnumerable<PokerCard> cards, out ThreeOfAKindPokerHand strongestHand) {
            var sortedCardList = GetSortedCardList (cards, HighestToLowestValue);

            // Find the highest Trio, if posible.
            if (ExtractStrongestParity (parityCount: 3, sortedCardList, out var highTrio)) {
                List<PokerCard> finalHand = new ();

                finalHand.AddRange (highTrio); // Add Trio.
                AddMissingHighCards (finalHand, sortedCardList); // Add Kickers.

                strongestHand = new (finalHand);
                return true;
            }

            // NO Trio found.
            strongestHand = null;
            return false;
        }

        #endregion

    }
}