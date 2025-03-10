using AudioSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terminal : MonoBehaviour
{
    [SerializeField] private GameObject terminal;

    public void OpenTerminalConsole()
    {
        terminal.SetActive(true);
    }
    public void CloseTerminalConsole()
    {
        terminal.SetActive(false);
    }
}
