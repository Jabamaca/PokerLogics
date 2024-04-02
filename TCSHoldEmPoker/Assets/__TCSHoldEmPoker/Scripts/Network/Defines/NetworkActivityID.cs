using System;

namespace TCSHoldEmPoker.Network.Define {
    public enum NetworkActivityID : Int16 {
    
        NULL_PREFIX                                     = 0x0000,
        POKER_GAME_EVENT_PREFIX                         = 0x0100,
        POKER_GAME_SIGNAL_PREFIX                        = 0x0200,

        // POKER GAME EVENTS
        // Connectivity
        POKER_GAME_EVENT_PLAYER_JOIN                    = POKER_GAME_EVENT_PREFIX | PokerGameEventType.PLAYER_JOIN,
        POKER_GAME_EVENT_PLAYER_LEAVE                   = POKER_GAME_EVENT_PREFIX | PokerGameEventType.PLAYER_LEAVE,
        // Game Progression
        POKER_GAME_EVENT_ANTE_START                     = POKER_GAME_EVENT_PREFIX | PokerGameEventType.ANTE_START,
        POKER_GAME_EVENT_ANTE_PHASE_CHANGE              = POKER_GAME_EVENT_PREFIX | PokerGameEventType.ANTE_PHASE_CHANGE,
        POKER_GAME_EVENT_ANTE_TURN_CHANGE               = POKER_GAME_EVENT_PREFIX | PokerGameEventType.ANTE_TURN_CHANGE,
        POKER_GAME_EVENT_ANTE_END                       = POKER_GAME_EVENT_PREFIX | PokerGameEventType.ANTE_END,
        // Card Dealing
        POKER_GAME_EVENT_PLAYER_CARD_DEAL               = POKER_GAME_EVENT_PREFIX | PokerGameEventType.PLAYER_CARD_DEAL,
        POKER_GAME_EVENT_COMMUNITY_CARD_DEAL            = POKER_GAME_EVENT_PREFIX | PokerGameEventType.COMMUNITY_CARD_DEAL,
        // Player Action
        POKER_GAME_EVENT_PLAYER_BET_BLIND               = POKER_GAME_EVENT_PREFIX | PokerGameEventType.PLAYER_BET_BLIND,
        POKER_GAME_EVENT_PLAYER_BET_CHECK               = POKER_GAME_EVENT_PREFIX | PokerGameEventType.PLAYER_BET_CHECK,
        POKER_GAME_EVENT_PLAYER_BET_CALL                = POKER_GAME_EVENT_PREFIX | PokerGameEventType.PLAYER_BET_CALL,
        POKER_GAME_EVENT_PLAYER_BET_CALL_ALL_IN         = POKER_GAME_EVENT_PREFIX | PokerGameEventType.PLAYER_BET_CALL_ALL_IN,
        POKER_GAME_EVENT_PLAYER_BET_RAISE               = POKER_GAME_EVENT_PREFIX | PokerGameEventType.PLAYER_BET_RAISE,
        POKER_GAME_EVENT_PLAYER_BET_RAISE_ALL_IN        = POKER_GAME_EVENT_PREFIX | PokerGameEventType.PLAYER_BET_RAISE_ALL_IN,
        POKER_GAME_EVENT_PLAYER_BET_FOLD                = POKER_GAME_EVENT_PREFIX | PokerGameEventType.PLAYER_BET_FOLD,
        // Win Condition
        POKER_GAME_EVENT_TABLE_GATHER_WAGERS            = POKER_GAME_EVENT_PREFIX | PokerGameEventType.TABLE_GATHER_WAGERS,
        POKER_GAME_EVENT_ALL_PLAYER_CARDS_REVEAL        = POKER_GAME_EVENT_PREFIX | PokerGameEventType.ALL_PLAYER_CARDS_REVEAL,
        POKER_GAME_EVENT_PLAYER_WIN                     = POKER_GAME_EVENT_PREFIX | PokerGameEventType.PLAYER_WIN,

        // POKER GAME SIGNALS
        // Connectivity
        POKER_GAME_SIGNAL_PLAYER_REQUEST_JOIN           = POKER_GAME_SIGNAL_PREFIX | PokerGameSignalType.PLAYER_REQUEST_JOIN,
        POKER_GAME_SIGNAL_PLAYER_REQUEST_LEAVE          = POKER_GAME_SIGNAL_PREFIX | PokerGameSignalType.PLAYER_REQUEST_LEAVE,
        // Player Action
        POKER_GAME_SIGNAL_PLAYER_INPUT_BET_CHECK        = POKER_GAME_SIGNAL_PREFIX | PokerGameSignalType.PLAYER_INPUT_BET_CHECK,
        POKER_GAME_SIGNAL_PLAYER_INPUT_BET_CALL         = POKER_GAME_SIGNAL_PREFIX | PokerGameSignalType.PLAYER_INPUT_BET_CALL,
        POKER_GAME_SIGNAL_PLAYER_INPUT_BET_RAISE        = POKER_GAME_SIGNAL_PREFIX | PokerGameSignalType.PLAYER_INPUT_BET_RAISE,
        POKER_GAME_SIGNAL_PLAYER_INPUT_BET_FOLD         = POKER_GAME_SIGNAL_PREFIX | PokerGameSignalType.PLAYER_INPUT_BET_FOLD,
    }
}