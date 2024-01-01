using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class mazeSensor : ISensor
{
    public byte[] GetCompressedObservation()
    {
        throw new System.NotImplementedException();
    }

    public CompressionSpec GetCompressionSpec()
    {
        throw new System.NotImplementedException();
    }

    public string GetName()
    {
        throw new System.NotImplementedException();
    }

    public ObservationSpec GetObservationSpec()
    {
        throw new System.NotImplementedException();
    }

    public void Reset()
    {
        throw new System.NotImplementedException();
    }

    public int Write(ObservationWriter writer)
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
 

    void ISensor.Update()
    {
        throw new System.NotImplementedException();
    }
}
