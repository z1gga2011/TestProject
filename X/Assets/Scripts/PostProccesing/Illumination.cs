using AudioSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class Illumination : MonoBehaviour
{
    [Header("глобальное освещение")]

    public UnityEngine.Rendering.Universal.Light2D lightAura;
    public UnityEngine.Rendering.Universal.Light2D globalLight;

    public int RefreshTime = 100;
    public bool readyToIllumination = true;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Room")
        {
            lightAura.enabled = true;
            globalLight.GetComponent<Animator>().Play("ToRoom");

            collision.GetComponent<RoomShadow>().isVisible = false;

            AudioManager.instance.SetReverb(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Room")
        {
            lightAura.enabled = false;

            if (readyToIllumination)
            {
                globalLight.GetComponent<Animator>().Play("OutRoom");

                StartCoroutine(Refresh());
            }
            else 
            {
                globalLight.GetComponent<Animator>().Play("QuickOutRoom");
            }

            collision.GetComponent<RoomShadow>().isVisible = true;

            AudioManager.instance.SetReverb(false);
        }
    }
    private IEnumerator Refresh()
    {
        yield return new WaitForSeconds(2.3f);
        readyToIllumination = false;
        yield return new WaitForSeconds(RefreshTime);
        readyToIllumination = true;
    }
}
