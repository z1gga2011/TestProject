using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GlobalSettings", menuName = "ScriptableObjects/Settings", order = 1)]
public class Settings : ScriptableObject
{
    public float AmbientVolume;
    public float FXVolume;
}
