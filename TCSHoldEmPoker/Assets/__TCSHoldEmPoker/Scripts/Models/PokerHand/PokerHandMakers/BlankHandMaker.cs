using TCSHoldEmPoker.Models.Define;

namespace TCSHoldEmPoker.Models {
    public static class BlankHandMaker {

        public static readonly PokerHand BlankHand = new (PokerHandRankEnum.NULL, new PokerCard[] {
                PokerCard.BLANK,
                PokerCard.BLANK,
                PokerCard.BLANK,
                PokerCard.BLANK,
                PokerCard.BLANK,
        });

    }
}