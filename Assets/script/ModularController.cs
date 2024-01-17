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
    public int modeularIndex;
    int minX;
    int minY;
    public int plusNum;
    public override void Initialize()
    {
        AgentType = "ModularController";
        testTool = new TestTool(this);
        ActionDic = new Dictionary<ModularAgent, ActionBuffers>();
        foreach (var modular in modulars)
        {
            ActionDic[modular] = new ActionBuffers();
        }
    }
    public override void OnEpisodeBegin()
    {
        base.OnEpisodeBegin();
        Vector2Int startPoint= new Vector2Int(0,0);
        this.transform.position = getPos(startPoint);
        curPos = startPoint;
        foreach (ModularAgent model in modulars) { 
        
            model.gameObject.transform.position= getPos(startPoint);
        }
        stageCtrl.mapReset(IcreatItem);
        minX = curPos.x;
        minY = curPos.y;
        foodnum = 0;
        minDis = Vector2Int.Distance(curPos, goal);
    }


    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(curPos);
        sensor.AddObservation(modeularIndex);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        stepNum++;
        List<int> contorlSignaSeed = new List<int>();
        for (int i = 0; i < modulars.Count; i++)
        {
            for (int j = 0; j < actions.DiscreteActions[i]; j++)
            {
                contorlSignaSeed.Add(i);
            }
        }


        for (int i = 0; i < plusNum; i++)
        {

            contorlSignaSeed.Add(1);
        }



        if (contorlSignaSeed.Count == 0) {
            for (int i = 0; i < modulars.Count; i++)
            {

                contorlSignaSeed.Add(i);

            }

        }





        modeularIndex = contorlSignaSeed[Random.Range(0, contorlSignaSeed.Count - 1)];
        
       
        int  contorlSignal = modulars[modeularIndex].contorlSignal;
        



         
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
                    AddReward(300);
                    foodnum++;
                    curPos = newPos;
                    this.transform.position = getPos(curPos);
                    curPos = newPos;
                    stageCtrl.map[newPos.x, newPos.y].x = 0;
                    //Debug.Log("eat");
                    stageCtrl.DestoryItem(newPos);
                    
                    break;
                case 5:
                    AddReward(300);
                    //Debug.Log("goal");
                    //EndEpisode();
                    EndEpisodeForTest();
                    break;

            }
            float currDis = Vector2Int.Distance(curPos, goal);
            if (currDis < minDis)
            {
                AddReward(5f);
                minDis = currDis;
            }
            /*AddReward(-0.01f);
            if (curPos.x > minX)
            {
                AddReward(1f);
                minX = curPos.x;
            }
            if (curPos.y > minY)
            {
                AddReward(1f);
                minY = curPos.y;
            }*/
        }
    }

    public void IcreatItem()
    {
        stageCtrl.creatGoal(new Vector2Int(34, 34));
        stageCtrl.creatItem(stageCtrl.foodNums, stageCtrl.food, 2, 0.85f, 12, stageCtrl.width - 1, 0, stageCtrl.height - 12);


    }



}
