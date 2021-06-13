using System;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundTiler : MonoBehaviour
{
    public GameObject backgroundTile;
    
    public int numberOfTilesX = 100;
    public int numberOfTilesY = 100;

    public float spriteWidth = 1;
    public float spriteHeight = 1;
    
    private List<GameObject> _backgroundList = new List<GameObject>();

    public void Start()
    {
        Create();
    }
    
    public void Create()
    {
        for (var x = 0; x < numberOfTilesX; x++)
        {
            for (var y = 0; y < numberOfTilesY; y++)
            {
                var xNorm = x - numberOfTilesX / 2;
                var yNorm = y - numberOfTilesY / 2;
                var positionToPlaceTile = new Vector2(xNorm * spriteWidth, yNorm * spriteHeight);
                var tile = Instantiate(backgroundTile, positionToPlaceTile, Quaternion.identity, transform);
                _backgroundList.Add(tile);
            }
        }
    }

    public void Destroy()
    {
        foreach (var tile in _backgroundList)
        {
            DestroyImmediate(tile);
        }
        _backgroundList = new List<GameObject>();
    }
}