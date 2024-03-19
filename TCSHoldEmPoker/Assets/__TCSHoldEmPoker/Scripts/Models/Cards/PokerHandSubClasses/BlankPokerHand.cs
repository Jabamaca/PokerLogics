using System.Collections.Generic;
using System.Linq.Expressions;
using TCSHoldEmPoker.Models.Define;

namespace TCSHoldEmPoker.Models {
    public sealed class BlankPokerHand : PokerHand {

        #region Properties

        public override PokerHandRankEnum HandRank => PokerHandRankEnum.NULL;
        public static readonly BlankPokerHand BlankHand = new (new () {
                PokerCard.BLANK,
                PokerCard.BLANK,
                PokerCard.BLANK,
                PokerCard.BLANK,
                PokerCard.BLANK,
            });

        #endregion

        #region Constructors

        private BlankPokerHand (List<PokerCard> cards) : base (cards) { }

        #endregion

        #region Methods

        #endregion

    }
}