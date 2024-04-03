using System;
using System.Collections.Generic;
using TCSHoldEmPoker.Data;
using TCSHoldEmPoker.Models.Define;
using TCSHoldEmPoker.Network.Define;
using TCSHoldEmPoker.Network.Events;

namespace TCSHoldEmPoker.Network.Data {
    public static class PokerGameEventByteConverter {

        private delegate int UniqueEventDataProcess<EVT> (byte[] bytes, out EVT evt, int startIndex = 0) where EVT : PokerGameEvent;

        #region Methods

        private static int BytesToPokerGameEvent<EVT> (byte[] bytes, out EVT evt, UniqueEventDataProcess<EVT> uniqueDataProcess, int startIndex = 0) 
            where EVT : PokerGameEvent {

            int i = startIndex;
            // COMMON DATA
            i += ByteConverterUtils.SIZEOF_NETWORK_ACTIVITY_START;      // START of Network Activity Signature
            i += ByteConverterUtils.SIZEOF_NETWORK_ACTIVITY_ID;         // Network Activity ID
            // Game Table ID
            Int32 gameTableID = BitConverter.ToInt32 (bytes, startIndex: i);
            i += sizeof (Int32);

            i = uniqueDataProcess (bytes, out evt, startIndex: i);
            evt.gameTableID = gameTableID;
            i += ByteConverterUtils.SIZEOF_NETWORK_ACTIVITY_END;        // END of Network Activity Signature
            return i;
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
            // Network Activity END Signature
            foreach (byte startByte in BitConverter.GetBytes (ByteConverterUtils.NETWORK_ACTIVITY_END)) {
                returnBytes[i] = startByte;
                i++;
            }

            return returnBytes;
        }

        #region Connectivity Game Events Conversion

        // PLAYER JOIN

        public static int BytesToPokerGameEventPlayerJoin (byte[] bytes, out PlayerJoinGameEvent evt, int startIndex = 0) {
            return BytesToPokerGameEvent (bytes, out evt, PokerGameEventPlayerJoinUniqueDataProcess, startIndex);
        }

        private static int PokerGameEventPlayerJoinUniqueDataProcess (byte[] bytes, out PlayerJoinGameEvent evt, int startIndex = 0) {
            int i = startIndex;
            // Player ID
            Int32 playerID = BitConverter.ToInt32 (bytes, startIndex: i);
            i += sizeof (Int32);
            // Buy-In Chips
            Int32 buyInChips = BitConverter.ToInt32 (bytes, startIndex: i);
            i += sizeof (Int32);

            evt = new () {
                playerID = playerID,
                buyInChips = buyInChips,
            };
            return i;
        }

