using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchMenu : MonoBehaviour
{
    public void ShowRanks()
    {
        SceneManager.LoadScene(1);
    }

    public void BackToMainPage()
    {
        SceneManager.LoadScene(0);
    }

    public void SwitchPage(int sceneNumber)
    {
        SceneManager.LoadScene(sceneNumber);
    }
}
