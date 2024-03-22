namespace TCSHoldEmPoker.Network.Define {
    public enum PokerGameEventType {
    
        NULL                    = 0x00,

        // Connectivity Events
        PLAYER_JOIN             = 0x01,
        PLAYER_LEAVE            = 0x02,

        // Game Progression Events
        ANTE_START              = 0x03,
        ANTE_PHASE_CHANGE       = 0x04,
        ANTE_TURN_CHANGE        = 0x05,
        ANTE_END                = 0x06,

        // Card Related Events
        PLAYER_CARD_DEAL        = 0x07,
        COMMUNITY_CARD_REVEAL   = 0x08,

        // Player Action Events
        PLAYER_BET_BLIND        = 0x09,
        PLAYER_BET_CHECK        = 0x0A,
        PLAYER_BET_CALL         = 0x0B,
        PLAYER_BET_CALL_ALL_IN  = 0x0C,
        PLAYER_BET_RAISE        = 0x0D,
        PLAYER_BET_RAISE_ALL_IN = 0x0E,
        PLAYER_FOLD             = 0x0F,

        // Win Condition Events
        TABLE_GATHER_WAGERS     = 0x10,
        PLAYER_CARDS_REVEAL     = 0x12,
        PLAYER_WIN              = 0x13,
    }
}