        public static byte[] BytesFromPokerGameEventPlayerJoin (PlayerJoinGameEvent evt) {
            int eventSize = ByteConverterUtils.SIZEOF_POKER_GAME_EVENT_PLAYER_JOIN;
            return BytesFromPokerGameEvent (evt, eventSize,
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

        public static int BytesToPokerGameEventPlayerLeave (byte[] bytes, out PlayerLeaveGameEvent evt, int startIndex = 0) {
            return BytesToPokerGameEvent (bytes, out evt, PokerGameEventPlayerLeaveUniqueDataProcess, startIndex);
        }

        private static int PokerGameEventPlayerLeaveUniqueDataProcess (byte[] bytes, out PlayerLeaveGameEvent evt, int startIndex = 0) {
            int i = startIndex;
            // Player ID
            Int32 playerID = BitConverter.ToInt32 (bytes, startIndex: i);
            i += sizeof (Int32);

            evt = new () {
                playerID = playerID,
            };
            return i;
        }

        public static byte[] BytesFromPokerGameEventPlayerLeave (PlayerLeaveGameEvent evt) {
            int eventSize = ByteConverterUtils.SIZEOF_POKER_GAME_EVENT_PLAYER_LEAVE;
            return BytesFromPokerGameEvent (evt, eventSize,
                uniqueDataProcess: (evt) => {
                    List<byte> uniqueByteList = new ();

                    // Player ID
                    foreach (byte playerIDByte in BitConverter.GetBytes (evt.playerID))
                        uniqueByteList.Add (playerIDByte);

                    return uniqueByteList;
                });
        }

        #endregion

        #region Ante Progression Game Events Conversion

        // ANTE START

        public static int BytesToPokerGameEventAnteStart (byte[] bytes, out AnteStartGameEvent evt, int startIndex = 0) {
            return BytesToPokerGameEvent (bytes, out evt, PokerGameEventAnteStartUniqueDataProcess, startIndex);
        }

        public static int PokerGameEventAnteStartUniqueDataProcess (byte[] bytes, out AnteStartGameEvent evt, int startIndex = 0) {
            int i = startIndex;
            // NO UNIQUE DATA

            evt = new ();
            return i;
        }

        public static byte[] BytesFromPokerGameEventAnteStart (AnteStartGameEvent evt) {
            int eventSize = ByteConverterUtils.SIZEOF_POKER_GAME_EVENT_ANTE_START;
            return BytesFromPokerGameEvent (evt, eventSize,
                uniqueDataProcess: (evt) => {
                    List<byte> uniqueByteList = new ();
                    // NO UNIQUE DATA
                    return uniqueByteList;
                });
        }

        // ANTE PHASE CHANGE

        public static int BytesToPokerGameEventAntePhaseChange (byte[] bytes, out GamePhaseChangeGameEvent evt, int startIndex = 0) {
            return BytesToPokerGameEvent (bytes, out evt, PokerGameEventAntePhaseChangeUniqueDataProcess, startIndex);
        }

        public static int PokerGameEventAntePhaseChangeUniqueDataProcess (byte[] bytes, out GamePhaseChangeGameEvent evt, int startIndex = 0) {
            int i = startIndex;
            // New Game Phase
            PokerGamePhaseEnum newGamePhase = (PokerGamePhaseEnum)bytes[i];
            i += ByteConverterUtils.SIZEOF_GAME_PHASE;

            evt = new () {
                gamePhase = newGamePhase,
            };
            return i;
        }

        public static byte[] BytesFromPokerGameEventAntePhaseChange (GamePhaseChangeGameEvent evt) {
            int eventSize = ByteConverterUtils.SIZEOF_POKER_GAME_EVENT_ANTE_PHASE_CHANGE;
            return BytesFromPokerGameEvent (evt, eventSize,
                uniqueDataProcess: (evt) => {
                    List<byte> uniqueByteList = new () {
                        // New Game Phase
                        (byte)evt.gamePhase
                    };

                    return uniqueByteList;
                });
        }

        // ANTE TURN CHANGE

        public static int BytesToPokerGameEventAnteTurnChange (byte[] bytes, out ChangeTurnSeatIndexGameEvent evt, int startIndex = 0) {
            return BytesToPokerGameEvent (bytes, out evt, PokerGameEventAnteTurnChangeUniqueDataProcess, startIndex);
        }

        public static int PokerGameEventAnteTurnChangeUniqueDataProcess (byte[] bytes, out ChangeTurnSeatIndexGameEvent evt, int startIndex = 0) {
            int i = startIndex;
            // Turning Seat Index
            Int16 seatIndex = BitConverter.ToInt16 (bytes, startIndex: i);
            i += sizeof (Int16);

            evt = new () {
                seatIndex = seatIndex,
            };
            return i;
        }

        public static byte[] BytesFromPokerGameEventAnteTurnChange (ChangeTurnSeatIndexGameEvent evt) {
            int eventSize = ByteConverterUtils.SIZEOF_POKER_GAME_EVENT_ANTE_TURN_CHANGE;
            return BytesFromPokerGameEvent (evt, eventSize,
                uniqueDataProcess: (evt) => {
                    List<byte> uniqueByteList = new ();

                    // Turning Seat Index
                    foreach (byte seatIndexByte in BitConverter.GetBytes (evt.seatIndex))
                        uniqueByteList.Add (seatIndexByte);

                    return uniqueByteList;
                });
        }

        // ANTE END

        public static int BytesToPokerGameEventAnteEnd (byte[] bytes, out AnteEndGameEvent evt, int startIndex = 0) {
            return BytesToPokerGameEvent (bytes, out evt, PokerGameEventAnteEndUniqueDataProcess, startIndex);
        }

        public static int PokerGameEventAnteEndUniqueDataProcess (byte[] bytes, out AnteEndGameEvent evt, int startIndex = 0) {
            int i = startIndex;
            // NO UNIQUE DATA

            evt = new ();
            return i;
        }

        public static byte[] BytesFromPokerGameEventAnteEnd (AnteEndGameEvent evt) {
            int eventSize = ByteConverterUtils.SIZEOF_POKER_GAME_EVENT_ANTE_END;
            return BytesFromPokerGameEvent (evt, eventSize,
                uniqueDataProcess: (evt) => {
                    List<byte> uniqueByteList = new ();
                    // NO UNIQUE DATA
                    return uniqueByteList;
                });
        }

        #endregion

        #region Card Dealing Game Events Conversion

        // PLAYER CARD DEAL

        public static int BytesToPokerGameEventPlayerCardDeal (byte[] bytes, out PlayerCardsDealGameEvent evt, int startIndex = 0) {
            return BytesToPokerGameEvent (bytes, out evt, PokerGameEventPlayerDealCardUniqueDataProcess, startIndex);
        }

        private static int PokerGameEventPlayerDealCardUniqueDataProcess (byte[] bytes, out PlayerCardsDealGameEvent evt, int startIndex = 0) {
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

            evt = new () {
                playerID = playerID,
                cards = cards,
            };
            return i;
        }

        public static byte[] BytesFromPokerGameEventPlayerCardDeal (PlayerCardsDealGameEvent evt) {
            int eventSize = ByteConverterUtils.SIZEOF_POKER_GAME_EVENT_PLAYER_CARD_DEAL;
            return BytesFromPokerGameEvent (evt, eventSize,
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

        public static int BytesToPokerGameEventCommunityCardDeal (byte[] bytes, out CommunityCardDealGameEvent evt, int startIndex = 0) {
            return BytesToPokerGameEvent (bytes, out evt, PokerGameEventCommunityCardDealUniqueDataProcess, startIndex);
        }

        private static int PokerGameEventCommunityCardDealUniqueDataProcess (byte[] bytes, out CommunityCardDealGameEvent evt, int startIndex = 0) {
            int i = startIndex;
            // Revealed Card
            PokerCard card = GameModelByteConverter.ByteToPokerCard (bytes[i]);
            i += ByteConverterUtils.SIZEOF_CARD_DATA;
            // Community Card Index
            Int16 cardIndex = BitConverter.ToInt16 (bytes, startIndex: i);
            i += sizeof (Int16);

            evt = new () {
                pokerCard = card,
                cardIndex = cardIndex,
            };
            return i;
        }

        public static byte[] BytesFromPokerGameEventCommunityCardDeal (CommunityCardDealGameEvent evt) {
            int eventSize = ByteConverterUtils.SIZEOF_POKER_GAME_EVENT_COMMUNITY_CARD_DEAL;
            return BytesFromPokerGameEvent (evt, eventSize,
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

        #endregion

        #region Win Condition Game Events Conversion

        // TABLE GATHER WAGERS

        public static int BytesToPokerGameEventTableGatherWagers (byte[] bytes, out TableGatherWagersGameEvent evt, int startIndex = 0) {
            return BytesToPokerGameEvent (bytes, out evt, PokerGameEventTableGatherWagersUniqueDataProcess, startIndex);
        }

        private static int PokerGameEventTableGatherWagersUniqueDataProcess (byte[] bytes, out TableGatherWagersGameEvent evt, int startIndex = 0) {
            int i = startIndex;
            // New Cash Pot
            Int32 newCashPot = BitConverter.ToInt32 (bytes, startIndex: i);
            i += sizeof (Int32);

            evt = new () {
                newCashPot = newCashPot,
            };
            return i;
        }

        public static byte[] BytesFromPokerGameEventTableGatherWagers (TableGatherWagersGameEvent evt) {
            int eventSize = ByteConverterUtils.SIZEOF_POKER_GAME_EVENT_TABLE_GATHER_WAGERS;
            return BytesFromPokerGameEvent (evt, eventSize,
                uniqueDataProcess: (evt) => {
                    List<byte> uniqueByteList = new ();
                    // New Cash Pot
                    foreach (byte newCashPotByte in BitConverter.GetBytes (evt.newCashPot))
                        uniqueByteList.Add (newCashPotByte);

                    return uniqueByteList;
                });
        }

        // ALL PLAYER CARDS REVEAL

        public static int BytesToPokerGameEventAllPlayerCardsReveal (byte[] bytes, out AllPlayerCardsRevealGameEvent evt, int startIndex = 0) {
            return BytesToPokerGameEvent (bytes, out evt, PokerGameEventAllPlayerCardsRevealUniqueDataProcess, startIndex);
        }

        private static int PokerGameEventAllPlayerCardsRevealUniqueDataProcess (byte[] bytes, out AllPlayerCardsRevealGameEvent evt, int startIndex = 0) {
            int i = startIndex;
            // Participating Player Count
            Int16 playerCount = BitConverter.ToInt16 (bytes, startIndex: i);
            i += sizeof (Int16);
            // Revealed Player Hands
            Dictionary<Int32, IReadOnlyList<PokerCard>> revealedHands = new ();
            for (int p = 0; p < playerCount; p++) {
                // Player ID (KEY)
                Int32 playerID = BitConverter.ToInt32 (bytes, startIndex: i);
                i += sizeof (Int32);
                // Player Cards (VALUE)
                List<PokerCard> cards = new ();
                for (int c = 0; c < HoldEmPokerDefines.POKER_PLAYER_DEAL_COUNT; c++) {
                    PokerCard card = GameModelByteConverter.ByteToPokerCard (bytes[i]);
                    i += ByteConverterUtils.SIZEOF_CARD_DATA;
                    cards.Add (card);
                }
                revealedHands.Add (playerID, cards);
            }

            evt = new () {
                revealedHands = revealedHands,
            };
            return i;
        }

        public static byte[] BytesFromPokerGameEventAllPlayerCardsReveal (AllPlayerCardsRevealGameEvent evt) {
            int eventSize = ByteConverterUtils.SIZEOF_POKER_GAME_EVENT_ALL_PLAYER_CARDS_REVEAL_BASE +
                (evt.revealedHands.Count * ByteConverterUtils.SIZEOF_POKER_GAME_EVENT_ALL_PLAYER_CARDS_REVEAL_PLAYER);
            return BytesFromPokerGameEvent (evt, eventSize,
                uniqueDataProcess: (evt) => {
                    List<byte> uniqueByteList = new ();
                    // Participating Player Count
                    foreach (byte playerCountByte in BitConverter.GetBytes ((Int16)evt.revealedHands.Count))
                        uniqueByteList.Add (playerCountByte);
                    // Revealed Player Hands
                    foreach (var kvp in evt.revealedHands) {
                        // Player ID (KEY)
                        foreach (byte playerIDByte in BitConverter.GetBytes (kvp.Key))
                            uniqueByteList.Add (playerIDByte);
                        // Player Cards (VALUE)
                        foreach (var card in kvp.Value)
                            uniqueByteList.Add (GameModelByteConverter.ByteFromPokerCard (card));
                    }

                    return uniqueByteList;
                });
        }

        // PLAYER WIN

        public static int BytesToPokerGameEventPlayerWin (byte[] bytes, out PlayerWinGameEvent evt, int startIndex = 0) {
            return BytesToPokerGameEvent (bytes, out evt, PokerGameEventPlayerWinUniqueDataProcess, startIndex);
        }

        private static int PokerGameEventPlayerWinUniqueDataProcess (byte[] bytes, out PlayerWinGameEvent evt, int startIndex = 0) {
            int i = startIndex;
            // Player ID
            Int32 playerID = BitConverter.ToInt32 (bytes, startIndex: i);
            i += sizeof (Int32);
            // Chips Won
            Int32 chipsWon = BitConverter.ToInt32 (bytes, startIndex: i);
            i += sizeof (Int32);

            evt = new () {
                playerID = playerID,
                chipsWon = chipsWon,
            };
            return i;
        }

        public static byte[] BytesFromPokerGameEventPlayerWin (PlayerWinGameEvent evt) {
            int eventSize = ByteConverterUtils.SIZEOF_POKER_GAME_EVENT_PLAYER_WIN;
            return BytesFromPokerGameEvent (evt, eventSize,
                uniqueDataProcess: (evt) => {
                    List<byte> uniqueByteList = new ();
                    // Player ID
                    foreach (byte playerIDByte in BitConverter.GetBytes (evt.playerID))
                        uniqueByteList.Add (playerIDByte);
                    // Chips Won
                    foreach (byte chipsWonByte in BitConverter.GetBytes (evt.chipsWon))
                        uniqueByteList.Add (chipsWonByte);

                    return uniqueByteList;
                });
        }

        #endregion

        #endregion

    }
}