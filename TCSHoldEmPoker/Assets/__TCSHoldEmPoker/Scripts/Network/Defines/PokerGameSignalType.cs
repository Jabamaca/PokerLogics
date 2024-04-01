using System;

namespace TCSHoldEmPoker.Network.Define {
    public enum PokerGameSignalType : Int16 {
    
        NULL                                    = 0x0000,

        // Connectivity Signals
        PLAYER_REQUEST_JOIN                     = 0x0001,
        PLAYER_REQUEST_LEAVE                    = 0x0002,

        // Player Action Signals
        PLAYER_INPUT_BET_CHECK                  = 0x0003,
        PLAYER_INPUT_BET_CALL                   = 0x0004,
        PLAYER_INPUT_BET_RAISE                  = 0x0005,
        PLAYER_INPUT_BET_FOLD                   = 0x0006,

    }
}