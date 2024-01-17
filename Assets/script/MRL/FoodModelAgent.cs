using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Unity.VisualScripting;
using UnityEngine;

public class FoodModelAgent : ModularAgent
{
    
    public Vector2Int startPoint;
    
    public List<Vector2Int> PointList;

    Vector2Int foodPoint;
    

    
    public override void Initialize()
    {
        AgentType = "FoodModelAgent";
    }
    public override void OnEpisodeBegin()
    {

        this.foodnum = 0;
        if (!isModular)
        {
            this.transform.position = getPos(startPoint);
            stageCtrl.mapReset(IcreatItem);
            curPos = startPoint;
            /* int foodIndex = Random.Range(0, PointList.Count);
         foodPoint = PointList[foodIndex];
         /*while (true) {
             int startIndex = Random.Range(0, PointList.Count);

             if (startIndex != foodIndex) {
                 startPoint = PointList[startIndex];
                 foodPoint = PointList[foodIndex];
                 break;
             }
         } */
        }
        else curPos = controller.curPos;




    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(foodnum);
        sensor.AddObservation(foodnum/ stageCtrl.foodNums);
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

                    /* if (stageCtrl.map[newPos.x, newPos.y].y > 3)
                         AddReward(-0.5f * stageCtrl.map[newPos.x, newPos.y].y);

                     if (stageCtrl.map[newPos.x, newPos.y].y < 255)
                         stageCtrl.map[newPos.x, newPos.y].y++;*/

                    if (!isModular)
                    {
                        curPos = newPos;
                        this.transform.position = getPos(curPos);
                    }
                    else curPos = controller.curPos;
                        break;
                    case -1://wall
                        AddReward(-1);
                        break;
                    case 2://food
                        if (!isModular) {
                        curPos = newPos;
                        this.transform.position = getPos(curPos);
                        stageCtrl.map[newPos.x, newPos.y].x = 0;
                        stageCtrl.DestoryItem(newPos);
                    }
                        
                        
                        
                        AddReward(100 + 50 * foodnum);
                    //Debug.Log("eat");
                        this.foodnum++;
                        

                        //stageCtrl.map[newPos.x, newPos.y].y++;


                        break;
                    case 5:
                    if (this.foodnum < 12) AddReward(-1000);
                    else AddReward(90 * foodnum);
                    if (!isModular)
                    {

                        EndEpisode();
                        Debug.Log("goal");
                    }
                        break;

                }

            }
            if (foodnum == 15)
            {
                AddReward(100 * foodnum);
                //Debug.Log("completed");
                if (!isModular) EndEpisode();
            }
           
        
        




    }

    


    public void IcreatItem() {
        stageCtrl.creatGoal(new Vector2Int(34, 34));
        stageCtrl.creatItem(stageCtrl.foodNums, stageCtrl.food, 2, 0.85f, 12, stageCtrl.width - 1, 0, stageCtrl.height-12);


    }
}
