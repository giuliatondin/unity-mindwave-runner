using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Image[] lifeHearts;

    // Variables of coins
    public Text coinText;

    // Game over panel
    public GameObject gameOverPanel;

    // Update points in screen
    public Text scoreText;

    // Update attention in screen
    public Text attentionText;

    // Update speed in screen
    public Text speedText;

    public void UpdateLives(int lives){
        for(int i = 0; i < lifeHearts.Length; i++){
            if(lives > i){
                lifeHearts[i].color = Color.white;
            } else {
                lifeHearts[i].color = Color.black;
            }
        }
    }

    public void UpdateCoins(int coin){
        coinText.text = coin.ToString();
    }

    public void UpdateScore(int score){
        scoreText.text = "Score: " + score + "m";
    }

    public void UpdateAttention(int attention) {
        attentionText.text = "Atenção: " + attention;
    }

    public void UpdateSpeed(int speed) {
        speedText.text = "Velocidade: " + speed;
    }

}
