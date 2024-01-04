using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Unity.VisualScripting;
using UnityEngine;

public class FoodModelAgent : Maze_Agent
{
    
    Vector2Int startPoint;
    
    public List<Vector2Int> PointList;

    Vector2Int foodPoint;

    
    public override void Initialize()
    {
        
    }
    public override void OnEpisodeBegin()
    {
        foodnum = 0;
        startPoint = new Vector2Int(11,8);
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
        this.transform.position = getPos(startPoint);
        stageCtrl.mapReset();
        curPos = startPoint;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(foodnum);
        sensor.AddObservation(foodnum/ PointList.Count);
    }


    public override void OnActionReceived(ActionBuffers actions)
    {



        int contorlSignal = actions.DiscreteActions[0];

        
        Vector2Int newPos = curPos + getActionVector(contorlSignal);
        if (newPos.y < 0 || newPos.y > stageCtrl.height - 1 || newPos.x < 0 || newPos.x > stageCtrl.width - 1) AddReward(-1);
        else
        {

            switch (stageCtrl.map[newPos.x, newPos.y].x)
            {
                case 0:
                    curPos = newPos;
                    /*if (stageCtrl.map[newPos.x, newPos.y].y > 3) 
                        AddReward(-0.5f* stageCtrl.map[newPos.x, newPos.y].y);
                    
                    if(stageCtrl.map[newPos.x, newPos.y].y < 255)
                        stageCtrl.map[newPos.x, newPos.y].y++;*/

                    this.transform.position = getPos(curPos);
                    break;
                case 255://wall
                    AddReward(-1);
                    break;
                case 2://food
                    this.transform.position = getPos(curPos);
                    stageCtrl.map[newPos.x, newPos.y].x = 0;
                    AddReward(100+ 50*foodnum);
                    Debug.Log("eat");
                    foodnum++;
                    stageCtrl.DestoryItem(newPos);
                    
                    //stageCtrl.map[newPos.x, newPos.y].y++;
                    
                    
                    break;
            }
     
        }
        if (foodnum == stageCtrl.foodNums)
        {
            AddReward(100 * foodnum);
            Debug.Log("completed");
            EndEpisode();
        }
        AddReward(-0.1f);



    }

    Vector2Int getNewAlex(int minX ,int maxX, int minY , int maxY)
    {
        int newX = 0;
        int newY = 0;
        while (true) { 
            newX = Random.Range(minX, maxX);
            newY = Random.Range(minY, maxY);
            if (stageCtrl.map[newX, newY].x == 0) break;
        }
        return new Vector2Int(newX, newY);
    }


    public void IcreatItem() {

        stageCtrl.foodNums = PointList.Count;
        stageCtrl.creatItemWithPosition(stageCtrl.food , 2, PointList);


    }
}
