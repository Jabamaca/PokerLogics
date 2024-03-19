using System.Collections.Generic;
using TCSHoldEmPoker.Models.Define;

namespace TCSHoldEmPoker.Models {
    public class HighCardPokerHand : PokerHand {

        #region Properties

        public override PokerHandRankEnum HandRank => PokerHandRankEnum.HIGH_CARD;

        #endregion

        #region Constructors

        private HighCardPokerHand (List<PokerCard> cards) : base (cards) { }

        #endregion

        #region Methods

        internal static bool GetStrongestHand (IEnumerable<PokerCard> cards, out HighCardPokerHand strongestHand) {
            var sortedCardList = GetSortedCardList (cards, HighestToLowestValue);

            List<PokerCard> finalCardList = new ();

            AddMissingHighCards (finalCardList, sortedCardList);

            strongestHand = new (finalCardList);
            return true;
        }

        #endregion

    }
}