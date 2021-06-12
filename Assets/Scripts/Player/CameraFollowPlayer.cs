using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    public GameObject player;

    // Update is called once per frame
    void Update()
    {
        if (!player.IsDestroyed())
        {
            transform.SetPositionAndRotation(new Vector3(player.transform.position.x, player.transform.position.y, -10), Quaternion.identity);
        }
    }
}
