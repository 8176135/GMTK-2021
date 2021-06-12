using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thruster : MonoBehaviour
{
    private MainBlock mainBlock;

    // Start is called before the first frame update
    void Start()
    {
        mainBlock = this.GetComponent<MainBlock>();
        mainBlock.connectedToParent.AddListener(c => c.NewThruster(this));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
