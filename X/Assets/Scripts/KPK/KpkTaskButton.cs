using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using AudioSystem;

public class KpkTaskButton : MonoBehaviour
{
    public string TaskName;
    [SerializeField] private string TaskInfo;

    [SerializeField] private int curentProgress;
    [SerializeField] private int progress;

    [SerializeField] private UnityEvent CompleteEvent;
    [SerializeField] private AudioClip completeSound;

    private enum Type
    {
        defaultTask, collectableTask
    }

    [SerializeField] private Type TaskType;

    public int id;

    private void Start()
    {
        transform.GetChild(0).GetComponent<Text>().text = TaskName;
    }
    public void OpenInfoPanel()
    {
        if(TaskType == Type.defaultTask)
        {
            Kpk.instance.OpenInfoPanel(TaskName, TaskInfo);
        }
        else
        {
            Kpk.instance.OpenInfoPanel(TaskName, TaskInfo + "\n" + curentProgress + "/" + progress);
        }
    }
    public void AddProgress()
    {
        curentProgress++;
        UpdateProgress();
    }
    public void UpdateProgress()
    {
        if (curentProgress == progress) CompleteTask();
    }
    public void CompleteTask()
    {
        if (CompleteEvent != null) CompleteEvent.Invoke();
        transform.GetChild(0).GetComponent<Text>().text = "(выполнено)";
        AudioManager.instance.PlayFX(completeSound, false);
    }
}
