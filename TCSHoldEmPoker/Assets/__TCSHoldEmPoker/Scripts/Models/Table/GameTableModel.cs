using TCSHoldEmPoker.Defines;
using TCSHoldEmPoker.Models.Define;
using TCSHoldEmPoker.Data;
using System.Collections.Generic;

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

        #region Constructors

        #endregion

        #region Methods

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