namespace TCSHoldEmPoker.Models.Define {
    public enum PokerGamePhase {
        NULL        = 0x0,
        WAITING     = 0x1,
        PRE_FLOP    = 0x2,
        THE_FLOP    = 0x3,
        THE_TURN    = 0x4,
        THE_RIVER   = 0x5,
        SHOWDOWN    = 0x6,
        WINNING     = 0x7,
    }
}
