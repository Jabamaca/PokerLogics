using System;
using System.Collections.Generic;
using TCSHoldEmPoker.Models.Define;
using TCSHoldEmPoker.Network.Define;
using TCSHoldEmPoker.Network.Activity.PokerGameEvents;

namespace TCSHoldEmPoker.Network.Data {
    public static class PokerGameEventByteConverter {

        #region Defines

        private delegate void UniqueDataToEventProcess<EVT> (byte[] bytes, ref int currentDataIndex, out EVT evt) where EVT : PokerGameEvent;
        private delegate IReadOnlyList<byte> UniqueDataFromEventProcess<EVT> (EVT evt) where EVT : PokerGameEvent;

        #endregion

        #region Methods

        private static void BytesToPokerGameEvent<EVT> (byte[] bytes, ref int currentDataIndex, UniqueDataToEventProcess<EVT> uniqueDataProcess, out EVT evt) 
            where EVT : PokerGameEvent {

            // COMMON DATA
            currentDataIndex += ByteConverterUtils.SIZEOF_NETWORK_ACTIVITY_START;                   // START of Network Activity Signature
            currentDataIndex += ByteConverterUtils.SizeOf (NetworkActivityID.SAMPLE);               // Network Activity ID
            // Game Table ID
            ByteConverterUtils.BytesToInt32 (bytes, ref currentDataIndex, out var gameTableID);

            uniqueDataProcess (bytes, ref currentDataIndex, out evt);
            evt.gameTableID = gameTableID;
            currentDataIndex += ByteConverterUtils.SIZEOF_NETWORK_ACTIVITY_END;                     // END of Network Activity Signature
        }

        private static byte[] BytesFromPokerGameEvent<EVT> (EVT evt, int eventSize, UniqueDataFromEventProcess<EVT> uniqueDataProcess) 
            where EVT : PokerGameEvent {

            if (evt == null)
                return null;

            byte[] returnBytes = new byte[eventSize];

            int currentDataIndex = 0;
            // Network Activity START Signature
            ByteConverterUtils.AddBytesToArray (bytesToAdd: BitConverter.GetBytes (ByteConverterUtils.NETWORK_ACTIVITY_START),
                bytesLocation: returnBytes, ref currentDataIndex);
            // Network Activity ID
            Int16 netWorkActivityID = (Int16)((Int16)NetworkActivityID.POKER_GAME_EVENT_PREFIX | (Int16)evt.GameEventType);
            ByteConverterUtils.AddBytesToArray (bytesToAdd: BitConverter.GetBytes (netWorkActivityID),
                bytesLocation: returnBytes, ref currentDataIndex);
            // Game Table ID
            ByteConverterUtils.AddBytesToArray (bytesToAdd: BitConverter.GetBytes (evt.gameTableID),
                bytesLocation: returnBytes, ref currentDataIndex);
            // Unique Data
            ByteConverterUtils.AddBytesToArray (bytesToAdd: uniqueDataProcess (evt),
                bytesLocation: returnBytes, ref currentDataIndex);
            // Network Activity END Signature
            ByteConverterUtils.AddBytesToArray (bytesToAdd: BitConverter.GetBytes (ByteConverterUtils.NETWORK_ACTIVITY_END),
                bytesLocation: returnBytes, ref currentDataIndex);

            return returnBytes;
        }

        #region Connectivity Game Events Conversion

        // PLAYER JOIN

        public static void BytesToPokerGameEventPlayerJoin (byte[] bytes, ref int currentDataIndex, out PlayerJoinGameEvent evt) {
            BytesToPokerGameEvent (bytes, ref currentDataIndex, PokerGameEventPlayerJoinUniqueDataProcess, out evt);
        }

        private static void PokerGameEventPlayerJoinUniqueDataProcess (byte[] bytes, ref int currentDataIndex, out PlayerJoinGameEvent evt) {
            // Player ID
            ByteConverterUtils.BytesToInt32 (bytes, ref currentDataIndex, out var playerID);
            // Buy-In Chips
            ByteConverterUtils.BytesToInt32 (bytes, ref currentDataIndex, out var buyInChips);

            evt = new () {
                playerID = playerID,
                buyInChips = buyInChips,
            };
        }

