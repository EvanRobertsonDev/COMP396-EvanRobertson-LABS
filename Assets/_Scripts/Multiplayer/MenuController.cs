using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{

    [SerializeField] GameObject canvas;
    [SerializeField] Button hostButton;
    [SerializeField] Button joinButton;

    private void Start()
    {
        hostButton.onClick.AddListener(() =>
        {
            if (NetworkManager.Singleton == null) { return; }
            NetworkManager.Singleton.StartHost();
            HideMenu();
        });

        joinButton.onClick.AddListener(() =>
        {
            if (NetworkManager.Singleton == null) { return; }
            NetworkManager.Singleton.StartClient();
            HideMenu();
        });
    }

    void HideMenu()
    {
        canvas.SetActive(false);
    }
}
