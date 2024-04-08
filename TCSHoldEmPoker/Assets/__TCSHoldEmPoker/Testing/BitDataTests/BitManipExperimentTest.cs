using UnityEngine;
using NUnit.Framework;
using System;
using GameUtils.Observing;
using System.Collections.Generic;

public class BitManipExperimentTest {
    private abstract class SuperObs : Observable {
    }

    private class SubObs1 : SuperObs {
    }

    private class SubObs2 : SuperObs {
    }

    [Test]
    public void Observable_Experiment_1 () {
        GlobalObserver.AddObserver<SuperObs> (SuperObsEvent);
        GlobalObserver.AddObserver<SubObs1> (SubObs1Event);
        GlobalObserver.AddObserver<SubObs2> (SubObs2Event);

        List<SuperObs> obsList = new () {
            new SubObs1 (),
            new SubObs2 (),
            new SubObs1 (),
            new SubObs2 (),
            new SubObs2 (),
            new SubObs1 (),
            new SubObs1 (),
            new SubObs2 (),
        };

        foreach (SuperObs obs in obsList) {
            GlobalObserver.NotifyObservers (obs);
        }
        GlobalObserver.NotifyObservers (new SubObs1 ());
        GlobalObserver.NotifyObservers (new SubObs2 ());

        GlobalObserver.RemoveObserver<SuperObs> (SuperObsEvent);
        GlobalObserver.RemoveObserver<SubObs1> (SubObs1Event);
        GlobalObserver.RemoveObserver<SubObs2> (SubObs2Event);
    }

    private void SuperObsEvent (SuperObs obs) {
        Debug.Log ("***** SUPER *****");
    }

    private void SubObs1Event (SubObs1 obs) {
        Debug.Log ("***** SUB 1 *****");
    }

    private void SubObs2Event (SubObs2 obs) {
        Debug.Log ("***** SUB 2 *****");
    }

    [Test]
    public void BitManip_Experiment_1 () {
        byte[] byteTest = new byte[4] { 0x81, 0x00, 0x00, 0x10 };
        Debug.Log ("BYTES: " + BitConverter.ToString (byteTest));
        uint intTest32 = BitConverter.ToUInt32 (byteTest);
        Debug.Log ("TEST INT32: " + BitConverter.ToString (BitConverter.GetBytes(intTest32)));
        uint intTest16 = BitConverter.ToUInt16 (byteTest);
        Debug.Log ("TEST INT16: " + BitConverter.ToString (BitConverter.GetBytes (intTest16)));
    }

    [Test]
    public void BitManip_Experiment_2 () {
        byte[] outBytes = new byte[3] { 0xCC, 0xFF, 0xFF };
        Debug.Log ("BEFORE BYTES: " + BitConverter.ToString (outBytes));
        ModifyInside (outBytes);
        Debug.Log ("AFTER BYTES: " + BitConverter.ToString (outBytes));
    }

    private void ModifyInside (byte[] inBytes) {
        inBytes[0] = 0x00;
    }

}