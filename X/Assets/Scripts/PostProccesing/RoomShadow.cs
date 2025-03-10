using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(SpriteRenderer))]
public class RoomShadow : MonoBehaviour
{
    public GameObject Light;
    public GameObject Shadows;

    private SpriteRenderer Shadow = null;

    /*[HideInInspector] */public bool isVisible = true;

    void Start()
    {
        Shadow = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        SwitchVisible();
    }
    private void SwitchVisible()
    {
        var color = Shadow.color;

        if (isVisible)
        {
            color.a += 100 * Time.fixedDeltaTime;
            color.a = Mathf.Clamp(color.a, 0, 1);
            if (Shadow != null) Shadow.color = color;
            Shadow.color = color;

            if(Light != null) Light.SetActive(false);
            if (Shadows != null) Shadows.SetActive(false);
        }
        else
        {
            color.a -= 100 * Time.fixedDeltaTime;
            color.a = Mathf.Clamp(color.a, 0, 1);
            if (Shadow != null) Shadow.color = color;
            Shadow.color = color;

            if (Light != null) Light.SetActive(true);
            if (Shadows != null) Shadows.SetActive(true);
        }
    }
}
