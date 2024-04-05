using System;
using System.Collections.Generic;
using TCSHoldEmPoker.Network.Define;
using TCSHoldEmPoker.Network.GameInputs;

namespace TCSHoldEmPoker.Network.Data {
    public static class PokerGameInputByteConverter {

        #region Defines

        private delegate void UniqueDataToInputProcess<INPUT> (byte[] bytes, ref int currentDataIndex, out INPUT input) where INPUT : PokerGameInput;
        private delegate IReadOnlyList<byte> UniqueDataFromInputProcess<INPUT> (INPUT input) where INPUT : PokerGameInput;

        #endregion

        #region Methods

        private static void BytesToPokerGameInput<INPUT> (byte[] bytes, ref int currentDataIndex, UniqueDataToInputProcess<INPUT> uniqueDataProcess, out INPUT input)
            where INPUT : PokerGameInput {

            // COMMON DATA
            currentDataIndex += ByteConverterUtils.SIZEOF_NETWORK_ACTIVITY_START;      // START of Network Activity Signature
            currentDataIndex += ByteConverterUtils.SIZEOF_NETWORK_ACTIVITY_ID;         // Network Activity ID
            // Game Table ID
            ByteConverterUtils.BytesToInt32 (bytes, ref currentDataIndex, out var gameTableID);
            // Player ID
            ByteConverterUtils.BytesToInt32 (bytes, ref currentDataIndex, out var playerID);

            uniqueDataProcess (bytes, ref currentDataIndex, out input);
            input.gameTableID = gameTableID;
            input.playerID = playerID;
            currentDataIndex += ByteConverterUtils.SIZEOF_NETWORK_ACTIVITY_END;        // END of Network Activity Signature
        }

        private static byte[] BytesFromPokerGameInput<INPUT> (INPUT input, int eventSize, UniqueDataFromInputProcess<INPUT> uniqueDataProcess) 
            where INPUT : PokerGameInput {

            if (input == null)
                return null;

            byte[] returnBytes = new byte[eventSize];

            int currentDataIndex = 0;
            // Network Activity START Signature
            ByteConverterUtils.AddBytesToArray (bytesToAdd: BitConverter.GetBytes (ByteConverterUtils.NETWORK_ACTIVITY_START),
                bytesLocation: returnBytes, ref currentDataIndex);
            // Network Activity ID
            Int16 netWorkActivityID = (Int16)((Int16)NetworkActivityID.POKER_GAME_INPUT_PREFIX | (Int16)input.GameInputType);
            ByteConverterUtils.AddBytesToArray (bytesToAdd: BitConverter.GetBytes (netWorkActivityID),
                bytesLocation: returnBytes, ref currentDataIndex);
            // Game Table ID
            ByteConverterUtils.AddBytesToArray (bytesToAdd: BitConverter.GetBytes (input.gameTableID),
                bytesLocation: returnBytes, ref currentDataIndex);
            // Player ID
            ByteConverterUtils.AddBytesToArray (bytesToAdd: BitConverter.GetBytes (input.playerID),
                bytesLocation: returnBytes, ref currentDataIndex);
            // Unique Data
            ByteConverterUtils.AddBytesToArray (bytesToAdd: uniqueDataProcess (input),
                bytesLocation: returnBytes, ref currentDataIndex);
            // Network Activity END Signature
            ByteConverterUtils.AddBytesToArray (bytesToAdd: BitConverter.GetBytes (ByteConverterUtils.NETWORK_ACTIVITY_END),
                bytesLocation: returnBytes, ref currentDataIndex);

            return returnBytes;
        }

        #region Connectivity Game Input Conversion

        // PLAYER JOIN REQUEST

        public static void BytesToPokerGameInputPlayerRequestJoin (byte[] bytes, ref int currentDataIndex, out PlayerJoinRequestGameInput input) {
            BytesToPokerGameInput (bytes, ref currentDataIndex, PokerGameInputPlayerRequestJoinUniqueDataProcess, out input);
        }

        private static void PokerGameInputPlayerRequestJoinUniqueDataProcess (byte[] bytes, ref int currentDataIndex, out PlayerJoinRequestGameInput input) {
            // Buy-In Chips
            ByteConverterUtils.BytesToInt32 (bytes, ref currentDataIndex, out var buyInChips);

            input = new () {
                buyInChips = buyInChips,
            };
        }

        public static byte[] BytesFromPokerGameInputPlayerRequestJoin (PlayerJoinRequestGameInput input) {
            int eventSize = ByteConverterUtils.SIZEOF_POKER_GAME_INPUT_PLAYER_REQUEST_JOIN;
            return BytesFromPokerGameInput (input, eventSize,
                uniqueDataProcess: (input) => {
                    List<byte> uniqueByteList = new ();
                    // Buy-In Chips
                    uniqueByteList.AddRange (BitConverter.GetBytes (input.buyInChips));

                    return uniqueByteList;
                });
        }

        // PLAYER LEAVE REQUEST

        public static void BytesToPokerGameInputPlayerRequestLeave (byte[] bytes, ref int currentDataIndex, out PlayerLeaveRequestGameInput input) {
            BytesToPokerGameInput (bytes, ref currentDataIndex, PokerGameInputPlayerRequestLeaveUniqueDataProcess, out input);
        }

