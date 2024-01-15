using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class AllMapSensorComponent : SensorComponent
{
    public stageCtrl stageCtrl;
    public Maze_Agent maze_Agent;
    public int stackedNum;
    
    public AllMapSensorComponent()
    {
    }

    public override ISensor[] CreateSensors()
    {

        return new ISensor[] { new StackingSensor(new AllMapSensor(stageCtrl, maze_Agent), stackedNum) };
        //return new ISensor[] {  };
    }
}
