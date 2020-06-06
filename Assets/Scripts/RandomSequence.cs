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
        int bankIndex = -1;

        public void ResetBankIndex()
        {
            bankIndex = -1;
        }

        private readonly List<WeightenedAudioClip> _clips;
        private readonly AudioSource _source;

        public RandomSequence(AudioSource source, IEnumerable<WeightenedAudioClip> clips)
        {
            _source = source;
            _clips = clips.ToList();
        }

        public IEnumerator Play(float minPitch, float maxPitch)
        {
            while (true)
            {
                bankIndex++;
                var clips = _clips.Where(c => c.clip != _source.clip).ToArray();
                var clip = GetNextRandomClip(bank[bankIndex]);
                _source.clip = clip;
                _source.Play();
                _source.pitch = Random.Range(minPitch, maxPitch);

                //Debug.Log($"Playing new clip {clip.name}");

                yield return new WaitForSeconds(clip.length);
            }
        }

        public void PlaySingle(float minPitch, float maxPitch)
        {
            var clips = _clips.Where(c => c.clip != _source.clip).ToArray();
            var clip = GetNextRandomClip(clips);
            _source.clip = clip;
            _source.PlayOneShot(clip, 1F);
            _source.pitch = Random.Range(minPitch, maxPitch);

            //Debug.Log($"Playing new clip {clip.name}");
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
