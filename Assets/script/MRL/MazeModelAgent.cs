using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class MazeModelAgent : ModularAgent
{
    
    
    public List<Vector2Int> startPointList;
    int startPointIndex;
    public Vector2Int startPoint;
    int minX;
    int minY;


    public override void Initialize()
    {

        
        if (!isModular)
        {
            startPointIndex = -1;
            goal = new Vector2Int(stageCtrl.width - 1, stageCtrl.height - 1);
        }
        


    }

    public override void CollectObservations(VectorSensor sensor)
    {
      // sensor.AddObservation(curPos);
    }


    public override void OnEpisodeBegin()
    {
        if (!isModular)
        {
            startPointIndex++;
            startPoint = startPointList[startPointIndex % startPointList.Count];
            //Debug.Log(startPoint);
            this.transform.position = getPos(startPoint);
            curPos = startPoint;
            minDis = Vector2Int.Distance(curPos, goal);
            stageCtrl.mapReset();
           
        }
        else
        {
            curPos = controller.curPos;
            goal = controller.goal;
            minDis = Vector2Int.Distance(curPos, goal);
        }

        minX = curPos.x;
        minY = curPos.y;
    }


    

    public override void OnActionReceived(ActionBuffers actions)
    {
        
        contorlSignal = actions.DiscreteActions[0];
        
        
            Vector2Int newPos = curPos + getActionVector(contorlSignal);

            if (newPos.y < 0 || newPos.y > stageCtrl.height - 1 || newPos.x < 0 || newPos.x > stageCtrl.width - 1) AddReward(-1);
            else
            {

                switch (stageCtrl.map[newPos.x, newPos.y].x)
                {
                    case 0:
                    if (!isModular) curPos = newPos;
                    else curPos = controller.curPos;
                    break;

                    case 2:
                     if (!isModular) curPos = newPos;
                        else curPos = controller.curPos;
                        break;

                case -1://wall
                        AddReward(-1);
                        break;

                    case 5://goal

                        AddReward(500);
                        if (!isModular) {
                            curPos = newPos;
                            Debug.Log("goal");
                            EndEpisode();
                           }
                        else curPos = controller.curPos;
                        
                       
                        break;


                }
                if (!isModular) this.transform.position = getPos(curPos);
                AddReward(-0.1f);
            

          /*  float currDis = Vector2Int.Distance(curPos, goal);
            if (currDis < minDis)
            {
                AddReward(5f);
                minDis = currDis;
            }*/
            
            if(curPos.x > minX)
            {
                AddReward(5f);
                minX = curPos.x;
            }
            if (curPos.y > minY)
            {
                AddReward(5f);
                minY = curPos.y;
            }

        }

        



        






    }

    


}
