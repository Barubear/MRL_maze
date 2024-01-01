using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using UnityEngine;

public class ModelAgent : Agent
{
    
    public ModularController controller;
    public modelType modelType;
    public int modelNum;
    
   

}

public enum modelType {

    inde,
    model

}
