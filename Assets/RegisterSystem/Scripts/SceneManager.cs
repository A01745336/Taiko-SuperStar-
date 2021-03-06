using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;

public class SceneManager : MonoBehaviour
{
    #region Login
    [Header("Login Inputs")]
    public InputField m_PasswordInputLogin;
    public InputField m_UserInputLogin;
    #endregion

    #region Register
    [Header("Register Inputs")]
    [SerializeField] private InputField m_Username;
    [SerializeField] private InputField m_Email;
    [SerializeField] private InputField m_Password;
    [SerializeField] private InputField m_ConfirmPassword;
    [SerializeField] private Text m_ErrorText;



    [SerializeField] private GameObject m_PanelLogin;
    [SerializeField] private GameObject m_PanelRegister;
    #endregion
    private NetworkManager m_NetworkManager;
    public Toggle toggleSesion;

    [Header("Direcciones de corre")]
    public string[] Emails;
    public bool cuentaRegistradaConExito;
    public int MaxLenght;


    void Start()
    {
        m_NetworkManager = FindObjectOfType <NetworkManager>();

        if (PlayerPrefs.HasKey("toggleIsOn") == true)
        {
            toggleSesion.isOn = Convert.ToBoolean(PlayerPrefs.GetInt("toggleIsOn"));
            if (toggleSesion.isOn)
            {
                m_PasswordInputLogin.text = PlayerPrefs.GetString("SavePasswordToggle_Data", "");
                m_UserInputLogin.text = PlayerPrefs.GetString("SaveUserToggle_Data", "");

            }
            else
            {
                m_PasswordInputLogin.text =  "";
                m_UserInputLogin.text = "";
            }
        }
    }

    //<summary>
    //Orden para enviar datos
    //user
    //email
    //pass
    //<summary>

    public void submitLogin()
    {

                if (m_PasswordInputLogin.text == "" || m_UserInputLogin.text == "")
                {
                    m_ErrorText.text = "Error 444: Check that no field is empty";
                    return;
                }

                m_NetworkManager.LoginUser(m_UserInputLogin.text, m_PasswordInputLogin.text, delegate (Response response)
                {
                    m_ErrorText.text = "Logging wait a moment...";
                    m_ErrorText.text = response.message;

                    if (response.done)
                    {
                        if (toggleSesion.isOn)
                        {
                            PlayerPrefs.SetString("SavePasswordToggle_Data", m_PasswordInputLogin.text);
                            PlayerPrefs.SetString("SaveUserToggle_Data", m_UserInputLogin.text);
                            var valueSave = Convert.ToInt32 (toggleSesion.isOn);
                            PlayerPrefs.SetInt ("toggleIsOn", valueSave);
                        }

                        //UnityEngine.SceneManagement.SceneManager.LoadScene("LobbyMenu");
                    }
                });
    }

    void blankRegisterSpace()
    {
        m_Username.text = "";
        m_Email.text = "";
        m_Password.text = "";
        m_ConfirmPassword.text = "";
    }

    public void SubmitRegister()
    {
        foreach (string emailSet in Emails)
        {
            if (m_Email.text.Contains(emailSet)) {
                cuentaRegistradaConExito = true;
                if (m_Username.text == "" || m_Email.text == "" || m_Password.text == "" || m_ConfirmPassword.text == "")
                {
                    m_ErrorText.text = "Error 444: Check that no field is empty";
                    return;
                }

                if (m_Password.text == m_ConfirmPassword.text)
                {
                    if (m_Password.text.Length >= MaxLenght) {
                        m_ErrorText.text = "Processing information please wait a moment";
                        m_NetworkManager.SubmitRegister(m_Username.text, m_Email.text, m_Password.text, delegate (Response response)
                        {

                            m_ErrorText.text = response.message;
                            if (response.done == true)
                            {
                                ////ESCRIBIR CODIGO DE ACEPTACION AL REGISTRARSE
                                cuentaRegistradaConExito = false;
                                blankRegisterSpace();
                                print("Account Created successfully");
                            }
                            else
                            {
                                ///ACCION AL NO REGISTRAR CUENTA
                                
                            }
                        });
                    }
                    else
                    {
                        m_ErrorText.text = "Your password must contain at least 8 characters";
                    }
                }
                else
                {
                    m_ErrorText.text = "Error 565: There are data that are not similar, try again";
                    return;
                }
            }

            if (!m_Email.text.Contains (emailSet) && !cuentaRegistradaConExito)
            {
                    m_ErrorText.text = "Error 877: We're sorry, but a valid email cannot be read.";
            }
        }
    }
}