using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
//using static UnityEngine.GraphicsBuffer;

public class RotationSubscriber : AbsValuelistener
{

    enum Axises
    {
        X, Y, Z,
    }

    //[SerializeField] IDataPublisher _DataPublisher;
    //[SerializeField] int _DataIndex;
    [Header("Rotation configuration")]
    [SerializeField]
    [Tooltip("Data key of the value to be used")]
    protected string _DataKey;
    [Tooltip("Axis to rotate on")]
    [SerializeField] Axises rotateAxis;
    [Tooltip("Is the data coming in in radians or degrees?")]
    [SerializeField] private bool _isRadians;
    [Tooltip("Rotation offset")]
    [SerializeField] private float _offset;
    [Tooltip("Scale of the rotation (often 1 or -1)")]
    [SerializeField] private float _factor = 1;
    [Header("Debug")]
    [Tooltip("Angle to move towards. While it can be manually modified, it's mostly for visualization, as this updates automatically if information is passed through the subscribed class")]
    [SerializeField] private float _goalAngle;
    private Vector3 goal_rotation_vector;


    /// <summary>
    /// Action used to set up rotation.
    /// It rotates the object a bit, saves the EulerAngles, and then changes to rotateObjectByGoal, which updates the rotation every call.
    /// </summary>
    private Action _rotationUpdate;


    private Action<string> _updateJoint;


    /// <summary>
    /// Updates joint goal, but takes a string instead of a number
    /// </summary>
    public void parseStringAngleIntoJoint(string angle_string)
    {
        _goalAngle = float.Parse(angle_string);
    }

    /// <summary>
    /// Updates joint goal, but takes a string instead of a number
    /// </summary>
    public void parseStringRadianIntoJoint(string radian_string)
    {
        _goalAngle = float.Parse(radian_string) * Mathf.Rad2Deg;
    }

    /// <summary>
    /// Subscribes to the given _DataPublisher and uses the number in the index defined in _DataIndex.
    /// (Both variables inherited from the AValueListener class)
    /// </summary>
    void OnEnable()
    {
        //_DataPublisher.Subscribe((string[] str_array) =>
        //{
        //    parseStringIntoJoint(str_array[_DataIndex]);
        //});
    }

    // Initially sets the _rotationUpdate function to be rotationSetUp()
    protected override void Start()
    {
        _rotationUpdate = rotationSetUp;
        base.Start();
        //_rotationUpdate = rotateObjectByGoal;

        if (_isRadians)
        {
            _updateJoint = parseStringRadianIntoJoint;
        } else
        {
            _updateJoint = parseStringAngleIntoJoint;
        }

        SubscribeToData(_DataKey, _updateJoint);
    }


    void FixedUpdate()
    {
        _rotationUpdate();
    }

    private int i = 0;
    /// <summary>
    /// This function is a quick hack to get the appropiate EulerAngles
    /// in order to perform rotation using those instead of having to deal with Quaternions.
    /// It basically rotates the object a bit, saves the EulerAngles, and changes the _rotationUpdate function to rotateObjectByGoal()
    /// </summary>
    void rotationSetUp()
    {
        i++;
        transform.Rotate(Vector3.down, Space.Self);
        if (i>20)
        {
            goal_rotation_vector = transform.localEulerAngles;
            _goalAngle = goal_rotation_vector[(int)rotateAxis] + _offset;
            _rotationUpdate = rotateObjectByGoal;
        }
    }

    /// <summary>
    /// Calculates the adjusted goal based on the value of _goal_angle and the added offset and factor
    /// and updates the localEulerAngles to this result
    /// </summary>
    void rotateObjectByGoal()
    {
        goal_rotation_vector[(int)rotateAxis] = _factor * (_goalAngle - _offset);
        transform.localEulerAngles = goal_rotation_vector;
    }

}
