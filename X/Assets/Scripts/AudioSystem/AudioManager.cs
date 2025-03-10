using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;


namespace AudioSystem
{
    public class AudioManager : MonoBehaviour
    {
        public Settings settings;
        public static AudioManager instance;

        public AudioSource m_AudioSource;
        public AudioSource fx_AudioSource;

        public bool isFading;

        [SerializeField] private List<AudioSource> music;
        [SerializeField] private List<AudioSource> fx;

        private void Awake()
        {
            if (instance == null) instance = this; //если нет на сцене, задаем на него ссылку
            else if (instance == this) Destroy(gameObject); //если уже есть, удаляем

            DontDestroyOnLoad(this);

            LoadAudioVolume();

            RefreshAudioSources();

            SetFXVolume(settings.FXVolume);
            SetAmbientVolume(settings.AmbientVolume);
        }
        private void Update()
        {


            if (isFading) Fade();
            else UnFade();
        }
        private void OnLevelWasLoaded(int level)
        {
            isFading = false;
            RefreshAudioSources();
        }
        public void PlayFX(AudioClip FX, bool isLoop)
        {
            if (isLoop)
            {
                if (!fx_AudioSource.isPlaying)
                {
                    fx_AudioSource.PlayOneShot(FX);
                }
            }
            else
            {
                fx_AudioSource.PlayOneShot(FX);
            }
        }
        public void PlayAmbient(AudioClip Ambient, bool isLoop)
        {
            m_AudioSource.Stop();
            m_AudioSource.clip = Ambient;

            if (isLoop)
            {
                if (!m_AudioSource.isPlaying)
                {
                    m_AudioSource.PlayOneShot(Ambient);
                }
            }
            else
            {
                m_AudioSource.PlayOneShot(Ambient);
            }

            m_AudioSource.Play();
        }
        public void SetReverb(bool state)
        {
            fx_AudioSource.GetComponent<AudioReverbFilter>().enabled = state;
        }
        public void RefreshAudioSources()
        {
            music.Clear();
            fx.Clear();

            AudioSource[] audioSources = FindObjectsOfType<AudioSource>();

            for (int i = 0; i < audioSources.Length - 1; i++)
            {
                if (audioSources[i].gameObject.tag == "ambient") music.Add(audioSources[i]);
                else if (audioSources[i].gameObject.tag == "fx") fx.Add(audioSources[i]);
            }
        }
        public void LoadAudioVolume()
        {
            fx_AudioSource.volume = settings.FXVolume;
            m_AudioSource.volume = settings.AmbientVolume;
        }
        public void SetFXVolume(float value)
        {
            for (int i = 0; i < fx.Count; i++)
            {
                fx[i].volume = value;
            }

            settings.FXVolume = value;
            fx_AudioSource.volume = value;
        }
        public void SetAmbientVolume(float value)
        {
            settings.AmbientVolume = value;
            m_AudioSource.volume = value;
        }
        public void Pause()
        {
            m_AudioSource.Pause();
            fx_AudioSource.Pause();

            for (int i = 0; i < fx.Count; i++)
            {
                fx[i].Pause();
            }
            for (int i = 0; i < music.Count; i++)
            {
                music[i].Pause();
            }
        }
        public void Play()
        {
            m_AudioSource.UnPause();
            fx_AudioSource.UnPause();

            for (int i = 0; i < fx.Count; i++)
            {
                fx[i].UnPause();
            }
            for (int i = 0; i < music.Count; i++)
            {
                music[i].UnPause();
            }
        }
        public void Fade()
        {
            float fxvolume = fx_AudioSource.volume;
            float ambientvolume = m_AudioSource.volume;

            if (fxvolume >= 0) fxvolume -= 0.01f;
            if (ambientvolume >= 0) ambientvolume -= 0.01f;

            fx_AudioSource.volume = fxvolume;
            m_AudioSource.volume = ambientvolume;

            for (int i = 0; i < fx.Count; i++)
            {
                fx[i].volume = fxvolume;
            }
            for (int i = 0; i < music.Count; i++)
            {
                music[i].volume = ambientvolume;
            }
        }
        public void UnFade()
        {
            float fxvolume = fx_AudioSource.volume;
            float ambientvolume = m_AudioSource.volume;

            if (fxvolume < settings.FXVolume)
            {
                fxvolume += 0.005f;
                fx_AudioSource.volume = fxvolume;

                for (int i = 0; i < fx.Count; i++)
                {
                    fx[i].volume = fxvolume;
                }
            }
            else
            {
                SetFXVolume(settings.FXVolume);
            }
            if (ambientvolume < settings.AmbientVolume)
            {
                ambientvolume += 0.005f;
                m_AudioSource.volume = ambientvolume;

                for (int i = 0; i < music.Count; i++)
                {
                    music[i].volume = ambientvolume;
                }
            }
            else
            {
                SetAmbientVolume(settings.AmbientVolume);
            }
        }
    }
}
