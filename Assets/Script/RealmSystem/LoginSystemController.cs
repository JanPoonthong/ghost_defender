using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoginSystemController : MonoBehaviour
{
    public GameObject _loginPage;
    public GameObject _registerPage;
    public GameObject _registerWithGoogle;

    [Header("Login Input")]
    public GameObject _loginEmail;
    public GameObject _loginPassword;
    public GameObject _ErrorMessageLogin;

    [Header("Register Input")]
    public GameObject _RegisterEmail;
    public GameObject _RegisterPassword;
    public GameObject _RegisterPasswordConfirm;
    public GameObject _ErrorMessageRegister;


    [Header("SetPlayerName Input")]
    public GameObject _PlayerName;
    public GameObject _ErrorMessagePlayerName;


    static public LoginSystemController Instance;

    private void Awake()
    {
        _loginPage.SetActive(true);
        _registerPage.SetActive(false);
        _registerWithGoogle.SetActive(false);

        Instance = this;
    }

    public void Login()
    {
        RemoveallTextInput();
        _loginPage.SetActive(true);
        _registerPage.SetActive(false);
        _registerWithGoogle.SetActive(false);
    }

    public void Register()
    {
        RemoveallTextInput();
        _loginPage.SetActive(false);
        _registerPage.SetActive(true);
        _registerWithGoogle.SetActive(false);
    }

    public void SetPlayername()
    {
        _loginPage.SetActive(false);
        _registerPage.SetActive(false);
        _registerWithGoogle.SetActive(true);
    }

    public void setErrorMessageLogin(string message)
    {
        _ErrorMessageLogin.GetComponent<TextMeshProUGUI>().text = message;
    }

    public void setErrorMessageRegister(string message)
    {
        _ErrorMessageRegister.GetComponent<TextMeshProUGUI>().text = message;
    }

    public void setErrorMessagePlayerName(string message)
    {
        _ErrorMessagePlayerName.GetComponent<TextMeshProUGUI>().text = message;
    }

    public (string email, string password)? GetInputPlayerLogin()
    {
        string email = _loginEmail.GetComponent<TMP_InputField>().text;
        string password = _loginPassword.GetComponent<TMP_InputField>().text;

        if (string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(password))
        {
            setErrorMessageLogin("All fields must be filled out.");
            return null;
        }

        return (email, password);
    }

    public (string registerEmail, string registerPassword)? GetInputPlayerRegister()
    {
        string registerEmail = _RegisterEmail.GetComponent<TMP_InputField>().text;
        string registerPassword = _RegisterPassword.GetComponent<TMP_InputField>().text;
        string registerPasswordconfirm = _RegisterPasswordConfirm.GetComponent<TMP_InputField>().text;

        if (string.IsNullOrWhiteSpace(registerEmail) ||
            string.IsNullOrWhiteSpace(registerPassword) ||
            string.IsNullOrWhiteSpace(registerPasswordconfirm))
        {
            setErrorMessageRegister("All fields must be filled out.");
            return null;
        }

        if (registerPassword == registerPasswordconfirm)
        {
            return (registerEmail, registerPassword);
        }
        else
        {
            setErrorMessageRegister("Passwords do not match.");
            return null;
        }
    }

    public string GetInputPlayerName()
    {
        string playername = _PlayerName.GetComponent<TMP_InputField>().text;

        if (string.IsNullOrWhiteSpace(playername))
        {
            setErrorMessagePlayerName("All fields must be filled out.");
            return null;
        }

        return playername;
    }

    private void RemoveallTextInput()
    {
        _loginEmail.GetComponent<TMP_InputField>().text = "";
        _loginPassword.GetComponent<TMP_InputField>().text = "";
        _RegisterEmail.GetComponent<TMP_InputField>().text = "";
        _RegisterPassword.GetComponent<TMP_InputField>().text = "";
        _RegisterPasswordConfirm.GetComponent<TMP_InputField>().text = "";
    }
}
