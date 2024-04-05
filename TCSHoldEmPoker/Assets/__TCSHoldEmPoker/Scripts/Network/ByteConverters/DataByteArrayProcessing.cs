using GameUtils;
using System;
using System.Collections.Generic;
using TCSHoldEmPoker.Network.Define;
using TCSHoldEmPoker.Network.GameEvents;
using TCSHoldEmPoker.Network.GameInputs;

namespace TCSHoldEmPoker.Network.Data {
    public static class DataByteArrayProcessing {

        private const byte STX_BYTE = 0x02;
        private const byte ETX_BYTE = 0x03;
        private const byte DLE_BYTE = 0x10;

        public static byte[] FrameDataByteArray (IEnumerable<byte> rawDataBytes) {
            List<byte> framedBytesList = new () {
                // START OF FRAME
                DLE_BYTE,
                STX_BYTE,
            };

            foreach (byte dataByte in rawDataBytes) {
                if (dataByte == DLE_BYTE)
                    // Add Extra DLE Byte
                    framedBytesList.Add (DLE_BYTE);

                framedBytesList.Add (dataByte);
            }

            // END OF FRAME
            framedBytesList.Add (DLE_BYTE);
            framedBytesList.Add (ETX_BYTE);

            return framedBytesList.ToArray ();
        }

        public static byte[] DeFrameDataByteArray (IEnumerable<byte> framedDataBytes) {
            List<byte> deFramedByteList = new ();

            bool dleDetected = false;
            bool frameStarted = false;

            foreach (byte dataByte in framedDataBytes) {
                if (!frameStarted) { // PRE-FRAME DETECTION
                    if (dataByte == DLE_BYTE) {
                        dleDetected = true;
                    } else if (dataByte == STX_BYTE && dleDetected) {
                        // Start of Frame Detected
                        frameStarted = true;
                        dleDetected = false;
                    } else {
                        dleDetected = false;
                    }

                } else { // INSIDE OF FRAME DATA PROCESS
                    if (dataByte == DLE_BYTE) {
                        if (dleDetected) {
                            deFramedByteList.Add (dataByte);
                            dleDetected = false;
                        } else {
                            dleDetected = true;
                        }
                    } else if (dataByte == ETX_BYTE && dleDetected) {
                        // End of Frame Detected
                        break;
                    } else {
                        deFramedByteList.Add (dataByte);
                    }
                }
            }

            return deFramedByteList.ToArray ();
        }

        public static NetworkActivityFrame ProcessDeFramedByteArrayIntoNetworkActivities (byte[] deframedDataBytes) {
            NetworkActivityFrame netFrame = new ();

            int dataLength = deframedDataBytes.Length;
            int currentDataIndex = 0;

            while (currentDataIndex < dataLength) {
                if (BitConverter.ToUInt16 (deframedDataBytes, startIndex: currentDataIndex) == ByteConverterUtils.NETWORK_ACTIVITY_START) {
                    NetworkActivityID networkActivityID = (NetworkActivityID)BitConverter.ToInt16 (deframedDataBytes, 
                        startIndex: currentDataIndex + ByteConverterUtils.SIZEOF_NETWORK_ACTIVITY_START);
                    NetworkActivityID networkActivityPrefix = (NetworkActivityID)((Int16)networkActivityID & 0xFF00);

                    switch (networkActivityPrefix) {
                        case NetworkActivityID.POKER_GAME_EVENT_PREFIX:
                            netFrame.AddGameEvent (ProcessDeFramedByteArrayIntoGameEvent (deframedDataBytes, ref currentDataIndex, networkActivityID));
                            break;
                        case NetworkActivityID.POKER_GAME_INPUT_PREFIX:
                            netFrame.AddGameInput (ProcessDeFramedByteArrayIntoGameInput (deframedDataBytes, ref currentDataIndex, networkActivityID));
                            break;
                        default:
                            // Move-On to Next Byte
                            currentDataIndex++;
                            break;
                    }
                }
            }

            return netFrame;
        }

