using Grpc.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UIElements;
using static stageCtrl;

public class stageCtrl : MonoBehaviour
{
    //public Maze_Agent agent;


    public GameObject cell;
    public GameObject wall;
    public GameObject food;
    public GameObject boom;
    public GameObject goal;


    /*
     
     width or height = 6*n -1
     n = Í¨Â·¤ÎÊý
     
     */
    public int width;
    public int height;



    public int foodNums;
    public int boomNum;
    public float foodSeed;
    public float boomSeed;


    public Vector2Int[,] map;


    public bool haveGoal;
    public int mapDataIndex;
    List<GameObject> itemList;
    List<GameObject> wallList;
    string[] mapDataStr;
    string mapData;

    bool isFirst;

    public delegate void generateItemFuc();
    void Awake()
    {
        itemList = new List<GameObject>();
        wallList= new List<GameObject>();
        map = new Vector2Int[width,height];
        mapDataStr = File.ReadAllLines(".\\Assets\\script\\mapData.txt");
        mapData = mapDataStr[mapDataIndex];
    }
   
   
    public void generateMap() {

        if (wallList.Count == 0) isFirst = true;
        else isFirst = false;
        creatWall(mapData);
        creatItem(foodNums, food, 2);
        creatItem(boomNum, boom ,3);
        if(haveGoal)
            creatGoal();

    }
    public void creatWall(string data) {

        int index = 0;
        int n = (((width + 1) / 6) - 1) * (((height + 1) / 6) - 1) ;
        string[] mapData = data.Split(',');
        if (mapData.Length > n)
        {

            throw new System.Exception("mapData is not match to stage size: mapdata Lenth is " + mapData.Length + "  ,stage size is " + n);
        
        }
        if (isFirst) {

            for (int i = -1; i < width + 1; i++)
            {

                Vector3 newPos = new Vector3(this.transform.position.x + 1.5f * i, 0.5f, this.transform.position.z - (1.5f * -1));
                GameObject newCell = Instantiate(wall, newPos, Quaternion.identity);
                newCell.transform.SetParent(this.transform);
                newPos = new Vector3(this.transform.position.x + 1.5f * i, 0.5f, this.transform.position.z - (1.5f * height));
                newCell = Instantiate(wall, newPos, Quaternion.identity);
                newCell.transform.SetParent(this.transform);
            }
            for (int i = -1; i < height + 1; i++)
            {
                Vector3 newPos = new Vector3(this.transform.position.x + 1.5f * -1, 0.5f, this.transform.position.z - (1.5f * i));
                GameObject newCell = Instantiate(wall, newPos, Quaternion.identity);
                newCell.transform.SetParent(this.transform);
                newPos = new Vector3(this.transform.position.x + 1.5f * (width), 0.5f, this.transform.position.z - (1.5f * i));
                newCell = Instantiate(wall, newPos, Quaternion.identity);
                newCell.transform.SetParent(this.transform);
            }

        }
        


        for (int i = 5; i < width; i += 6)
        {
            
            for (int j = 5; j < height; j += 6)
            {
                Vector3 newPos;
                GameObject newCell;
                if (isFirst) {
                    newPos = new Vector3(this.transform.position.x + 1.5f * i, 0.5f, this.transform.position.z - (1.5f * j));
                    newCell = Instantiate(wall, newPos, Quaternion.identity);
                    newCell.transform.SetParent(this.transform);
                }
                
                map[i, j] = new Vector2Int(255, 255);
                int x = i;
                int y = j;
                    
                    string dic = mapData[index];
                    if (dic == "r" && map[x + 1, y].x != 255)//right
                    {
                        for (int k = 1; k < 6; k++)
                        {
                            if (isFirst) {
                                newPos = new Vector3(this.transform.position.x + 1.5f * (x + k), 0.5f, this.transform.position.z - (1.5f * y));
                                newCell = Instantiate(wall, newPos, Quaternion.identity);
                                newCell.transform.SetParent(this.transform);
                                wallList.Add(newCell);
                            }
                            map[x + k, y] = new Vector2Int(255, 255);
                            
                        }
                    index++;
                    
                    
                    }

                    if (dic == "l" && map[x - 1, y].x != 255)//left
                    {
                        for (int k = 1; k < 6; k++)
                        {
                            if (isFirst) {
                                newPos = new Vector3(this.transform.position.x + 1.5f * (x - k), 0.5f, this.transform.position.z - (1.5f * y));
                                newCell = Instantiate(wall, newPos, Quaternion.identity);
                                newCell.transform.SetParent(this.transform);
                                wallList.Add(newCell);
                            }
                            map[x - k, y] = new Vector2Int(255, 255);
                            
                        }
                    index++;
                    

                    }
                    if (dic == "u" && map[x, y - 1].x != 255)//up
                    {
                        for (int k = 1; k < 6; k++)
                        {
                            if (isFirst) {
                                newPos = new Vector3(this.transform.position.x + 1.5f * x, 0.5f, this.transform.position.z - (1.5f * (y - k)));
                                newCell = Instantiate(wall, newPos, Quaternion.identity);
                                newCell.transform.SetParent(this.transform);
                                wallList.Add(newCell);
                            }
                            map[x, y - k] = new Vector2Int(255, 255);
                            
                        }
                    index++;
                    

                    }
                    if (dic == "d" && map[x, y + 1].x != 255)//down
                    {
                        for (int k = 1; k < 6; k++)
                        {
                            if (isFirst) {
                                newPos = new Vector3(this.transform.position.x + 1.5f * x, 0.5f, this.transform.position.z - (1.5f * (y + k)));
                                newCell = Instantiate(wall, newPos, Quaternion.identity);
                                newCell.transform.SetParent(this.transform);
                                wallList.Add(newCell);
                        }
                            map[x, y + k] = new Vector2Int(255, 255);
                            
                        }
                    index++;
                    

                    }
                if (index > mapData.Length - 1) break;

                

                
            }
        }

    }

