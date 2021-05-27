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
    [SerializeField]
	private MindwaveController m_Controller;
    private int m_BlinkStrength = 0;

    public GameObject btnStart;
    public GameObject btnRetry;
    private Text waitDescriptionText;
    private Text sceneDescription;
    private Text blinkControllerText;

    // Start is called before the first frame update
    void Start() {
        waitDescriptionText = GameObject.Find("Waiting Text").GetComponent<Text>();
        sceneDescription = GameObject.Find("Scene Description").GetComponent<Text>();
        //blinkControllerText = GameObject.Find("Blink Controller Text").GetComponent<Text>();

        // Change description text depending of the chosen scene //TODO: Define text description
        if(Menu.sceneControl == 1) sceneDescription.text = "Coloque o headset e prepare-se para correr! Coloque o headset e prepare-se para correr! Coloque o headset e prepare-se para correr!";
        else if(Menu.sceneControl == 2) sceneDescription.text = "Coloque o headset e foque para receber a recompensa! Coloque o headset e foque para receber a recompensa! Coloque o headset e foque para receber a recompensa!";
    }

    // Update is called once per frame
    void Update() {
        MindwaveManager.Instance.Controller.OnUpdateMindwaveData += OnUpdateMindwaveData;
        MindwaveManager.Instance.Controller.OnUpdateBlink += OnUpdateBlink;
        ConnectMindwave();
        //BlinkController();
    }

    public void OnUpdateMindwaveData(MindwaveDataModel _Data) {
        m_MindwaveData = _Data;
    }

    public void OnUpdateBlink(int _BlinkStrength) {
		m_BlinkStrength = _BlinkStrength;
	}

    public void ConnectMindwave() {
        // If Mindwave Headset send data, the button to start next scene is activate
        if (m_MindwaveData.eegPower.delta > 0) {
            waitDescriptionText.text = "Conexão estabelecida"; // TODO: Add retry if connect failed
            btnStart.SetActive(true);
        }

        if(MindwaveController.isTimeout) {
            btnRetry.SetActive(true);
        }  
    }

    // Function to change to scene
    public void ChangeScene() {
        if(Menu.sceneControl == 1) GameManager.gm.StartRun();
        else if(Menu.sceneControl == 2) GameManager.gm.StartReward();
    }

    // TODO: Parei aqui
    public void RetryConnection() {
        MindwaveManager.Instance.Controller.Connect();
        MindwaveController.isTimeout = false;
        btnRetry.SetActive(false);
    }

    // public void BlinkController() {
    //     if(m_BlinkStrength > 100) {
    //         blinkControllerText.text = "Piscou forte";
    //     } else {
    //         blinkControllerText.text = "Esperando isso funcionar";
    //     }
    // }
}
