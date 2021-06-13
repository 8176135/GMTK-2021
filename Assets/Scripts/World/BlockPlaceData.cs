using System;
using UnityEngine;

[Serializable]
public struct BlockPlaceData
{
    public GameObject block;
    public int maxPasses;

    public BlockPlaceData(GameObject block, int maxPasses)
    {
        this.block = block;
        this.maxPasses = maxPasses;
    }
}