        private static PokerGameEvent ProcessDeFramedByteArrayIntoGameEvent (byte[] deframedDataBytes, ref int currentDataIndex, NetworkActivityID networkActivityID) {
            switch (networkActivityID) {
                // CONNECTIVITY
                case NetworkActivityID.POKER_GAME_EVENT_PLAYER_JOIN: {
                    PokerGameEventByteConverter.BytesToPokerGameEventPlayerJoin (deframedDataBytes, ref currentDataIndex, out var evt);
                    return evt;
                }
                case NetworkActivityID.POKER_GAME_EVENT_PLAYER_LEAVE: {
                    PokerGameEventByteConverter.BytesToPokerGameEventPlayerLeave (deframedDataBytes, ref currentDataIndex, out var evt);
                    return evt;
                }
                // ANTE PROGRESSION
                case NetworkActivityID.POKER_GAME_EVENT_ANTE_START: {
                    PokerGameEventByteConverter.BytesToPokerGameEventAnteStart (deframedDataBytes, ref currentDataIndex, out var evt);
                    return evt;
                }
                case NetworkActivityID.POKER_GAME_EVENT_ANTE_PHASE_CHANGE: {
                    PokerGameEventByteConverter.BytesToPokerGameEventAntePhaseChange (deframedDataBytes, ref currentDataIndex, out var evt);
                    return evt;
                }
                case NetworkActivityID.POKER_GAME_EVENT_ANTE_TURN_CHANGE: {
                    PokerGameEventByteConverter.BytesToPokerGameEventAnteTurnChange (deframedDataBytes, ref currentDataIndex, out var evt);
                    return evt;
                }
                case NetworkActivityID.POKER_GAME_EVENT_ANTE_END: {
                    PokerGameEventByteConverter.BytesToPokerGameEventAnteEnd (deframedDataBytes, ref currentDataIndex, out var evt);
                    return evt;
                }
                // CARD DEALING
                case NetworkActivityID.POKER_GAME_EVENT_PLAYER_CARD_DEAL: {
                    PokerGameEventByteConverter.BytesToPokerGameEventPlayerCardDeal (deframedDataBytes, ref currentDataIndex, out var evt);
                    return evt;
                }
                case NetworkActivityID.POKER_GAME_EVENT_COMMUNITY_CARD_DEAL: {
                    PokerGameEventByteConverter.BytesToPokerGameEventCommunityCardDeal (deframedDataBytes, ref currentDataIndex, out var evt);
                    return evt;
                }
                // PLAYER ACTION
                case NetworkActivityID.POKER_GAME_EVENT_PLAYER_BET_BLIND: {
                    PokerGameEventByteConverter.BytesToPokerGameEventPlayerBetBlind (deframedDataBytes, ref currentDataIndex, out var evt);
                    return evt;
                }
                case NetworkActivityID.POKER_GAME_EVENT_PLAYER_BET_CHECK: {
                    PokerGameEventByteConverter.BytesToPokerGameEventPlayerBetCheck (deframedDataBytes, ref currentDataIndex, out var evt);
                    return evt;
                }
                case NetworkActivityID.POKER_GAME_EVENT_PLAYER_BET_CALL_BASIC: {
                    PokerGameEventByteConverter.BytesToPokerGameEventPlayerBetCallBasic (deframedDataBytes, ref currentDataIndex, out var evt);
                    return evt;
                }
                case NetworkActivityID.POKER_GAME_EVENT_PLAYER_BET_CALL_ALL_IN: {
                    PokerGameEventByteConverter.BytesToPokerGameEventPlayerBetCallAllIn (deframedDataBytes, ref currentDataIndex, out var evt);
                    return evt;
                }
                case NetworkActivityID.POKER_GAME_EVENT_PLAYER_BET_RAISE_BASIC: {
                    PokerGameEventByteConverter.BytesToPokerGameEventPlayerBetRaiseBasic (deframedDataBytes, ref currentDataIndex, out var evt);
                    return evt;
                }
                case NetworkActivityID.POKER_GAME_EVENT_PLAYER_BET_RAISE_ALL_IN: {
                    PokerGameEventByteConverter.BytesToPokerGameEventPlayerBetRaiseAllIn (deframedDataBytes, ref currentDataIndex, out var evt);
                    return evt;
                }
                case NetworkActivityID.POKER_GAME_EVENT_PLAYER_BET_FOLD: {
                    PokerGameEventByteConverter.BytesToPokerGameEventPlayerBetFold (deframedDataBytes, ref currentDataIndex, out var evt);
                    return evt;
                }
                // WIN CONDITION
                case NetworkActivityID.POKER_GAME_EVENT_TABLE_GATHER_WAGERS: {
                    PokerGameEventByteConverter.BytesToPokerGameEventTableGatherWagers (deframedDataBytes, ref currentDataIndex, out var evt);
                    return evt;
                }
                case NetworkActivityID.POKER_GAME_EVENT_ALL_PLAYER_CARDS_REVEAL: {
                    PokerGameEventByteConverter.BytesToPokerGameEventAllPlayerCardsReveal (deframedDataBytes, ref currentDataIndex, out var evt);
                    return evt;
                }
                case NetworkActivityID.POKER_GAME_EVENT_PLAYER_WIN: {
                    PokerGameEventByteConverter.BytesToPokerGameEventPlayerWin (deframedDataBytes, ref currentDataIndex, out var evt);
                    return evt;
                }
            }

            // If Network Activity ID is invalid, move-on to Next Byte
            currentDataIndex++;
            return null;
        }

