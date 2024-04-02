using System;
using System.Collections.Generic;
using TCSHoldEmPoker.Data;
using TCSHoldEmPoker.Models.Define;
using TCSHoldEmPoker.Network.Define;
using TCSHoldEmPoker.Network.Events;
using UnityEngine.Diagnostics;

namespace TCSHoldEmPoker.Network.Data {
    public static class PokerGameEventByteConverter {

        #region Methods

        private static bool BytesToPokerGameEvent<EVT> (byte[] bytes, int eventSize, out EVT evt, Func<byte[], int, EVT> uniqueDataProcess, int startIndex = 0) 
            where EVT : PokerGameEvent {

            if (bytes.Length - startIndex < eventSize) {
                evt = null;
                return false;
            }

            int i = startIndex;
            // COMMON DATA
            i += ByteConverterUtils.SIZEOF_NETWORK_ACTIVITY_START;      // START of Network Activity Signature
            i += ByteConverterUtils.SIZEOF_NETWORK_ACTIVITY_ID;         // Network Activity ID
            // Game Table ID
            Int32 gameTableID = BitConverter.ToInt32 (bytes, startIndex: i);
            i += sizeof (Int32);

            evt = uniqueDataProcess (bytes, i);
            evt.gameTableID = gameTableID;
            return true;
        }

        private static byte[] BytesFromPokerGameEvent<EVT> (EVT evt, int eventSize, Func<EVT, List<byte>> uniqueDataProcess) where EVT : PokerGameEvent {
            if (evt == null)
                return null;

            byte[] returnBytes = new byte[eventSize];

            int i = 0;
            // Network Activity START Signature
            foreach (byte startByte in BitConverter.GetBytes (ByteConverterUtils.NETWORK_ACTIVITY_START)) {
                returnBytes[i] = startByte;
                i++;
            }
            // Network Activity ID
            Int16 netWorkActivityID = (Int16)((Int16)NetworkActivityID.POKER_GAME_EVENT_PREFIX | (Int16)evt.GameEventType);
            foreach (byte nIDByte in BitConverter.GetBytes (netWorkActivityID)) {
                returnBytes[i] = nIDByte;
                i++;
            }
            // Game Table ID
            foreach (byte tableStakeByte in BitConverter.GetBytes (evt.gameTableID)) {
                returnBytes[i] = tableStakeByte;
                i++;
            }
            // Unique Data
            foreach (byte uniqueByte in uniqueDataProcess (evt)) {
                returnBytes[i] = uniqueByte;
                i++;
            }
            // Network Activity START Signature
            foreach (byte startByte in BitConverter.GetBytes (ByteConverterUtils.NETWORK_ACTIVITY_END)) {
                returnBytes[i] = startByte;
                i++;
            }

            return returnBytes;
        }

        // PLAYER JOIN

        public static bool BytesToPokerGameEventPlayerJoin (byte[] bytes, out PlayerJoinGameEvent evt, int startIndex = 0) {
            return BytesToPokerGameEvent (bytes, eventSize: ByteConverterUtils.SIZEOF_POKER_GAME_EVENT_PLAYER_JOIN, out evt,
                uniqueDataProcess: (bytes, startIndex) => {
                    int i = startIndex;
                    // Player ID
                    Int32 playerID = BitConverter.ToInt32 (bytes, startIndex: i);
                    i += sizeof (Int32);
                    // Buy-In Chips
                    Int32 buyInChips = BitConverter.ToInt32 (bytes, startIndex: i);
                    i += sizeof (Int32);

                    return new () {
                        playerID = playerID,
                        buyInChips = buyInChips,
                    };
                }, startIndex);
        }

        public static byte[] BytesFromPokerGameEventPlayerJoin (PlayerJoinGameEvent evt) {
            return BytesFromPokerGameEvent (evt, eventSize: ByteConverterUtils.SIZEOF_POKER_GAME_EVENT_PLAYER_JOIN,
                uniqueDataProcess: (evt) => {
                    List<byte> uniqueByteList = new ();

                    // Player ID
                    foreach (byte playerIDByte in BitConverter.GetBytes (evt.playerID))
                        uniqueByteList.Add (playerIDByte);
                    // Buy-In Chips
                    foreach (byte chipsByte in BitConverter.GetBytes (evt.buyInChips))
                        uniqueByteList.Add (chipsByte);

                    return uniqueByteList;
                });
        }

        // PLAYER LEAVE

