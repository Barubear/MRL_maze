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
    int ViewWidth;
    int ViewHeight;
    int channelNum;
    public MyGridSensorBase(stageCtrl _stageCtrl, Maze_Agent _maze_Agent, int _ViewWidth,int _ViewHeight,int _channelNum) {
        stageCtrl = _stageCtrl;
        maze_Agent = _maze_Agent;
        
        ViewWidth = _ViewWidth;
        if (ViewWidth % 2 != 1)
            ViewWidth++;
        ViewHeight = _ViewHeight;
        if (ViewHeight % 2 != 1)
            ViewHeight++;
        channelNum = _channelNum;
        dataRaw = new byte[ViewWidth * ViewHeight * channelNum];
    }

    public byte[] GetCompressedObservation()
    {
        /*string str = "";
        for (var h = 0; h < ViewWidth *ViewHeight; h++)
        {

            str += dataRaw[h];
            if (h % ViewWidth == 0) str += "\n";
        }
        Debug.Log(str);*/
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
        return ObservationSpec.Visual(ViewHeight, ViewWidth, channelNum);
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
        for (var h = 0; h < ViewHeight; h++)
        {
            
            for (var w = 0; w < ViewWidth; w++)
            {

                //Debug.Log(w+","+h);
                int newX = w + maze_Agent.curPos.x - (ViewWidth-1)/2;
                int newY = h + maze_Agent.curPos.y - (ViewHeight - 1) / 2;
                if (newY < 0 || newY > stageCtrl.height - 1 || newX < 0 || newX > stageCtrl.width - 1)
                {
                    
                    writer[h, w, 0] = -1;
                    //writer[h, w, 1] = 1000;
                    dataRaw[index] = Convert.ToByte(255);
                    //dataRaw[index + 1] = Convert.ToByte(255);
                    str += "-" + " ";

                    
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
                        if(stageCtrl.map[newX, newY].x == -1) dataRaw[index] = Convert.ToByte(255);
                        else dataRaw[index] = Convert.ToByte(stageCtrl.map[newX, newY].x);
                        writer[h, w, 0] = stageCtrl.map[newX, newY].x;
                        str += stageCtrl.map[newX, newY].x + " ";
                    }

                    
                   /* if(stageCtrl.map[newX, newY].y>255) dataRaw[index+1] = Convert.ToByte(255);
                    else dataRaw[index+1] = Convert.ToByte(stageCtrl.map[newX, newY].y);
                    writer[h, w, 1] = stageCtrl.map[newX, newY].y;*/

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
