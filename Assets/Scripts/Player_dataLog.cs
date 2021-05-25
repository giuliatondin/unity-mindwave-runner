using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Player_dataLog : MonoBehaviour
{
    private string folderPath;

    private MindwaveDataModel m_MindwaveData;
    private MindwaveDataModel _Data;
    int dataControl = 0;

    void CreateText()
    {
        // Path of the file
        string path = folderPath + "/" + Login.userName + "-EEGLog_" + System.DateTime.Now.ToString("dd-MM-yy") + ".txt";
        // Check if file exist
        if (!File.Exists(path))
        {
            // Add data columns in file
            File.WriteAllText(path, "attention meditation delta theta velocity\n");
            //File.WriteAllText(path, "attention velocity\n");
        }
        //Content of the file
        if (m_MindwaveData.eegPower.delta > 0 && m_MindwaveData.eegPower.delta != dataControl)
        {
            dataControl = m_MindwaveData.eegPower.delta;
            // Add text to file
            string content = m_MindwaveData.eSense.attention.ToString() + " " + m_MindwaveData.eSense.meditation.ToString() + " " + m_MindwaveData.eegPower.delta.ToString() + " " + m_MindwaveData.eegPower.theta.ToString() + " " + Player.speed + " " + "\n";
            //string content = m_MindwaveData.eSense.attention.ToString() + " " + Player.speed + " " + "\n";
            File.AppendAllText(path, content);
        }
    }

    // Start is called before the first frame update
    void Start() { 
        // Create Log's folder if not exists
        folderPath = Application.dataPath + "/Users/" + Login.userName + "/Logs";
        if(!Directory.Exists(folderPath)){
            Directory.CreateDirectory(folderPath);
        } 
    }

    // Update is called once per frame
    void Update()
    {
        MindwaveManager.Instance.Controller.OnUpdateMindwaveData += OnUpdateMindwaveData;
        CreateText();
    }

    public void OnUpdateMindwaveData(MindwaveDataModel _Data)
    {
        m_MindwaveData = _Data;
    }
}
