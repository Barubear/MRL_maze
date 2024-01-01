using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

public class item : MonoBehaviour
{
    int id;
    int x;
    int y;
    stageCtrl stageCtrl;
    //Maze_Agent agent;
    public void set(int _id, int _x, int _y,stageCtrl _stageCtrl)
    {
        id = _id;
        x = _x;
        y = _y;
        stageCtrl = _stageCtrl;
        
        
    }
    

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("agent"))
        {
            Maze_Agent agent = other.gameObject.GetComponent<Maze_Agent>();
            switch (id)
            {
                case 2://food
                    //agent.AddReward(10);
                    agent.foodnum++;
                    break;
                case 3://bomb
                    //agent.AddReward(-20);
                    agent.bobmnum++;
                    break;
            }
            stageCtrl.map[x, y] = Vector2Int.zero;
            Destroy(gameObject);
        }
    }

   

}
