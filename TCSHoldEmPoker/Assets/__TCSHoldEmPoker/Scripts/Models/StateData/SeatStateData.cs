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

        #region Internal Methods

        public override bool Equals (object obj) {
            var other = obj as SeatStateData;

            if (!this.seatedPlayerStateData.Equals (other.seatedPlayerStateData) ||
                this.didCheck != other.didCheck ||
                this.isPlaying != other.isPlaying ||
                this.currentWager != other.currentWager) {

                return false;
            }

            return true;
        }

        public override int GetHashCode () {
            return base.GetHashCode ();
        }

        #endregion

    }
}