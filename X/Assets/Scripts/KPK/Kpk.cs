using AudioSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kpk : MonoBehaviour
{
    public static Kpk instance;

    public List<KpkInfoButton> KpkDataButtons;
    public List<KpkInfoButton> KpkJournalButtons;
    public List<KpkInfoButton> AllData;
    public List<KpkInfoButton> AllJournal;

    public AudioClip AddDataSound;
    public AudioClip AddTaskSound;

    [SerializeField] private KpkInfoPannel infoPanel;
    [SerializeField] private Transform KpkDataPanel;
    [SerializeField] private Transform KpkJournalPanel;
    [SerializeField] private Transform KpkTaskPanel;
    //[SerializeField] private KpkMapPanel mapPanel;
    private void Awake()
    {
        instance = this;

        gameObject.SetActive(false);

        //LoadData();
    }
    public void AddData(KpkInfoButton data)
    {
        bool checkExtistData = false;

        for(int i = 0; i < KpkDataButtons.Count; i++)
        {
            if (data.id == KpkDataButtons[i].id)
            {
                checkExtistData = true;
            }
        }

        if(!checkExtistData)
        {
            KpkInfoButton newData = Instantiate(data);
            newData.transform.SetParent(KpkDataPanel);
            newData.transform.localScale = new Vector3(1, 1, 1);

            KpkDataButtons.Add(newData);
            AudioManager.instance.PlayFX(AddDataSound, false);

            UpdateData();
        }    
    }
    public void AddJournal(KpkInfoButton data)
    {
        bool checkExtistJournal = false;

        for (int i = 0; i < KpkJournalButtons.Count; i++)
        {
            if (data.id == KpkJournalButtons[i].id)
            {
                checkExtistJournal = true;
            }
        }

        if (!checkExtistJournal)
        {
            KpkInfoButton newJournal = Instantiate(data);
            newJournal.transform.SetParent(KpkJournalPanel);
            newJournal.transform.localScale = new Vector3(1, 1, 1);

            KpkJournalButtons.Add(newJournal);
            AudioManager.instance.PlayFX(AddDataSound, false);

            UpdateData();
        }
    }
    public void AddTask(KpkTaskButton task)
    {
        KpkTaskButton newTask = Instantiate(task);
        newTask.transform.SetParent(KpkTaskPanel);
        newTask.transform.localScale = new Vector3(1, 1, 1);

        AudioManager.instance.PlayFX(AddTaskSound, false);
    }
    public void AddTaskProgress(string taskName)
    {
        for(int i = 0; i < KpkTaskPanel.childCount; i++)
        {
            if(KpkTaskPanel.GetChild(i).GetComponent<KpkTaskButton>().TaskName ==  taskName)
            {
                KpkTaskPanel.GetChild(i).GetComponent<KpkTaskButton>().AddProgress();
            }
        }
    }
    public void OpenInfoPanel(string fileName, string fileInfo)
    {
        infoPanel.gameObject.SetActive(true);
        infoPanel.SetText(fileName, fileInfo);
    }
    public void Save()
    {
        SaveJournal();
        SaveData();
    }
    public void Load()
    {
        LoadJournal();
        LoadData();
    }
    #region
    private void UpdateData()
    {
        for(int i = 0; i < KpkDataButtons.Count; i++)
        {
            if (KpkDataButtons[i] == null)
            {
                Destroy(KpkDataButtons[i].gameObject);
                KpkDataButtons.Remove(KpkDataButtons[i]);
            }
        }
        for (int i = 0; i < KpkJournalButtons.Count; i++)
        {
            if (KpkJournalButtons[i] == null)
            {
                Destroy(KpkJournalButtons[i].gameObject);
                KpkJournalButtons.Remove(KpkJournalButtons[i]);
            }
        }
    }

    private void SaveData()
    {
        int DataLenght = 0;

        for (int i = 0; i < KpkDataButtons.Count; i++)
        {
            if (KpkDataButtons[i] != null)
            {
                PlayerPrefs.SetInt("data" + i, KpkDataButtons[i].id);
                DataLenght++;

                PlayerPrefs.SetInt("dataLenght", DataLenght);

                Debug.Log("data " + i + " sucecfully saved!");
            }
        }
    }
    private void LoadData()
    {
        for (int dataindex = 0; dataindex < KpkDataButtons.Count; dataindex++)
        {
            if (KpkDataButtons[dataindex] != null) { Destroy(KpkDataButtons[dataindex].gameObject); }
        }

        KpkDataButtons.Clear();


        for (int i = 0; i < PlayerPrefs.GetInt("dataLenght"); i++)
        {
            for (int j = 0; j < AllData.Count; j++)
            {
                if (AllData[j].id == PlayerPrefs.GetInt("data" + i))
                {
                    AddLoadData(AllData[j]);

                    Debug.Log("journal " + i + " sucecfully load!");
                }
            }
        }
    }
    public void AddLoadData(KpkInfoButton data)
    {
        KpkInfoButton newData = Instantiate(data);
        newData.transform.SetParent(KpkDataPanel);
        newData.transform.localScale = new Vector3(1, 1, 1);

        KpkDataButtons.Add(newData);
    }

    private void SaveJournal()
    {
        int journalLenght = 0;

        for(int i = 0; i < KpkJournalButtons.Count; i++)
        {
            if(KpkJournalButtons[i] != null)
            {
                PlayerPrefs.SetInt("journal" + i, KpkJournalButtons[i].id);
                journalLenght++;

                PlayerPrefs.SetInt("journalLenght", journalLenght);

                Debug.Log("journal " + i + " sucecfully saved!");
            }
        }
    }
    private void LoadJournal()
    {
        for(int dataindex = 0; dataindex < KpkJournalButtons.Count; dataindex++)
        {
            if(KpkJournalButtons[dataindex] != null) { Destroy(KpkJournalButtons[dataindex].gameObject); }
        }

        KpkJournalButtons.Clear();
 
        for (int i = 0; i < PlayerPrefs.GetInt("journalLenght"); i++)
        {
            for(int j = 0; j < AllJournal.Count; j++)
            {
                if (AllJournal[j].id == PlayerPrefs.GetInt("journal" + i))
                {
                    AddLoadJournal(AllJournal[j]);

                    Debug.Log("journal " + i + " sucecfully load!");
                }
            }
        }
    }
    public void AddLoadJournal(KpkInfoButton data)
    {
        KpkInfoButton newJournal = Instantiate(data);
        newJournal.transform.SetParent(KpkJournalPanel);
        newJournal.transform.localScale = new Vector3(1, 1, 1);

        KpkJournalButtons.Add(newJournal);
    }
    #endregion
}