        public static byte[] BytesFromPokerGameEventPlayerJoin (PlayerJoinGameEvent evt) {
            int eventSize = ByteConverterUtils.SizeOf (evt);
            return BytesFromPokerGameEvent (evt, eventSize,
                uniqueDataProcess: (evt) => {
                    List<byte> uniqueByteList = new ();
                    // Player ID
                    uniqueByteList.AddRange (BitConverter.GetBytes (evt.playerID));
                    // Buy-In Chips
                    uniqueByteList.AddRange (BitConverter.GetBytes (evt.buyInChips));

                    return uniqueByteList;
                });
        }

        // PLAYER LEAVE

        public static void BytesToPokerGameEventPlayerLeave (byte[] bytes, ref int currentDataIndex, out PlayerLeaveGameEvent evt) {
            BytesToPokerGameEvent (bytes, ref currentDataIndex, PokerGameEventPlayerLeaveUniqueDataProcess, out evt);
        }

        private static void PokerGameEventPlayerLeaveUniqueDataProcess (byte[] bytes, ref int currentDataIndex, out PlayerLeaveGameEvent evt) {
            // Player ID
            ByteConverterUtils.BytesToInt32 (bytes, ref currentDataIndex, out var playerID);

            evt = new () {
                playerID = playerID,
            };
        }

        public static byte[] BytesFromPokerGameEventPlayerLeave (PlayerLeaveGameEvent evt) {
            int eventSize = ByteConverterUtils.SizeOf (evt);
            return BytesFromPokerGameEvent (evt, eventSize,
                uniqueDataProcess: (evt) => {
                    List<byte> uniqueByteList = new ();
                    // Player ID
                    uniqueByteList.AddRange (BitConverter.GetBytes (evt.playerID));

                    return uniqueByteList;
                });
        }

        #endregion

        #region Ante Progression Game Events Conversion

        // ANTE START

        public static void BytesToPokerGameEventAnteStart (byte[] bytes, ref int currentDataIndex, out AnteStartGameEvent evt) {
            BytesToPokerGameEvent (bytes, ref currentDataIndex, PokerGameEventAnteStartUniqueDataProcess, out evt);
        }

        public static void PokerGameEventAnteStartUniqueDataProcess (byte[] bytes, ref int currentDataIndex, out AnteStartGameEvent evt) {
            // NO UNIQUE DATA

            evt = new ();
        }

        public static byte[] BytesFromPokerGameEventAnteStart (AnteStartGameEvent evt) {
            int eventSize = ByteConverterUtils.SizeOf (evt);
            return BytesFromPokerGameEvent (evt, eventSize,
                uniqueDataProcess: (evt) => {
                    List<byte> uniqueByteList = new ();
                    // NO UNIQUE DATA

                    return uniqueByteList;
                });
        }

        // ANTE PHASE CHANGE

        public static void BytesToPokerGameEventAntePhaseChange (byte[] bytes, ref int currentDataIndex, out GamePhaseChangeGameEvent evt) {
            BytesToPokerGameEvent (bytes, ref currentDataIndex, PokerGameEventAntePhaseChangeUniqueDataProcess, out evt);
        }

        public static void PokerGameEventAntePhaseChangeUniqueDataProcess (byte[] bytes, ref int currentDataIndex, out GamePhaseChangeGameEvent evt) {
            // New Game Phase
            GameModelByteConverter.BytesToGamePhase (bytes, ref currentDataIndex, out var newGamePhase);

            evt = new () {
                gamePhase = newGamePhase,
            };
        }

