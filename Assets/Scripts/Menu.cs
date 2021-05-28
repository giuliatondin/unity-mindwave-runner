using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{

    // Reference to the missions descriptions of Game Manager
    public Text[] missionDescription, missionReward, missionProgress;

    // Reference to the reward button
    public GameObject[] rewardButton;

    // Text that show in menu quantity of fishes
    public Text coinsText;

    // Variable that control wich scene is to load next
    public static int sceneControl = 0; // if 1 = run, 2 = reward;

    // Variable that control wich mission is select to collect reward
    public static int missionIndex;

    // Start is called before the first frame update
    void Start() {
        // display user name in menu
        Text welcome = GameObject.Find("Welcome").GetComponent<Text>();
        welcome.text = "Olá, " + Login.userName +"! Complete as missões apresentadas abaixo:";
        
        // display missions
        SetMission();

        if(ProgressBar.missionControl != -1) {
            missionIndex = ProgressBar.missionControl;
            GetReward(missionIndex);
        }
    }

    // Update quantity of fishes collected in rewards in menu
    public void UpdateCoins(int coins) {
        coinsText.text = coins.ToString();
    }

    // Update is called once per frame
    void Update() {

    }

    // Function called when button start is pressed
    public void ConnectMindwave() {
        sceneControl = 1;
        SceneManager.LoadScene("ConnectHeadset");
    }

    // Function called when button cancel is pressed
    public void CloseGame() {
        SceneManager.LoadScene("Login");
    }

    public void SetName(string name)
    {
        Text userName = GameObject.Find("Welcome").GetComponent<Text>();
        userName.text = "Boas vindas, " + name;
        Debug.Log(userName.text);
    }

    public void SetMission()
    {
        // Select 2 missions
        for (int i = 0; i < 2; i++)
        {
            MissionBase mission = GameManager.gm.GetMission(i);
            missionDescription[i].text = mission.GetMissionDescription();
            missionReward[i].text = "Recompensa: " + mission.reward;
            missionProgress[i].text = mission.progress + mission.currentProgress + " / " + mission.max;
            if (mission.GetMissionComplete())
            {
                rewardButton[i].SetActive(true);
            }
        }

        // Call save function to save data of the game
        // Everytime that a mission is setted, data of the game is save
        GameManager.gm.Save();
    }

    // Function called when button reward is pressed
    public void CollectReward(int index) {
        sceneControl = 2;
        missionIndex = index;
        SceneManager.LoadScene("ConnectHeadset");
    }

    // Function called in button that collect reward
    // if collected reward, call function GenerateMission that set new mission to the game
    // and update total of fishes collected in rewards
    public void GetReward(int missionIndex) {
        GameManager.gm.coins += GameManager.gm.GetMission(missionIndex).reward;
        UpdateCoins(GameManager.gm.coins);
        rewardButton[missionIndex].SetActive(false);
        GameManager.gm.GenerateMission(missionIndex);
    }
}
