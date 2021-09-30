using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MindwaveHandler : MonoBehaviour
{
    public static MindwaveHandler mh;

    // Variables that collect mindwave data
    private MindwaveDataModel m_MindwaveData;
    private MindwaveDataModel _Data;

    public GameObject btnStart;
    public GameObject btnRetry;
    private Text waitDescriptionText;
    private Text sceneDescription;

    [HideInInspector]
    public static bool newActivity;

    public string[] runTips;
    public string[] rewardTips;
    public string[] bonusTips;

    // Start is called before the first frame update
    void Start() {
        newActivity = true;
        
        GameManager.gm.ResumeGame();

        waitDescriptionText = GameObject.Find("Waiting Text").GetComponent<Text>();
        sceneDescription = GameObject.Find("Scene Description").GetComponent<Text>();

        // Change description text depending of the chosen scene 
        if(Menu.sceneControl == 1) sceneDescription.text = runTips[Random.Range(0, runTips.Length)];
        else if(Menu.sceneControl == 2) sceneDescription.text = rewardTips[Random.Range(0, rewardTips.Length)];
        else if(Menu.sceneControl == 3) sceneDescription.text = bonusTips[Random.Range(0, bonusTips.Length)];
    }

    // Update is called once per frame
    void Update() {
        MindwaveManager.Instance.Controller.OnUpdateMindwaveData += OnUpdateMindwaveData;
        ConnectMindwave();
    }

    public void OnUpdateMindwaveData(MindwaveDataModel _Data) {
        m_MindwaveData = _Data;
    }

    public void ConnectMindwave() {
        // If Mindwave Headset send data, the button to start next scene is activate
        if (m_MindwaveData.eegPower.delta > 0) {
            waitDescriptionText.text = "Conexão estabelecida"; 
            btnStart.SetActive(true);
        } 

        if(MindwaveController.isTimeout) {
            waitDescriptionText.text = "Falha na conexão";
            btnRetry.SetActive(true);
        }  
    }

    // Function to change to scene
    public void ChangeScene() {
        waitDescriptionText.text = "Iniciando..."; 
        btnStart.SetActive(false);
        if(Menu.sceneControl == 1 && Menu.trackIndex == 0) GameManager.gm.StartRunDay();
        else if(Menu.sceneControl == 1 && Menu.trackIndex == 1) GameManager.gm.StartRunNight();
        else if(Menu.sceneControl == 2) GameManager.gm.StartReward();
        else if(Menu.sceneControl == 3) GameManager.gm.StartBonus();
    }

    // Function to retry connection when timeout is true
    public void RetryConnection() {
        MindwaveManager.Instance.Controller.Connect();
        MindwaveController.isTimeout = false;
        waitDescriptionText.text = "Aguarde a conexão com o headset...";
        btnRetry.SetActive(false);
    }
}
