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
            _playerID = playerID;
            _chipsInHand = buyInChips;
        }

        public PlayerModel (PlayerStateData playerStateData) {
            _playerID = playerStateData.playerID;
            _chipsInHand = playerStateData.chipsInHand;
        }

        #endregion

        #region Methods

        public void SpendChips (int chips, out int spentChips) {
            if (chips >= _chipsInHand) {
                SpendAllChips (out spentChips);
                return;
            }

            spentChips = chips;
            _chipsInHand -= chips;
        }

        public void GainChips (int chips) {
            if (chips <= 0)
                return;

            _chipsInHand += chips;
        }

        public void SpendAllChips (out int spentChips) {
            spentChips = _chipsInHand;
            _chipsInHand = 0;
        }

        public PlayerStateData ConvertToStateData () {
            return new PlayerStateData {
                playerID = _playerID,
                chipsInHand = _chipsInHand,
            };
        }

        #endregion

    }
}