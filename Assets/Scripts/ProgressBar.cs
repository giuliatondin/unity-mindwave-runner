using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[ExecuteInEditMode()]
public class ProgressBar : MonoBehaviour
{
    // Attention variables
    private float attention = 0; // Attention value to set the progress
    private bool progressControl = false; // Control the progress of the bar regarding attention
    private float attentionAux;

    // Progress bar variables
    public Image mask; // Background image used in progress bar
    private float maximum = 100; // Maximum value of progress bar
    private float current = 5; // Current value of progress bar
    public Sprite decrementProgress;
    public Sprite incrementProgress;

    // Mindwave controller variables
    private MindwaveDataModel m_MindwaveData;

    // Start is called before the first frame update
    void Start() {
        float fillAmount = 0;
        mask.fillAmount = fillAmount;
    }

    // Update is called once per frame
    void Update() {
        MindwaveManager.Instance.Controller.OnUpdateMindwaveData += OnUpdateMindwaveData;
    }

    // Calculate progress
    void GetCurrentFill() {
        if(current >= maximum) {
            //Debug.Log("Chegou ao limite!"); // TODO: Colocar botão para coletar recompensa
            SceneManager.LoadScene("Menu");
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
        if(attention > 60) {
            Debug.Log("Boa, atenção em: " + attention); //TODO: Colocar texto para atualizar para o usuário saber o que precisa fazer
            GetComponent<Image>().color = Color.white;
            //mask.sprite = incrementProgress;
            current += 5;
        } else {
            Debug.Log("Aumente, atenção em: " + attention);
            if(current > 5) {
                current -= 1; //TODO: mudar cor quando aumenta e abaixa e sprite do progress bar
                GetComponent<Image>().color = Color.red;
                //Image test = GameObject.Find("Mask").GetComponent<Image>();
                //test.sprite = decrementProgress;
            }
        }
    }
}
