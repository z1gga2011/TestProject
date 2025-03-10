using AudioSystem;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

using UnityEngine.UI;

public class Door : MonoBehaviour
{
    public AudioClip DoorSound, LockedDoorSound;
    public SpriteRenderer _sprite, _shadow;

    public float Speed = 1f;
    public bool Locked; // закрыта ли дверь

    private bool InDoor, isOpen;

    private void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && InDoor && !isOpen)
        {
            if(!Locked)
            {
                isOpen = true;
                AudioManager.instance.PlayFX(DoorSound, false);
            }
            else
            {
                Vector2 InstantiatePos = new Vector2(transform.position.x, transform.position.y + (GetComponent<SpriteRenderer>().bounds.size.y / 2) + 1f);
                TipText tip = Instantiate(Resources.Load<TipText>("Tip/TipText"), InstantiatePos, Quaternion.identity);
                tip.SetText("закрыто");
                AudioManager.instance.PlayFX(LockedDoorSound, false);
            }
        }

        OpenDoor();
    }
    private void OpenDoor()
    {
        if (isOpen)
        {
            if(GetComponent<UnityEngine.Rendering.Universal.ShadowCaster2D>() != null) GetComponent<UnityEngine.Rendering.Universal.ShadowCaster2D>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;

            var color = _sprite.color;
            color.a -= Speed * Time.fixedDeltaTime;
            color.a = Mathf.Clamp(color.a, 0, 1);
            _sprite.color = color;

            if (_shadow != null)
            {
                var shadowcolor = _shadow.color;
                shadowcolor.a -= Speed * Time.fixedDeltaTime;
                shadowcolor.a = Mathf.Clamp(shadowcolor.a, 0, 1);
                _shadow.color = shadowcolor;
            }

            if (color.a == 0)
            {
                if (_shadow != null) Destroy(_shadow.gameObject);
                Destroy(gameObject);
            }
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            InDoor = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            InDoor = false;
        }
    }
}
