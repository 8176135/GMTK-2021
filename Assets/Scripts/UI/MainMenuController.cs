using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void Play()
    {
        GetComponent<AudioSource>().Play();
        SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
    }
}
