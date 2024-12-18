using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public void StartClient()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void Exit() 
    {
        Application.Quit();
    }
}
