using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// Base script for bonus of the game
public class BonusBase : MonoBehaviour {

    [HideInInspector]    
    public static int max;
    private int reward;

    [HideInInspector]
    public static bool getBonus = false;

    public int MissionsBonusCheck() {
        return GameManager.gm.bonusMissionsCheck;
    }

    public string MissionsBonusDesc() {
        if(GameManager.gm.bonusMissionsCheck > 1) { 
            return "Você ganhou mais " + GameManager.gm.bonusMissionsRewards + " moedas por completar " + GameManager.gm.bonusMissionsCheck + " missões!";
        } else { 
            return "Você ganhou mais " + GameManager.gm.bonusMissionsRewards + " moedas por completar sua primeira missão!";
        }
    }

    public void GetMissionBonus() {  
        reward = GameManager.gm.bonusMissionsRewards;
        GameManager.gm.coins += reward;
        ProgressBar.missionControl = -1;
    }

    public void RemoveMissionBonus() {
        // GameManager.gm.bonusMissionsCheck = GameManager.gm.bonusMissionsCheck.Skip(1).ToArray();
        // GameManager.gm.bonusMissionsRewards = GameManager.gm.bonusMissionsRewards.Skip(1).ToArray();
        if(GameManager.gm.bonusMissionsCheck == 1) GameManager.gm.bonusMissionsCheck = 5;
        else if(GameManager.gm.bonusMissionsCheck > 5) GameManager.gm.bonusMissionsCheck += 5;
        GameManager.gm.bonusMissionsRewards += 10;
        GameManager.gm.Save();
        getBonus = false;
    }
}