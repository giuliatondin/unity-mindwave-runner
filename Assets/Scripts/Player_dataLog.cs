using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System;

public class Player_dataLog : MonoBehaviour {
    private string folderPath;

    private MindwaveDataModel m_MindwaveData;
    private MindwaveDataModel _Data;
    public Player player;

    int dataControl = 0;
    int textControl = 0; // 1 - run, 2 - reward
    [HideInInspector]
    public static int run = 1; // num of runs in one day
    int reward = 0; // num of rewards in one day
    int bonus = 0;

    [HideInInspector]
    public static float attentionTotal = 0;
    [HideInInspector]
    public static int registers = 0;

    void CreateText(int control) {
        if(control == 1) {
            // Path of the file
            string path = folderPath + "/" + Login.userName + "-RunLog_" + System.DateTime.Now.ToString("dd-MM-yy") + ".txt";
            // Check if file exist
            if (!File.Exists(path)) {
                //File.WriteAllText(path, "run;delta;theta;lowAlpha;HighAlpha;lowBeta;highBeta;lowGamma;highGamma;attention;velocity;currentLife;coins;distance(m);timestamp(s)\n");
                File.WriteAllText(path, "run;lowBeta;highBeta;attention;velocity;currentLife;coins;distance(m);timestamp(s)\n");
                MindwaveHandler.newActivity = false;
                run = 1;
            } else if(MindwaveHandler.newActivity == true) {
                MindwaveHandler.newActivity = false;
                run = ReadLastLine(path);
            }
            //Content of the file
            if (m_MindwaveData.eegPower.delta > 0 && Player.timeCounter > 0)  {
                //dataControl = m_MindwaveData.eegPower.delta;
                // string content = run + ";" + MindwaveManager.Instance.Calibrator.EvaluateRatio(Brainwave.Delta, m_MindwaveData.eegPower.delta) + ";" + MindwaveManager.Instance.Calibrator.EvaluateRatio(Brainwave.Theta, m_MindwaveData.eegPower.theta) + ";" + MindwaveManager.Instance.Calibrator.EvaluateRatio(Brainwave.LowAlpha, m_MindwaveData.eegPower.lowAlpha) + ";" + MindwaveManager.Instance.Calibrator.EvaluateRatio(Brainwave.HighAlpha, m_MindwaveData.eegPower.highAlpha) + ";" + MindwaveManager.Instance.Calibrator.EvaluateRatio(Brainwave.LowBeta, m_MindwaveData.eegPower.lowBeta) + ";" + MindwaveManager.Instance.Calibrator.EvaluateRatio(Brainwave.HighBeta, m_MindwaveData.eegPower.highBeta) + ";" + MindwaveManager.Instance.Calibrator.EvaluateRatio(Brainwave.LowGamma, m_MindwaveData.eegPower.lowGamma) + ";" + MindwaveManager.Instance.Calibrator.EvaluateRatio(Brainwave.HighGamma, m_MindwaveData.eegPower.highGamma) + ";" + m_MindwaveData.eSense.attention + ";" + Player.speed + ";" + Player.currentLife + ";" + Player.player.coins + ";" + Player.player.score + ";" + Player.timeCounter + "\n";

                string content = run + ";" + m_MindwaveData.eegPower.lowBeta.ToString() + ";" + m_MindwaveData.eegPower.highBeta.ToString() + ";" + m_MindwaveData.eSense.attention.ToString() + ";" + Player.speed + ";" + Player.currentLife + ";" + Player.player.coins + ";" + Player.player.score + ";" + Player.timeCounter + "\n";

                File.AppendAllText(path, content);

                attentionTotal += m_MindwaveData.eSense.attention;
                registers++;
            }
        } 
    
        else if(control == 2) {
            string path = folderPath + "/" + Login.userName + "-RewardLog_" + System.DateTime.Now.ToString("dd-MM-yy") + ".txt";
            if (!File.Exists(path)) {
                File.WriteAllText(path, "reward;delta;theta;lowAlpha;HighAlpha;lowBeta;highBeta;lowGamma;highGamma;attention;currentProgress;timestamp(s)\n");
                MindwaveHandler.newActivity = false;
                reward = 1;
            } else if(MindwaveHandler.newActivity == true) {
                MindwaveHandler.newActivity = false;
                reward = ReadLastLine(path);
            }
            if(ProgressBar.current > 100) {
                ProgressBar.current = 100;
            }
            if (m_MindwaveData.eegPower.delta > 0 && Player.timeCounter > 0 && ProgressBar.current <= 100)  {
                //dataControl = m_MindwaveData.eegPower.delta;
                string content = reward + ";" + MindwaveManager.Instance.Calibrator.EvaluateRatio(Brainwave.Delta, m_MindwaveData.eegPower.delta) + ";" + MindwaveManager.Instance.Calibrator.EvaluateRatio(Brainwave.Theta, m_MindwaveData.eegPower.theta) + ";" + MindwaveManager.Instance.Calibrator.EvaluateRatio(Brainwave.LowAlpha, m_MindwaveData.eegPower.lowAlpha) + ";" + MindwaveManager.Instance.Calibrator.EvaluateRatio(Brainwave.HighAlpha, m_MindwaveData.eegPower.highAlpha) + ";" + MindwaveManager.Instance.Calibrator.EvaluateRatio(Brainwave.LowBeta, m_MindwaveData.eegPower.lowBeta) + ";" + MindwaveManager.Instance.Calibrator.EvaluateRatio(Brainwave.HighBeta, m_MindwaveData.eegPower.highBeta) + ";" + MindwaveManager.Instance.Calibrator.EvaluateRatio(Brainwave.LowGamma, m_MindwaveData.eegPower.lowGamma) + ";" + MindwaveManager.Instance.Calibrator.EvaluateRatio(Brainwave.HighGamma, m_MindwaveData.eegPower.highGamma) + ";" + m_MindwaveData.eSense.attention.ToString() + ";" + ProgressBar.current + ";" + Player.timeCounter + "\n";
                File.AppendAllText(path, content);
            }
        }

        else if(control == 3) {
            string path = folderPath + "/" + Login.userName + "-BonusLog_" + System.DateTime.Now.ToString("dd-MM-yy") + ".txt";
            if (!File.Exists(path)) {
                File.WriteAllText(path, "bonus;lowBeta;highBeta;meditation;currentProgress;timestamp(s)\n");
                MindwaveHandler.newActivity = false;
                bonus = 1;
            } else if(MindwaveHandler.newActivity == true) {
                MindwaveHandler.newActivity = false;
                bonus = ReadLastLine(path);
                ProgressBar.current = 0;
            }
            if(ProgressBar.current > 100) {
                ProgressBar.current = 100;
            }
            if (m_MindwaveData.eegPower.delta > 0 && ProgressBar.current <= 100)  {
                //dataControl = m_MindwaveData.eegPower.delta;
                string content = bonus + ";" + m_MindwaveData.eegPower.lowBeta.ToString() + ";" + m_MindwaveData.eegPower.highBeta.ToString() + ";" + m_MindwaveData.eSense.meditation.ToString() + ";" + ProgressBar.current + ";" + Player.timeCounter + "\n";
                File.AppendAllText(path, content);
            }
        }
    } 

    // Start is called before the first frame update
    void Start() { 
        // Create Log's folder if not exists
        folderPath = Application.dataPath + "/Users/" + Login.userName + "/Logs";
        if(!Directory.Exists(folderPath)){
            Directory.CreateDirectory(folderPath);
        }

        if(Menu.sceneControl == 1) {
            textControl = 1;
            player = FindObjectOfType<Player>();
        }
        else if(Menu.sceneControl == 2) textControl = 2;
        else if(Menu.sceneControl == 3) textControl = 3;
    }

    // Update is called once per frame
    void Update() {
        MindwaveManager.Instance.Controller.OnUpdateMindwaveData += OnUpdateMindwaveData;
        CreateText(textControl);
        
        if(Menu.sceneControl == 1 || Menu.sceneControl == 2 || Menu.sceneControl == 3) DontDestroyOnLoad(gameObject);
        else Destroy(gameObject);
    }

    public void OnUpdateMindwaveData(MindwaveDataModel _Data) {
        m_MindwaveData = _Data;
    }

    public int ReadLastLine(string path) {
        string lastLine = File.ReadLines(path).Last();
        int value = (int)Char.GetNumericValue(lastLine[0]) + 1;
        return value;
    }
}
