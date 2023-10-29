using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRain : MonoBehaviour
{
    WeatherManager theWeather;
    public bool rain;
    // Start is called before the first frame update
    void Start()
    {
        theWeather = FindObjectOfType<WeatherManager>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (rain)
        {
            theWeather.RainDrop();
        }
        else
        {
            theWeather.RainStop();
        }
    }
}
