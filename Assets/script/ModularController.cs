using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using UnityEngine;

public class ModularController : Maze_Agent
{
    public List<ModularAgent> modulars;
    public Dictionary<ModularAgent, ActionBuffers> ActionDic;


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
        
        stageCtrl.mapReset();
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        int contorlindx = 0;
        if (actions.DiscreteActions[0] > actions.DiscreteActions[1]) contorlindx = 1;
        else if (actions.DiscreteActions[0] < actions.DiscreteActions[1]) contorlindx = 0;
        else contorlindx = Random.Range(0,2);

        int contorlSignal = modulars[contorlindx].contorlSignal;
        Vector2Int newPos = curPos + getActionVector(contorlSignal);
        if (newPos.y < 0 || newPos.y > stageCtrl.height - 1 || newPos.x < 0 || newPos.x > stageCtrl.width - 1) AddReward(-10);
        else
        {

            switch (stageCtrl.map[newPos.x, newPos.y].x)
            {
                case 0:
                    curPos = newPos;
                    this.transform.position = getPos(curPos);
                    break;
                case 255://wall
                    AddReward(-10);
                    break;
                case 2://food
                    AddReward(150);
                    curPos = newPos;
                    this.transform.position = getPos(curPos);
                    curPos = newPos;
                    stageCtrl.map[newPos.x, newPos.y].x = 0;
                    Debug.Log("eat");
                    stageCtrl.DestoryItem(newPos);
                    
                    break;
                case 5:
                    AddReward(500);
                    EndEpisode();

                    break;

            }
            AddReward(-0.1f);
        }
    }

    public void getActionFormModular(ActionBuffers actions) {

        int contorlSignal =  actions.DiscreteActions[0];
        
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
                case 255://wall
                    AddReward(-1);
                    break;
                case 2://food
                    this.transform.position = getPos(curPos);



                    break;
                case 5:

                    EndEpisode();

                    break;

            }

        }

    }



}
