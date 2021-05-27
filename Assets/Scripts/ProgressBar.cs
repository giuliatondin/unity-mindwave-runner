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
    private float current = 15; // Current value of progress bar
    private bool getMaximum = false;
    public Text progressText;
    public Sprite decrementProgress;
    public Sprite incrementProgress;
    public Sprite successProgress;

    // Button to collect reward
    public GameObject btnCollect;
    public static int missionControl = -1;

    // Mindwave controller variables
    private MindwaveDataModel m_MindwaveData;


    // Variables for change mask sprite of progress bar
    public Image[] imageChild;
    public Image imageParent;

    // Start is called before the first frame update
    void Start() {
        float fillAmount = 0;
        mask.fillAmount = fillAmount;

        btnCollect.SetActive(false);

        missionControl = Menu.missionIndex;

        Debug.Log("Index da missão: " + missionControl);

        imageParent = GameObject.Find("Progress Bar").GetComponent<Image>();
        imageChild = imageParent.GetComponentsInChildren<Image>();
    }

    // Update is called once per frame
    void Update() {
        MindwaveManager.Instance.Controller.OnUpdateMindwaveData += OnUpdateMindwaveData;
    }

    // Calculate progress
    void GetCurrentFill() {
        if(current >= maximum) {
            imageChild[2].sprite = successProgress;
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
                imageChild[2].sprite = incrementProgress;
                current += 5;
            } else {
                progressText.text = "Volte a focar no crescimento da barra";
                if(current > 5) {
                    current -= 1;
                    imageChild[2].sprite = decrementProgress;
                }
            }
        } else {
            current = maximum; 
        } 
    }

    public void EndActivity() {
        MindwaveManager.Instance.Controller.Disconnect();
        SceneManager.LoadScene("Menu");
    }
}
