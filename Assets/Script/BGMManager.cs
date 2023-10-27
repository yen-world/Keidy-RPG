using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    // 싱글톤
    static public BGMManager instance;

    // BGM에 사용될 audio 모음
    public AudioClip[] cilps;

    AudioSource source;

    WaitForSeconds waitTime = new WaitForSeconds(0.01f);
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void Play(int _playMusicTrack)
    {
        source.volume = 1f;
        // 플레이어에 몇 번째 음악을 재생 시킬지
        source.clip = cilps[_playMusicTrack];
        source.Play();
    }

    public void Stop()
    {
        source.Stop();
    }

    public void Pasue()
    {
        source.Pause();
    }

    public void SetVolume(float _volume)
    {
        source.volume = _volume;
    }

    public void UnPause()
    {
        source.UnPause();
    }
    // 음악이 점점 소리가 줄어들듯이 만들어주는 함수
    public void FadeOutMusic()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOutMusicCoroutine());
    }

    IEnumerator FadeOutMusicCoroutine()
    {
        for (float i = 1.0f; i >= 0; i -= 0.01f)
        {
            source.volume = i;
            yield return waitTime;
        }
    }

    // 음악이 점점 소리가 커지듯이 만들어주는 함수
    public void FadeInMusic()
    {
        StopAllCoroutines();
        StartCoroutine(FadeInMusicCoroutine());
    }

    IEnumerator FadeInMusicCoroutine()
    {
        for (float i = 0f; i <= 1; i += 0.01f)
        {
            source.volume = i;
            yield return waitTime;
        }
    }
}
