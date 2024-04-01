using System;
using System.Collections.Generic;
using TCSHoldEmPoker.Data;
using TCSHoldEmPoker.Network.Define;
using TCSHoldEmPoker.Network.Events;

namespace TCSHoldEmPoker.Network.Data {
    public static class PokerGameEventByteConverter {

        #region Methods

        private static bool BytesToPokerGameEvent<EVT> (byte[] bytes, int eventSize, out EVT evt, Func<byte[], int, EVT> uniqueDataProcess, int startIndex = 0) 
            where EVT : PokerGameEvent {

            if (bytes.Length - startIndex < eventSize) {
                evt = null;
                return false;
            }

            // COMMON DATA
            int i = startIndex;
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

        #endregion

    }
}