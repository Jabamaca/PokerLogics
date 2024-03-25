using TCSHoldEmPoker.Models.Define;
using TCSHoldEmPoker.Data;
using System.Collections.Generic;
using System;

namespace TCSHoldEmPoker.Models {

    public abstract class GameTableModel {

        protected const int TABLE_CAPACITY = HoldEmPokerDefines.POKER_TABLE_CAPACITY;
        protected const int CARD_DEAL_COUNT = HoldEmPokerDefines.HOLDEM_POKER_DEAL_COUNT;
        protected const int COMMUNITY_CARD_COUNT = HoldEmPokerDefines.COMMUNITY_CARD_COUNT;

        #region Properties

        protected int _gameTableID;
        public int GameTableID => _gameTableID;

        // Wagering Properties
        protected int _minimumWager;
        public int MinimumWager => _minimumWager;
        protected int _currentTableStake = 0;
        protected int _cashPot = 0;

        // Turning Properties
        protected int _currentTurnSeatIndex = 0;
        protected readonly TableSeatModel[] _playerSeats = new TableSeatModel[TABLE_CAPACITY];
        protected TableSeatModel CurrentTurningSeat => _playerSeats[_currentTurnSeatIndex];

        // Game Phase Properties
        protected PokerGamePhase _currentGamePhase = PokerGamePhase.WAITING;
        protected readonly PokerCard[] _communityCards = new PokerCard[COMMUNITY_CARD_COUNT];

        #endregion

        #region Methods

        #region Displayable Info Methods

        public int CashPot => _cashPot;
        public PokerCard[] CommunityCards => _communityCards.Clone () as PokerCard[];

        public bool TryGetPlayerIsPlaying (int playerID, out bool isPlaying) {
            if (FindSeatWithPlayerID (playerID, out var seat)) {
                isPlaying = seat.IsPlaying;
                return true;
            }

            isPlaying = false;
            return false;
        }

        public bool TryGetPlayerIsChecked (int playerID, out bool isChecked) {
            if (FindSeatWithPlayerID (playerID, out var seat)) {
                isChecked = seat.DidCheck;
                return true;
            }

            isChecked = false;
            return false;
        }

        public bool TryGetPlayerCurrentWager (int playerID, out int currentWager) {
            if (FindSeatWithPlayerID (playerID, out var seat)) {
                currentWager = seat.CurrentWager;
                return true;
            }

            currentWager = 0;
            return false;
        }

        public bool TryGetPlayerChipsInHand (int playerID, out int chipsInHand) {
            if (FindSeatWithPlayerID (playerID, out var seat)) {
                chipsInHand = seat.SeatedPlayerChips;
                return true;
            }

            chipsInHand = 0;
            return false;
        }

        public bool TryGetPlayerCards (int playerID, out IReadOnlyList<PokerCard> cards) {
            if (FindSeatWithPlayerID (playerID, out var seat)) {
                cards = seat.DealtCards;
                return true;
            }

            cards = new List<PokerCard> (); // Empty List.
            return false;
        }

        public int GetPlayerIDAtIndex (int seatIndex) {
            if (seatIndex < 0 || seatIndex >= TABLE_CAPACITY) {
                return -1;
            }

            return _playerSeats[seatIndex].SeatedPlayerID;
        }

        #endregion

        #region Table-Seat Utility Methods

        protected bool FindSeatWithPlayerID (int playerID, out TableSeatModel seat) {
            for (int i = 0; i < TABLE_CAPACITY; i++) {
                if (_playerSeats[i].SeatedPlayerID == playerID) {
                    seat = _playerSeats[i];
                    return true;
                }
            }

            seat = null;
            return false;
        }

        protected void RemoveCommunityCards () {
            for (int i = 0; i < COMMUNITY_CARD_COUNT; i++) {
                _communityCards[i] = PokerCard.BLANK;
            }
        }

        protected void ReadyPlayersForAnte () {
            for (int i = 0; i < TABLE_CAPACITY; i++) {
                if (_playerSeats[i] == null)
                    continue;

                _playerSeats[i].SurrenderCards ();
                _playerSeats[i].SetReadyForAnte ();
            }
        }

        #endregion

        #region Game State Methods

        public TableStateData ConvertToStateData () {
            List<SeatStateData> seatStateOrder = new ();
            foreach (var seat in _playerSeats) {
                if (seat != null) {
                    seatStateOrder.Add (seat.ConvertToStateData ());
                }
            }

            List<PokerCard> cardOrder = new ();
            cardOrder.AddRange (_communityCards);

            return new TableStateData {
                minimumWager = _minimumWager,
                currentTableStake = _currentTableStake,
                cashPot = _cashPot,

                currentTurnPlayerIndex = _currentTurnSeatIndex,
                seatStateDataOrder = seatStateOrder,

                currentGamePhase = _currentGamePhase,
                communityCardsOrder = cardOrder
            };
        }

        #endregion

        #region Wagering Methods

        protected void RemoveAllChecks () {
            for (int i = 0; i < TABLE_CAPACITY; i++) {
                _playerSeats[i].Uncheck ();
            }
        }

        #endregion

        #endregion

    }

}