        private static PokerGameInput ProcessDeFramedByteArrayIntoGameInput (byte[] deframedDataBytes, ref int currentDataIndex, NetworkActivityID networkActivityID) {
            switch (networkActivityID) {
                // CONNECTIVITY
                case NetworkActivityID.POKER_GAME_INPUT_PLAYER_REQUEST_JOIN: {
                    PokerGameInputByteConverter.BytesToPokerGameInputPlayerRequestJoin (deframedDataBytes, ref currentDataIndex, out var input);
                    return input;
                }
                case NetworkActivityID.POKER_GAME_INPUT_PLAYER_REQUEST_LEAVE: {
                    PokerGameInputByteConverter.BytesToPokerGameInputPlayerRequestLeave (deframedDataBytes, ref currentDataIndex, out var input);
                    return input;
                }
                // PLAYER ACTION
                case NetworkActivityID.POKER_GAME_INPUT_PLAYER_ACTION_BET_CHECK: {
                    PokerGameInputByteConverter.BytesToPokerGameInputPlayerActionBetCheck (deframedDataBytes, ref currentDataIndex, out var input);
                    return input;
                }
                case NetworkActivityID.POKER_GAME_INPUT_PLAYER_ACTION_BET_CALL: {
                    PokerGameInputByteConverter.BytesToPokerGameInputPlayerActionBetCall (deframedDataBytes, ref currentDataIndex, out var input);
                    return input;
                }
                case NetworkActivityID.POKER_GAME_INPUT_PLAYER_ACTION_BET_RAISE: {
                    PokerGameInputByteConverter.BytesToPokerGameInputPlayerActionBetRaise (deframedDataBytes, ref currentDataIndex, out var input);
                    return input;
                }
                case NetworkActivityID.POKER_GAME_INPUT_PLAYER_ACTION_BET_FOLD: {
                    PokerGameInputByteConverter.BytesToPokerGameInputPlayerActionBetFold (deframedDataBytes, ref currentDataIndex, out var input);
                    return input;
                }
            }

            // If Network Activity ID is invalid, move-on to Next Byte
            currentDataIndex++;
            return null;
        }
    }
}