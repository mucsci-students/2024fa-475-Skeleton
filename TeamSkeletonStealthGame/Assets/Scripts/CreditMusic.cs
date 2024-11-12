using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditMusic : MonoBehaviour
{
    private AudioSource source;

    public Animator transition;

    private void Start() {
        source = GetComponent<AudioSource>();
        source.volume = 0f;
        StartCoroutine(FadeIn(true, source, 3f, 0.77f));
        StartCoroutine(FadeIn(false, source, 3f, 0f));



        StartCoroutine(FadeOut(true, source, 3f, 0f));
        StartCoroutine(FadeOut(false, source, 3f, 0.77f));

    }

    public IEnumerator FadeIn(bool fadeIn, AudioSource source, float duration, float targetVol) {
        if (!fadeIn) {
            double lengthOfSource = (double)source.clip.samples/source.clip.frequency;
            yield return new WaitForSecondsRealtime((float)(lengthOfSource - duration));
        }

        float time = 0f;
        float startVol = source.volume;
        while(time < duration) {
            time +=  Time.deltaTime;
            source.volume = Mathf.Lerp(startVol, targetVol, time/duration);
            yield return null;
        }

        yield break;
    }

    public IEnumerator FadeOut(bool fadeIn, AudioSource source, float duration, float targetVol) {
        yield return new WaitForSecondsRealtime(28f);
        if (!fadeIn) {
            double lengthOfSource = (double)source.clip.samples/source.clip.frequency;
            yield return new WaitForSecondsRealtime((float)(lengthOfSource - duration));
        }

        float time = 0f;
        float startVol = source.volume;
        while(time < duration) {
            time +=  Time.deltaTime;
            source.volume = Mathf.Lerp(startVol, targetVol, time/duration);
            yield return null;
        }

        yield break;
    }
}
