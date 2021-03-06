using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thruster : MonoBehaviour
{
    private MainBlock mainBlock;
    public Transform rendererTransform;
    public Rigidbody2D Rigidbody2D;
    public Animator animator;

    private static readonly int Power = Animator.StringToHash("Power");

    // Start is called before the first frame update
    void Start()
    {
        mainBlock = this.GetComponent<MainBlock>();
        Rigidbody2D = this.GetComponent<Rigidbody2D>();
        mainBlock.connectedToParent.AddListener(c => c.NewThruster(this));
    }

    public void SetVisuals(float newRotation, float force)
    {
        animator.SetFloat(Power, force);

        if (force == 0) return;
        var rotation = newRotation - transform.eulerAngles.z;
        rendererTransform.localRotation = Quaternion.RotateTowards(rendererTransform.localRotation, Quaternion.Euler(0,0,rotation), 4f);
    }
}