    public void creatGoal() {

        map[width - 1, height - 1] = new Vector2Int(5, 0);
        Vector3 newPos = new Vector3(this.transform.position.x + 1.5f * (width - 1), 0.5f, this.transform.position.z - (1.5f * (height - 1)));
        GameObject newCell = Instantiate(goal, newPos, Quaternion.identity);
        itemList.Add(newCell);
        newCell.transform.SetParent(this.transform);
        
    }

    public void creatGoal(Vector2Int pos)
    {
        if (map[pos.x, pos.y].x != 0)
            throw new System.Exception("given position have other object, object code: " + map[pos.x, pos.y].x);


        map[pos.x, pos.y] = new Vector2Int(5, 0);
        Vector3 newPos = new Vector3(this.transform.position.x + 1.5f * (pos.x), 0.5f, this.transform.position.z - (1.5f * (pos.y)));
        GameObject newCell = Instantiate(goal, newPos, Quaternion.identity);
        itemList.Add(newCell);
        newCell.transform.SetParent(this.transform);

    }

    //Random
    public void creatWall()
    {

        for (int i = 5; i < width; i += 6)
        {
            for (int j = 5; j < height; j += 6)
            {
                Vector3 newPos = new Vector3(this.transform.position.x + 1.5f * i, 0.5f, this.transform.position.z - (1.5f * j));
                GameObject newCell = Instantiate(wall, newPos, Quaternion.identity);
                newCell.transform.SetParent(this.transform);
                map[i, j] = new Vector2Int(255, 255);
                int x = i;
                int y = j;
                while (true)
                {
                    int dic = Random.Range(0, 4);
                    if (dic == 0 && map[x + 1, y].x != -1)//right
                    {
                        for (int k = 1; k < 6; k++)
                        {
                            map[x + k, y] = new Vector2Int(255, 255);
                            newPos = new Vector3(this.transform.position.x + 1.5f * (x + k), 0.5f, this.transform.position.z - (1.5f * y));
                            newCell = Instantiate(wall, newPos, Quaternion.identity);
                            newCell.transform.SetParent(this.transform);
                            wallList.Add(newCell);
                        }
                        break;

                    }

                    if (dic == 1 && map[x - 1, y].x != -1)//left
                    {
                        for (int k = 1; k < 6; k++)
                        {
                            map[x - k, y] = new Vector2Int(255, 255);
                            newPos = new Vector3(this.transform.position.x + 1.5f * (x - k), 0.5f, this.transform.position.z - (1.5f * y));
                            newCell = Instantiate(wall, newPos, Quaternion.identity);
                            newCell.transform.SetParent(this.transform);
                            wallList.Add(newCell);
                        }
                        break;

                    }
                    if (dic == 2 && map[x, y - 1].x != -1)//up
                    {
                        for (int k = 1; k < 6; k++)
                        {
                            map[x, y - k] = new Vector2Int(255, 255);
                            newPos = new Vector3(this.transform.position.x + 1.5f * x, 0.5f, this.transform.position.z - (1.5f * (y - k)));
                            newCell = Instantiate(wall, newPos, Quaternion.identity);
                            newCell.transform.SetParent(this.transform);
                            wallList.Add(newCell);
                        }
                        break;

                    }
                    if (dic == 3 && map[x, y + 1].x != -1)//down
                    {
                        for (int k = 1; k < 6; k++)
                        {
                            map[x, y + k] = new Vector2Int(255, 255);
                            newPos = new Vector3(this.transform.position.x + 1.5f * x, 0.5f, this.transform.position.z - (1.5f * (y + k)));
                            newCell = Instantiate(wall, newPos, Quaternion.identity);
                            newCell.transform.SetParent(this.transform);
                            wallList.Add(newCell);
                        }
                        break;

                    }


                }
            }
        }
    }