        public static bool BytesToPokerGameEventPlayerLeave (byte[] bytes, out PlayerLeaveGameEvent evt, int startIndex = 0) {
            return BytesToPokerGameEvent (bytes, eventSize: ByteConverterUtils.SIZEOF_POKER_GAME_EVENT_PLAYER_LEAVE, out evt,
                uniqueDataProcess: (bytes, startIndex) => {
                    int i = startIndex;
                    // Player ID
                    Int32 playerID = BitConverter.ToInt32 (bytes, startIndex: i);
                    i += sizeof (Int32);

                    return new () {
                        playerID = playerID,
                    };
                }, startIndex);
        }

        public static byte[] BytesFromPokerGameEventPlayerLeave (PlayerLeaveGameEvent evt) {
            return BytesFromPokerGameEvent (evt, eventSize: ByteConverterUtils.SIZEOF_POKER_GAME_EVENT_PLAYER_LEAVE,
                uniqueDataProcess: (evt) => {
                    List<byte> uniqueByteList = new ();

                    // Player ID
                    foreach (byte playerIDByte in BitConverter.GetBytes (evt.playerID))
                        uniqueByteList.Add (playerIDByte);

                    return uniqueByteList;
                });
        }

        // PLAYER CARD DEAL

        public static bool BytesToPokerGameEventPlayerCardDeal (byte[] bytes, out PlayerCardsDealGameEvent evt, int startIndex = 0) {
            return BytesToPokerGameEvent (bytes, eventSize: ByteConverterUtils.SIZEOF_POKER_GAME_EVENT_PLAYER_CARD_DEAL, out evt,
                uniqueDataProcess: (bytes, startIndex) => {
                    int i = startIndex;
                    // Player ID
                    Int32 playerID = BitConverter.ToInt32 (bytes, startIndex: i);
                    i += sizeof (Int32);
                    // Player Cards
                    List<PokerCard> cards = new ();
                    for (int c = 0; c < HoldEmPokerDefines.POKER_PLAYER_DEAL_COUNT; c++) {
                        cards.Add (GameModelByteConverter.ByteToPokerCard (bytes[i]));
                        i += ByteConverterUtils.SIZEOF_CARD_DATA;
                    }

                    return new () {
                        playerID = playerID,
                        cards = cards,
                    };
                }, startIndex);
        }

        public static byte[] BytesFromPokerGameEventPlayerCardDeal (PlayerCardsDealGameEvent evt) {
            return BytesFromPokerGameEvent (evt, eventSize: ByteConverterUtils.SIZEOF_POKER_GAME_EVENT_PLAYER_CARD_DEAL,
                uniqueDataProcess: (evt) => {
                    List<byte> uniqueByteList = new ();

                    // Player ID
                    foreach (byte playerIDByte in BitConverter.GetBytes (evt.playerID))
                        uniqueByteList.Add (playerIDByte);
                    // Player Cards
                    foreach (var card in evt.cards)
                        uniqueByteList.Add (GameModelByteConverter.ByteFromPokerCard (card));

                    return uniqueByteList;
                });
        }

        // COMMUNITY CARD DEAL

        public static bool BytesToPokerGameEventCommunityCardDeal (byte[] bytes, out CommunityCardDealGameEvent evt, int startIndex = 0) {
            return BytesToPokerGameEvent (bytes, eventSize: ByteConverterUtils.SIZEOF_POKER_GAME_EVENT_COMMUNITY_CARD_DEAL, out evt,
                uniqueDataProcess: (bytes, startIndex) => {
                    int i = startIndex;
                    // Revealed Card
                    PokerCard card = GameModelByteConverter.ByteToPokerCard (bytes[i]);
                    i += ByteConverterUtils.SIZEOF_CARD_DATA;
                    // Community Card Index
                    Int16 cardIndex = BitConverter.ToInt16 (bytes, startIndex: i);
                    i += sizeof (Int16);

                    return new () {
                        pokerCard = card,
                        cardIndex = cardIndex,
                    };
                }, startIndex);
        }

        public static byte[] BytesFromPokerGameEventCommunityCardDeal (CommunityCardDealGameEvent evt) {
            return BytesFromPokerGameEvent (evt, eventSize: ByteConverterUtils.SIZEOF_POKER_GAME_EVENT_COMMUNITY_CARD_DEAL,
                uniqueDataProcess: (evt) => {
                    List<byte> uniqueByteList = new () {
                        // Revealed Card
                        GameModelByteConverter.ByteFromPokerCard (evt.pokerCard)
                    };
                    // Community Card Index
                    foreach (byte cardIndexByte in BitConverter.GetBytes (evt.cardIndex))
                        uniqueByteList.Add (cardIndexByte);

                    return uniqueByteList;
                });
        }

