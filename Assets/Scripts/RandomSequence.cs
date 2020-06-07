using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Random = UnityEngine.Random;

namespace Vasya
{

    [Serializable]
    public struct WeightenedAudioClip
    {
        public AudioClip clip;
        public int weight;
    }

    public class RandomSequence
    {
        public static int bankIndex = -1;

        private readonly PlayRandom.AudioBank[] _banks;
        private readonly AudioSource _source;

        public RandomSequence(AudioSource source, PlayRandom.AudioBank[] banks)
        {
            _source = source;
            _banks = banks;
        }

        public IEnumerator Play(float minPitch, float maxPitch)
        {
            while (bankIndex < _banks.Length - 1)
            {
                bankIndex++;
                var clip = GetNextRandomClip(_banks[bankIndex].Clips);
                _source.clip = clip;
                _source.PlayOneShot(_source.clip);
                _source.pitch = Random.Range(minPitch, maxPitch);

                Debug.Log($"Playing new clip {clip.name}");

                yield return new WaitForSeconds(clip.length);
            }
        }

        int GetTotalWeights(IEnumerable<WeightenedAudioClip> clips)
        {
            return clips.Sum(c => c.weight);
        }

        AudioClip GetNextRandomClip(WeightenedAudioClip[] clips)
        {
            var total = GetTotalWeights(clips);
            var random = Random.Range(0, total);
            foreach (var clipMeta in clips)
            {
                Debug.Assert(clipMeta.weight != 0);
                if (random < clipMeta.weight)
                    return clipMeta.clip;
                random -= clipMeta.weight;
            }

            throw new Exception("Not reached");
        }
    }
}