        public static byte[] BytesFromPokerGameEventAntePhaseChange (GamePhaseChangeGameEvent evt) {
            int eventSize = ByteConverterUtils.SizeOf (evt);
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

        public static void BytesToPokerGameEventAnteTurnChange (byte[] bytes, ref int currentDataIndex, out ChangeTurnSeatIndexGameEvent evt) {
            BytesToPokerGameEvent (bytes, ref currentDataIndex, PokerGameEventAnteTurnChangeUniqueDataProcess, out evt);
        }

        public static void PokerGameEventAnteTurnChangeUniqueDataProcess (byte[] bytes, ref int currentDataIndex, out ChangeTurnSeatIndexGameEvent evt) {
            // Turning Seat Index
            ByteConverterUtils.BytesToInt16 (bytes, ref currentDataIndex, out var seatIndex);

            evt = new () {
                seatIndex = seatIndex,
            };
        }

        public static byte[] BytesFromPokerGameEventAnteTurnChange (ChangeTurnSeatIndexGameEvent evt) {
            int eventSize = ByteConverterUtils.SizeOf (evt);
            return BytesFromPokerGameEvent (evt, eventSize,
                uniqueDataProcess: (evt) => {
                    List<byte> uniqueByteList = new ();
                    // Turning Seat Index
                    uniqueByteList.AddRange (BitConverter.GetBytes (evt.seatIndex));

                    return uniqueByteList;
                });
        }

        // ANTE END

        public static void BytesToPokerGameEventAnteEnd (byte[] bytes, ref int currentDataIndex, out AnteEndGameEvent evt) {
            BytesToPokerGameEvent (bytes, ref currentDataIndex, PokerGameEventAnteEndUniqueDataProcess, out evt);
        }

        public static void PokerGameEventAnteEndUniqueDataProcess (byte[] bytes, ref int currentDataIndex, out AnteEndGameEvent evt) {
            // NO UNIQUE DATA

            evt = new ();
        }

        public static byte[] BytesFromPokerGameEventAnteEnd (AnteEndGameEvent evt) {
            int eventSize = ByteConverterUtils.SizeOf (evt);
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

        public static void BytesToPokerGameEventPlayerCardDeal (byte[] bytes, ref int currentDataIndex, out PlayerCardsDealGameEvent evt) {
            BytesToPokerGameEvent (bytes, ref currentDataIndex, PokerGameEventPlayerDealCardUniqueDataProcess, out evt);
        }

        private static void PokerGameEventPlayerDealCardUniqueDataProcess (byte[] bytes, ref int currentDataIndex, out PlayerCardsDealGameEvent evt) {
            // Player ID
            ByteConverterUtils.BytesToInt32 (bytes, ref currentDataIndex, out var playerID);
            // Player Cards
            List<PokerCard> cards = new ();
            for (int c = 0; c < HoldEmPokerDefines.POKER_PLAYER_DEAL_COUNT; c++) {
                GameModelByteConverter.BytesToPokerCard (bytes, ref currentDataIndex, out var card);
                cards.Add (card);
            }

            evt = new () {
                playerID = playerID,
                cards = cards,
            };
        }

        public static byte[] BytesFromPokerGameEventPlayerCardDeal (PlayerCardsDealGameEvent evt) {
            int eventSize = ByteConverterUtils.SizeOf (evt);
            return BytesFromPokerGameEvent (evt, eventSize,
                uniqueDataProcess: (evt) => {
                    List<byte> uniqueByteList = new ();
                    // Player ID
                    uniqueByteList.AddRange (BitConverter.GetBytes (evt.playerID));
                    // Player Cards
                    foreach (var card in evt.cards)
                        uniqueByteList.AddRange (GameModelByteConverter.BytesFromPokerCard (card));

                    return uniqueByteList;
                });
        }

        // COMMUNITY CARD DEAL

        public static void BytesToPokerGameEventCommunityCardDeal (byte[] bytes, ref int currentDataIndex, out CommunityCardDealGameEvent evt) {
            BytesToPokerGameEvent (bytes, ref currentDataIndex, PokerGameEventCommunityCardDealUniqueDataProcess, out evt);
        }

        private static void PokerGameEventCommunityCardDealUniqueDataProcess (byte[] bytes, ref int currentDataIndex, out CommunityCardDealGameEvent evt) {
            // Revealed Card
            GameModelByteConverter.BytesToPokerCard (bytes, ref currentDataIndex, out var card);
            // Community Card Index
            ByteConverterUtils.BytesToInt16 (bytes, ref currentDataIndex, out var cardIndex);

            evt = new () {
                pokerCard = card,
                cardIndex = cardIndex,
            };
        }

        public static byte[] BytesFromPokerGameEventCommunityCardDeal (CommunityCardDealGameEvent evt) {
            int eventSize = ByteConverterUtils.SizeOf (evt);
            return BytesFromPokerGameEvent (evt, eventSize,
                uniqueDataProcess: (evt) => {
                    List<byte> uniqueByteList = new ();
                    // Revealed Card
                    uniqueByteList.AddRange (GameModelByteConverter.BytesFromPokerCard (evt.pokerCard));
                    // Community Card Index
                    uniqueByteList.AddRange (BitConverter.GetBytes (evt.cardIndex));

                    return uniqueByteList;
                });
        }

        #endregion

        #region Player Action Game Events Conversion

        // PLAYER BET BLIND

        public static void BytesToPokerGameEventPlayerBetBlind (byte[] bytes, ref int currentDataIndex, out PlayerBetBlindGameEvent evt) {
            BytesToPokerGameEvent (bytes, ref currentDataIndex, PokerGameEventPlayerBetBlindUniqueDataProcess, out evt);    
        }

        private static void PokerGameEventPlayerBetBlindUniqueDataProcess (byte[] bytes, ref int currentDataIndex, out PlayerBetBlindGameEvent evt) {
            // Player ID
            ByteConverterUtils.BytesToInt32 (bytes, ref currentDataIndex, out var playerID);
            // Chips Spent
            ByteConverterUtils.BytesToInt32 (bytes, ref currentDataIndex, out var chipsSpent);

            evt = new () {
                playerID = playerID,
                chipsSpent = chipsSpent,
            };
        }

        public static byte[] BytesFromPokerGameEventPlayerBetBlind (PlayerBetBlindGameEvent evt) {
            int eventSize = ByteConverterUtils.SIZEOF_POKER_GAME_EVENT_BET_BLIND;
            return BytesFromPokerGameEvent (evt, eventSize,
                uniqueDataProcess: (evt) => {
                    List<byte> uniqueByteList = new ();
                    // Player ID
                    uniqueByteList.AddRange (BitConverter.GetBytes (evt.playerID));
                    // Chips Spent
                    uniqueByteList.AddRange (BitConverter.GetBytes (evt.chipsSpent));

                    return uniqueByteList;
                });
        }

        // PLAYER BET CHECK

        public static void BytesToPokerGameEventPlayerBetCheck (byte[] bytes, ref int currentDataIndex, out PlayerBetCheckGameEvent evt) {
            BytesToPokerGameEvent (bytes, ref currentDataIndex, PokerGameEventPlayerBetCheckUniqueDataProcess, out evt);
        }

        private static void PokerGameEventPlayerBetCheckUniqueDataProcess (byte[] bytes, ref int currentDataIndex, out PlayerBetCheckGameEvent evt) {
            // Player ID
            ByteConverterUtils.BytesToInt32 (bytes, ref currentDataIndex, out var playerID);

            evt = new () {
                playerID = playerID,
            };
        }

        public static byte[] BytesFromPokerGameEventPlayerBetCheck (PlayerBetCheckGameEvent evt) {
            int eventSize = ByteConverterUtils.SIZEOF_POKER_GAME_EVENT_BET_CHECK;
            return BytesFromPokerGameEvent (evt, eventSize,
                uniqueDataProcess: (evt) => {
                    List<byte> uniqueByteList = new ();
                    // Player ID
                    uniqueByteList.AddRange (BitConverter.GetBytes (evt.playerID));

                    return uniqueByteList;
                });
        }

        // PLAYER BET CALL

        public static void BytesToPokerGameEventPlayerBetCallBasic (byte[] bytes, ref int currentDataIndex, out PlayerBetCallGameEvent evt) {
            BytesToPokerGameEvent (bytes, ref currentDataIndex, PokerGameEventPlayerBetCallUniqueDataProcessBasic, out evt);
        }

        public static void BytesToPokerGameEventPlayerBetCallAllIn (byte[] bytes, ref int currentDataIndex, out PlayerBetCallGameEvent evt) {
            BytesToPokerGameEvent (bytes, ref currentDataIndex, PokerGameEventPlayerBetCallUniqueDataProcessAllIn, out evt);
        }

        private static void PokerGameEventPlayerBetCallUniqueDataProcessBasic (byte[] bytes, ref int currentDataIndex, out PlayerBetCallGameEvent evt) {
            PokerGameEventPlayerBetCallUniqueDataProcess (bytes, ref currentDataIndex, isAllIn: false, out evt);
        }

        private static void PokerGameEventPlayerBetCallUniqueDataProcessAllIn (byte[] bytes, ref int currentDataIndex, out PlayerBetCallGameEvent evt) {
            PokerGameEventPlayerBetCallUniqueDataProcess (bytes, ref currentDataIndex, isAllIn: true, out evt);
        }

        private static void PokerGameEventPlayerBetCallUniqueDataProcess (byte[] bytes, ref int currentDataIndex, bool isAllIn, out PlayerBetCallGameEvent evt) {
            // Player ID
            ByteConverterUtils.BytesToInt32 (bytes, ref currentDataIndex, out var playerID);
            // Chips Spent
            ByteConverterUtils.BytesToInt32 (bytes, ref currentDataIndex, out var chipsSpent);

            evt = new () {
                playerID = playerID,
                chipsSpent = chipsSpent,
                isAllIn = isAllIn,
            };
        }

        public static byte[] BytesFromPokerGameEventPlayerBetCall (PlayerBetCallGameEvent evt) {
            int eventSize = ByteConverterUtils.SIZEOF_POKER_GAME_EVENT_BET_CALL;
            return BytesFromPokerGameEvent (evt, eventSize,
                uniqueDataProcess: (evt) => {
                    List<byte> uniqueByteList = new ();
                    // Player ID
                    uniqueByteList.AddRange (BitConverter.GetBytes (evt.playerID));
                    // Chips Spent
                    uniqueByteList.AddRange (BitConverter.GetBytes (evt.chipsSpent));

                    return uniqueByteList;
                });
        }

        // PLAYER BET RAISE

        public static void BytesToPokerGameEventPlayerBetRaiseBasic (byte[] bytes, ref int currentDataIndex, out PlayerBetRaiseGameEvent evt) {
            BytesToPokerGameEvent (bytes, ref currentDataIndex, PokerGameEventPlayerBetRaiseUniqueDataProcessBasic, out evt);
        }

        public static void BytesToPokerGameEventPlayerBetRaiseAllIn (byte[] bytes, ref int currentDataIndex, out PlayerBetRaiseGameEvent evt) {
            BytesToPokerGameEvent (bytes, ref currentDataIndex, PokerGameEventPlayerBetRaiseUniqueDataProcessAllIn, out evt);
        }

        private static void PokerGameEventPlayerBetRaiseUniqueDataProcessBasic (byte[] bytes, ref int currentDataIndex, out PlayerBetRaiseGameEvent evt) {
            PokerGameEventPlayerBetRaiseUniqueDataProcess (bytes, ref currentDataIndex, isAllIn: false, out evt);
        }

        private static void PokerGameEventPlayerBetRaiseUniqueDataProcessAllIn (byte[] bytes, ref int currentDataIndex, out PlayerBetRaiseGameEvent evt) {
            PokerGameEventPlayerBetRaiseUniqueDataProcess (bytes, ref currentDataIndex, isAllIn: true, out evt);
        }

        private static void PokerGameEventPlayerBetRaiseUniqueDataProcess (byte[] bytes, ref int currentDataIndex, bool isAllIn, out PlayerBetRaiseGameEvent evt) {
            // Player ID
            ByteConverterUtils.BytesToInt32 (bytes, ref currentDataIndex, out var playerID);
            // Chips Spent
            ByteConverterUtils.BytesToInt32 (bytes, ref currentDataIndex, out var chipsSpent);

            evt = new () {
                playerID = playerID,
                chipsSpent = chipsSpent,
                isAllIn = isAllIn,
            };
        }

        public static byte[] BytesFromPokerGameEventPlayerBetRaise (PlayerBetRaiseGameEvent evt) {
            int eventSize = ByteConverterUtils.SIZEOF_POKER_GAME_EVENT_BET_RAISE;
            return BytesFromPokerGameEvent (evt, eventSize,
                uniqueDataProcess: (evt) => {
                    List<byte> uniqueByteList = new ();
                    // Player ID
                    uniqueByteList.AddRange (BitConverter.GetBytes (evt.playerID));
                    // Chips Spent
                    uniqueByteList.AddRange (BitConverter.GetBytes (evt.chipsSpent));

                    return uniqueByteList;
                });
        }

        // PLAYER BET FOLD

        public static void BytesToPokerGameEventPlayerBetFold (byte[] bytes, ref int currentDataIndex, out PlayerFoldGameEvent evt) {
            BytesToPokerGameEvent (bytes, ref currentDataIndex, PokerGameEventPlayerBetFoldUniqueDataProcess, out evt);
        }

        private static void PokerGameEventPlayerBetFoldUniqueDataProcess (byte[] bytes, ref int currentDataIndex, out PlayerFoldGameEvent evt) {
            // Player ID
            ByteConverterUtils.BytesToInt32 (bytes, ref currentDataIndex, out var playerID);

            evt = new () {
                playerID = playerID,
            };
        }

        public static byte[] BytesFromPokerGameEventPlayerBetFold (PlayerFoldGameEvent evt) {
            int eventSize = ByteConverterUtils.SIZEOF_POKER_GAME_EVENT_BET_FOLD;
            return BytesFromPokerGameEvent (evt, eventSize,
                uniqueDataProcess: (evt) => {
                    List<byte> uniqueByteList = new ();
                    // Player ID
                    uniqueByteList.AddRange (BitConverter.GetBytes (evt.playerID));

                    return uniqueByteList;
                });
        }

        #endregion

        #region Win Condition Game Events Conversion

        // UPDATE MAIN PRIZE POT

        public static void BytesToPokerGameEventTableUpdateMainPrize (byte[] bytes, ref int currentDataIndex, out UpdateMainPrizePotGameEvent evt) {
            BytesToPokerGameEvent (bytes, ref currentDataIndex, PokerGameEventTableUpdateMainPrizeUniqueDataProcess, out evt);
        }

        private static void PokerGameEventTableUpdateMainPrizeUniqueDataProcess (byte[] bytes, ref int currentDataIndex, out UpdateMainPrizePotGameEvent evt) {
            // New Cash Pot
            ByteConverterUtils.BytesToInt32 (bytes, ref currentDataIndex, out var newCashPot);

            evt = new () {
                wagerPerPlayer = newCashPot,
            };
        }

        public static byte[] BytesFromPokerGameEventTableUpdateMainPrize (UpdateMainPrizePotGameEvent evt) {
            int eventSize = ByteConverterUtils.SizeOf (evt);
            return BytesFromPokerGameEvent (evt, eventSize,
                uniqueDataProcess: (evt) => {
                    List<byte> uniqueByteList = new ();
                    // New Cash Pot
                    uniqueByteList.AddRange (BitConverter.GetBytes (evt.wagerPerPlayer));

                    return uniqueByteList;
                });
        }

        // CREATE SIDE PRIZE POT

        public static void BytesToPokerGameEventTableCreateSidePrize (byte[] bytes, ref int currentDataIndex, out CreateSidePrizePotGameEvent evt) {
            BytesToPokerGameEvent (bytes, ref currentDataIndex, PokerGameEventTableCreateSidePrizeUniqueDataProcess, out evt);
        }

        private static void PokerGameEventTableCreateSidePrizeUniqueDataProcess (byte[] bytes, ref int currentDataIndex, out CreateSidePrizePotGameEvent evt) {
            // New Cash Pot
            ByteConverterUtils.BytesToInt32 (bytes, ref currentDataIndex, out var newCashPot);

            evt = new () {
                wagerPerPlayer = newCashPot,
            };
        }

        public static byte[] BytesFromPokerGameEventTableCreateSidePrize (CreateSidePrizePotGameEvent evt) {
            int eventSize = ByteConverterUtils.SizeOf (evt);
            return BytesFromPokerGameEvent (evt, eventSize,
                uniqueDataProcess: (evt) => {
                    List<byte> uniqueByteList = new ();
                    // New Cash Pot
                    uniqueByteList.AddRange (BitConverter.GetBytes (evt.wagerPerPlayer));

                    return uniqueByteList;
                });
        }

        // ALL PLAYER CARDS REVEAL

        public static void BytesToPokerGameEventAllPlayerCardsReveal (byte[] bytes, ref int currentDataIndex, out AllPlayerCardsRevealGameEvent evt) {
            BytesToPokerGameEvent (bytes, ref currentDataIndex, PokerGameEventAllPlayerCardsRevealUniqueDataProcess, out evt);
        }

        private static void PokerGameEventAllPlayerCardsRevealUniqueDataProcess (byte[] bytes, ref int currentDataIndex, out AllPlayerCardsRevealGameEvent evt) {
            // Participating Player Count
            ByteConverterUtils.BytesToInt16 (bytes, ref currentDataIndex, out var playerCount);
            // Revealed Player Hands
            Dictionary<Int32, IReadOnlyList<PokerCard>> revealedHands = new ();
            for (int p = 0; p < playerCount; p++) {
                // Player ID (KEY)
                ByteConverterUtils.BytesToInt32 (bytes, ref currentDataIndex, out var playerID);
                // Player Cards (VALUE)
                List<PokerCard> cards = new ();
                for (int c = 0; c < HoldEmPokerDefines.POKER_PLAYER_DEAL_COUNT; c++) {
                    GameModelByteConverter.BytesToPokerCard (bytes, ref currentDataIndex, out var card);
                    cards.Add (card);
                }
                revealedHands.Add (playerID, cards);
            }

            evt = new () {
                revealedHands = revealedHands,
            };
        }

        public static byte[] BytesFromPokerGameEventAllPlayerCardsReveal (AllPlayerCardsRevealGameEvent evt) {
            int eventSize = ByteConverterUtils.SizeOf (evt);

            return BytesFromPokerGameEvent (evt, eventSize,
                uniqueDataProcess: (evt) => {
                    List<byte> uniqueByteList = new ();
                    // Participating Player Count
                    uniqueByteList.AddRange (BitConverter.GetBytes ((Int16)evt.revealedHands.Count));
                    // Revealed Player Hands
                    foreach (var kvp in evt.revealedHands) {
                        // Player ID (KEY)
                        uniqueByteList.AddRange (BitConverter.GetBytes (kvp.Key));
                        // Player Cards (VALUE)
                        foreach (var card in kvp.Value)
                            uniqueByteList.AddRange (GameModelByteConverter.BytesFromPokerCard (card));
                    }

                    return uniqueByteList;
                });
        }

        // PLAYER WIN

        public static void BytesToPokerGameEventPlayerWin (byte[] bytes, ref int currentDataIndex, out PlayerWinGameEvent evt) {
            BytesToPokerGameEvent (bytes, ref currentDataIndex, PokerGameEventPlayerWinUniqueDataProcess, out evt);
        }

        private static void PokerGameEventPlayerWinUniqueDataProcess (byte[] bytes, ref int currentDataIndex, out PlayerWinGameEvent evt) {
            // Player ID
            ByteConverterUtils.BytesToInt32 (bytes, ref currentDataIndex, out var playerID);
            // Chips Won
            ByteConverterUtils.BytesToInt32 (bytes, ref currentDataIndex, out var chipsWon);

            evt = new () {
                playerID = playerID,
                chipsWon = chipsWon,
            };
        }

        public static byte[] BytesFromPokerGameEventPlayerWin (PlayerWinGameEvent evt) {
            int eventSize = ByteConverterUtils.SizeOf (evt);
            return BytesFromPokerGameEvent (evt, eventSize,
                uniqueDataProcess: (evt) => {
                    List<byte> uniqueByteList = new ();
                    // Player ID
                    uniqueByteList.AddRange (BitConverter.GetBytes (evt.playerID));
                    // Chips Won
                    uniqueByteList.AddRange (BitConverter.GetBytes (evt.chipsWon));

                    return uniqueByteList;
                });
        }

        #endregion

        #endregion

    }
}