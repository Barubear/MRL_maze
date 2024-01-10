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
    

    public override void Initialize()
    {

        
        if (!isModular)
        {
            startPointIndex = -1;
            goal = new Vector2Int(stageCtrl.width - 1, stageCtrl.height - 1);
        }
        


    }

    public override void OnEpisodeBegin()
    {
        if (!isModular)
        {
            startPointIndex++;
            startPoint = startPointList[startPointIndex % startPointList.Count];
            Debug.Log(startPoint);
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
    }


    

    public override void OnActionReceived(ActionBuffers actions)
    {
        
        contorlSignal = actions.DiscreteActions[0];
        if (!isModular)
        {
            Vector2Int newPos = curPos + getActionVector(contorlSignal);

            if (newPos.y < 0 || newPos.y > stageCtrl.height - 1 || newPos.x < 0 || newPos.x > stageCtrl.width - 1) AddReward(-10);
            else
            {

                switch (stageCtrl.map[newPos.x, newPos.y].x)
                {
                    case 0:
                        curPos = newPos;
                        break;
                    case -1://wall
                        AddReward(-10);
                        break;

                    case 5://goal
                        AddReward(500);
                        curPos = newPos;
                        Debug.Log("goal");
                        EndEpisode();
                        break;


                }
                this.transform.position = getPos(curPos);
                AddReward(-0.1f);
            }

            float currDis = Vector2Int.Distance(curPos, goal);
            if (currDis < minDis)
            {
                AddReward(20f);
                minDis = currDis;
            }
        }
        else {
            //controller.ActionDic[this] = actions;
            float currDis = Vector2Int.Distance(curPos, goal);
            if (currDis < minDis)
            {
                
                minDis = currDis;
            }
        }

        



        






    }

    


}
