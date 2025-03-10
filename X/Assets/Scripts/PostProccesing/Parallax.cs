using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public float paralaxEffect;

    private float length, startPos;
    private Camera main;

    private void Awake()
    {
        startPos = transform.position.x;
        main = Camera.main;

        if (transform.GetComponent<SpriteRenderer>())
        {
            length = transform.GetComponent<SpriteRenderer>().bounds.size.x;
        }
    }
    private void FixedUpdate()
    {
        float dist = main.transform.position.x * paralaxEffect;
        transform.position = new Vector3(startPos + dist, transform.position.y, transform.position.z);
    }
}
