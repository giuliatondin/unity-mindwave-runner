﻿using System.Collections;
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
    private int m_BlinkStrength = 0;

    public GameObject btnStart;
    public Text waitDescriptionText;
    public Text blinkControllerText;

    // Start is called before the first frame update
    // TODO: Change waiting text and button text
    void Start() {
        btnStart.SetActive(false);
        waitDescriptionText = GameObject.Find("Waiting Text").GetComponent<Text>();
        //blinkControllerText = GameObject.Find("Blink Controller Text").GetComponent<Text>();
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
        if (m_MindwaveData.eegPower.delta > 0) {
            waitDescriptionText.text = "Conexão estabelecida";
            btnStart.SetActive(true);
        } 
    }

    // public void BlinkController() {
    //     if(m_BlinkStrength > 100) {
    //         blinkControllerText.text = "Piscou forte";
    //     } else {
    //         blinkControllerText.text = "Esperando isso funcionar";
    //     }
    // }

    // Function to change to scene
    public void ChangeScene() {
        if(Menu.sceneControl == 1) {
            GameManager.gm.StartRun();
        } else if(Menu.sceneControl == 2) {
            GameManager.gm.StartReward();
        }
    }

    public void StartRun() {
        GameManager.gm.StartRun();
    }
}