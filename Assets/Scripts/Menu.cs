using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{

    // Reference to the missions descriptions of Game Manager
    public Text[] missionDescription, missionReward, missionProgress;
    public Image[] missionProgressBar;

    // Reference to the reward button
    public GameObject[] rewardButton;
    
    // Text that show in menu quantity of fishes
    public Text coinsText;

    // Variable that control wich scene is to load next
    public static int sceneControl = 0; // if 1 = run, 2 = reward;

    // Variable that control wich mission is select to collect reward
    public static int missionIndex;

    // Variable that control wich track is select
    [HideInInspector]
    public static int trackIndex = 0;

    // Reference to tracks panel
    public Button[] tracksButton;
    public Image[] tracksButtonBorder; 

    // Start is called before the first frame update
    void Start() {
        // display user name in menu
        Text welcome = GameObject.Find("Welcome").GetComponent<Text>();
        welcome.text = "Complete as missões apresentadas abaixo:";

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

    public void SetMission()
    {
        // Select 2 missions
        for (int i = 0; i < 2; i++)
        {
            MissionBase mission = GameManager.gm.GetMission(i);
            missionDescription[i].text = mission.GetMissionDescription();
            missionReward[i].text = "Recompensa: " + mission.reward;

            missionProgress[i].text = mission.progress + mission.currentProgress + " / " + mission.max;

            float fillAmount = ((float)mission.progress + (float)mission.currentProgress) / (float)mission.max;
            if(fillAmount > 1) missionProgressBar[i].fillAmount = 1;
            else missionProgressBar[i].fillAmount = fillAmount;

            if (mission.GetMissionComplete()) {
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

    public void SetTrack(int index) {
        if(index == 0) {
            trackIndex = 0;
            tracksButtonBorder[0].gameObject.SetActive(true);
            tracksButtonBorder[1].gameObject.SetActive(false);
        } else if(index == 1)  {
            trackIndex = 1;
            tracksButtonBorder[1].gameObject.SetActive(true);
            tracksButtonBorder[0].gameObject.SetActive(false);
        }
    }
}
