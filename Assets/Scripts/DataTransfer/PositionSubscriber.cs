using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// An attempt at updating ReadAndAdjustPosition.cs. Currently unused
/// It groups all movement updates into one script, as opposed to needing 1 object for each axis.
/// Needs work.
/// </summary>
public class PositionSubscriber : AValuelistener{

    [Serializable] class Data
    {
        public int dataIndex = -1;
        public float offset = 0;
        public float factor = 1;
        public float posGoal = 0;

        public void updatePos(string[] str_array)
        {
            posGoal = factor * (float.Parse(str_array[dataIndex]) + offset);
        }

    }

    //[SerializeField] MonoBehaviour _DataPublisher;
    [Header("Axis Movement configuration")]
    [Tooltip("Settings for the X axis movement")]
    [SerializeField] Data XAxis;
    [Tooltip("Settings for the Y axis movement")]
    [SerializeField] Data YAxis;
    [Tooltip("Settings for the Z axis movement")]
    [SerializeField] Data ZAxis;

    private Action<string[]> UpdateGoalPosition;

    [SerializeField] private Vector3 all_posGoal;

    // Start is called before the first frame update
    void OnEnable()
    {
        all_posGoal = transform.localPosition;

        if (XAxis.dataIndex > -1)
        {
            UpdateGoalPosition += XAxis.updatePos;
        }
        if (YAxis.dataIndex > -1)
        {
            UpdateGoalPosition += YAxis.updatePos;
        }
        if (ZAxis.dataIndex > -1)
        {
            UpdateGoalPosition += ZAxis.updatePos;
        }

        _DataPublisher.Subscribe(UpdateGoalPosition);
    }


    private void Start()
    {
        XAxis.posGoal = transform.localPosition.x;
        YAxis.posGoal = transform.localPosition.y;
        ZAxis.posGoal = transform.localPosition.z;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        all_posGoal.x = XAxis.posGoal;
        all_posGoal.y = YAxis.posGoal;
        all_posGoal.z = ZAxis.posGoal;
        transform.localPosition = all_posGoal;
    }
}
