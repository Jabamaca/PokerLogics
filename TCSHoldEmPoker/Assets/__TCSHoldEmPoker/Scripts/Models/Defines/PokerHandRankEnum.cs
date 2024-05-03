namespace TCSHoldEmPoker.Models.Define {
    public enum PokerHandRankEnum {
        SAMPLE          = 0xFF,

        NULL            = 0x00,
        HIGH_CARD       = 0x01,
        ONE_PAIR        = 0x02,
        TWO_PAIR        = 0x03,
        THREE_OF_A_KIND = 0x04,
        STRAIGHT        = 0x05,
        FLUSH           = 0x06,
        FULL_HOUSE      = 0x07,
        FOUR_OF_A_KIND  = 0x08,
        STRAIGHT_FLUSH  = 0x09,
        ROYAL_FLUSH     = 0x0A,
    }
}