using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using catHomerun.Utils;
using System;
using UnityEngine.SceneManagement;

public class GameManager : SingletonGlobalSelfInstancing<GameManager>
{
    public UIScript menu;
    public bool multiplayerActive = false;
    public UIScript.CharacterChoice player1character;
    public UIScript.CharacterChoice player2character;
    public string scene;
    public int p1Points = 0;
    public int p2Points = 0;


    public void LoadLevel()
    {
        SceneManager.LoadScene(scene);
    }

    public void EnableMultiplayer()
    {
        multiplayerActive = true;
    }
}
