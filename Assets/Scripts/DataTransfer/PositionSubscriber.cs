using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// An attempt at updating ReadAndAdjustPosition.cs. Currently unused
/// It groups all movement updates into one script, as opposed to needing 1 object for each axis.
/// Needs work.
/// </summary>
public class PositionSubscriber : AbsValuelistener{

    [Serializable] class Data
    {
        public bool enabled = false;
        public string _DataKey;
        public float offset = 0;
        public float factor = 1;
        public float posGoal = 0;

        public void UpdatePos(string position)
        {
            posGoal = factor * (float.Parse(position) + offset);
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


    [Header("Debug")]
    [SerializeField] private Vector3 all_posGoal;


    protected override void Start()
    {
        base.Start();

        all_posGoal = transform.localPosition;

        // Subscribe each axis to data if they are enabled
        if (XAxis.enabled)
        {
            SubscribeToData(XAxis._DataKey, XAxis.UpdatePos);
        }
        if (YAxis.enabled)
        {
            SubscribeToData(YAxis._DataKey, YAxis.UpdatePos);
        }
        if (ZAxis.enabled)
        {
            SubscribeToData(ZAxis._DataKey, ZAxis.UpdatePos);
        }

        XAxis.posGoal = transform.localPosition.x;
        YAxis.posGoal = transform.localPosition.y;
        ZAxis.posGoal = transform.localPosition.z;
    }

    // Update is called once per frame
    //void Update()
    //{
    //
    //}

    private void FixedUpdate()
    {
        all_posGoal.x = XAxis.posGoal;
        all_posGoal.y = YAxis.posGoal;
        all_posGoal.z = ZAxis.posGoal;
        transform.localPosition = all_posGoal;
    }
}
