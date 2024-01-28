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
    public UIScript.CharacterChoice player3character;
    public UIScript.CharacterChoice player4character;
    public string scene;
    public int playerCount;
    public AudioClip song;


    public int p1Points = 0;
    public int p2Points = 0;
    public int p3Points = 0;
    public int p4Points = 0;

    public void Start()
    {
        GetComponent<AudioSource>().enabled = true;
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene("GameSceneMarcel");
    }

    public void EnableMultiplayer()
    {
        multiplayerActive = true;
    }

    public void FindNewMenu()
    {
        try
        {
            this.menu = FindObjectOfType<UIScript>();

        }catch(Exception e)
        {
            Debug.LogWarning("No Menu Found " + e);
        }
    }
}
