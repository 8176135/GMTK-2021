using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Player
{
    /// <summary>
    /// When the player dies, the Replay Menu needs to be activated.
    /// </summary>
    public class PlayerDeathChecker: MonoBehaviour
    {
        public GameObject player;
        public GameObject replayMenu;
        public GameObject scoreUI;
        public void Update()
        {
            if (player.IsDestroyed() && !replayMenu.activeSelf)
            {
                replayMenu.SetActive(true);
                scoreUI.SetActive(false);
            }
        }
    }
}