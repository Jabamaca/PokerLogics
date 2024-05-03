using NUnit.Framework;
using System;
using System.Collections.Generic;
using TCSHoldEmPoker.Data;
using TCSHoldEmPoker.Models.Define;
using TCSHoldEmPoker.Network.Activity;
using TCSHoldEmPoker.Network.Data;
using TCSHoldEmPoker.Network.Define;

public class NetworkActivityByteTest {
    private void TestNetworkActivityByteArray (byte[] activityBytes, int expectedSize, NetworkActivityID expectedNetActID) {
        int i = 0;
        UInt16 startSig = BitConverter.ToUInt16 (activityBytes, startIndex: i);
        Assert.AreEqual (startSig, ByteConverterUtils.NETWORK_ACTIVITY_START);
        i += ByteConverterUtils.SIZEOF_NETWORK_ACTIVITY_START;
        NetworkActivityID testID = (NetworkActivityID)BitConverter.ToInt16 (activityBytes, startIndex: i);
        Assert.IsTrue (testID == expectedNetActID);
        UInt16 endSig = BitConverter.ToUInt16 (activityBytes, startIndex: activityBytes.Length - ByteConverterUtils.SIZEOF_NETWORK_ACTIVITY_END);
        Assert.AreEqual (endSig, ByteConverterUtils.NETWORK_ACTIVITY_END);
        Assert.AreEqual (activityBytes.Length, expectedSize);
    }

    [Test]
    public void PokerData_TableStateDataConversion_Test () {
        TableStateData tsd1 = new () {
            minimumWager = 1500,
            currentTableStake = 2500,
            mainPrizeStateData = new PrizePotStateData {
                prizeAmount = 6000,
                qualifiedPlayerIDs = new List<Int32> {
                    2345, 9292, 1818, 8008
                },
            },
            sidePrizeStateDataList = new List<PrizePotStateData> {
                new PrizePotStateData {
                    prizeAmount = 7500,
                qualifiedPlayerIDs = new List<Int32> {
                    2345, 9292, 1818, 8008, 6654
                },
                }
            },
            seatStateDataOrder = new () {
                new () {
                    seatedPlayerStateData = new () {
                        playerID = -1,
                        chipsInHand = 0,
                    },
                    didCheck = false,
                    isPlaying = false,
                    currentWager = 0,
                },
                new () {
                    seatedPlayerStateData = new () {
                        playerID = 2345,
                        chipsInHand = 103000,
                    },
                    didCheck = false,
                    isPlaying = true,
                    currentWager = 1500,
                },
                new () {
                    seatedPlayerStateData = new () {
                        playerID = -1,
                        chipsInHand = 0,
                    },
                    didCheck = false,
                    isPlaying = false,
                    currentWager = 0,
                },
                new () {
                    seatedPlayerStateData = new () {
                        playerID = -1,
                        chipsInHand = 0,
                    },
                    didCheck = false,
                    isPlaying = false,
                    currentWager = 0,
                },
                new () {
                    seatedPlayerStateData = new () {
                        playerID = 6654,
                        chipsInHand = 4500,
                    },
                    didCheck = false,
                    isPlaying = false,
                    currentWager = 0,
                },
                new () {
                    seatedPlayerStateData = new () {
                        playerID = 9292,
                        chipsInHand = 89000,
                    },
                    didCheck = false,
                    isPlaying = true,
                    currentWager = 1500,
                },
                new () {
                    seatedPlayerStateData = new () {
                        playerID = 1818,
                        chipsInHand = 43500,
                    },
                    didCheck = false,
                    isPlaying = true,
                    currentWager = 2000,
                },
                new () {
                    seatedPlayerStateData = new () {
                        playerID = 8008,
                        chipsInHand = 103000,
                    },
                    didCheck = true,
                    isPlaying = true,
                    currentWager = 2500,
                },
            },
            currentGamePhase = PokerGamePhaseEnum.THE_FLOP,
            currentTurnPlayerIndex = 1,
            communityCardsOrder = new () {
                PokerCard.CARD_J_C,
                PokerCard.CARD_9_D,
                PokerCard.CARD_A_C,
                PokerCard.BLANK,
                PokerCard.BLANK,
            }
        };

        PokerGameStateUpdate stateUpdate1 = new () {
            gameTableID = 7410112,
            updatedTableStateData = tsd1,
        };
        int expectedSize = ByteConverterUtils.SizeOf (stateUpdate1);
        byte[] updateBytes = NetworkActivityByteConverter.BytesFromPokerGameStateUpdate (stateUpdate1);
        TestNetworkActivityByteArray (updateBytes, expectedSize,
            expectedNetActID: NetworkActivityID.POKER_GAME_STATE_UPDATE);

        int currentDataIndex = 0;
        NetworkActivityByteConverter.BytesToPokerGameStateUpdate (updateBytes, ref currentDataIndex, out var stateUpdate2);
        Assert.AreEqual (currentDataIndex, expectedSize);
        Assert.AreEqual (stateUpdate1.gameTableID, stateUpdate2.gameTableID);
        Assert.IsTrue (stateUpdate1.updatedTableStateData.Equals (stateUpdate2.updatedTableStateData));
    }
}