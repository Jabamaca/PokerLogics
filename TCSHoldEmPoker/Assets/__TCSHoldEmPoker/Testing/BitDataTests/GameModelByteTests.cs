using NUnit.Framework;
using System.Collections.Generic;
using TCSHoldEmPoker.Network.Data;
using TCSHoldEmPoker.Models;
using TCSHoldEmPoker.Models.Define;
using TCSHoldEmPoker.Data;
using System;

public class GameModelByteTests {

    [Test] 
    public void PokerData_ByteToPokerCardConvert_Test () {
        byte[] cardByte = new byte[] { 0x54 }; // (byte)CardValueEnum.FIVE | (byte)CardSuitEnum.SPADES;

        int currentDataIndex = 0;
        GameModelByteConverter.BytesToPokerCard (cardByte, ref currentDataIndex, out var cardTest);
        Assert.AreEqual (currentDataIndex, ByteConverterUtils.SizeOf (cardTest));
        Assert.AreEqual (cardTest.Value, CardValueEnum.FIVE);
        Assert.AreEqual (cardTest.Suit, CardSuitEnum.SPADES);
    }

    [Test]
    public void PokerData_PokerCardToByteConvert_Test () {
        PokerCard card1 = PokerCard.CARD_2_H;
        byte[] cardByte = GameModelByteConverter.BytesFromPokerCard (card1);
        int currentDataIndex = 0;
        GameModelByteConverter.BytesToPokerCard (cardByte, ref currentDataIndex, out var card2);

        Assert.AreEqual (cardByte.Length, ByteConverterUtils.SizeOf (card1));
        Assert.AreEqual (currentDataIndex, ByteConverterUtils.SizeOf (card2));
        Assert.IsTrue (card1.Equals (card2));
    }

    [Test]
    public void PokerData_PokerHandConversion_Test () {
        PokerCard[] cards = new PokerCard[] {
            PokerCard.CARD_2_D,
            PokerCard.CARD_2_C,
            PokerCard.CARD_2_H,
            PokerCard.CARD_5_H,
            PokerCard.CARD_5_C,
        };

        PokerHand hand1 = PokerHandFactory.GetHighestPokerHandWithCardSet (cards);
        byte[] handBytes = GameModelByteConverter.BytesFromPokerHand (hand1);
        Assert.AreEqual (handBytes.Length, ByteConverterUtils.SizeOf (hand1));
        int currentDataIndex = 0;
        GameModelByteConverter.BytesToPokerHand (handBytes, ref currentDataIndex, out var hand2);
        Assert.AreEqual (currentDataIndex, ByteConverterUtils.SizeOf (hand1));

        Assert.AreEqual (hand1.CompareTo (hand2), 0); // Check Compare equal.
    }

    [Test]
    public void PokerData_PlayerStateDataConversion_Test () {
        PlayerStateData psd1 = new () {
            playerID = 1064,
            chipsInHand = 50000,
        };

        byte[] psdBytes = GameModelByteConverter.BytesFromPlayerStateData (psd1);
        Assert.AreEqual (psdBytes.Length, ByteConverterUtils.SizeOf (psd1));
        int currentDataIndex = 0;
        GameModelByteConverter.BytesToPlayerStateData (psdBytes, ref currentDataIndex, out var psd2);
        Assert.AreEqual (currentDataIndex, ByteConverterUtils.SizeOf (psd2));

        Assert.IsTrue (psd1.Equals (psd2));
    }

    [Test]
    public void PokerData_SeatStateDataConversion_Test () {
        SeatStateData ssd1 = new () {
            seatedPlayerStateData = new () {
                playerID = 2345,
                chipsInHand = 103000,
            },
            didCheck = false,
            isPlaying = true,
            currentWager = 55000,
        };

        byte[] ssdBytes = GameModelByteConverter.BytesFromSeatStateData (ssd1);
        Assert.AreEqual (ssdBytes.Length, ByteConverterUtils.SizeOf (ssd1));
        int currentDataIndex = 0;
        GameModelByteConverter.BytesToSeatStateData (ssdBytes, ref currentDataIndex, out var ssd2);
        Assert.AreEqual (currentDataIndex, ByteConverterUtils.SizeOf (ssd1));

        Assert.IsTrue (ssd1.Equals (ssd2));
    }

    [Test]
    public void PokerData_PrizePotStateDataConversion_Test () {
        PrizePotStateData ppsd1 = new PrizePotStateData () {
            prizeAmount = 11000,
            qualifiedPlayerIDs = new List<Int32> {
                44223,
                11222,
                12222
            }
        };

        int expectedSize = ByteConverterUtils.SizeOf (ppsd1);

        byte[] ppsdBytes = GameModelByteConverter.BytesFromPrizePotStateData (ppsd1);
        Assert.AreEqual (ppsdBytes.Length, expectedSize);
        int currentDataIndex = 0;
        GameModelByteConverter.BytesToPrizePotStateData (ppsdBytes, ref currentDataIndex, out var ppsd2);
        Assert.AreEqual (currentDataIndex, expectedSize);

        Assert.IsTrue (ppsd1.Equals (ppsd2));
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

        int expectedSize = ByteConverterUtils.SIZEOF_TABLE_STATE_DATA_BASE;
        expectedSize += tsd1.mainPrizeStateData.qualifiedPlayerIDs.Count * ByteConverterUtils.SIZEOF_PRIZE_POT_STATE_DATA_PLAYER;
        foreach (var sidePrizeData in tsd1.sidePrizeStateDataList) {
            expectedSize += ByteConverterUtils.SIZEOF_PRIZE_POT_STATE_DATA_BASE;
            expectedSize += sidePrizeData.qualifiedPlayerIDs.Count * ByteConverterUtils.SIZEOF_PRIZE_POT_STATE_DATA_PLAYER;
        }

        byte[] tsdBytes = GameModelByteConverter.BytesFromTableStateData (tsd1);
        Assert.AreEqual (tsdBytes.Length, expectedSize);
        int currentDataIndex = 0;
        GameModelByteConverter.BytesToTableStateData (tsdBytes, ref currentDataIndex, out var tsd2);
        Assert.AreEqual (currentDataIndex, expectedSize);

        Assert.IsTrue (tsd1.Equals (tsd2));
    }
}