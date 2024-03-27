using System;

namespace TCSHoldEmPoker.Data {
    [Serializable]
    public class PlayerStateData {

        #region Properties

        public Int32 playerID;
        public Int32 chipsInHand;

        #endregion

        #region Internal Methods

        public override bool Equals (object obj) {
            var other = obj as PlayerStateData;

            if (this.playerID != other.playerID ||
                this.chipsInHand != other.chipsInHand) {

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