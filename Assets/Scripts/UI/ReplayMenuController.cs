using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReplayMenuController : MonoBehaviour
{
    public void Replay()
    {
        SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
    }
}
