using NUnit.Framework;
using TCSHoldEmPoker.Network.Data;
using TCSHoldEmPoker.Models;
using TCSHoldEmPoker.Models.Define;
using TCSHoldEmPoker.Data;

public class GameModelByteTests {

    [Test] 
    public void PokerData_ByteToPokerCardConvert_Test () {
        byte[] cardByte = new byte[] { 0x54 }; // (byte)CardValueEnum.FIVE | (byte)CardSuitEnum.SPADES;

        GameModelByteConverter.BytesToPokerCard (cardByte, out var cardTest);
        Assert.AreEqual (cardTest.Value, CardValueEnum.FIVE);
        Assert.AreEqual (cardTest.Suit, CardSuitEnum.SPADES);
    }

    [Test]
    public void PokerData_PokerCardToByteConvert_Test () {
        PokerCard card1 = PokerCard.CARD_2_H;
        byte[] cardByte = GameModelByteConverter.BytesFromPokerCard (card1);
        int cardDataSize = GameModelByteConverter.BytesToPokerCard (cardByte, out var card2);

        Assert.AreEqual (cardByte.Length, ByteConverterUtils.SIZEOF_CARD_DATA);
        Assert.AreEqual (cardDataSize, ByteConverterUtils.SIZEOF_CARD_DATA);
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
        Assert.AreEqual (handBytes.Length, ByteConverterUtils.SIZEOF_HAND_DATA);
        Assert.AreEqual (GameModelByteConverter.BytesToPokerHand (handBytes, out var hand2), ByteConverterUtils.SIZEOF_HAND_DATA);

        Assert.AreEqual (hand1.CompareTo (hand2), 0); // Check Compare equal.
    }

    [Test]
    public void PokerData_PlayerStateDataConversion_Test () {
        PlayerStateData psd1 = new () {
            playerID = 1064,
            chipsInHand = 50000,
        };

        byte[] psdBytes = GameModelByteConverter.BytesFromPlayerStateData (psd1);
        Assert.AreEqual (psdBytes.Length, ByteConverterUtils.SIZEOF_PLAYER_STATE_DATA);
        Assert.AreEqual (GameModelByteConverter.BytesToPlayerStateData (psdBytes, out var psd2), 
            ByteConverterUtils.SIZEOF_PLAYER_STATE_DATA);

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
        Assert.AreEqual (ssdBytes.Length, ByteConverterUtils.SIZEOF_SEAT_STATE_DATA);
        Assert.AreEqual (GameModelByteConverter.BytesToSeatStateData (ssdBytes, out var ssd2), 
            ByteConverterUtils.SIZEOF_SEAT_STATE_DATA);

        Assert.IsTrue (ssd1.Equals (ssd2));
    }

    [Test]
    public void PokerData_TableStateDataConversion_Test () {
        TableStateData tsd1 = new () {
            minimumWager = 1500,
            currentTableStake = 2500,
            cashPot = 11500,
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

        byte[] tsdBytes = GameModelByteConverter.BytesFromTableStateData (tsd1);
        Assert.AreEqual (tsdBytes.Length, ByteConverterUtils.SIZEOF_TABLE_STATE_DATA);
        Assert.AreEqual (GameModelByteConverter.BytesToTableStateData (tsdBytes, out var tsd2),
            ByteConverterUtils.SIZEOF_TABLE_STATE_DATA);

        Assert.IsTrue (tsd1.Equals (tsd2));
    }
}