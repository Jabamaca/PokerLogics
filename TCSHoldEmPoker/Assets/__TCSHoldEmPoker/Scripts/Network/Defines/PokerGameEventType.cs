using System;

namespace TCSHoldEmPoker.Network.Define {
    public enum PokerGameEventType {
    
        NULL                            = 0x0000,

        // Connectivity Events
        PLAYER_JOIN                     = 0x0001,
        PLAYER_LEAVE                    = 0x0002,
        PLAYER_GET_KICKED               = 0x0003,

        // Ante Progression Events
        ANTE_START                      = 0x0010,
        ANTE_PHASE_CHANGE               = 0x0011,
        ANTE_TURN_CHANGE                = 0x0012,
        ANTE_END                        = 0x0013,

        // Card Dealing Events
        PLAYER_CARD_DEAL                = 0x001A,
        COMMUNITY_CARD_DEAL             = 0x001B,

        // Player Action Events
        PLAYER_BET_BLIND                = 0x0020,
        PLAYER_BET_CHECK                = 0x0021,
        PLAYER_BET_CALL_BASIC           = 0x0022,
        PLAYER_BET_CALL_ALL_IN          = 0x0023,
        PLAYER_BET_RAISE_BASIC          = 0x0024,
        PLAYER_BET_RAISE_ALL_IN         = 0x0025,
        PLAYER_BET_FOLD                 = 0x0026,

        // Win Condition Events
        UPDATE_MAIN_PRIZE_POT           = 0x0030,
        CREATE_SIDE_PRIZE_POT           = 0x0031,
        ALL_PLAYER_CARDS_REVEAL         = 0x0032,
        PLAYER_WIN                      = 0x0033,
    }
}