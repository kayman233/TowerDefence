using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace FXs
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundFx: MonoBehaviour
    {
        [SerializeField]
        private AudioClip m_AudioClip;

        [SerializeField, Range(-3, 3)] private float m_MinPitch;
        [SerializeField, Range(-3, 3)] private float m_MaxPitch;

        private void Awake()
        {
            AudioSource m_AudioSource = GetComponent<AudioSource>();
            m_AudioSource.clip = m_AudioClip;
            m_AudioSource.pitch = Random.Range(m_MinPitch, m_MaxPitch);
            
            m_AudioSource.Play();
        }
    }
}