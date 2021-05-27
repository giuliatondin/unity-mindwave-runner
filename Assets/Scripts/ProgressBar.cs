using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ProgressBar : MonoBehaviour {
    // Attention variables
    private float attention = 0; // Attention value to set the progress
    private bool progressControl = false; // Control the progress of the bar regarding attention
    private float attentionAux;

    // Progress bar variables
    public Image mask; // Background image used in progress bar
    private float maximum = 100; // Maximum value of progress bar
    private float current = 5; // Current value of progress bar
    private bool getMaximum = false;
    public Text progressText;
    public Sprite decrementProgress;
    public Sprite incrementProgress;

    // Button to collect reward
    public GameObject btnCollect;
    public static int missionControl = 0;

    // Mindwave controller variables
    private MindwaveDataModel m_MindwaveData;

    // Start is called before the first frame update
    void Start() {
        float fillAmount = 0;
        mask.fillAmount = fillAmount;

        btnCollect.SetActive(false);

        missionControl = Menu.missionIndex;
    }

    // Update is called once per frame
    void Update() {
        MindwaveManager.Instance.Controller.OnUpdateMindwaveData += OnUpdateMindwaveData;
    }

    // Calculate progress
    void GetCurrentFill() {
        if(current >= maximum) {
            progressText.text = "Parabéns, você conseguiu! Colete sua recompensa apertando no botão abaixo";
            getMaximum = true;
            mask.fillAmount = maximum;
            btnCollect.SetActive(true); 
        } else {
            GetCurrentAttention();
            float fillAmount = current/maximum;
            mask.fillAmount = fillAmount;
        }
    }

    // Update attention data
    public void OnUpdateMindwaveData(MindwaveDataModel _Data)
    {
        m_MindwaveData = _Data;
        attentionAux = m_MindwaveData.eSense.attention;
        if(!progressControl) {
            attention = m_MindwaveData.eSense.attention;
            progressControl = true;
        } else {
            if(attentionAux != attention) {
                attention = m_MindwaveData.eSense.attention;
                GetCurrentFill();
            }
        }
    }

    // Get current attention to fill the progress bar
    void GetCurrentAttention() {
        if(!getMaximum) {
            if(attention > 60) {
                progressText.text = "Muito bem, continue focando na barra";
                GetComponent<Image>().color = Color.white;
                //mask.sprite = incrementProgress;
                current += 5;
            } else {
                progressText.text = "Volte a focar no crescimento da barra";
                if(current > 5) {
                    current -= 1; //TODO: mudar cor quando aumenta e abaixa e sprite do progress bar
                    GetComponent<Image>().color = Color.red;
                    //Image test = GameObject.Find("Mask").GetComponent<Image>();
                    //test.sprite = decrementProgress;
                }
            }
        } else {
            current = maximum; 
            GetComponent<Image>().color = Color.green;
        } 
    }

    public void EndActivity() {
        MindwaveManager.Instance.Controller.Disconnect();
        SceneManager.LoadScene("Menu");
    }
}
