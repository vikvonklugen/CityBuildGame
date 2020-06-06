using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.Audio;
using System.Linq;
using System;
using System.Collections.Generic;


[RequireComponent(typeof(AudioSource))]
public class PlayRandom : MonoBehaviour
{
    [SerializeField]
    private Vasya.WeightenedAudioClip[] _clips;
    private AudioSource _source;

    private Vasya.RandomSequence _sequence;

    public AudioMixerGroup output;
    public float minPitch = .95f;
    public float maxPitch = 1.05f;
    //    public Transform[] SpawnPoints;
    public float timeLeft;

    void Start()
    {
        _source = GetComponent<AudioSource>();
        timeLeft = 0.0f;

        _sequence = new Vasya.RandomSequence(_source, _clips);

        _source.outputAudioMixerGroup = output;
    }
    public void StartSound()
    {
        StartCoroutine(_sequence.Play(minPitch, maxPitch));
    }
}