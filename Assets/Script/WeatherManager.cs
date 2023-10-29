using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherManager : MonoBehaviour
{
    // 싱글톤 패턴을 위한 instance
    static public WeatherManager instance;

    // 날씨에 따른 날씨 효과음
    AudioManager theAudio;

    // ParticleSystem을 제어하기 위한 변수
    public ParticleSystem rain;

    // 비가 내리는 소리의 Sound 이름
    public string rain_sound;

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
        theAudio = FindObjectOfType<AudioManager>();
    }

    // AudioManager의 Play함수로 빗소리 재생 및 파티클 실행
    public void Rain()
    {
        theAudio.Play(rain_sound);
        rain.Play();
    }

    // 빗소리 및 파티클 중지
    public void RainStop()
    {
        theAudio.Stop(rain_sound);
        rain.Stop();
    }

    // 파티클의 떨어지는 양 조절
    public void RainDrop()
    {
        rain.Emit(10);
    }
}