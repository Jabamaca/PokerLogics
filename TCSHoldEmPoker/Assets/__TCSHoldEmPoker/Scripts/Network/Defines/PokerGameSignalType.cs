namespace TCSHoldEmPoker.Network.Define {
    public enum PokerGameSignalType {
    
        NULL                    = 0x00,

        // Connectivity Signals
        PLAYER_JOIN_REQUEST     = 0x01,
        PLAYER_LEAVE_REQUEST    = 0x02,

        // Player Action Signals
        PLAYER_BET_CHECK        = 0x03,
        PLAYER_BET_CALL         = 0x04,
        PLAYER_BET_RAISE        = 0x05,
        PLAYER_BET_FOLD         = 0x06,

    }
}