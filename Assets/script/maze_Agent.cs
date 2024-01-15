using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using UnityEngine;

public class Maze_Agent : Agent
{
    public stageCtrl stageCtrl;

    
    protected float minDis;
    public Vector2Int goal;
    public int foodnum;
    public int bobmnum;
    public Vector2Int curPos;
    
    
    protected Vector3 getPos(Vector2Int alex)
    {
        return new Vector3(stageCtrl.transform.position.x + 1.5f * alex.x, 0.5f, stageCtrl.transform.position.z - (1.5f * alex.y));

    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        /*var aout = actionsOut.DiscreteActions;

        if (Input.GetKeyDown(KeyCode.A)) aout[0] = 2;
        else if (Input.GetKeyDown(KeyCode.D)) aout[0] = 1;
        else if (Input.GetKeyDown(KeyCode.W)) aout[0] = 4;
        else if (Input.GetKeyDown(KeyCode.S)) aout[0] = 3;
        else aout[0] = 0;*/
    }


    public Vector2Int getActionVector(int contorlSignal) {

        Vector2Int vector2 = Vector2Int.zero;
        switch (contorlSignal)
        {
            case 0:
                vector2 = Vector2Int.zero;
                break;
            case 1:
                vector2 = new Vector2Int(1, 0);

                break;
            case 2:
                vector2 = new Vector2Int(-1, 0);
                break;
            case 3:
                vector2 = new Vector2Int(0, -1);
                break;
            case 4:
                vector2 = new Vector2Int(0, 1);
                break;

        }
        return vector2;
    }
}
