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

    // Start is called before the first frame update
    void Start() {
        waitDescriptionText = GameObject.Find("Waiting Text").GetComponent<Text>();
        sceneDescription = GameObject.Find("Scene Description").GetComponent<Text>();

        // Change description text depending of the chosen scene 
        if(Menu.sceneControl == 1) sceneDescription.text = "Coloque o headset e prepare-se para correr! Desvie dos obstáculos, colete estrelas e ganhe o dobro de seu valor ao manter um nível elevado de atenção no jogo. Então, vamos lá, concentrAÇÃO!";
        else if(Menu.sceneControl == 2) sceneDescription.text = "Coloque o headset e prepare-se para a recompensa! Você precisa concentrar-se no crescimento da barra de progresso para completá-la e assim receber sua recompensa. As moedas estão te esperando, então vamos lá!";
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
        btnStart.SetActive(false);
        if(Menu.sceneControl == 1 && Menu.trackIndex == 0) GameManager.gm.StartRunDay();
        else if(Menu.sceneControl == 1 && Menu.trackIndex == 1) GameManager.gm.StartRunNight();
        else if(Menu.sceneControl == 2) GameManager.gm.StartReward();
    }

    // Function to retry connection when timeout is true
    public void RetryConnection() {
        Debug.Log("Entrou no retry");
        MindwaveManager.Instance.Controller.Connect();
        MindwaveController.isTimeout = false;
        waitDescriptionText.text = "Aguarde a conexão com o headset...";
        btnRetry.SetActive(false);
    }
}
