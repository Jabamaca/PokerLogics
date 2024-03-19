namespace TCSHoldEmPoker.Models.Define {
    public enum PokerHandRankEnum {
        NULL            = 0x0,
        HIGH_CARD       = 0x1,
        ONE_PAIR        = 0x2,
        TWO_PAIR        = 0x3,
        THREE_OF_A_KIND = 0x4,
        STRAIGHT        = 0x5,
        FLUSH           = 0x6,
        FULL_HOUSE      = 0x7,
        FOUR_OF_A_KIND  = 0x8,
        STRAIGHT_FLUSH  = 0x9,
        ROYAL_FLUSH     = 0xA,
    }
}