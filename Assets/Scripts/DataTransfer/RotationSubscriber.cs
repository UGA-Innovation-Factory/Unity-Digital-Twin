using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
//using static UnityEngine.GraphicsBuffer;

public class RotationSubscriber : AValuelistener
{

    enum Axises
    {
        X, Y, Z,
    }

    //[SerializeField] IDataPublisher _DataPublisher;
    //[SerializeField] int _DataIndex;

    [Tooltip("Axis to rotate on")]
    [SerializeField] Axises rotate_axis;
    [Tooltip("Rotation offset")]
    [SerializeField] private float _offset;
    [Tooltip("Scale of the rotation (often 1 or -1)")]
    [SerializeField] private float _factor = 1;
    [Tooltip("Angle to move towards. While it can be manually modified, it's mostly for visualization, as this updates automatically if information is passed through the subscribed class")]
    [SerializeField] private float _goal_angle;
    private Vector3 goal_rotation_vector;


    private delegate void rotationUpdate();
    private rotationUpdate _rotationUpdate;

    /// <summary>
    /// Updates joint goal. Actual angles are updated on Update()
    /// </summary>
    public void updateJointValue(float angle)
    {
        _goal_angle = angle;
    }

    /// <summary>
    /// Updates joint goal, but takes a string instead of a number
    /// </summary>
    public void parseStringIntoJoint(string angle_string)
    {
        _goal_angle = float.Parse(angle_string);
    }

    /// <summary>
    /// Subscribes to the given _DataPublisher and uses the number in the index defined in _DataIndex.
    /// (Both variables inherited from the AValueListener class)
    /// </summary>
    void OnEnable()
    {
        _DataPublisher.Subscribe((string[] str_array) =>
        {
            parseStringIntoJoint(str_array[_DataIndex]);
        });
    }

    // Initially sets the _rotationUpdate function to be rotationSetUp()
    void Start()
    {
        _rotationUpdate = rotationSetUp;
        //_rotationUpdate = rotateObjectByGoal;
    }


    void FixedUpdate()
    {
        _rotationUpdate();
    }

    private int i = 0;
    /// <summary>
    /// This function is a quick hack to get the appropiate EulerAngles
    /// in order to perform rotation using those instead of having to deal with Quaternions.
    /// It basically rotates the object a bit, saves the EulerAngles, and changes the _rotationUpdate() function
    /// to rotateObjectByGoal()
    /// </summary>
    void rotationSetUp()
    {
        i++;
        transform.Rotate(Vector3.down, Space.Self);
        if (i>20)
        {
            goal_rotation_vector = transform.localEulerAngles;
            _goal_angle = goal_rotation_vector[(int)rotate_axis] + _offset;
            _rotationUpdate = rotateObjectByGoal;
        }
    }

    /// <summary>
    /// Calculates the adjusted goal based on the value of _goal_angle and the added offset and factor
    /// and updates the localEulerAngles to this result
    /// </summary>
    void rotateObjectByGoal()
    {
        goal_rotation_vector[(int)rotate_axis] = _factor * (_goal_angle - _offset);
        transform.localEulerAngles = goal_rotation_vector;
    }

}
