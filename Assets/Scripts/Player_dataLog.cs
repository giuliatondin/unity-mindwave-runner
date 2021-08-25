using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

// When not in nightrun or focustoreward scene, don't record data

public class Player_dataLog : MonoBehaviour
{
    private string folderPath;

    private MindwaveDataModel m_MindwaveData;
    private MindwaveDataModel _Data;
    int dataControl = 0;
    int textControl = 0; // 1 - run, 2 - reward

    void CreateText(int control) {
        if(control == 1) {
            // Path of the file
            string path = folderPath + "/" + Login.userName + "-RunLog_" + System.DateTime.Now.ToString("dd-MM-yy") + ".txt";
            // Check if file exist
            if (!File.Exists(path))
            {
                // Add data columns in file
                File.WriteAllText(path, "lowBeta highBeta attention velocity\n");
            }
            //Content of the file
            if (m_MindwaveData.eegPower.delta > 0 && m_MindwaveData.eegPower.delta != dataControl && Player.speed > 0)  {
                dataControl = m_MindwaveData.eegPower.delta;
                string content = m_MindwaveData.eegPower.lowBeta.ToString() + " " + m_MindwaveData.eegPower.highBeta.ToString() + " " + m_MindwaveData.eSense.attention.ToString() + " " + Player.speed + " " + "\n";
                File.AppendAllText(path, content);
            }
        } 
        
        // TODO: Nao gravar dados quando não está na cena, registro para cada tentativa de obter recompensa

        else if(control == 2) {
            string path = folderPath + "/" + Login.userName + "-RewardLog_" + System.DateTime.Now.ToString("dd-MM-yy") + ".txt";
            if (!File.Exists(path)) {
                File.WriteAllText(path, "lowBeta highBeta attention currentProgress \n");
            }
            if (m_MindwaveData.eegPower.delta > 0 && m_MindwaveData.eegPower.delta != dataControl && ProgressBar.current <= 100)  {
                dataControl = m_MindwaveData.eegPower.delta;
                string content = m_MindwaveData.eegPower.lowBeta.ToString() + " " + m_MindwaveData.eegPower.highBeta.ToString() + " " + m_MindwaveData.eSense.attention.ToString() + " " + ProgressBar.current + " " + "\n";
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

        if(Menu.sceneControl == 1) textControl = 1;
        else if(Menu.sceneControl == 2) textControl = 2;
    }

    // Update is called once per frame
    void Update()
    {
        MindwaveManager.Instance.Controller.OnUpdateMindwaveData += OnUpdateMindwaveData;
        CreateText(textControl);
    }

    public void OnUpdateMindwaveData(MindwaveDataModel _Data)
    {
        m_MindwaveData = _Data;
    }
}
