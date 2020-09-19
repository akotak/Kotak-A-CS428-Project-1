using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.CodeDom.Compiler;
using System.Collections.Specialized;

public class WindAPIScript : MonoBehaviour
{
    public GameObject TextObject;
    public GameObject gameObject;
    string url = "http://api.openweathermap.org/data/2.5/weather?lat=41.88&lon=-87.6&APPID=ea221d31eaa7a58a76121d5e68d4acc2&units=imperial";

    void Start()
    {
        // wait a couple seconds to start and then refresh every 900 seconds

        InvokeRepeating("GetDataFromWeb", 2f, 900f);
    }

    void GetDataFromWeb()
    {

        StartCoroutine(GetRequest(url));
    }

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            string deg;
            string degVal;
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

                string speed = getStringVal(str, "speed", "deg");
                string speedVal = speed.Trim(',', ':', '"');

                // takes into account if there is gust in the string, then find degrees that way
                if (str.Contains("gust"))
                {
                    deg = getStringVal(str, "deg", "gust");
                    degVal = deg.Trim(',', ':', '"', '}');
                }
               
                // find the degrees normally in the string
                else
                {
                    deg = getStringVal(str, "deg", "all");
                    degVal = deg.Trim('c', 'l', 'o', 'u', 'd', 's', ',', ':', '"', '}', '{');
                }

                // get the degrees value
                float degValue = float.Parse(degVal);
                Quaternion rotation = Quaternion.Euler(90, degValue, 0);
                gameObject.transform.rotation = rotation; // rotate the wind sock in the direction of where it's pointing

                // gets the wind speed value
                float speedValue = float.Parse(speedVal);
                Vector3 scale = new Vector3(0.1f, 0.1f + (speedValue * 0.01f), 0.1f); // scale the orange cylinder (wind sock) depending on the wind speed
                gameObject.transform.localScale = scale;

                TextObject.GetComponent<TextMeshPro>().text = speedVal + " mph " + getDirection(degValue);

            }
        }
    }

    // get the direction of the degree value
    public string getDirection(float degValue)
    {
        // north
        if (degValue > 337.5 || degValue < 22.5)
        {
            return "N";
        }
        // north east
        if (degValue > 22.5 && degValue < 67.5)
        {
            return "NE";
        }
        // east
        if (degValue > 67.5 && degValue < 112.5)
        {
            return "E";
        }
        // south east
        if (degValue > 112.5 && degValue < 157.5)
        {
            return "SE";
        }
        // south
        if (degValue > 157.5 && degValue < 202.5)
        {
            return "S";
        }
        // south west
        if (degValue > 202.5 && degValue < 247.5)
        {
            return "SW";
        }
        // west
        if (degValue > 247.5 && degValue < 292.5)
        {
            return "W";
        }
        // north west
        if (degValue > 292.5 && degValue < 337.5)
        {
            return "NW";
        }
        return "";
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