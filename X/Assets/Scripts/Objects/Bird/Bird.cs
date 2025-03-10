using AudioSystem;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class Bird : MonoBehaviour
{
    private Vector2 RandomFlyPosition;

    [SerializeField] private AudioClip[] calm;
    [SerializeField] private AudioClip[] fly;

    private bool isTriggered, isFly, isCalnSoundPlaying;
    private void Awake()
    {
        GetComponent<Animator>().enabled = false;
        RandomFlyPosition = new Vector2(transform.position.x + Random.Range(-100, 100), transform.position.y + Random.Range(100, 200));
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            isTriggered = true;
        }
    }
    private void Update()
    {
        if(!isCalnSoundPlaying)
        {
            StartCoroutine(CalmSoundPlay());
            isCalnSoundPlaying = true;
        }

        if (isTriggered)
        {
            transform.position = Vector2.MoveTowards(transform.position, RandomFlyPosition, 5 * Time.fixedDeltaTime);
            GetComponent<Animator>().enabled = true;

            if (!isFly)
            {
                GetComponent<AudioSource>().PlayOneShot(fly[Random.Range(0, fly.Length - 1)]);
                isFly = true;
            }

            StartCoroutine(DestroyBird());
        }
    }
    private IEnumerator DestroyBird()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }
    private IEnumerator CalmSoundPlay()
    {
        yield return new WaitForSeconds(Random.Range(10, 60));
        GetComponent<AudioSource>().PlayOneShot(calm[Random.Range(0, calm.Length - 1)]);
        isCalnSoundPlaying = false;
    }
}
