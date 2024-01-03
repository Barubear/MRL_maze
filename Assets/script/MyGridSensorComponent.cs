using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class MyGridSensorComponent : SensorComponent
{
    public stageCtrl stageCtrl;
    public Maze_Agent maze_Agent;
    public int stackedNum;
    public int ViewWidth;
    public int ViewHeight;
    public int channelNum;
    public MyGridSensorComponent()
    {
    }

    public override ISensor[] CreateSensors()
    {

        return new ISensor[] { new StackingSensor(new MyGridSensorBase(stageCtrl, maze_Agent,ViewWidth, ViewHeight, channelNum), stackedNum) };
        //return new ISensor[] {  };
    }
}
