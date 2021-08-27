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

    // Variable that control the game manager and close of game
    [HideInInspector]
    public static bool gameStart = true;

    // Reference to tracks panel
    public Image[] tracks;
    public Text tracksCostText;
    public GameObject trackBlockText;
    public GameObject buyTrackPanel;
    public GameObject confirmBuyButton;
    public GameObject[] buyTrackWarnings;
    private bool buyTrackIsOpen = false;
    public Text trackWarning;

    // Start is called before the first frame update
    void Start() {
        gameStart = true;

        Text welcome = GameObject.Find("Welcome").GetComponent<Text>();
        welcome.text = "Complete as missões apresentadas abaixo:";

        coinsText.text = GameManager.gm.coins.ToString();

        SetMission(); // display missions

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
    void Update() {}

    // Function called when button start is pressed
    public void ConnectMindwave() {
        // if track isn't block, load connectheadset scene
        if(GameManager.gm.tracksCost[trackIndex] == 0) {
            sceneControl = 1;
            SceneManager.LoadScene("ConnectHeadset");
        } else { // if it is, show warning 
            StartCoroutine(ShowTrackWarning()); 
        }
    }

    // 
    IEnumerator ShowTrackWarning() {
        trackWarning.gameObject.SetActive(true);
        yield return new WaitForSeconds(3);
        trackWarning.gameObject.SetActive(false);
    }

    // Function called when button cancel is pressed
    public void CloseGame() {
        GameManager.gm.Save();
        GameManager.gm.CloseGame();
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
        ProgressBar.missionControl = -1;
    }

    public void SetTrack(int index) {
        if(index == 0) {
            trackIndex = 0;
        } else if(index == 1)  {
            trackIndex = 1;
        }
    }

    public void ChangeTrack(int index) {
        trackIndex += index;
        if(trackIndex >= tracks.Length) trackIndex = 0;
        else if(trackIndex <= 0) trackIndex = tracks.Length - 1;

        for(int i = 0; i < tracks.Length; i++) {
            if(i == trackIndex) tracks[i].gameObject.SetActive(true);
            else tracks[i].gameObject.SetActive(false);
        }

        string cost = "";
        if(GameManager.gm.tracksCost[trackIndex] != 0) {
            cost = GameManager.gm.tracksCost[trackIndex].ToString();
            trackBlockText.gameObject.SetActive(true);
            tracks[trackIndex].color = new Color32(255, 255, 255, 100);
        } else if(GameManager.gm.tracksCost[trackIndex] == 0) {
            trackBlockText.gameObject.SetActive(false);
            tracks[trackIndex].color = new Color32(255, 255, 255, 255);
        }
        tracksCostText.text = cost;
    
    }

    // Function to open and close buy track panel
    public void OpenBuyTrackPanel() {
        if(buyTrackIsOpen) {
            buyTrackPanel.SetActive(false);
            buyTrackIsOpen = false;
        } else {
            buyTrackPanel.SetActive(true);
            buyTrackIsOpen = true;
            if(GameManager.gm.tracksCost[trackIndex] <= GameManager.gm.coins) {
                buyTrackWarnings[0].SetActive(true);
                buyTrackWarnings[1].SetActive(false);
            } else { 
                buyTrackWarnings[0].SetActive(false);
                buyTrackWarnings[1].SetActive(true);
            }
        }
    }

    // Function to get coins to buy track
    public void BuyTrack() {
        GameManager.gm.coins -= GameManager.gm.tracksCost[trackIndex];
        GameManager.gm.tracksCost[trackIndex] = 0;
        GameManager.gm.Save();
    }
}
