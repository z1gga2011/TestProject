using AudioSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorKey : MonoBehaviour
{
    public Door DoorToOpen; // от какой двери ключ
    public AudioClip KeySound;
    public void GetKey()
    {
        DoorToOpen.Locked = false;
    }
    public void GetInventoryKey()
    {
        GetKey();

        PlayerMovement character = FindObjectOfType<PlayerMovement>();

        Vector2 InstantiatePos = new Vector2(character.transform.position.x, character.transform.position.y + (character.GetComponent<SpriteRenderer>().bounds.size.y / 2) + 1f);
        TipText tip = Instantiate(Resources.Load<TipText>("Tip/TipText"), InstantiatePos, Quaternion.identity);
        tip.SetText("ключ");
        AudioManager.instance.PlayFX(KeySound, false);
    }
    public void GetObjectKey()
    {
        GetKey();

        Vector2 InstantiatePos = new Vector2(transform.position.x, transform.position.y + (GetComponent<SpriteRenderer>().bounds.size.y / 2) + 1f);
        TipText tip = Instantiate(Resources.Load<TipText>("Tip/TipText"), InstantiatePos, Quaternion.identity);
        tip.SetText("ключ");
        AudioManager.instance.PlayFX(KeySound, false);

        Destroy(gameObject);
    }
}
