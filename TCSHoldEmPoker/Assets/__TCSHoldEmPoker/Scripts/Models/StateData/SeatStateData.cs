using System;

namespace TCSHoldEmPoker.Data {
    [Serializable]
    public class SeatStateData {

        #region Properties

        public PlayerStateData seatedPlayerStateData;
        public bool didCheck;
        public bool isPlaying;
        public int currentWager;

        #endregion

    }
}