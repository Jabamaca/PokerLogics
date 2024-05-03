using System;
using TCSHoldEmPoker.Data;
using TCSHoldEmPoker.Network.Activity;
using TCSHoldEmPoker.Network.Define;

namespace TCSHoldEmPoker.Network.Data {
    public static class NetworkActivityByteConverter {

        #region Methods

        public static void BytesToPokerGameStateUpdate (byte[] bytes, ref int currentDataIndex, out PokerGameStateUpdate stateUpdate) {
            currentDataIndex += ByteConverterUtils.SIZEOF_NETWORK_ACTIVITY_START;       // START of Network Activity Signature
            currentDataIndex += ByteConverterUtils.SizeOf (NetworkActivityID.SAMPLE);   // Network Activity ID
            // Game Table ID
            ByteConverterUtils.BytesToInt32 (bytes, ref currentDataIndex, out var gameTableID);
            // Updated Table State Data
            GameModelByteConverter.BytesToTableStateData (bytes, ref currentDataIndex, out var updatedTableStateData);

            stateUpdate = new () {
                gameTableID = gameTableID,
                updatedTableStateData = updatedTableStateData,
            };
            currentDataIndex += ByteConverterUtils.SIZEOF_NETWORK_ACTIVITY_END;         // END of Network Activity Signature
        }

        public static byte[] BytesFromPokerGameStateUpdate (PokerGameStateUpdate stateUpdate) {
            int dataSize = ByteConverterUtils.SIZEOF_POKER_GAME_STATE_UPDATE_BASE;
            dataSize += stateUpdate.updatedTableStateData.mainPrizeStateData.qualifiedPlayerIDs.Count * ByteConverterUtils.SIZEOF_PRIZE_POT_STATE_DATA_PLAYER;
            foreach (var sidePrizeData in stateUpdate.updatedTableStateData.sidePrizeStateDataList) {
                dataSize += ByteConverterUtils.SIZEOF_PRIZE_POT_STATE_DATA_BASE;
                dataSize += ByteConverterUtils.SIZEOF_PRIZE_POT_STATE_DATA_PLAYER * sidePrizeData.qualifiedPlayerIDs.Count;
            }

            byte[] returnBytes = new byte[dataSize];

            int currentDataIndex = 0;
            // Network Activity START Signature
            ByteConverterUtils.AddBytesToArray (bytesToAdd: BitConverter.GetBytes (ByteConverterUtils.NETWORK_ACTIVITY_START),
                bytesLocation: returnBytes, ref currentDataIndex);
            // Network Activity ID
            Int16 netWorkActivityID = (Int16)NetworkActivityID.POKER_GAME_STATE_UPDATE;
            ByteConverterUtils.AddBytesToArray (bytesToAdd: BitConverter.GetBytes (netWorkActivityID),
                bytesLocation: returnBytes, ref currentDataIndex);
            // Game Table ID
            ByteConverterUtils.AddBytesToArray (bytesToAdd: BitConverter.GetBytes (stateUpdate.gameTableID),
                bytesLocation: returnBytes, ref currentDataIndex);
            // Updated Table State Data
            ByteConverterUtils.AddBytesToArray (bytesToAdd: GameModelByteConverter.BytesFromTableStateData (stateUpdate.updatedTableStateData),
                bytesLocation: returnBytes, ref currentDataIndex);
            // Network Activity END Signature
            ByteConverterUtils.AddBytesToArray (bytesToAdd: BitConverter.GetBytes (ByteConverterUtils.NETWORK_ACTIVITY_END),
                bytesLocation: returnBytes, ref currentDataIndex);

            return returnBytes;
        }

        #endregion

    }
}