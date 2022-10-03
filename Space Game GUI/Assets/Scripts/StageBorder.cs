using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu] 
public class StageBorder : ScriptableObject {
    [SerializeField] private Vector2 mMinBorder = new Vector2(0, 0);
    [SerializeField] private Vector2 mMaxBorder = new Vector2(0, 0);

    public Vector2 minBorder => mMinBorder;
    public Vector2 maxBorder => mMaxBorder;
}