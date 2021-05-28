using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Track : MonoBehaviour
{
    public GameObject[] obstacles;
    public List<GameObject> newObstacles;
    private float obstaclesControl; // control the increment of the number of obstacles

    // Min and max of obstables in scene
    public Vector2 numberOfObstacles;

    // Variables of points in the game
    public GameObject coin;
    public Vector2 numberOfCoins; // min and max of coins in game
    public List<GameObject> newCoins;
    public Vector2 numberCoinsInLane; // min and max of coins in same lane

    // Start is called before the first frame update
    void Start()
    {
        obstaclesControl = numberOfObstacles.x; // set obstaclesControl with min number of obstacles

        int newNumberOfCoins = (int)Random.Range(numberOfCoins.x, numberOfCoins.y);

        for(int i = 0; i < newNumberOfCoins; i++){
            newCoins.Add(Instantiate(coin, transform));
            newCoins[i].SetActive(false);
        }

        InstantiateObstacles();
        PositionateObstacles();
        PositionateCoins();
    }

    void InstantiateObstacles() {
        int newNumberOfObstacles = (int)obstaclesControl;
    
        for (int i = 0; i < newNumberOfObstacles; i++)
        {
            // Instatiate desired quantity of obstacles
            newObstacles.Add(Instantiate(obstacles[Random.Range(0, obstacles.Length)], transform));
            newObstacles[i].SetActive(false);
        }
    }

    // Positionate obstacles in scene
    void PositionateObstacles() {
        for (int i = 0; i < newObstacles.Count; i++)
        {
            float posZMin = (379f / newObstacles.Count) + (379f / newObstacles.Count) * i;
            float posZMax = (379f / newObstacles.Count) + (379f / newObstacles.Count) * i;
            // call obstacle of list
            newObstacles[i].transform.localPosition = new Vector3(0, 0, Random.Range(posZMin, posZMax));
            newObstacles[i].SetActive(true);
            // verify if obstacle have component of ChangeLane
            // a obstacle have this component if it was added in unity
            // if have this component, positionate obstacle in random lane between -1 and 1
            int randomLane = Random.Range(-1, 2);
            if(newObstacles[i].GetComponent<ChangeLane>() != null) newObstacles[i].GetComponent<ChangeLane>().PositionLane(randomLane);
        }
    }

    // Positionate coins in scene
    void PositionateCoins() {
        float minZPos = 10f;
        for(int i = 0; i < newCoins.Count; i++){
            float maxZPos = minZPos + 5f;
            float randomZPos = Random.Range(minZPos, maxZPos);
            newCoins[i].transform.localPosition = new Vector3(transform.position.x, transform.position.y, randomZPos);
            newCoins[i].SetActive(true);
            CoinsInLane();
            // positionate coin in some lane
            minZPos = randomZPos + 1;
        }
    }

    // Ensures good distribution of coins in lanes
    void CoinsInLane() {
        int aux = -2;
        for(int i = 0; i < newCoins.Count; i++){
            int randomLane = Random.Range(-1, 2);
            if(aux == randomLane && randomLane == -1) randomLane = Random.Range(0, 2);  
            else if(aux == randomLane && randomLane == 0) randomLane = -1;
            else if(aux == randomLane && randomLane == 1) randomLane = Random.Range(-1, 1);
            int numCoinsInLane = (int)Random.Range(numberCoinsInLane.x, numberCoinsInLane.y); // min and max of consecutive coins in one lane
            for(int j = 0; j < numCoinsInLane; j++) {
                newCoins[i].GetComponent<ChangeLane>().PositionLane(randomLane);
                i++;
                if(i == newCoins.Count) return;
            }
            aux = randomLane;
        }
    }

    // Repeating scenario
    private void OnTriggerEnter(Collider other){
        if(other.CompareTag("Player")){
            transform.position = new Vector3(0, 0, transform.position.z + 379 * 2);
            obstaclesControl += 3;
            if(obstaclesControl > numberOfObstacles.y) obstaclesControl = numberOfObstacles.y;
            InstantiateObstacles();
            PositionateObstacles();
            PositionateCoins();
        }    
    }
}
