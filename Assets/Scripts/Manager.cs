using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    #region Variables
    [SerializeField] GameObject player;
    [SerializeField] GameObject tree;
    [SerializeField] GameObject soldier;
    [SerializeField] GameObject hospital;
    [SerializeField] GameObject uiSoldier;
    [SerializeField] GameObject uiHelicopter;
    [SerializeField] GameObject uiGameStatus;

    public int rescueCount = 0;
    public int soldierCount = 0;
    public int soldierTreshold = 3;
    public int totalSoldierCount = 0;
    float upperX;
    float lowerX;
    float upperY;
    float lowerY;
    public bool hasWon = false;
    public bool hasLost = false;
    AudioSource audioSource;
    #endregion

    void Start()
    {
        upperX = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0)).x;
        upperY = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0)).y;
        lowerX = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).x + (upperX * 1.25f);
        lowerY = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).y;

        audioSource = GetComponent<AudioSource>();

        SpawnGameObjects();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RemoveAllGameObjects();
            totalSoldierCount = 0;
            soldierCount = 0;
            rescueCount = 0;
            hasWon = false;
            hasLost = false;
            SpawnGameObjects();
            uiHelicopter.GetComponent<Text>().text = soldierCount.ToString();
            uiSoldier.GetComponent<Text>().text = rescueCount.ToString();
        }

        if (rescueCount == totalSoldierCount)
        {
            hasWon = true;
        }

        if (hasWon)
        {
            uiGameStatus.GetComponent<Text>().text = "You Win!";
        }
        else if (hasLost)
        {
            uiGameStatus.GetComponent<Text>().text = "You Lose!";
        }
        else
        {
            uiGameStatus.GetComponent<Text>().text = "";
        }
    }

    public void HandleSoldierPickup(GameObject soldier)
    {
        if (soldierCount < soldierTreshold)
        {
            soldierCount++;
            audioSource.Play();
            uiHelicopter.GetComponent<Text>().text = soldierCount.ToString();
            Destroy(soldier);
        }
    }

    public void HandleSoldierDeposit()
    {
        rescueCount += soldierCount; 
        soldierCount = 0;
        uiHelicopter.GetComponent<Text>().text = soldierCount.ToString();
        uiSoldier.GetComponent<Text>().text = rescueCount.ToString();
    }

    void SpawnGameObjects()
    {


        Instantiate(player, new Vector3(-1f, 0, 0), Quaternion.identity);

        Instantiate(hospital, new Vector3(-3f, 0, 0), Quaternion.identity);

        List<Vector3> treeLocations = new List<Vector3>();
        
        // Spawn first tree
        Vector3 firstVector = new Vector3(Random.Range(lowerX, upperX), Random.Range(lowerY, upperY), 0);
        Instantiate(tree, new Vector3 (Random.Range(lowerX, upperX), Random.Range(lowerY, upperY), 0), Quaternion.identity);
        treeLocations.Add(firstVector);
        
        // Spawn 2 to 4 trees
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

        // Spawn first soldier
        Vector3 firstSoldierVector = new Vector3(Random.Range(lowerX, upperX), Random.Range(lowerY, upperY), 0);
        Instantiate(soldier, new Vector3 (Random.Range(lowerX, upperX), Random.Range(lowerY, upperY), 0), Quaternion.identity);
        treeLocations.Add(firstSoldierVector);
        totalSoldierCount++;

        // Spawn 3 to 5 soldiers
        for (int i = 0; i < Random.Range(3, 5); i++)
        {
            Vector3 tempVector = new Vector3 (Random.Range(lowerX, upperX), Random.Range(lowerY, upperY), 0);
            bool soldierSpawned = false;
            bool isSpawnable = true;

            while(!soldierSpawned)
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
                    Instantiate(soldier, tempVector, Quaternion.identity);
                    totalSoldierCount++;
                    soldierSpawned = true;
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

        // Remove all soldier game objects
        GameObject[] soldiers = GameObject.FindGameObjectsWithTag("Soldier");
        foreach (GameObject soldier in soldiers)
        {
            Destroy(soldier);
        }

        // Remove all hospital game objects
        GameObject[] hospitals = GameObject.FindGameObjectsWithTag("Hospital");
        foreach (GameObject hospital in hospitals)
        {
            Destroy(hospital);
        }
    }
}