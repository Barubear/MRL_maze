using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents.Sensors;
using UnityEngine;



public class MyGridSensorBase : ISensor
{
    stageCtrl stageCtrl;
    Maze_Agent maze_Agent;
    byte[] dataRaw;

    public MyGridSensorBase(stageCtrl _stageCtrl, Maze_Agent _maze_Agent) {
        stageCtrl = _stageCtrl;
        maze_Agent = _maze_Agent;
        dataRaw = new byte[21*21*1];
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
        return "MyGridSensor";
    }

    public ObservationSpec GetObservationSpec()
    {
        return ObservationSpec.Visual(21, 21, 1);
    }

    public void Reset()
    {
        
    }

    public void Update()
    {
        
    }

    public int Write(ObservationWriter writer)
    {
        string str = "";
        int index = 0;
        for (var h = 0; h < 20; h++)
        {
            
            for (var w = 0; w < 20; w++)
            {
                int newX = w + maze_Agent.curPos.x - 10;
                int newY = h + maze_Agent.curPos.y - 10;
                if (newY < 0 || newY > stageCtrl.height - 1 || newX < 0 || newX > stageCtrl.width - 1)
                {
                    dataRaw[index] = Convert.ToByte(255);
                    writer[h, w, 0] = 255;
                    //writer[h, w, 1] = 1000;
                    //dataRaw[index + 1] = Convert.ToByte(255);
                    str += 255 + " ";

                    //writer[h, w, 2] = 1000;
                    //dataRaw[index + 2] = Convert.ToByte(255);
                }
                else {
                    if (newX == maze_Agent.curPos.x && newY == maze_Agent.curPos.y)
                    {
                        dataRaw[index] = 1;
                        writer[h, w, 0] = 1;
                        str += 1 + " ";
                    }
                    else
                    {
                        dataRaw[index] = Convert.ToByte(stageCtrl.map[newX, newY].x);
                        writer[h, w, 0] = stageCtrl.map[newX, newY].x;
                        str += stageCtrl.map[newX, newY].x + " ";
                    }

                    /*writer[h, w, 1] = stageCtrl.map[newX, newY].y;
                    dataRaw[index + 1] = Convert.ToByte(stageCtrl.map[newX, newY].y);
                    float dis = Vector2.Distance(maze_Agent.curPos, new Vector2Int(stageCtrl.width - 1, stageCtrl.height - 1));
                    writer[h, w, 2] = dis;
                    dataRaw[index + 2] = Convert.ToByte(dis);*/
                   
                }
                index ++;
                

            }
            str += "\n";
        }
        
        //Debug.Log(str);

            
        


        return index;
    }


    
}
