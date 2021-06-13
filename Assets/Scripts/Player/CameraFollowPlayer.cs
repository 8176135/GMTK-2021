using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    public MainBlock player;
    public Camera camera;

    void Start()
    {
        camera = GetComponent<Camera>();
        player = FindObjectOfType<PlayerController>().GetComponent<MainBlock>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!player.IsDestroyed())
        {
            var position = player.transform.position;
            transform.SetPositionAndRotation(new Vector3(position.x, position.y, -10), Quaternion.identity);
            camera.orthographicSize = player.BlockCount * 0.2f + 5;
        }
    }
}
