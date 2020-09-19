using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.CodeDom.Compiler;

public class WeatherAPIScript : MonoBehaviour
{
    public GameObject TemperatureTextObject;
    public GameObject HumidityTextObject;
    string temp, tempVal; // get the string temperature value
    string humidity, humidityVal; // get the string humidity value
    string url = "http://api.openweathermap.org/data/2.5/weather?lat=41.88&lon=-87.6&APPID=ea221d31eaa7a58a76121d5e68d4acc2&units=imperial";


    void Start()
    {

        // wait a couple seconds to start and then refresh every 900 seconds

        InvokeRepeating("GetDataFromWeb", 2f, 900f);
    }

    void Update()
    {
        // displays the information onto the Text GameObjects
        TemperatureTextObject.GetComponent<TextMeshPro>().text = tempVal + " F";

        HumidityTextObject.GetComponent<TextMeshPro>().text = humidityVal + "%";
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
                temp = getStringVal(str, "temp", "feels_like");
                tempVal = temp.Trim(',', ':', '"');

                humidity = getStringVal(str, "humidity", "visibility");
                humidityVal = humidity.Trim(',', ':', '"', '}');

            }

            
        }
    }

    // function to return what is between two strings
    public string getStringVal(string str, string str1, string str2)
    {
        if(str.Contains(str1) && str.Contains(str2))
        {
            int startIndex = str.IndexOf(str1) + str1.Length;
            int endIndex = str.IndexOf(str2);
            return str.Substring(startIndex, endIndex - startIndex);
        }
        return "";
    }
}