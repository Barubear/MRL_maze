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

        //int contorlSignal = actions.DiscreteActions[0];
        int contorlindx = 0;
        int contorlSignal = ActionDic[modulars[contorlindx]].DiscreteActions[0];
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
