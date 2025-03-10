using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KpkInfoButton : MonoBehaviour
{
    public string fileName;
    [Multiline(10)] public string fileInfo;

    public int id;

    private void Awake()
    {
        transform.GetChild(0).GetComponent<Text>().text = fileName;
    }
    public void OpenInfoPanel()
    {
        Kpk.instance.OpenInfoPanel(fileName, fileInfo);
    }
}
