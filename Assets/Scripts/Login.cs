using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    // Global variable 
    public static string userName;

    public InputField inputUser;
    public GameObject panelLogin;

    private void Start() {
        panelLogin.SetActive(false);
        // Login button
        Button btnLogin = GameObject.Find("Login Button").GetComponent<Button>();
        btnLogin.onClick.AddListener(GetUserNameOnClick);
    }

    private void GetUserNameOnClick() {
        userName = inputUser.text;
        LoginHandler();
    }

    private void LoginHandler() {
        panelLogin.SetActive(true);

        Text userNameDisplay = GameObject.Find("Username Text").GetComponent<Text>();
        userNameDisplay.text = userName;

        Button btnCancel = GameObject.Find("Cancel Button").GetComponent<Button>();
        btnCancel.onClick.AddListener(Start);

        Button btnConfirm = GameObject.Find("Confirm Button").GetComponent<Button>();
        btnConfirm.onClick.AddListener(EnterGame);
    }

    private void EnterGame() {
        Debug.Log("Você fez login como " + userName);
        this.gameObject.SetActive(false);
        SceneManager.LoadScene("Menu");
    }
}
