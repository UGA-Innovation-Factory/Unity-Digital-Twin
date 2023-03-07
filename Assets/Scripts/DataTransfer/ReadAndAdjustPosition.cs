using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

//OLD AND UNUSED


/// <summary>
/// Script in charge of moving the assigned Unity object in a single axis.
/// It subscribes to a data publisher, which should publish a
/// list of strings, and takes a specific string (defined by DataIndex) from said list
/// to set it as the "position goal".
/// Through FixedUpdate() it moves the object towards said goal
/// taking into account a given offset and scale(factor)
/// </summary>
public class ReadAndAdjustPosition : AValuelistener
{


    enum Axises
    {
        X, Y, Z,
    }
    [Tooltip("Axis to move on")]
    [SerializeField] Axises movement_axis;
    [Tooltip("Movement offset")]
    [SerializeField] private float _offset;
    [Tooltip("Scale of the movement (often 1 or -1)")]
    [SerializeField] private float _factor = 1;
    [Tooltip("Position to move towards. While it can be manually modified, it's mostly for visualization, as this updates automatically if information is passed through the subscribed class")]
    [SerializeField] private float _goal_position;

    private delegate void positionUpdate(float pos);
    private positionUpdate _positionUpdate;

    // Takes a number in the form of a string, casts it to float, and saves is as the
    // position goal
    public void parseStringIntoPosition(string pos_string)
    {
        _goal_position = float.Parse(pos_string);
    }

    /*
    public override void parseStringIntoValue(string str)
    {
        parseStringIntoPosition(str);
    }
    */

    //Subscribes to the publisher
    void OnEnable()
    {
        _DataPublisher.Subscribe((string[] str_array) =>
        {
            parseStringIntoPosition(str_array[_DataIndex]);
        });
    }

    // Depending on the Axis selected, the _positionUpdate function
    // is defined. As of right now, this script only updates a single
    // Axis
    void Start()
    {
        switch (movement_axis)
        {

            case Axises.X: 
                _positionUpdate = (pos =>
                {
                    float goal = pos - transform.localPosition.x;
                    transform.localPosition += goal * Vector3.right;
                } );
                break;
            case Axises.Y:
                _positionUpdate = (pos =>
                {
                    float goal = pos - transform.localPosition.y;
                    transform.localPosition += goal * Vector3.up;
                });
                break;
            case Axises.Z:
                _positionUpdate = (pos =>
                {
                    float goal = pos - transform.localPosition.z;
                    transform.localPosition += goal * Vector3.forward;
                });
                break;
        }


    }

    void Update()
    {

    }

    // Calculates the adjusted goal based on the given goal and the added offset and factor
    // and calls the defined _positionUpdate function using the result
    void FixedUpdate()
    {
        float adjusted_goal = _factor * (_goal_position + _offset);
        _positionUpdate(adjusted_goal);
    }
}
