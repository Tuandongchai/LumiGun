using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Button StartBtn, ControlsBtn, BackBtn;
    [SerializeField] CanvasGroup FrontUI, ControlsUI;
    [SerializeField] LevelManager levelManager;

    private void Start()
    {
        StartBtn.onClick.AddListener(StartGame);
        ControlsBtn.onClick.AddListener(SwithToControlUI);
        BackBtn.onClick.AddListener(SwitchToFrontUI);
    }

    private void SwitchToFrontUI()
    {
        ControlsUI.blocksRaycasts = false;
        ControlsUI.alpha = 0;

        FrontUI.blocksRaycasts = true;
        FrontUI.alpha = 1;
    }

    private void SwithToControlUI()
    {
        ControlsUI.blocksRaycasts = true;
        ControlsUI.alpha = 1;

        FrontUI.blocksRaycasts = false;
        FrontUI.alpha = 0;
    }

    private void StartGame()
    {
        levelManager.LoadFirstLevel();
    }
} 
