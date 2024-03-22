using System.Collections.Generic;
using TCSHoldEmPoker.Models.Define;
using TCSHoldEmPoker.Data;

namespace TCSHoldEmPoker.Models {

    public class TableSeatModel {

        #region Properties

        private PlayerModel _seatedPlayer;
        public bool IsSeatEmpty => _seatedPlayer == null;
        public int SeatedPlayerID => IsSeatEmpty ? -1 : _seatedPlayer.PlayerID;
        public int SeatedPlayerChips => IsSeatEmpty ? 0 : _seatedPlayer.ChipsInHand;

        private bool _didCheck = false;
        public bool DidCheck => _didCheck;
        private bool _isPlaying = false;
        public bool IsPlaying => _isPlaying && !IsSeatEmpty;
        private int _currentWager;
        public int CurrentWager => _currentWager;

        private readonly List<PokerCard> _dealtCards = new ();
        public IReadOnlyList<PokerCard> DealtCards => _dealtCards;

        #endregion

        #region Constructors

        public TableSeatModel () {
            _seatedPlayer = null;
            _didCheck = false;
            _isPlaying = false;
            _currentWager = 0;
        }

        public TableSeatModel (SeatStateData ssd) {
            if (ssd.seatedPlayerStateData != null) {
                _seatedPlayer = new PlayerModel (ssd.seatedPlayerStateData);
            }
            _didCheck = ssd.didCheck;
            _isPlaying = ssd.isPlaying;
            _currentWager = ssd.currentWager;
        }

        #endregion

        #region Methods

        #region Playing Methods

        public bool SeatPlayer (PlayerModel player) {
            if (player == null ||               // No player to be sat.
                !IsSeatEmpty)          // Seat already occupied.
                return false;

            _didCheck = false;
            _isPlaying = false;
            _seatedPlayer = player;
            return true;
        }

        public void UnseatPlayer () {
            _didCheck = false;
            _isPlaying = false;
            _seatedPlayer = null;
        }

        public void SurrenderCards () {
            _dealtCards.Clear ();
        }

        public void ReceiveCard (PokerCard card) {
            _dealtCards.Add (card);
        }

        public void ReceiveCard (IEnumerable<PokerCard> cards) {
            _dealtCards.AddRange (cards);
        }

        public void SetReadyForAnte () {
            _didCheck = false;
            _isPlaying = true;
        }

        public void GiveChips (int chipsWon) {
            if (IsSeatEmpty)
                return;

            _seatedPlayer.GainChips (chipsWon);
        }

        #endregion

        #region Wagering Methods

        public void RaiseWagerTo (int newWager, out int spentChips) {
            spentChips = 0;
            if (IsSeatEmpty)
                return; // No player to wager chips.

            if (newWager < _currentWager)
                return; // Waged chips on bets only go up.

            int wagerDiff = newWager - _currentWager;
            if (newWager > _seatedPlayer.ChipsInHand) {
                _seatedPlayer.SpendAllChips (out spentChips); // Forced All-In
            } else {
                _seatedPlayer.SpendChips (wagerDiff, out spentChips);
            }
            _currentWager += spentChips;

            _didCheck = true;
        }

        public void WagerAllIn (out int spentChips) {
            spentChips = 0;
            if (IsSeatEmpty)
                return; // No player to wager chips.

            _seatedPlayer.SpendAllChips (out spentChips);
            _currentWager += spentChips;

            _didCheck = true;
        }

        public void SpendToWager (int chipsToSpend) {
            _seatedPlayer.SpendChips (chipsToSpend, out int spentChips);
            _currentWager += spentChips;

            _didCheck = true;
        }

        public void DoCheck () {
            _didCheck = true;
        }

        public void FoldHand () {
            _didCheck = false;
            _isPlaying = false;
        }

        public void Uncheck () {
            _didCheck = false;
        }

        public int CollectWageredChips () {
            if (_currentWager <= 0) {
                return 0;
            }

            int collectedChips = _currentWager;
            _didCheck = false;
            _currentWager = 0;
            return collectedChips;
        }

        #endregion

        #region State Data Methods

        public SeatStateData ConvertToStateData () {
            PlayerStateData psd = _seatedPlayer?.ConvertToStateData ();
            return new SeatStateData {
                seatedPlayerStateData = psd,
                didCheck = _didCheck,
                isPlaying = _isPlaying,
                currentWager = _currentWager,
            };
        }

        #endregion

        #endregion

    }

}