    //random with seed
    public void creatItem(int num ,GameObject perfb,int code,float seed)
    {
        int curNum = 0;
        
        while (curNum < num) {
            int x = GenerateRandomNumber(2, width - 1,11, width - 1,seed);
            int y = GenerateRandomNumber(2, width - 1, 2, width - 7, seed);
            if (map[x, y].x == 0) {
                Vector3 newPos = new Vector3(this.transform.position.x + 1.5f * x, 0.5f, this.transform.position.z - (1.5f * y));
                GameObject newCell = Instantiate(perfb, newPos, Quaternion.identity);
                newCell.transform.SetParent(this.transform);
                newCell.GetComponent<item>().set(code,x,y,this);
                map[x, y] =new Vector2Int(code,0);
                curNum++;
                itemList.Add(newCell);
                
            }
        }



    }


    //completely random
    public void creatItem(int num, GameObject perfb, int code)
    {
        int curNum = 0;

        while (curNum < num)
        {
            int x = Random.Range(0, width);
            int y = Random.Range(0, height);
            if (map[x, y].x == 0)
            {
                Vector3 newPos = new Vector3(this.transform.position.x + 1.5f * x, 0.5f, this.transform.position.z - (1.5f * y));
                GameObject newCell = Instantiate(perfb, newPos, Quaternion.identity);
                newCell.transform.SetParent(this.transform);
                newCell.GetComponent<item>().set(code, x, y, this);
                map[x, y] = new Vector2Int(code, 0);
                curNum++;
                itemList.Add(newCell);

            }
        }



    }


    public void creatItemWithPosition(GameObject perfb, int code,Vector2Int pos)
    {

        if (map[pos.x, pos.y].x == 0)
        {
            Vector3 newPos = new Vector3(this.transform.position.x + 1.5f * pos.x, 0.5f, this.transform.position.z - (1.5f * pos.y));
            GameObject newCell = Instantiate(perfb, newPos, Quaternion.identity);
            newCell.transform.SetParent(this.transform);
            newCell.GetComponent<item>().set(code, pos.x, pos.y, this);
            map[pos.x, pos.y] = new Vector2Int(code, 0);

            itemList.Add(newCell);

        }
        else { 
        
            throw new System.Exception("given position have other object, object code: " + map[pos.x, pos.y].x);
        }
        



    }

    public void creatItemWithPosition(GameObject perfb, int code, List<Vector2Int> posList)
    {
        foreach (Vector2Int pos in posList) {

            if (map[pos.x, pos.y].x == 0)
            {
                Vector3 newPos = new Vector3(this.transform.position.x + 1.5f * pos.x, 0.5f, this.transform.position.z - (1.5f * pos.y));
                GameObject newCell = Instantiate(perfb, newPos, Quaternion.identity);
                newCell.transform.SetParent(this.transform);
                newCell.GetComponent<item>().set(code, pos.x, pos.y, this);
                map[pos.x, pos.y] = new Vector2Int(code, 0);

                itemList.Add(newCell);

            }
            else
            {

                throw new System.Exception("given position {"+ pos.x+ ","+pos.y+"} have other object, object code: " + map[pos.x, pos.y].x);
            }
        }
       




    }



    private int GenerateRandomNumber(int min , int max, int more_possble_min , int more_possble_max,float _seed=0.5f)
    {
        float seed = _seed;
        if (seed >= 1 || seed <=0) {
            seed = 0.5f;
        }
        
        int randomNumber = Random.Range(min, max);

        
        if (randomNumber < more_possble_min || randomNumber > more_possble_max)
        {
            
            float additionalProbability = Random.Range(0f, 1f);
            if (additionalProbability < seed)
            {
                randomNumber = GenerateRandomNumber(min, max, more_possble_min, more_possble_max, seed);
            }
        }

        
        return randomNumber;
    }

    public void mapReset()
    {
        foreach (var item in itemList)
        {
            Destroy(item);
        }
        itemList.Clear();
        map = new Vector2Int[width, height];
        generateMap();
    }

    public void mapReset(generateItemFuc generateMapFuc)
    {
        foreach (var item in itemList)
        {
            Destroy(item);
        }
        itemList.Clear();
        map = new Vector2Int[width, height];
        if (wallList.Count == 0) isFirst = true;
        else isFirst = false;
        creatWall(mapData);
        generateMapFuc();
        if (haveGoal)
            creatGoal();
        
    }
}
