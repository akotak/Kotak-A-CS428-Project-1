using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.CodeDom.Compiler;

public class WeatherAPIScript2 : MonoBehaviour
{
    public GameObject clearSky, scatteredClouds, fewClouds, brokenClouds,
            showerRain, rain, snow, thunderstorm, mist;
    int count = -2; // used to give gameObjects unique IDs for a list
    float id;
    string url = "http://api.openweathermap.org/data/2.5/weather?lat=41.88&lon=-87.6&APPID=ea221d31eaa7a58a76121d5e68d4acc2&units=imperial";

    void Start()
    {
        // wait a couple seconds to start and then refresh every 900 seconds
        InvokeRepeating("GetDataFromWeb", 2f, 900f);
    }

    void Update()
    {
        pressArrowKeys(); // when user presses left or right arrow key, it will show the other weather conditions

        // shows current weather condition
        if (count == -2)
        {
            checkWeatherConditions(id);
        }
    }

    void GetDataFromWeb()
    {
        StartCoroutine(GetRequest(url));
    }

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string str = webRequest.downloadHandler.text;

            if (webRequest.isNetworkError)
            {
                Debug.Log(": Error: " + webRequest.error);
            }
            else
            {
                // print out the weather data to make sure it makes sense
                Debug.Log(":\nReceived: " + webRequest.downloadHandler.text);

                string weather = getStringVal(str, "id", "main");
                string weatherType = weather.Trim(',', ':', '"');
                id = float.Parse(weatherType);
            }
        }
    }

    // debugging - shows the other weather conditions besides current one
    public void pressArrowKeys()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            count++;
            if (count == 0)
            {
                mist.SetActive(false);
                clearSky.SetActive(true);
            }
            if (count == 1)
            {
                clearSky.SetActive(false);
                fewClouds.SetActive(true);
            }
            if (count == 2)
            {
                fewClouds.SetActive(false);
                scatteredClouds.SetActive(true);
            }
            if (count == 3)
            {
                scatteredClouds.SetActive(false);
                brokenClouds.SetActive(true);
            }
            if (count == 4)
            {
                brokenClouds.SetActive(false);
                showerRain.SetActive(true);
            }
            if (count == 5)
            {
                showerRain.SetActive(false);
                rain.SetActive(true);
            }
            if (count == 6)
            {
                rain.SetActive(false);
                snow.SetActive(true);
            }
            if (count == 7)
            {
                snow.SetActive(false);
                thunderstorm.SetActive(true);
            }
            if (count == 8)
            {
                thunderstorm.SetActive(false);
                mist.SetActive(true);
            }
            // if user reaches the last weather condition, goes back to the beginning of the list
            if (count == 9)
            {
                mist.SetActive(false);
                count = 0;
                clearSky.SetActive(true);
            }
            Debug.Log(count + "after pressing arrow key");
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            count--;
            if (count == 0)
            {
                fewClouds.SetActive(false);
                clearSky.SetActive(true);
            }
            if (count == 1)
            {
                scatteredClouds.SetActive(false);
                fewClouds.SetActive(true);
            }
            if (count == 2)
            {
                brokenClouds.SetActive(false);
                scatteredClouds.SetActive(true);
            }
            if (count == 3)
            {
                showerRain.SetActive(false);
                brokenClouds.SetActive(true);
            }
            if (count == 4)
            {
                rain.SetActive(false);
                showerRain.SetActive(true);
            }
            if (count == 5)
            {
                snow.SetActive(false);
                rain.SetActive(true);
            }
            if (count == 6)
            {
                thunderstorm.SetActive(false);
                snow.SetActive(true);
            }
            if (count == 7)
            {
                mist.SetActive(false);
                thunderstorm.SetActive(true);
            }
            if (count == 8)
            {
                clearSky.SetActive(false);
                mist.SetActive(true);
            }
            // if user reaches the last weather condition, goes back to the beginning of the list
            if (count == -1)
            {
                clearSky.SetActive(false);
                mist.SetActive(true);
                count = 8;
            }
            Debug.Log(count + "after pressing arrow key");
        }
    }

    // sets the current weather condition using their id
    public void checkWeatherConditions(float id)
    {
        // clear sky
        if (id == 800)
        {
            count = 0;
            clearSky.SetActive(true);
        }
        // few clouds
        if (id == 801)
        {
            count = 1;
            fewClouds.SetActive(true);
        }
        // scattered clouds
        if (id == 802)
        {
            count = 2;
            scatteredClouds.SetActive(true);
        }
        // broken clouds
        if (id == 803 || id == 804)
        {
            count = 3;
            brokenClouds.SetActive(true);
        }
        // shower rain
        if ((id >= 300 && id <= 321) || (id >= 520 && id <= 531))
        {
            count = 4;
            showerRain.SetActive(true);
        }
        // rain
        if (id >= 500 && id <= 504)
        {
            count = 5;
            rain.SetActive(true);
        }
        // snow
        if ((id >= 600 && id <= 622) || id == 511)
        {
            count = 6;
            snow.SetActive(true);
        }
        // thunderstorm
        if (id >= 200 && id <= 232)
        {
            count = 7;
            thunderstorm.SetActive(true);
        }
        // mist
        if (id >= 701 && id <= 781)
        {
            count = 8;
            mist.SetActive(true);
        }
    }

    // function to return what is between two strings
    public string getStringVal(string str, string str1, string str2)
    {
        if (str.Contains(str1) && str.Contains(str2))
        {
            int startIndex = str.IndexOf(str1) + str1.Length;
            int endIndex = str.IndexOf(str2);
            return str.Substring(startIndex, endIndex - startIndex);
        }
        return "";
    }
}