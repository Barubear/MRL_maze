using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class ModularController : Maze_Agent
{
    public List<ModularAgent> modulars;
    public Dictionary<ModularAgent, ActionBuffers> ActionDic;
    int modeularIndex;

    public override void Initialize()
    {
        ActionDic = new Dictionary<ModularAgent, ActionBuffers>();
        foreach (var modular in modulars)
        {
            ActionDic[modular] = new ActionBuffers();
        }
    }
    public override void OnEpisodeBegin()
    {
        Vector2Int startPoint= new Vector2Int(0,0);
        this.transform.position = getPos(startPoint);
        curPos = startPoint;
        foreach (ModularAgent model in modulars) { 
        
            model.gameObject.transform.position= getPos(startPoint);
        }
        stageCtrl.mapReset(IcreatItem);
    }


    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(curPos);
        sensor.AddObservation(modeularIndex);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        
        int contorlSignal = 0;
        if (actions.DiscreteActions[0] > actions.DiscreteActions[1])
        {

            contorlSignal = modulars[0].contorlSignal;

        }
        else if (actions.DiscreteActions[0] < actions.DiscreteActions[1])
        {
            modeularIndex = 1;
            contorlSignal = modulars[1].contorlSignal;
        }
        else {

            modeularIndex = -1;
            AddReward(-0.1f);
        }
        



         
        Vector2Int newPos = curPos + getActionVector(contorlSignal);
        if (newPos.y < 0 || newPos.y > stageCtrl.height - 1 || newPos.x < 0 || newPos.x > stageCtrl.width - 1) AddReward(-1);
        else
        {

            switch (stageCtrl.map[newPos.x, newPos.y].x)
            {
                case 0:
                    curPos = newPos;
                    this.transform.position = getPos(curPos);
                    break;
                case -1://wall
                    AddReward(-1);
                    break;
                case 2://food
                    AddReward(500);
                    curPos = newPos;
                    this.transform.position = getPos(curPos);
                    curPos = newPos;
                    stageCtrl.map[newPos.x, newPos.y].x = 0;
                    Debug.Log("eat");
                    stageCtrl.DestoryItem(newPos);
                    
                    break;
                case 5:
                    AddReward(1000);
                    EndEpisode();

                    break;

            }
            AddReward(-0.1f);
        }
    }

    public void IcreatItem()
    {
        stageCtrl.creatGoal(new Vector2Int(34, 34));
        stageCtrl.creatItem(stageCtrl.foodNums, stageCtrl.food, 2, 0.85f, 12, stageCtrl.width - 1, 0, stageCtrl.height - 12);


    }



}
