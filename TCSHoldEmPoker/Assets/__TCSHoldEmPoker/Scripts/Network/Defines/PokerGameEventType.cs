using System;

namespace TCSHoldEmPoker.Network.Define {
    public enum PokerGameEventType : Int16 {
    
        NULL                            = 0x0000,

        // Connectivity Events
        PLAYER_JOIN                     = 0x0001,
        PLAYER_LEAVE                    = 0x0002,

        // Game Progression Events
        ANTE_START                      = 0x0003,
        ANTE_PHASE_CHANGE               = 0x0004,
        ANTE_TURN_CHANGE                = 0x0005,
        ANTE_END                        = 0x0006,

        // Card Dealing Events
        PLAYER_CARD_DEAL                = 0x0007,
        COMMUNITY_CARD_DEAL             = 0x0008,

        // Player Action Events
        PLAYER_BET_BLIND                = 0x0009,
        PLAYER_BET_CHECK                = 0x000A,
        PLAYER_BET_CALL                 = 0x000B,
        PLAYER_BET_CALL_ALL_IN          = 0x000C,
        PLAYER_BET_RAISE                = 0x000D,
        PLAYER_BET_RAISE_ALL_IN         = 0x000E,
        PLAYER_BET_FOLD                 = 0x000F,

        // Win Condition Events
        TABLE_GATHER_WAGERS             = 0x0010,
        PLAYER_CARDS_REVEAL             = 0x0012,
        PLAYER_WIN                      = 0x0013,
    }
}