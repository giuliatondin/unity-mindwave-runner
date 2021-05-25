using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode()]
public class ProgressBar : MonoBehaviour
{
    public int maximum; // Maxium value of progress bar
    public int current; // Current value of progress bar
    public Image mask; // Background image used in progress bar


    // Start is called before the first frame update
    void Start() 
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetCurrentFill();
    }

    // Calculate progress
    void GetCurrentFill() {
        float fillAmount = (float)current/(float)maximum;
        mask.fillAmount = fillAmount;
    }
}
