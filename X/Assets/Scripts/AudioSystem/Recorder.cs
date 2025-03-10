using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recorder : MonoBehaviour
{
    public string TrackName; // название трека во всплывающем окне

    public AudioClip AudioToPlay; // что проигрывается при взаимодействии
    public AudioClip StartPlaySound, StopPlaySound;

    private AudioSource m_AudioSource;
    private bool coolDown;

    private void Start()
    {
        m_AudioSource = GetComponent<AudioSource>();
    }
    public void Play()
    {
        if (!coolDown)
        {
            if (!m_AudioSource.isPlaying) StartCoroutine(StartPlay());
            else StartCoroutine(StopPlay());
        }
    }
    private IEnumerator StartPlay()
    {
        if (TrackName.Length > 0)
        {
            Vector2 InstantiatePos = new Vector2(transform.position.x, transform.position.y + (GetComponent<SpriteRenderer>().bounds.size.y / 2) + 1f);
            TipText tip = Instantiate(Resources.Load<TipText>("Tip/TipText"), InstantiatePos, Quaternion.identity);
            tip.SetText(TrackName);
        }

        coolDown = true;
        m_AudioSource.PlayOneShot(StartPlaySound);
        yield return new WaitForSeconds(1f);
        m_AudioSource.PlayOneShot(AudioToPlay);
        coolDown = false;
        yield break;
    }
    private IEnumerator StopPlay()
    {
        coolDown = true;
        m_AudioSource.PlayOneShot(StopPlaySound);
        yield return new WaitForSeconds(1f);
        m_AudioSource.Stop();
        coolDown = false;
        yield break;
    }
}
