using UnityEngine;
using NUnit.Framework;
using System;

public class BitManipExperimentTest {

    [Test]
    public void BitManip_Experiment_1 () {
        byte[] byteTest = new byte[4] { 0x81, 0x00, 0x00, 0x10 };
        Debug.Log ("BYTES: " + BitConverter.ToString (byteTest));
        uint intTest32 = BitConverter.ToUInt32 (byteTest);
        Debug.Log ("TEST INT32: " + BitConverter.ToString (BitConverter.GetBytes(intTest32)));
        uint intTest16 = BitConverter.ToUInt16 (byteTest);
        Debug.Log ("TEST INT16: " + BitConverter.ToString (BitConverter.GetBytes (intTest16)));
    }

}