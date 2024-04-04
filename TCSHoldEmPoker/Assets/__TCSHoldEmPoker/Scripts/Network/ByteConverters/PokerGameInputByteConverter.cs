using System;
using System.Collections.Generic;
using TCSHoldEmPoker.Network.Define;
using TCSHoldEmPoker.Network.Inputs;

namespace TCSHoldEmPoker.Network.Data {
    public static class PokerGameInputByteConverted {

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

        public static void BytesToPokerGameInputPlayerJoinRequest (byte[] bytes, ref int currentDataIndex, out PlayerJoinRequestGameInput input) {
            BytesToPokerGameInput (bytes, ref currentDataIndex, PokerGameInputPlayerJoinRequestUniqueDataProcess, out input);
        }

        private static void PokerGameInputPlayerJoinRequestUniqueDataProcess (byte[] bytes, ref int currentDataIndex, out PlayerJoinRequestGameInput input) {
            // Buy-In Chips
            ByteConverterUtils.BytesToInt32 (bytes, ref currentDataIndex, out var buyInChips);

            input = new () {
                buyInChips = buyInChips,
            };
        }

        public static byte[] BytesFromPokerGameInputPlayerJoinRequest (PlayerJoinRequestGameInput input) {
            int eventSize = ByteConverterUtils.SIZEOF_POKER_GAME_INPUT_PLAYER_JOIN_REQUEST;
            return BytesFromPokerGameInput (input, eventSize,
                uniqueDataProcess: (evt) => {
                    List<byte> uniqueByteList = new ();
                    // Buy-In Chips
                    uniqueByteList.AddRange (BitConverter.GetBytes (evt.buyInChips));

                    return uniqueByteList;
                });
        }

        // PLAYER LEAVE REQUEST

        public static void BytesToPokerGameInputPlayerLeaveRequest (byte[] bytes, ref int currentDataIndex, out PlayerLeaveRequestGameInput input) {
            BytesToPokerGameInput (bytes, ref currentDataIndex, PokerGameInputPlayerLeaveRequestUniqueDataProcess, out input);
        }

        private static void PokerGameInputPlayerLeaveRequestUniqueDataProcess (byte[] bytes, ref int currentDataIndex, out PlayerLeaveRequestGameInput input) {
            // NO UNIQUE DATA

            input = new ();
        }

        public static byte[] BytesFromPokerGameInputPlayerLeaveRequest (PlayerLeaveRequestGameInput input) {
            int eventSize = ByteConverterUtils.SIZEOF_POKER_GAME_INPUT_PLAYER_LEAVE_REQUEST;
            return BytesFromPokerGameInput (input, eventSize,
                uniqueDataProcess: (evt) => {
                    List<byte> uniqueByteList = new ();
                    // NO UNIQUE DATA

                    return uniqueByteList;
                });
        }

        #endregion

        #region Player Action Game Input Conversion



        #endregion

        #endregion

    }
}