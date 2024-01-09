using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents.Actuators;
using UnityEngine;

public class ModularAgent : Maze_Agent
{
    public bool isModular = false;
    public int contorlSignal;
    public ModularController controller;


    public void sendAction(ActionBuffers actions) {

        controller.OnActionReceived(actions);
        
    }
}
