using AudioSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class TerminalConsole : MonoBehaviour
{
    [SerializeField] private Kpk mainKpk;

    [Header("Интерфейс консоли")]

    [SerializeField] private Text consoleText;
    [SerializeField] private InputField consoleInput;
    [SerializeField] private GameObject progressBarWindow;
    [SerializeField] private Image progressBar;


    [Header("файлы и команды")]

    [SerializeField] private KpkInfoButton[] _dataFilesForKpk;
    [SerializeField] private KpkInfoButton[] _JournalfilesForKpk;
    [SerializeField] private Folder[] folders;
    [SerializeField] private string[] commands;
    [SerializeField] private string IncorrectCommandError;


    [Header("звуки")]
    [SerializeField] private AudioClip startup;
    [SerializeField] private AudioClip shutdown;
    [SerializeField] private AudioClip click;
    [SerializeField] private AudioClip hddWrite;


    [SerializeField] private UnityEvent _eventAfterLoad;

    private int currentFolderIndex;
    private bool isDataLoaded = false;

    [System.Serializable]
    public struct File
    {
        public bool _isSystem;
        public string name;
        [Multiline(5)] public string info;
    }
    [System.Serializable]
    public struct Folder
    {
        public string name;
        public File[] file;
    }

    void Start()
    {
        currentFolderIndex = -1;
        consoleInput.interactable = false;

        GetComponent<AudioSource>().enabled = false;
    }
    private void OnEnable()
    {
        UpdateText();
        StartCoroutine(StartTerminal());

        consoleInput.onValueChanged.AddListener(delegate { PlayClickSound(); });

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if(player != null)
        {
            player.GetComponent<PlayerMovement>().enabled = false;
            player.GetComponent<Animator>().enabled = false;
        }

        mainKpk = Kpk.instance.GetComponent<Kpk>();
    }
    private void OnDisable()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.GetComponent<PlayerMovement>().enabled = true;
            player.GetComponent<Animator>().enabled = true;
        }
    }
    private IEnumerator StartTerminal()
    {
        AudioManager.instance.PlayFX(startup, false);

        yield return new WaitForSeconds(6f);

        progressBar.fillAmount = 0;
        progressBarWindow.SetActive(true);

        while (true)
        {
            progressBar.fillAmount += Random.Range(0f, 0.2f);
            yield return new WaitForSeconds(0.2f);

            if (progressBar.fillAmount == 1)
            {
                consoleInput.interactable = true;
                consoleInput.ActivateInputField();

                progressBarWindow.SetActive(false);

                consoleText.text = "резервная файловая система готова к использованию \n\nфрагментировано 0(0) из 0(0)";

                GetComponent<AudioSource>().enabled = true;

                yield break;
            }
        }   
    }
    private IEnumerator ShutdownTerminal()
    {
        while (true)
        {
            consoleInput.interactable = false;
            consoleText.text += "\n подготовка файловой системы";
            yield return new WaitForSeconds(0.1f);
            consoleText.text += "\n сохранение.";
            yield return new WaitForSeconds(0.1f);
            consoleText.text += "\n сохранение..";
            yield return new WaitForSeconds(0.1f);
            consoleText.text += "\n сохранение...";
            yield return new WaitForSeconds(0.2f);
            consoleText.text += "\n проверка наличия ошибок..";
            yield return new WaitForSeconds(0.2f);
            consoleText.text += "\n проверка наличия ошибок............";
            yield return new WaitForSeconds(0.1f);
            consoleText.text += "\n проверка наличия ошибок.....................";
            yield return new WaitForSeconds(0.01f);
            consoleText.text += "\n ОК";
            yield return new WaitForSeconds(0.01f);
            consoleText.text += "\n выключение";
            yield return new WaitForSeconds(1f);
            AudioManager.instance.PlayFX(shutdown, false);
            transform.parent.GetComponent<Terminal>().CloseTerminalConsole();
            GetComponent<AudioSource>().enabled = false;
            yield break;
        }
    }
    private IEnumerator LoadFilesToKpk()
    {
        if (mainKpk != null)
        {
            if (_JournalfilesForKpk.Length > 0)
            {
                for (int i = 0; i < _JournalfilesForKpk.Length; i++)
                {
                    yield return new WaitForSeconds(0.1f);
                    consoleText.text += "\n проверка данных... \n";
                    mainKpk.AddJournal(_JournalfilesForKpk[i]);
                    consoleText.text += "данные успешно загружены \n";
                }
            }
            else
            {
                consoleText.text += "данные для загрузки отсутствуют \n";
            }

            if (_dataFilesForKpk.Length > 0)
            {
                for (int i = 0; i < _dataFilesForKpk.Length; i++)
                {
                    yield return new WaitForSeconds(0.1f);
                    consoleText.text += "\n проверка файлов... \n";
                    mainKpk.AddData(_dataFilesForKpk[i]);
                    consoleText.text += "файлы успешно загружены \n";
                }
            }
            else
            {
                consoleText.text += "файлы для загрузки отсутствуют \n";
            }

            isDataLoaded = true;
        }
        else
        {
            ShowError("устройство для записи отсутствует \n");
        }
       
        _eventAfterLoad.Invoke();
        yield break;
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            consoleInput.text.ToLower();
            string[] command = consoleInput.text.Split(" ");

            AudioManager.instance.PlayFX(click, false);

            switch (command[0])
            {
                case "dir":
                    UpdateText();
                    ShowDirectory();
                    break;

                case "help":
                    UpdateText();
                    ShowHelp();
                    break;

                case "cd":
                    UpdateText();
                    if (command.Length > 1) ChangeDirectory(command[1]);
                    else ShowError("указан несуществующий каталог");
                    break;

                case "cd..":
                    UpdateText();
                    LeaveDirectory();
                    break;

                case "type":
                    if (command.Length > 1) ShowText(command[1]);
                    else ShowError("указан несуществующий файл");
                    break;

                case "load":
                    if(!isDataLoaded) StartCoroutine(LoadFilesToKpk());
                    else ShowError("данные уже были загружены");
                    break;

                case "sd":
                    StartCoroutine(ShutdownTerminal());
                    break;

                case "clear":
                    UpdateText();
                    break;

                default:
                    UpdateText();
                    ShowError(IncorrectCommandError);
                    break;
            }

            consoleInput.text = null;
            consoleInput.ActivateInputField();
        }
    }
    private void ShowDirectory()
    {
        if (currentFolderIndex == -1)
        {
            consoleText.text += "диск C:/" + "\n";

            for (int i = 0; i < folders.Length; i++)
            {
                consoleText.text += "\n" + folders[i].name;
                consoleText.text += "\n" + "---------------------------------------";
            }
        }
        else
        {
            if(folders[currentFolderIndex].file.Length < 1)
            {
                consoleText.text += folders[currentFolderIndex].name + "\n";

                ShowError("этот каталог пуст");
            }
            else
            {
                consoleText.text += folders[currentFolderIndex].name + "\n";

                for (int i = 0; i < folders[currentFolderIndex].file.Length; i++)
                {
                    consoleText.text += "\n" + folders[currentFolderIndex].file[i].name + "----" + folders[currentFolderIndex].file[i].info.Length + 20 + " байт";
                    consoleText.text += "\n" + "---------------------------------------";
                }
            }
        }
    }
    private void ChangeDirectory(string directory)
    {
        int startIndex = currentFolderIndex;

        for (int i = 0; i < folders.Length; i++)
        {
            if (directory == folders[i].name.ToLower())
            {
                currentFolderIndex = i;
                ShowDirectory();
            }
        }

        if (currentFolderIndex == startIndex) ShowError("указан несуществующий каталог");
    }
    private void LeaveDirectory()
    {
        currentFolderIndex = -1;
        ShowDirectory();
    }
    private void ShowHelp()
    {
        for (int i = 0; i < commands.Length; i++)
        {
            consoleText.text += "\n" + commands[i];
        }
    }
    private void ShowText(string fileName)
    {
        AudioManager.instance.PlayFX(hddWrite, false);
         
        if(currentFolderIndex != -1)
        {
            bool correctFileFound = false;

            for (int i = 0; i < folders[currentFolderIndex].file.Length; i++)
            {
                if (!folders[currentFolderIndex].file[i]._isSystem && folders[currentFolderIndex].file[i].name == fileName) { consoleText.text = folders[currentFolderIndex].file[i].name + "\n" + folders[currentFolderIndex].file[i].info; correctFileFound = true; }
            }

            if(correctFileFound == false) ShowError("неизвестный тип файла, возможно указан системный файл");
        }
        else
        {
            ShowError("несуществующий файл");
        }
    }
    private void ShowError(string errorMessege)
    {
        consoleText.text += "\n" + errorMessege;
    }
    private void UpdateText()
    {
        AudioManager.instance.PlayFX(hddWrite, false);
        consoleText.text = null;
    }
    private void PlayClickSound()
    {
        AudioManager.instance.PlayFX(click, false);
    }
}
