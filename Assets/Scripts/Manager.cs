using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    #region Variables
    [SerializeField] GameObject player;
    [SerializeField] GameObject tree;
    int rescueCount = 0;
    int soldierCount = 0;
    int soldierTreshold = 3;
    // [SerializeField] GameObject soldier;
    // [SerializeField] GameObject hospital;
    float upperX;
    float lowerX;
    float upperY;
    float lowerY;
    #endregion

    void Start()
    {
        upperX = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0)).x;
        upperY = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0)).y;
        lowerX = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).x + (upperX * 1.25f);
        lowerY = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).y;

        SpawnGameObjects();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RemoveAllGameObjects();
            SpawnGameObjects();
        }
    }

    void SpawnGameObjects()
    {


        Instantiate(player, new Vector3(-1f, 0, 0), Quaternion.identity);

        List<Vector3> treeLocations = new List<Vector3>();
        
        // Spawn first tree
        Vector3 firstVector = new Vector3(Random.Range(lowerX, upperX), Random.Range(lowerY, upperY), 0);
        Instantiate(tree, new Vector3 (Random.Range(lowerX, upperX), Random.Range(lowerY, upperY), 0), Quaternion.identity);
        treeLocations.Add(firstVector);
        
        // Spawn 3 to 5 trees
        for (int i = 0; i < Random.Range(2, 4); i++)
        {
            Vector3 tempVector = new Vector3 (Random.Range(lowerX, upperX), Random.Range(lowerY, upperY), 0);
            bool treeSpawned = false;
            bool isSpawnable = true;

            while(!treeSpawned)
            {
                foreach (Vector3 treeLocation in treeLocations)
                {
                    if (Vector3.Distance(tempVector, treeLocation) < 1.3f)
                    {
                        isSpawnable = false;
                    }
                }
                if (isSpawnable)
                {
                    treeLocations.Add(tempVector);
                    Instantiate(tree, tempVector, Quaternion.identity);
                    treeSpawned = true;
                    break;
                }
                tempVector = new Vector3(Random.Range(lowerX, upperX), Random.Range(lowerY, upperY), 0);
                isSpawnable = true;
            }
        }
    }

    void RemoveAllGameObjects()
    {
        // Remove all helicopter game objects
        GameObject[] helicopters = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject helicopter in helicopters)
        {
            Destroy(helicopter);
        }

        // Remove all tree game objects
        GameObject[] trees = GameObject.FindGameObjectsWithTag("Death");
        foreach (GameObject tree in trees)
        {
            Destroy(tree);
        }
    }
}