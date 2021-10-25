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
    public int[] time;
    public bool[] timeout;
    public string[] missionType;
    public int[] tracksCost;

    public int missionsCompleted;
    public int bonusMissionsCheck;
    public int bonusMissionsRewards;
}

public class GameManager : MonoBehaviour
{
    public static GameManager gm;

    // Reference of the created missions
    private MissionBase[] missions;

    // Coins collected in one game
    public int coins;

    // Total of missions completed in the game
    public int missionsCompleted;
    public int bonusMissionsCheck;
    public int bonusMissionsRewards;

    //public string userName = Login.userName;

    // Tracks availables in one game
    public int[] tracksCost;

    // Path to save file of game data
    private string folderPath;
    private string filePath;

    // Variables of the pause menu
    [HideInInspector]
    public static bool isPaused = false;
    
    // Function to save data
    public void Save() {
        BinaryFormatter bf = new BinaryFormatter();
        // create file in set path
        FileStream file = File.Create(filePath);

        // save game data from game manager
        PlayerData data = new PlayerData();
        // save coins in data object
        data.coins = coins;
        // save total of missions completed in data object
        data.missionsCompleted = missionsCompleted;
        data.bonusMissionsCheck = bonusMissionsCheck;
        data.bonusMissionsRewards = bonusMissionsRewards;
        // its necessary initiate each vector
        // size 2 because menu show two missions
        data.max = new int[2];
        data.progress = new int[2];
        data.currentProgress = new int[2];
        data.reward = new int[2];
        data.missionType = new string[2];
        data.timeout = new bool[2];
        data.time = new int[2];
        data.tracksCost = new int[tracksCost.Length]; 

        // save each value of each index in data object
        for (int i = 0; i < 2; i++) {
            data.max[i] = missions[i].max;
            data.progress[i] = missions[i].progress;
            data.currentProgress[i] = missions[i].currentProgress;
            data.reward[i] = missions[i].reward;
            data.timeout[i] = missions[i].timeout;
            data.time[i] = missions[i].time;
            data.missionType[i] = missions[i].missionType.ToString(); 
        } 

        for(int i  = 0; i < tracksCost.Length; i++) {
            data.tracksCost[i] = tracksCost[i];
        }

        // send all data content to file
        bf.Serialize(file, data);
        file.Close();
    }

    // Function to load data from file
    void Load() {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(filePath, FileMode.Open);
        // set all content of file in object data
        PlayerData data = (PlayerData)bf.Deserialize(file);
        file.Close();

        // set data of file in variables of the game
        coins = data.coins;
        missionsCompleted = data.missionsCompleted;
        bonusMissionsCheck = data.bonusMissionsCheck;
        bonusMissionsRewards = data.bonusMissionsRewards;

        for (int i = 0; i < 2; i++) {
            GameObject newMission = new GameObject("Mission" + i);
            // set game manager as parent of mission
            newMission.transform.SetParent(transform);
            // set mission type and add component

            // if (data.missionType[i] == MissionType.SingleRun.ToString()) {
            //     missions[i] = newMission.AddComponent<SingleRun>();
            //     missions[i].missionType = MissionType.SingleRun;
            // } else if (data.missionType[i] == MissionType.CollectSingleRun.ToString()) {
            //     missions[i] = newMission.AddComponent<CollectSingleRun>();
            //     missions[i].missionType = MissionType.CollectSingleRun;
            // } else if (data.missionType[i] == MissionType.TimeRun.ToString()) {
            //     missions[i] = newMission.AddComponent<TimeRun>();
            //     missions[i].missionType = MissionType.TimeRun;
            // }
            // FIXME: Alteração feita aqui
            missions[i] = newMission.AddComponent<TimeRun>();
            missions[i].missionType = MissionType.TimeRun;

            missions[i].max = data.max[i];
            missions[i].progress = data.progress[i];
            missions[i].currentProgress = data.currentProgress[i];
            missions[i].reward = data.reward[i];
            missions[i].timeout = data.timeout[i];
            missions[i].time = data.time[i];
        }

        for(int i = 0; i < tracksCost.Length; i++) {
            tracksCost[i] = data.tracksCost[i];
        }      
    }

    // Awake is called before start
    private void Awake() {
        if (gm == null) {
            gm = this;
        }
        else if (gm != this) {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        //Create path to save folder with user data
        folderPath = Application.dataPath + "/Users/" + Login.userName;

        // Check if user's folder already exists, if not, create new user
        if(!Directory.Exists(folderPath)){
            Directory.CreateDirectory(folderPath);
        } 

        // Create path to save file with game data
        filePath = folderPath + "/playerInfo-" + Login.userName + ".dat";

        // Instantiate two missions
        missions = new MissionBase[2];

        // Verify if file already exists, if yes, load data, else, create
        if (File.Exists(filePath)) {
            Load();
        } else {
            for (int i = 0; i < missions.Length; i++) {
                GameObject newMission = new GameObject("Mission" + i);
                newMission.transform.SetParent(transform);
                // create vector of mission's type 
                MissionType[] missionTypes = { MissionType.SingleRun, MissionType.CollectSingleRun, MissionType.TimeRun };
                // int randomType = Random.Range(0, missionTypes.Length);
                // // verify which mission was selected
                // if (randomType == (int)MissionType.SingleRun) {
                //     missions[i] = newMission.AddComponent<SingleRun>();
                // } else if (randomType == (int)MissionType.CollectSingleRun) {
                //     missions[i] = newMission.AddComponent<CollectSingleRun>();
                // } else if (randomType == (int)MissionType.TimeRun) {
                //     missions[i] = newMission.AddComponent<TimeRun>();
                // } 
                // FIXME: Alteração feita aqui
                missions[i] = newMission.AddComponent<TimeRun>();
                missions[i].Created();
            }
        }
    }

    // Start is called before the first frame update
    void Start() {       
    }

    // Update is called once per frame
    void Update() {
    }

    public void CloseGame() {
        Destroy(gameObject);
    }

    public void PauseGame() {
        Time.timeScale = 0;
        isPaused = true;
    }

    public void ResumeGame() {
        Time.timeScale = 1;
        isPaused = false;
    }

    public void StartRunNight() {
        SceneManager.LoadScene("NightRun");
    }

    public void StartRunDay() {
        SceneManager.LoadScene("DayRun");
    }

    public void StartReward() {
        SceneManager.LoadScene("FocusToReward");
    }

    public void StartBonus() {
        SceneManager.LoadScene("MeditateToReward");
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
        MissionType[] missionTypes = { MissionType.SingleRun, MissionType.CollectSingleRun, MissionType.TimeRun };
        // int randomType = Random.Range(0, missionTypes.Length);
        // // verify which mission was selected
        // if (randomType == (int)MissionType.SingleRun) {
        //     missions[index] = newMission.AddComponent<SingleRun>();
        // } else if (randomType == (int)MissionType.CollectSingleRun) {
        //     missions[index] = newMission.AddComponent<CollectSingleRun>();
        // } else if (randomType == (int)MissionType.TimeRun) {
        //     missions[index] = newMission.AddComponent<TimeRun>();
        // }
        // FIXME: Alteração feita aqui
        missions[index] = newMission.AddComponent<TimeRun>();
        missions[index].Created();

        FindObjectOfType<Menu>().SetMission();
    }
}
