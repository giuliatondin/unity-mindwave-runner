using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Criar missões para jogo

// Help in mission selection
public enum MissionType {
    SingleRun, TotalMeters, FishesSingleRun
}

// Base script for missions of the game
public abstract class MissionBase : MonoBehaviour
{
    public int max;
    public int progress;
    public int reward;
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

// Mission 1: Run [1000, 2000...] meters in one race
public class SingleRun : MissionBase {
    public override void Created()
    {
        missionType = MissionType.SingleRun;
        int[] maxValues = { 100, 200, 300, 400 };
        int randomMaxValue = Random.Range(0, maxValues.Length);
        int[] rewards = { 100, 200, 300, 400 }; // win coins if complete each meters
        reward = rewards[randomMaxValue];
        max = maxValues[randomMaxValue];
        progress = 0;
    }

    public override string GetMissionDescription() {
        return "Corra " + max + "m em uma corrida";
    }

    public override void RunStart() {
        progress = 0;
        player = FindObjectOfType<Player>();
    }

    public override void Update()
    {
        if (player == null) return;
        progress = (int)player.score;
    }
}

// Mission 2: Run [10000, 20000...] meters in the game
public class TotalMeters : MissionBase
{
    public override void Created()
    {
        missionType = MissionType.TotalMeters;
        int[] maxValues = { 100, 200, 300, 400 };
        int randomMaxValue = Random.Range(0, maxValues.Length);
        int[] rewards = { 100, 200, 300, 400 };
        reward = rewards[randomMaxValue];
        max = maxValues[randomMaxValue];
        progress = 0;
    }

    public override string GetMissionDescription()
    {
        return "Corra " + max + "m no total";
    }

    public override void RunStart()
    {
        progress += currentProgress;
        player = FindObjectOfType<Player>();
    }

    public override void Update()
    {
        if (player == null) return;
        currentProgress = (int)player.score;
    }
}

// Mission 3: Collect [100, 200... ] coins in a race
public class FishesSingleRun : MissionBase
{
    public override void Created()
    {
        missionType = MissionType.FishesSingleRun;
        int[] maxValues = { 100, 200, 300, 400, 500 };
        int randomMaxValue = Random.Range(0, maxValues.Length);
        int[] rewards = { 100, 200, 300, 400, 500 };
        reward = rewards[randomMaxValue];
        max = maxValues[randomMaxValue];
        progress = 0;
    }

    public override string GetMissionDescription()
    {
        return "Colete " + max + " estrelas em uma corrida";
    }

    public override void RunStart()
    {
        progress = 0;
        player = FindObjectOfType<Player>();
    }

    public override void Update()
    {
        if (player == null) return;
        progress = player.coins;
    }
}