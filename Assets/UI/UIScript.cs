using catHomerun.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIScript : MonoBehaviour//SingletonGlobalSelfInstancing<UIScript>
{
    [Serializable]
    public enum CharacterChoice
    {
        Cat,
        Otter,
        Fox
    }

    public TileSpawner tileSpawner;

    public Button exitBtn;
    public Button singleplayerBtn;
    public Button multiplayerBtn;
    public int currentPlayerCharacterChoice = 1;

    public int playercount = 1;

    public GameObject modeUI;
    public GameObject configUI;

    public TMPro.TextMeshProUGUI playerText;

    public void Start()
    {
        //configUI.SetActive(false);
        //characterImageP2.enabled = false;

        tileSpawner.Spawn();
    }

    //public void ChangeCharacter(string character)
    //{
    //    if (!GameManager.instance.multiplayerActive)
    //    {
    //        this.player1character = (CharacterChoice)Enum.Parse(typeof(CharacterChoice), character);
    //    }
    //    else
    //    {
    //        if(currentPlayerCharacterChoice == 1)
    //        {
    //            this.player1character = (CharacterChoice)Enum.Parse(typeof(CharacterChoice), character);
    //            currentPlayerCharacterChoice = 2;
    //            playerText.text = "Player 2";
    //        }else if (currentPlayerCharacterChoice == 2)
    //        {
    //            this.player2character = (CharacterChoice)Enum.Parse(typeof(CharacterChoice), character);
    //        }
    //    }

    //}

    public void SingleplayerClicked()
    {
        //configUI.SetActive(true);
        //modeUI.SetActive(false);
        GameManager.instance.playerCount = playercount;
    }

    public void MultiplayerClicked(int playerCount)
    {
        //configUI.SetActive(true);
        //modeUI.SetActive(false);
        //characterImageP2.enabled = true;
        this.playercount = playerCount;
        GameManager.instance.playerCount = playerCount;
        GameManager.instance.EnableMultiplayer();
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void StartLevel()
    {
        GameManager.instance.LoadLevel();
    }

}
