using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VSync : MonoBehaviour
{
    void Awake()
    {
        Application.targetFrameRate = 60;
    }
}
