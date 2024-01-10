using System.Collections;
using System.Collections.Generic;
using Unity.Barracuda;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class PPO_Agent : Maze_Agent
{
   

    public override void Initialize()
    {

    }

    public override void OnEpisodeBegin()
    {
        //Debug.Log("OnEpisodeBegin");
        
        curPos = Vector2Int.zero;
        goal = new Vector2Int(stageCtrl.width-1 , stageCtrl.height-1);
        minDis = Vector2Int.Distance(curPos, goal);
        stageCtrl.mapReset(IcreatItem);
        foodnum = 0;
        bobmnum = 0;
        this.transform.position = getPos(curPos);
        
    }

   /* public override void CollectObservations(VectorSensor sensor)
    {
        //str = "";
        Vector2Int pos = getAlex(transform.position);
        x = pos.x;
        y = pos.y;
        //25
        //Debug.Log($"{x} {y}");
        for (int i = 0; i < 13; i++)
        {
            for (int j = 0; j < 13; j++)
            {
                int newX = x + j - 7;
                int newY = y + i - 7;
                if (newX < 0 || newX > stageCtrl.width - 1 || newY < 0 || newY > stageCtrl.height - 1)
                {
                    sensor.AddObservation(new Vector2(-1, 1000));
                    
                    //str += -1+ + " ";
                }
                else {
                    if (newX == x && newY == y)
                    {
                        sensor.AddObservation(new Vector2( 1, stageCtrl.map[newX, newY].y));
                        
                        //str += 1 + " ";
                    }
                    else {
                        sensor.AddObservation(new Vector2(stageCtrl.map[newX, newY].x, stageCtrl.map[newX, newY].y));
                        
                        //str += stageCtrl.map[newX, newY] + " ";
                    }
                   
                }
                

            }
            //sensor.AddObservation(new Vector2(foodnum / stageCtrl.foodNums , bobmnum / stageCtrl.boomNum));
            //str += "\n";
        }
        //Debug.Log(str);
        






    }*/

public override void OnActionReceived(ActionBuffers actions)
    {

        
        int contorlSignal = actions.DiscreteActions[0];
        Vector2Int newPos = curPos + getActionVector(contorlSignal);
        if (newPos.y < 0 || newPos.y > stageCtrl.height - 1 || newPos.x < 0 || newPos.x > stageCtrl.width - 1) AddReward(-10);
        else {

            switch (stageCtrl.map[newPos.x, newPos.y].x) { 
                case 0:
                    //if (stageCtrl.map[newPos.x, newPos.y].y == 0) AddReward(1);
                    /*if (stageCtrl.map[newPos.x, newPos.y].y > 3) AddReward(-1* stageCtrl.map[newPos.x, newPos.y].y);

                    if(stageCtrl.map[newPos.x, newPos.y].y < 255)
                        stageCtrl.map[newPos.x, newPos.y].y++;*/
                    curPos = newPos;

                    break;
                case -1://wall
                    AddReward(-10);
                    break;
                case 2://food
                    AddReward(200);
                    /*stageCtrl.map[newPos.x, newPos.y].x = 0;
                    if (stageCtrl.map[newPos.x, newPos.y].y < 255)
                        stageCtrl.map[newPos.x, newPos.y].y++;*/
                    curPos = newPos;
                    this.transform.position = getPos(curPos);
                    stageCtrl.map[newPos.x, newPos.y].x = 0;
                    
                    Debug.Log("eat");
                    foodnum++;
                    stageCtrl.DestoryItem(newPos);
                    break;
                case 3://bomb
                    AddReward(-150);
                    /*stageCtrl.map[newPos.x, newPos.y].x = 0;
                    if (stageCtrl.map[newPos.x, newPos.y].y < 255)
                        stageCtrl.map[newPos.x, newPos.y].y++;*/
                    curPos = newPos;
                    this.transform.position = getPos(curPos);
                    break;
                case 5://goal
                    AddReward(300);
                    curPos = newPos;
                    this.transform.position = getPos(curPos);
                    EndEpisode();
                    break;


            }
            
            AddReward(-0.01f);
        }
        
        float currDis = Vector2Int.Distance(curPos, goal);
        if (currDis < minDis)
        {
            AddReward(10f);
            minDis = currDis;
        }
        





    }

     Vector2Int getAlex(Vector3 pos)
    {
        float x = pos.x - stageCtrl.transform.position.x;
        float y = pos.z - stageCtrl.transform.position.z;
        int alexX = Mathf.FloorToInt((x) / 1.5f);
        int alexY = Mathf.FloorToInt((y) / (-1.5f));
        return new Vector2Int(alexX, alexY);
        

    }
  

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var aout = actionsOut.DiscreteActions;
       
        if (Input.GetKeyDown(KeyCode.A)) aout[0] = 2;
        else if (Input.GetKeyDown(KeyCode.D)) aout[0] = 1;
        else if (Input.GetKeyDown(KeyCode.W)) aout[0] = 4;
        else if (Input.GetKeyDown(KeyCode.S)) aout[0] = 3;
        else aout[0] = 0;
    }
    public void IcreatItem()
    {
        stageCtrl.creatGoal(new Vector2Int(34, 34));
        stageCtrl.creatItem(stageCtrl.foodNums, stageCtrl.food, 2, 0.85f, 12, stageCtrl.width - 1, 0, stageCtrl.height - 12);


    }

}
    
