﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private NetworkManagerLobby _networkManager;

    [Header("UI")]
    [SerializeField] private GameObject _landingPagePanel;

    public void HostLobby()
    {
        _networkManager.StartHost();

        _landingPagePanel.SetActive(false);
    }
}


