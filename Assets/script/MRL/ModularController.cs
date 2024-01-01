using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

public class ModularController : Agent
{

    public List<ModelAgent> modelList;
    List<Vector2> actionList;
    // Start is called before the first frame update
    public override void Initialize()
    {

       actionList = new List<Vector2>();
       for (int i = 0; i < modelList.Count; i++)
       {
            modelList[i].modelNum = i;
            actionList.Add(Vector2.zero);
       }



    }
}
