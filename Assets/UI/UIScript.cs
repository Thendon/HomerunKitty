using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    [Serializable]
    public enum CharacterChoice
    {
        Cat,
        Otter
    }

    public Button exitBtn;
    public Button singleplayerBtn;
    public Button multiplayerBtn;
    public Image characterImageP1;
    public Image characterImageP2;
    public CharacterChoice player1character;
    public CharacterChoice player2character;
    public int currentPlayerCharacterChoice = 1;

    public GameObject modeUI;
    public GameObject configUI;

    public TMPro.TextMeshProUGUI playerText;

    public void Start()
    {
        configUI.SetActive(false);
        characterImageP2.enabled = false;
    }

    public void ChangeCharacter(string character)
    {
        if (!GameManager.instance.multiplayerActive)
        {
            this.player1character = (CharacterChoice)Enum.Parse(typeof(CharacterChoice), character);
        }
        else
        {
            if(currentPlayerCharacterChoice == 1)
            {
                this.player1character = (CharacterChoice)Enum.Parse(typeof(CharacterChoice), character);
                currentPlayerCharacterChoice = 2;
                playerText.text = "Player 2";
            }else if (currentPlayerCharacterChoice == 2)
            {
                this.player2character = (CharacterChoice)Enum.Parse(typeof(CharacterChoice), character);
            }
        }

    }

    public void SingleplayerClicked()
    {
        configUI.SetActive(true);
        modeUI.SetActive(false);
    }

    public void MultiplayerClicked()
    {
        configUI.SetActive(true);
        modeUI.SetActive(false);
        characterImageP2.enabled = true;
        
        GameManager.instance.EnableMultiplayer();
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void StartLevel()
    {
           
        GameManager.instance.player1character = player1character;
        GameManager.instance.player2character = player2character;
        GameManager.instance.LoadLevel();
    }

}
