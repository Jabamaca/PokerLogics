using TCSHoldEmPoker.Models.Define;
using TCSHoldEmPoker.Data;
using System.Collections.Generic;

namespace TCSHoldEmPoker.Models {

    public abstract class GameTableModel {

        protected const int TABLE_CAPACITY = HoldEmPokerDefines.POKER_TABLE_CAPACITY;
        protected const int CARD_DEAL_COUNT = HoldEmPokerDefines.POKER_PLAYER_DEAL_COUNT;
        protected const int COMMUNITY_CARD_COUNT = HoldEmPokerDefines.POKER_COMMUNITY_CARD_COUNT;

        #region Properties

        protected int _gameTableID;
        public int GameTableID => _gameTableID;

        // Wagering Properties
        protected int _minimumWager;
        public int MinimumWager => _minimumWager;
        protected int _currentTableStake = 0;
        protected PrizePotModel _mainPrizePot;
        protected readonly List<PrizePotModel> _sidePrizePots = new List<PrizePotModel> ();

        // Turning Properties
        protected readonly TableSeatModel[] _playerSeats = new TableSeatModel[TABLE_CAPACITY];
        protected TableSeatModel CurrentTurningSeat => _playerSeats[_currentTurnSeatIndex];

        // Game Phase Properties
        protected PokerGamePhaseEnum _currentGamePhase = PokerGamePhaseEnum.WAITING;
        protected int _currentTurnSeatIndex = 0;
        protected readonly PokerCard[] _communityCards = new PokerCard[COMMUNITY_CARD_COUNT];

        #endregion

        #region Methods

        #region Displayable Info Methods

        public PokerGamePhaseEnum CurrentGamePhase => _currentGamePhase;
        public PokerCard[] CommunityCards => _communityCards.Clone () as PokerCard[];

        public List<int> GetPrizePots () {
            List<int> returnList = new ();

            returnList.Add (_mainPrizePot.prizeAmount);
            foreach (var sidePrizePot in _sidePrizePots) {
                returnList.Add (sidePrizePot.prizeAmount);
            }

            return returnList;
        }

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

        public int GetTurningPlayerID () {
            return GetPlayerIDAtIndex (_currentTurnSeatIndex);
        }

        public IReadOnlyList<int> GetPrizePotAmounts () {
            List<int> returnList = new ();

            foreach (var sidePrizePot in _sidePrizePots)
                returnList.Add (sidePrizePot.prizeAmount);

            returnList.Add (_mainPrizePot.prizeAmount);

            return returnList;
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

        protected IReadOnlyList<int> GetPlayerIDsWithChips () {
            List<int> returnList = new ();

            foreach (var seat in _playerSeats) {
                if (seat.SeatedPlayerChips > 0 && seat.IsPlaying) {
                    returnList.Add (seat.SeatedPlayerID);
                }
            }

            return returnList;
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

        protected bool GetSortedAllInSeats (out IReadOnlyList<TableSeatModel> sortedAllInSeats) {
            List<TableSeatModel> allInSeats = new ();

            foreach (var seat in _playerSeats) {
                if (seat.IsAllIn) {
                    allInSeats.Add (seat);
                }
            }
            allInSeats.Sort ((s1, s2) => {
                return s1.CurrentWager.CompareTo (s2.CurrentWager);
            });

            sortedAllInSeats = allInSeats;
            return sortedAllInSeats.Count > 0;
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

            List<PrizePotStateData> sidePrizePotStateOrder = new ();
            foreach (var sidePrizePot in _sidePrizePots) {
                if (sidePrizePot != null) {
                    sidePrizePotStateOrder.Add (sidePrizePot.ConvertToStateData ());
                }
            }

            return new TableStateData {
                minimumWager = _minimumWager,
                currentTableStake = _currentTableStake,
                mainPrizeStateData = _mainPrizePot.ConvertToStateData (),
                sidePrizeStateDataList = sidePrizePotStateOrder,

                currentTurnPlayerIndex = (short)_currentTurnSeatIndex,
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