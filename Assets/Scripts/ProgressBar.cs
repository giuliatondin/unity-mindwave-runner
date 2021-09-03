using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ProgressBar : MonoBehaviour {

    // Attention variables
    private float attention = 0; // Attention value to set the progress
    private float attentionAux;

    // Meditation variables
    private float meditation = 0;
    private float meditationAux;
    
    // Control the progress of the bar regarding attention or meditation
    private bool progressControl = false; 

    // Progress bar variables
    public Image mask; // Background image used in progress bar
    private float maximum = 100; // Maximum value of progress bar
    [HideInInspector]
    public static float current; // Current value of progress bar
    [HideInInspector]
    public static bool getMaximum;
    public Text progressText;
    public Sprite decrementProgress;
    public Sprite incrementProgress;
    public Sprite successProgress;
    private  bool getProgress; 

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
        getMaximum = false;
        getProgress = true;
        current = 0;

        float fillAmount = 0;
        mask.fillAmount = fillAmount;

        btnCollect.SetActive(false);

        missionControl = Menu.missionIndex;

        imageParent = GameObject.Find("Progress Bar").GetComponent<Image>();
        imageChild = imageParent.GetComponentsInChildren<Image>();

        Text description = GameObject.Find("Scene Description").GetComponent<Text>();
        if(Menu.sceneControl == 2) description.text = "Mantenha o foco para completar a barra de progresso e pegar sua recompensa!";
        else if(Menu.sceneControl == 3) description.text = "Mantenha-se relaxado";
    }

    // Update is called once per frame
    void Update() {
        if(Menu.sceneControl == 2 || Menu.sceneControl == 3) MindwaveManager.Instance.Controller.OnUpdateMindwaveData += OnUpdateMindwaveData;
    }

    // Calculate progress
    void GetCurrentFill() {
        if(getProgress) {
            if(current >= maximum) {
                imageChild[2].sprite = successProgress;
                progressText.text = "Parabéns, você conseguiu! Colete sua recompensa apertando no botão abaixo";
                mask.fillAmount = maximum;
                btnCollect.SetActive(true); 
            } else {
                if(Menu.sceneControl == 2) {
                    GetCurrentAttention();
                } else if(Menu.sceneControl == 3) {
                    GetCurrentMeditation();
                }
                float fillAmount = current/maximum;
                mask.fillAmount = fillAmount;
            }
        } 
    }

    // Update attention data
    public void OnUpdateMindwaveData(MindwaveDataModel _Data) {
        m_MindwaveData = _Data;
        if(Menu.sceneControl == 2) {
            attentionAux = m_MindwaveData.eSense.attention;
            if(!progressControl) {
                attention = m_MindwaveData.eSense.attention;
                progressControl = true;
            } else {
                if(attentionAux != attention) {
                    attention = m_MindwaveData.eSense.attention;
                    if(!getMaximum) {
                        GetCurrentFill();
                    }
                }
            }
        } else if(Menu.sceneControl == 3) {
            meditationAux = m_MindwaveData.eSense.meditation;
            if(!progressControl) {
                meditation = m_MindwaveData.eSense.meditation;
                progressControl = true;
            } else {
                if(meditationAux != meditation) {
                    meditation = m_MindwaveData.eSense.meditation;
                    if(!getMaximum) {
                        GetCurrentFill();
                    }
                }
            }
        }
    }

    // Get current attention to fill the progress bar
    void GetCurrentAttention() {
        Debug.Log("Atenção: " + attention);
        if(!getMaximum) {
            if(attention > 20) {
                progressText.text = "Muito bem, continue focando na barra";
                imageChild[2].sprite = incrementProgress;
                current += 5;
            } else {
                progressText.text = "Volte a focar no crescimento da barra";
                if(current > 0) {
                    current -= 1;
                    imageChild[2].sprite = decrementProgress;
                }
            }
        } else {
            current = maximum; 
        } 
    }

    // Get current meditation to fill the progress bar
    void GetCurrentMeditation() {
        if(!getMaximum) {
            if(meditation > 50) {
                progressText.text = "Muito bem, continue relaxando";
                imageChild[2].sprite = incrementProgress;
                current += 5;
            } else {
                progressText.text = "Volte a relaxar";
                if(current > 0) {
                    current -= 1;
                    imageChild[2].sprite = decrementProgress;
                }
            }
        } else {
            current = maximum; 
        } 
    }

    // Earn reward in menu call if getMaxium value in progress bar
    public void CollectReward() {
        if(Menu.sceneControl == 3) BonusBase.getBonus = true;
        else if(Menu.sceneControl == 2) getMaximum = true;
        CallMenu();
    }

    public void CallMenu() {
        getProgress = false;
        GameManager.gm.EndRun();
    }
}
