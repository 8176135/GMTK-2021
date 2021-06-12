using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This will make the thruster increase in power until max, then decrease in power until 0,
/// and will loop infinitely.
/// </summary>
public class ThrusterAnimationDisplay : MonoBehaviour
{
    [Header("Thruster Animation")]
    public float thrusterPower = 1;
    public float maxThrusterPower = 1;
    public float thrusterRate = 0.01f;

    [Header("Animation Toggle")]
    public bool shouldUpdate = true;

    // Animator
    private Animator _animator;
    private static readonly int Power = Animator.StringToHash("Power");

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!shouldUpdate) return;

        thrusterPower += thrusterRate;
        if(thrusterPower > maxThrusterPower)
        {
            thrusterPower = 0;
        }

        // Update animator
        _animator.SetFloat(Power, thrusterPower);
    }
}
