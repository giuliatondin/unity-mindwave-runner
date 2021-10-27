using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Help in mission selection
public enum MissionType {
    CollectSingleRun, TimeRun//, TotalMeters
}

// Base script for missions of the game
public abstract class MissionBase : MonoBehaviour
{
    public int max;
    public int progress;
    public int reward;
    public int time;
    public bool timeout;
    public Player player;
    public int currentProgress;
    public MissionType missionType;

    public int[] tracks = {1, 0};

    // This functions are implemented in each mission's script
    public abstract void Created();
    public abstract string GetMissionDescription();
    public abstract void RunStart();
    public abstract void Update();

    public bool GetMissionComplete() {
        if ((progress + currentProgress) >= max) return true;
        else return false;
    }
}

// Mission 1: Collect [100, 200... ] coins in a race
public class CollectSingleRun : MissionBase
{
    public override void Created()
    {
        missionType = MissionType.CollectSingleRun;
        int[] maxValues = { 100, 200, 300, 500 };
        int randomMaxValue = Random.Range(0, maxValues.Length);
        int[] rewards = { 50, 100, 150, 300 };
        reward = rewards[randomMaxValue];
        max = maxValues[randomMaxValue];
        time = 0;
        progress = 0;
    }

    public override string GetMissionDescription()
    {
        return "Colete " + max + " estrelas em uma corrida";
    }

    public override void RunStart()
    {
        progress = 0;
        timeout = false;
        player = FindObjectOfType<Player>();
    }

    public override void Update()
    {
        if (player == null) return;
        progress = player.coins;
    }
}

// Mission 2: Run [1000, 2000...] meters in [30, 50, 100]s
public class TimeRun : MissionBase {
    public override void Created()
    {
        missionType = MissionType.TimeRun;
        int[] maxValues = { 100, 150, 300, 500, 1000 };
        int[] timeLimit = { 10, 20, 40, 60, 120 };
        int[] rewards = { 50, 100, 200, 300, 600 }; 
        int randomMaxValue = Random.Range(0, maxValues.Length);
        reward = rewards[randomMaxValue];
        max = maxValues[randomMaxValue];
        time = timeLimit[randomMaxValue];
        progress = 0;
    }

    public override string GetMissionDescription() {
        return "Corra " + max + " metros em " + time  +" segundos em uma corrida";
    }

    public override void RunStart() {
        progress = 0;
        timeout = false;
        player = FindObjectOfType<Player>();
    }

    public override void Update()
    {
        if (player == null) return;
        if((int)Player.timeCounter <= time) {
            if((int)player.score == max) {
                timeout = false;
                progress = max;
            }
        } else if(progress != max) {
            timeout = true;
        }
    }
}