using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thruster : MonoBehaviour
{
    private MainBlock mainBlock;
    public Transform rendererTransform;
    public Animator state;

    private static readonly int Power = Animator.StringToHash("Power");

    // Start is called before the first frame update
    void Start()
    {
        mainBlock = this.GetComponent<MainBlock>();
        mainBlock.connectedToParent.AddListener(c => c.NewThruster(this));
    }

    public void setVisuals(float newRotation, float force)
    {
        state.SetFloat(Power, force);

        if (force == 0) return;
        var rotation = newRotation - transform.eulerAngles.z;
        rendererTransform.localRotation = Quaternion.RotateTowards(rendererTransform.localRotation, Quaternion.Euler(0,0,rotation), 4f);
    }
}
