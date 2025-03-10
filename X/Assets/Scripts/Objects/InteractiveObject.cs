using AudioSystem;
using Newtonsoft.Json.Serialization;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InteractiveObject : MonoBehaviour
{
    public Sprite Default, Active; // спрайты стандартный и активный
    [Multiline(2)] public string tipText; // текст подсказки


    [Header("Если подбираемый предмет")]
    [SerializeField] private bool destroyAfterInteract;
    [SerializeField] private string name;
    [SerializeField] private AudioClip pickSound;

    public UnityEvent InteractiveEvent, OnTriggerEvent, ExitTriggerEvent; // события

    private SpriteRenderer _spriteRenderer;
    private DialogSystem.DialogWindow TipWindow;

    private bool readyToInteract;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            OnTriggerEvent.Invoke();

            if (Active != null) _spriteRenderer.sprite = Active;

            if(TipWindow == null && tipText.Length > 0)
            {
                Vector2 InstantiatePos = new Vector2(transform.position.x, transform.position.y + (_spriteRenderer.bounds.size.y / 2) + 1f);

                if (tipText.Length >= 10 && tipText.Length <= 18)
                {
                    TipWindow = Instantiate(Resources.Load<DialogSystem.DialogWindow>("Tip/TipWindow"), InstantiatePos, Quaternion.identity);
                }
                else if (tipText.Length >= 18)
                {
                    TipWindow = Instantiate(Resources.Load<DialogSystem.DialogWindow>("Tip/TipWindow"), InstantiatePos, Quaternion.identity);
                }
                else
                {
                    TipWindow = Instantiate(Resources.Load<DialogSystem.DialogWindow>("Tip/TipWindow"), InstantiatePos, Quaternion.identity);
                }

                TipWindow.SetText(tipText);
            }

            readyToInteract = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        ExitTriggerEvent.Invoke();

        if (Active != null) _spriteRenderer.sprite = Default;

        if (collision.CompareTag("Player") && TipWindow != null)
        {
            if(TipWindow != null)
            {
                TipWindow.GetComponent<Animator>().Play("Fade");
            }
        }

        readyToInteract = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && readyToInteract)
        {
            InteractiveEvent.Invoke();

            if (destroyAfterInteract)
            {
                Vector2 InstantiatePos = new Vector2(transform.position.x, transform.position.y + (GetComponent<SpriteRenderer>().bounds.size.y / 2) + 1f);
                TipText tip = Instantiate(Resources.Load<TipText>("Tip/TipText"), InstantiatePos, Quaternion.identity);
                tip.SetText(name);
                AudioManager.instance.PlayFX(pickSound, false);

                Destroy(gameObject);
            }
        }
    }
}