        private static void PokerGameInputPlayerRequestLeaveUniqueDataProcess (byte[] bytes, ref int currentDataIndex, out PlayerLeaveRequestGameInput input) {
            // NO UNIQUE DATA

            input = new ();
        }

        public static byte[] BytesFromPokerGameInputPlayerRequestLeave (PlayerLeaveRequestGameInput input) {
            int eventSize = ByteConverterUtils.SIZEOF_POKER_GAME_INPUT_PLAYER_REQUEST_LEAVE;
            return BytesFromPokerGameInput (input, eventSize,
                uniqueDataProcess: (input) => {
                    List<byte> uniqueByteList = new ();
                    // NO UNIQUE DATA

                    return uniqueByteList;
                });
        }

        #endregion

        #region Player Action Game Input Conversion

        // PLAYER ACTION BET CHECK

        public static void BytesToPokerGameInputPlayerActionBetCheck (byte[] bytes, ref int currentDataIndex, out PlayerBetCheckActionGameInput input) {
            BytesToPokerGameInput (bytes, ref currentDataIndex, PokerGameInputPlayerActionBetCheckUniqueDataProcess, out input);
        }

        private static void PokerGameInputPlayerActionBetCheckUniqueDataProcess (byte[] bytes, ref int currentDataIndex, out PlayerBetCheckActionGameInput input) {
            // NO UNIQUE DATA

            input = new ();
        }

        public static byte[] BytesFromPokerGameInputPlayerActionBetCheck (PlayerBetCheckActionGameInput input) {
            int eventSize = ByteConverterUtils.SIZEOF_POKER_GAME_INPUT_PLAYER_ACTION_BET_CHECK;
            return BytesFromPokerGameInput (input, eventSize,
                uniqueDataProcess: (input) => {
                    List<byte> uniqueByteList = new ();
                    // NO UNIQUE DATA

                    return uniqueByteList;
                });
        }

        // PLAYER ACTION BET CALL

        public static void BytesToPokerGameInputPlayerActionBetCall (byte[] bytes, ref int currentDataIndex, out PlayerBetCallActionGameInput input) {
            BytesToPokerGameInput (bytes, ref currentDataIndex, PokerGameInputPlayerActionBetCallUniqueDataProcess, out input);
        }

        private static void PokerGameInputPlayerActionBetCallUniqueDataProcess (byte[] bytes, ref int currentDataIndex, out PlayerBetCallActionGameInput input) {
            // NO UNIQUE DATA

            input = new ();
        }

        public static byte[] BytesFromPokerGameInputPlayerActionBetCall (PlayerBetCallActionGameInput input) {
            int eventSize = ByteConverterUtils.SIZEOF_POKER_GAME_INPUT_PLAYER_ACTION_BET_CALL;
            return BytesFromPokerGameInput (input, eventSize,
                uniqueDataProcess: (input) => {
                    List<byte> uniqueByteList = new ();
                    // NO UNIQUE DATA

                    return uniqueByteList;
                });
        }

        // PLAYER ACTION BET RAISE

        public static void BytesToPokerGameInputPlayerActionBetRaise (byte[] bytes, ref int currentDataIndex, out PlayerBetRaiseActionGameInput input) {
            BytesToPokerGameInput (bytes, ref currentDataIndex, PokerGameInputPlayerActionBetRaiseUniqueDataProcess, out input);
        }

        private static void PokerGameInputPlayerActionBetRaiseUniqueDataProcess (byte[] bytes, ref int currentDataIndex, out PlayerBetRaiseActionGameInput input) {
            // New Stake
            ByteConverterUtils.BytesToInt32 (bytes, ref currentDataIndex, out var newStake);

            input = new () {
                newStake = newStake,
            };
        }

        public static byte[] BytesFromPokerGameInputPlayerActionBetRaise (PlayerBetRaiseActionGameInput input) {
            int eventSize = ByteConverterUtils.SIZEOF_POKER_GAME_INPUT_PLAYER_ACTION_BET_RAISE;
            return BytesFromPokerGameInput (input, eventSize,
                uniqueDataProcess: (input) => {
                    List<byte> uniqueByteList = new ();
                    // Buy-In Chips
                    uniqueByteList.AddRange (BitConverter.GetBytes (input.newStake));

                    return uniqueByteList;
                });
        }

        // PLAYER ACTION BET FOLD

        public static void BytesToPokerGameInputPlayerActionBetFold (byte[] bytes, ref int currentDataIndex, out PlayerBetFoldActionGameInput input) {
            BytesToPokerGameInput (bytes, ref currentDataIndex, PokerGameInputPlayerActionBetFoldUniqueDataProcess, out input);
        }

        private static void PokerGameInputPlayerActionBetFoldUniqueDataProcess (byte[] bytes, ref int currentDataIndex, out PlayerBetFoldActionGameInput input) {
            // NO UNIQUE DATA

            input = new ();
        }

        public static byte[] BytesFromPokerGameInputPlayerActionBetFold (PlayerBetFoldActionGameInput input) {
            int eventSize = ByteConverterUtils.SIZEOF_POKER_GAME_INPUT_PLAYER_ACTION_BET_FOLD;
            return BytesFromPokerGameInput (input, eventSize,
                uniqueDataProcess: (input) => {
                    List<byte> uniqueByteList = new ();
                    // NO UNIQUE DATA

                    return uniqueByteList;
                });
        }

        #endregion

        #endregion

    }
}