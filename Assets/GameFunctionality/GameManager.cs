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
    public string scene;
    public int playerCount;
    public AudioSource ingameSong;

    public void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "GameScene")
        {
            WorldManager worldManager = FindFirstObjectByType<WorldManager>();
            worldManager.Init();

            ingameSong.Play();

            if (playerCount == 1)
                PlayerManager.instance.SpawnPlayer();
            else
                PlayerManager.instance.SpawnPlayers();
        }
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void EnableMultiplayer()
    {
        multiplayerActive = true;
    }

    public void FindNewMenu()
    {
        ingameSong.Stop();
        try
        {
            this.menu = FindObjectOfType<UIScript>();

        }catch(Exception e)
        {
            Debug.LogWarning("No Menu Found " + e);
        }
    }
}
