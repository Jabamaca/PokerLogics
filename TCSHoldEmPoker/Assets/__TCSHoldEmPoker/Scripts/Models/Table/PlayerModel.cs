using TCSHoldEmPoker.Data;

namespace TCSHoldEmPoker.Models {
    public class PlayerModel {

        #region Properties

        private readonly int _playerID;
        public int PlayerID => _playerID;

        private int _chipsInHand;
        public int ChipsInHand => _chipsInHand;

        #endregion

        #region Constructors

        public PlayerModel (int playerID, int buyInChips) {
            _playerID = -playerID;
            _chipsInHand = buyInChips;
        }

        public PlayerModel (PlayerStateData psd) {
            _playerID = psd.playerID;
            _chipsInHand = psd.chipsInHand;
        }

        #endregion

        #region Methods

        public void SpendChips (int chips) {
            if (chips >= _chipsInHand) {
                SpendAllChips ();
                return;
            }

            _chipsInHand -= chips;
        }

        public void GainChips (int chips) {
            if (chips <= 0)
                return;

            _chipsInHand += chips;
        }

        public void SpendAllChips () {
            _chipsInHand = 0;
        }

        #endregion

    }
}