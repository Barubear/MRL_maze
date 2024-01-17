using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class TestTool 
{
    Maze_Agent agent;
    string log = ".\\Assets\\LogData\\";
    public TestTool(Maze_Agent _agent) 
    { 
        this.agent = _agent;
        log += agent.AgentType + "_" + System.DateTime.Now.GetHashCode()+".txt";
        
    }
    public TestTool(Maze_Agent _agent,string _fileName)
    {
        this.agent = _agent;
        log += _fileName;
    }

    public void doLog(string text) {

        File.AppendAllText(log, text);
    }



}
