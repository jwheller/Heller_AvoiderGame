using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject menu;
    public GameObject winMenu;

    public void playGame()
    {
        menu.SetActive(false); //Hide the main menu
    }
    public void quitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }
    public void winScreen()
    {
        Debug.Log("You Win!!!");
        winMenu.SetActive(true); //Make the win screen visible
    }
    void Start()
    {
        winMenu.SetActive(false); //Win menu is invisble when the game is started, you gotta earn that shit bro
    }
}
