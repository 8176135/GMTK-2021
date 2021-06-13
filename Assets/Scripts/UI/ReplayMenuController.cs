using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReplayMenuController : MonoBehaviour
{
    public void TitleScreen()
    {
        GetComponent<AudioSource>().Play();
        SceneManager.LoadScene("TitleScreen", LoadSceneMode.Single);
    }
}
