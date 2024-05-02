using System;
using System.Collections.Generic;
using TCSHoldEmPoker.Data;

namespace TCSHoldEmPoker.Models {
    public class PrizePotModel {

        #region Properties

        private int _prizeAmount;
        public int prizeAmount => _prizeAmount;
        private readonly List<int> _qualifiedPlayerIDs = new ();
        public IReadOnlyList<int> qualifiedPlayerIDs => _qualifiedPlayerIDs;

        #endregion

        #region Constructors

        // Constructor for client. No qualified players data needed.
        public PrizePotModel (int prizeAmount) {
            _prizeAmount = prizeAmount;
        }

        // Constructor for client.
        public PrizePotModel (IEnumerable<int> qualifiedPlayerIDs) {
            _prizeAmount = 0;
            _qualifiedPlayerIDs.AddRange (qualifiedPlayerIDs);
        }

        public PrizePotModel (PrizePotStateData prizePotStateData) {
            _prizeAmount = prizePotStateData.prizeAmount;
            _qualifiedPlayerIDs.AddRange (prizePotStateData.qualifiedPlayerIDs);
        }

        #endregion

        #region Methods

        public void DisqualifyPlayerIDFromPot (int playerID) {
            if (!_qualifiedPlayerIDs.Contains (playerID))
                return;

            _qualifiedPlayerIDs.Remove (playerID);
        }

        public void AddPrizeToPot (int prizeAdd) {
            _prizeAmount += prizeAdd;
        }

        public void SetPrizeAmount (int prizeAmount) {
            _prizeAmount = prizeAmount;
        }

        #region State Data Methods

        public PrizePotStateData ConvertToStateData () {
            List<Int32> qualifiedPlayerList = new ();
            qualifiedPlayerList.AddRange (_qualifiedPlayerIDs);

            return new PrizePotStateData {
                prizeAmount = _prizeAmount,
                qualifiedPlayerIDs = qualifiedPlayerList,
            };
        }

        #endregion

        #endregion

    }
}