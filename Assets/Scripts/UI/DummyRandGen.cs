using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DummyRandGen : MonoBehaviour
{
    string _string;
    Action<string[]> PublishStringArray;
    [SerializeField] float _manualNum = 0;
    void PublishString(string outString)
    {
        string[] string_array = outString.Split(',');
        PublishStringArray?.Invoke(string_array);
    }

    public void Subscribe(Action<string[]> function)
    {
        PublishStringArray += function;
    }
    public void Unsubscribe(Action<string[]> function)
    {
        PublishStringArray -= function;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    int i = 0;
    // Update is called once per frame
    void FixedUpdate()
    {
        i++;
        if (i>50) {
            i = 0;
            string output = "";
            for (int i = 0; i < 9; i++)
            {
                output += (i*100 + Random.value * 10).ToString("F") + ",";
            }
            output += _manualNum.ToString();
            PublishString(output);
        }
    }
}
