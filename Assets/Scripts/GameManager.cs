using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Libraries to save data from game
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Random = UnityEngine.Random;

[Serializable]
public class PlayerData
{
    // Data to save
    public int coins;
    public int[] max; // vector because is two missons
    public int[] progress;
    public int[] currentProgress;
    public int[] reward;
    public string[] missionType;
}

public class GameManager : MonoBehaviour
{
    public static GameManager gm;

    // Reference of the created missions
    private MissionBase[] missions;

    // Coins collected in one game
    public int coins;

    // Path to save file of game data
    private string folderPath;
    private string filePath;

    // Function to save data
    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        // create file in set path
        FileStream file = File.Create(filePath);

        // save game data from game manager
        PlayerData data = new PlayerData();
        // save coins in data object
        data.coins = coins;
        // its necessary initiate each vector
        // size 2 because menu show two missions
        data.max = new int[2];
        data.progress = new int[2];
        data.currentProgress = new int[2];
        data.reward = new int[2];
        data.missionType = new string[2];
        // save each value of each index in data object
        for (int i = 0; i < 2; i++)
        {
            data.max[i] = missions[i].max;
            data.progress[i] = missions[i].progress;
            data.currentProgress[i] = missions[i].currentProgress;
            data.reward[i] = missions[i].reward;
            data.missionType[i] = missions[i].missionType.ToString();
        }
        // send all data content to file
        bf.Serialize(file, data);
        file.Close();
    }

    // Function to load data from file
    void Load()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(filePath, FileMode.Open);
        // set all content of file in object data
        PlayerData data = (PlayerData)bf.Deserialize(file);
        file.Close();

        // set data of file in variables of the game
        coins = data.coins;

        for (int i = 0; i < 2; i++)
        {
            GameObject newMission = new GameObject("Mission" + i);
            // set game manager as parent of mission
            newMission.transform.SetParent(transform);
            // set mission type and add component
            if (data.missionType[i] == MissionType.SingleRun.ToString())
            {
                missions[i] = newMission.AddComponent<SingleRun>();
                missions[i].missionType = MissionType.SingleRun;
            }
            else if (data.missionType[i] == MissionType.TotalMeters.ToString())
            {
                missions[i] = newMission.AddComponent<TotalMeters>();
                missions[i].missionType = MissionType.TotalMeters;
            }
            else if (data.missionType[i] == MissionType.FishesSingleRun.ToString())
            {
                missions[i] = newMission.AddComponent<FishesSingleRun>();
                missions[i].missionType = MissionType.FishesSingleRun;
            }

            missions[i].max = data.max[i];
            missions[i].progress = data.progress[i];
            missions[i].currentProgress = data.currentProgress[i];
            missions[i].reward = data.reward[i];
        }
    }

    // Awake is called before start
    private void Awake()
    {
        if (gm == null)
        {
            gm = this;
        }
        else if (gm != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        // Create path to save folder with user data
        folderPath = Application.dataPath + "/Users/" + Login.userName;

        // Check if user's folder already exists, if not, create new user
        if(!Directory.Exists(folderPath)){
            Directory.CreateDirectory(folderPath);
            Debug.Log("Usuário criado");
        } 

        // Create path to save file with game data
        filePath = folderPath + "/playerInfo-" + Login.userName + ".dat";

        // Instantiate two missions
        missions = new MissionBase[2];

        // Verify if file already exists, if yes, load data, else, create
        if (File.Exists(filePath))
        {
            Load();
        }
        else
        {
            for (int i = 0; i < missions.Length; i++)
            {
                GameObject newMission = new GameObject("Mission" + i);
                newMission.transform.SetParent(transform);
                // create vector of mission's type 
                MissionType[] missionTypes = { MissionType.SingleRun, MissionType.FishesSingleRun, MissionType.TotalMeters };
                int randomType = Random.Range(0, missionTypes.Length);
                // verify which mission was selected
                if (randomType == (int)MissionType.SingleRun)
                {
                    missions[i] = newMission.AddComponent<SingleRun>();
                }
                else if (randomType == (int)MissionType.TotalMeters)
                {
                    missions[i] = newMission.AddComponent<TotalMeters>();
                }
                else if (randomType == (int)MissionType.FishesSingleRun)
                {
                    missions[i] = newMission.AddComponent<FishesSingleRun>();
                }
                missions[i].Created();
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartRun() {
        SceneManager.LoadScene("NightRun");
    }

    public void StartReward() {
        SceneManager.LoadScene("FocusToReward");
    }

    public void EndRun() {
        SceneManager.LoadScene("Menu");
    }

    public MissionBase GetMission(int index) {
        return missions[index];
    }

    public void StartMissions()
    {
        for (int i = 0; i < 2; i++)
        {
            missions[i].RunStart();
        }
    }

    // Destroy finished mission that already had reward collect and generate/set new mission to the user
    public void GenerateMission(int index)
    {
        Destroy(missions[index].gameObject);

        // Create new mission
        GameObject newMission = new GameObject("Mission" + index);
        newMission.transform.SetParent(transform);
        // create vector of mission's type 
        MissionType[] missionTypes = { MissionType.SingleRun, MissionType.FishesSingleRun, MissionType.TotalMeters };
        int randomType = Random.Range(0, missionTypes.Length);
        // verify which mission was selected
        if (randomType == (int)MissionType.SingleRun)
        {
            missions[index] = newMission.AddComponent<SingleRun>();
        }
        else if (randomType == (int)MissionType.TotalMeters)
        {
            missions[index] = newMission.AddComponent<TotalMeters>();
        }
        else if (randomType == (int)MissionType.FishesSingleRun)
        {
            missions[index] = newMission.AddComponent<FishesSingleRun>();
        }
        missions[index].Created();

        FindObjectOfType<Menu>().SetMission();
    }
}
