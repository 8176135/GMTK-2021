using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public Transform rendererTransform;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Aim(Vector2 target)
    {
        var rotation = Vector2.SignedAngle(Vector2.up, target - new Vector2(transform.localPosition.x, transform.localPosition.y));
        rendererTransform.localRotation = Quaternion.RotateTowards(rendererTransform.localRotation, Quaternion.Euler(0,0,rotation), 4f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