        // ANTE START

        public static bool BytesToPokerGameEventAnteStart (byte[] bytes, out AnteStartGameEvent evt, int startIndex = 0) {
            return BytesToPokerGameEvent (bytes, eventSize: ByteConverterUtils.SIZEOF_POKER_GAME_EVENT_ANTE_START, out evt,
                uniqueDataProcess: (bytes, startIndex) => {
                    // NO UNIQUE DATA
                    return new ();
                }, startIndex);
        }

        public static byte[] BytesFromPokerGameEventAnteStart (AnteStartGameEvent evt) {
            return BytesFromPokerGameEvent (evt, eventSize: ByteConverterUtils.SIZEOF_POKER_GAME_EVENT_ANTE_START,
                uniqueDataProcess: (evt) => {
                    List<byte> uniqueByteList = new ();
                    // NO UNIQUE DATA
                    return uniqueByteList;
                });
        }

        // ANTE PHASE CHANGE

        public static bool BytesToPokerGameEventAntePhaseChange (byte[] bytes, out GamePhaseChangeGameEvent evt, int startIndex = 0) {
            return BytesToPokerGameEvent (bytes, eventSize: ByteConverterUtils.SIZEOF_POKER_GAME_EVENT_ANTE_PHASE_CHANGE, out evt,
                uniqueDataProcess: (bytes, startIndex) => {
                    int i = startIndex;
                    // New Game Phase
                    PokerGamePhaseEnum newGamePhase = (PokerGamePhaseEnum)bytes[i];
                    i += ByteConverterUtils.SIZEOF_GAME_PHASE;

                    return new () {
                        gamePhase = newGamePhase,
                    };
                }, startIndex);
        }

        public static byte[] BytesFromPokerGameEventAntePhaseChange (GamePhaseChangeGameEvent evt) {
            return BytesFromPokerGameEvent (evt, eventSize: ByteConverterUtils.SIZEOF_POKER_GAME_EVENT_ANTE_PHASE_CHANGE,
                uniqueDataProcess: (evt) => {
                    List<byte> uniqueByteList = new () {
                        // New Game Phase
                        (byte)evt.gamePhase
                    };

                    return uniqueByteList;
                });
        }

        // ANTE TURN CHANGE

        public static bool BytesToPokerGameEventAnteTurnChange (byte[] bytes, out ChangeTurnSeatIndexGameEvent evt, int startIndex = 0) {
            return BytesToPokerGameEvent (bytes, eventSize: ByteConverterUtils.SIZEOF_POKER_GAME_EVENT_ANTE_TURN_CHANGE, out evt,
                uniqueDataProcess: (bytes, startIndex) => {
                    int i = startIndex;
                    // Turning Seat Index
                    Int16 seatIndex = BitConverter.ToInt16 (bytes, startIndex: i);
                    i += sizeof (Int16);

                    return new () {
                        seatIndex = seatIndex,
                    };
                }, startIndex);
        }

        public static byte[] BytesFromPokerGameEventAnteTurnChange (ChangeTurnSeatIndexGameEvent evt) {
            return BytesFromPokerGameEvent (evt, eventSize: ByteConverterUtils.SIZEOF_POKER_GAME_EVENT_ANTE_TURN_CHANGE,
                uniqueDataProcess: (evt) => {
                    List<byte> uniqueByteList = new ();

                    // Turning Seat Index
                    foreach (byte seatIndexByte in BitConverter.GetBytes (evt.seatIndex))
                        uniqueByteList.Add (seatIndexByte);

                    return uniqueByteList;
                });
        }

        // ANTE END

        public static bool BytesToPokerGameEventAnteEnd (byte[] bytes, out AnteEndGameEvent evt, int startIndex = 0) {
            return BytesToPokerGameEvent (bytes, eventSize: ByteConverterUtils.SIZEOF_POKER_GAME_EVENT_ANTE_END, out evt,
                uniqueDataProcess: (bytes, startIndex) => {
                    // NO UNIQUE DATA
                    return new ();
                }, startIndex);
        }

        public static byte[] BytesFromPokerGameEventAnteEnd (AnteEndGameEvent evt) {
            return BytesFromPokerGameEvent (evt, eventSize: ByteConverterUtils.SIZEOF_POKER_GAME_EVENT_ANTE_END,
                uniqueDataProcess: (evt) => {
                    List<byte> uniqueByteList = new ();
                    // NO UNIQUE DATA
                    return uniqueByteList;
                });
        }

        #endregion

    }
}