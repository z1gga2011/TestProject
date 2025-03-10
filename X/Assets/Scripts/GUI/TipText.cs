using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TipText : MonoBehaviour
{
    private TextMeshPro tipText;

    private void Awake()
    {
        tipText = transform.GetChild(0).GetComponent<TextMeshPro>();

        Invoke("DestroyTipText", 2f);
    }
    public void SetText(string text)
    {
        tipText.text = text;
    }
    public void DestroyTipText()
    {
        Destroy(gameObject);
    }
}
