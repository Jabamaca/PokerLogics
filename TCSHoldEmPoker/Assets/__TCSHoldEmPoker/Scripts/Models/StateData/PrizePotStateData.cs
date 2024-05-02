using System;
using System.Collections.Generic;
using GameUtils;

namespace TCSHoldEmPoker.Data {
    public class PrizePotStateData {

        #region Properties

        public Int32 prizeAmount;
        public List<Int32> qualifiedPlayerIDs;

        #endregion

        #region Internal Methods

        public override bool Equals (object obj) {
            var other = obj as PrizePotStateData;

            if (this.prizeAmount != other.prizeAmount) {
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