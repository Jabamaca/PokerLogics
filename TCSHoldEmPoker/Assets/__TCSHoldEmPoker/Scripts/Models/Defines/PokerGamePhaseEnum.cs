namespace TCSHoldEmPoker.Models.Define {
    public enum PokerGamePhaseEnum {
        NULL        = 0x00,
        WAITING     = 0x10,
        PRE_FLOP    = 0x20,
        THE_FLOP    = 0x30,
        THE_TURN    = 0x40,
        THE_RIVER   = 0x50,
        SHOWDOWN    = 0x60,
        WINNING     = 0x70,
    }
}
