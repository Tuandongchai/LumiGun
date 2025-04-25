using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameMenuUI : MonoBehaviour
{
    [SerializeField] Button resumeBtn, restartBtn, mainMenu;
    [SerializeField] UIManager uiManager;
    [SerializeField] LevelManager levelManager;

    private void Start()
    {
        resumeBtn.onClick.AddListener(ResumeGame);
        restartBtn.onClick.AddListener(RestartLevel);
        mainMenu.onClick.AddListener(BackToMainMenu);
        

    }

    private void RestartLevel()
    {
        levelManager.RestartCurrentLevel();
    }

    private void ResumeGame()
    {
        uiManager.SwithToGameplayUI();
    }

    private void BackToMainMenu()
    {
        levelManager.GoToMainMenu();
    }
}
