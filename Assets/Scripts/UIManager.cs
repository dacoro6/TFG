using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Button startHostButton;
    [SerializeField]
    private Button startClientButton;
    [SerializeField]
    private TMP_InputField ip;

    public string ipAddress = "127.0.0.1";

    UnityTransport transport;

    private void Awake()
    {
        Cursor.visible = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            ipAddress = ip.text;
            ip.text = "";
        }
    }

    private void Start()
    {
        startHostButton.onClick.AddListener(() =>
        {
            transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
            transport.SetConnectionData(ipAddress,6568);
            if(NetworkManager.Singleton.StartHost())
            {
                
            }
            else
            {

            }
        });

        startClientButton.onClick.AddListener(() =>
        {
            transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
            transport.SetConnectionData(ipAddress,6568);
             if(NetworkManager.Singleton.StartClient())
            {

            }
            else
            {

            }
  
        });

         
    }
}