using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class AllMapSensor : ISensor
{
    public int map;
    stageCtrl stageCtrl;
    Maze_Agent maze_Agent;
    byte[] dataRaw;
    int ViewWidth;
    int ViewHeight;

    public AllMapSensor(stageCtrl _stageCtrl, Maze_Agent _maze_Agent) {

        stageCtrl = _stageCtrl;
        maze_Agent = _maze_Agent;
        ViewWidth = stageCtrl.width;
        ViewHeight = stageCtrl.height;
        dataRaw = new byte[(ViewWidth + 2) * (ViewHeight + 2)];

    }
    

    public byte[] GetCompressedObservation()
    {
        return dataRaw;
    }

    public CompressionSpec GetCompressionSpec()
    {
        return CompressionSpec.Default();
    }

    public string GetName()
    {
        return "AllMapSensor";
    }

    public ObservationSpec GetObservationSpec()
    {
        return ObservationSpec.Visual(ViewHeight + 2, ViewWidth + 2, 1);
    }

    public void Reset()
    {
        dataRaw = new byte[(ViewWidth + 2) *( ViewHeight + 2)];
    }

    public int Write(ObservationWriter writer)
    {
        int index = 0;
        for (var h =-1; h < ViewHeight+1; h++)
        {

            for (var w = -1; w < ViewWidth+1; w++)
            {
                if (w < 0 || w >stageCtrl.width-1 || h < 0 || h >stageCtrl.height-1)
                {
                    writer[h+1, w+1, 0] = -1;
                    dataRaw[index] = Convert.ToByte(255);
                }
                else if (w == maze_Agent.curPos.x && h == maze_Agent.curPos.y)
                {
                    writer[h+1, w+1, 0] = 1;
                    dataRaw[index] = Convert.ToByte(1);
                }
                else {
                    int obs = stageCtrl.map[w, h].x;

                    writer[h+1, w+1, 0] = obs;
                    if(obs == -1) dataRaw[index] = Convert.ToByte(255);
                    else dataRaw[index] = Convert.ToByte(obs);
                }
                
                
                index++;
            }
        }
        return index;
    }

    
    void ISensor.Update()
    {
        int index = 0;
        for (var h = -1; h < ViewHeight + 1; h++)
        {

            for (var w = -1; w < ViewWidth + 1; w++)
            {
                if (w < 0 || w >= ViewWidth || h < 0 || h >= ViewHeight)
                {
                    
                    dataRaw[index] = Convert.ToByte(255);
                }
                else if (w == maze_Agent.curPos.x && h == maze_Agent.curPos.y)
                {
                    
                    dataRaw[index] = Convert.ToByte(1);
                }
                else
                {
                    int obs = stageCtrl.map[w, h].x;

                   
                    if (obs == -1) dataRaw[index] = Convert.ToByte(255);
                    else dataRaw[index] = Convert.ToByte(obs);
                }


                index++;
            }
        }
       

    }
}
