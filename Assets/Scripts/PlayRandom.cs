using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using Vasya;

public class PlayRandom : MonoBehaviour
{
    private System.Random random = new System.Random();

    public AudioSource _randomsource;
    public AudioSource _musicsource;
    public AudioSource _eventmusicsource;
    public AudioSource _uisource;

    private RandomSequence _sequence;

    [Tooltip("1: good_theme_1, 2: good_theme_2, 3: event_elemental, 4: event_humanoid, 5: event_undead")]
    public AudioClip[] musicTracks;

    [Tooltip("1: UIClick, 2: UInext, 3: UIback")]
    public AudioClip[] uiSounds;

    public AudioClip[] buildSounds;

    public AudioMixerGroup output;
    public AudioMixerGroup music;
    public AudioMixerGroup ui;
    public float minPitch = .95f;
    public float maxPitch = 1.05f;

    [System.Serializable]
    public class AudioBank
    {
        public WeightenedAudioClip[] Clips;
    }

    public AudioBank[] _audioBanks;

    void Start()
    {
        _sequence = new RandomSequence(_randomsource, _audioBanks);

        _randomsource.outputAudioMixerGroup = output;
        _musicsource.outputAudioMixerGroup = music;
        _eventmusicsource.outputAudioMixerGroup = music;
        _uisource.outputAudioMixerGroup = ui;
    }

    public void PlayUISound(string button)
    {
        if (button == "back")
        {
            _uisource.PlayOneShot(uiSounds[2]);
        }
        else if (button == "next")
        {
            _uisource.PlayOneShot(uiSounds[1]);
        }
        else
        {
            _uisource.PlayOneShot(uiSounds[0]);
        }
    }

    public void PlayBuildingSound(Building building, bool build = false)
    {
        _uisource.PlayOneShot(building.selectedSound);
        if (build)
        {
            foreach (AudioClip buildSound in buildSounds)
            {
                _uisource.PlayOneShot(buildSound);
            }
        }
    }

    public void StartSound()
    {
        RandomSequence.bankIndex = -1;
        StartCoroutine(_sequence.Play(minPitch, maxPitch));
    }

    public IEnumerator PlayMusicTrack(int trackIndex)
    {
        _eventmusicsource.volume = 1;
        if (trackIndex > 1)
        {
            while (GameManager.currentEvent != null)
            {
                AudioClip song = musicTracks[trackIndex];
                _eventmusicsource.PlayOneShot(song);
                Debug.Log($"Playing new clip {song.name}");
                yield return new WaitForSecondsRealtime(15);
            }
        }
        else
        {
            AudioClip song = musicTracks[trackIndex];
            _musicsource.PlayOneShot(song);
            Debug.Log($"Playing new clip {song.name}");
        }
    }

    public IEnumerator CrossFade()
    {
        while (_eventmusicsource.volume > 0)
        {
            _eventmusicsource.volume -= 0.01f;
            _musicsource.volume = 1 - _eventmusicsource.volume;
            yield return new WaitForSecondsRealtime(0.01f);
        }
    }
}