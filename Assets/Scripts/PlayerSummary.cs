using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System;

public class PlayerSummary : MonoBehaviour {
    private static string folderPath;

    static void FolderControl() {
        folderPath = Application.dataPath + "/Users/" + Login.userName + "/Logs/Summary";
        if(!Directory.Exists(folderPath)) {
            Directory.CreateDirectory(folderPath);
        }
    }

    public static void RunSummary(string runTime, float score, int coins) {
        FolderControl();
        string path = folderPath + "/" + Login.userName + "-SummaryRun_" + System.DateTime.Now.ToString("dd-MM-yy") + ".txt";
        if (!File.Exists(path)) {
            File.WriteAllText(path, "numRun;runTime(s);distance(m);coins;attentionMean\n");
        } 
        string content = Player_dataLog.run + ";" + runTime + ";" + (int)score + ";" + coins + ";" + Math.Ceiling(Player_dataLog.attentionTotal/Player_dataLog.registers) + "\n";
        File.AppendAllText(path, content);
    }

    void RewardSummary() {

    }

    void BonusSummary() {

    }
}
