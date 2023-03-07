using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UIElements;
using System;

/// <summary>
/// Class that manages the changes in cameras
/// </summary>
public class CameraManagerUIController : MonoBehaviour
{

    [Serializable]
    public class CMCamItem
    {
        [Tooltip("Name of the camera/object focused. Displayed on the UI when this camera is selected")]
        public string name;
        [Tooltip("Reference to the corresponding Cinemachine Virtual Camera")]
        public CinemachineVirtualCamera camera;
        [Tooltip("Reference to the object the selected camera is pointing at. Idealy the parent of an object that contains an instance of DisplayDataContainer.")]
        public GameObject targetObjectReference;
    }

    [Tooltip("List of the cameras to be used")]
    [SerializeField]
    private List<CMCamItem> cam_list;

    [Tooltip("Index of the current camera. Mostly for visualization.")]
    [SerializeField]
    private int current_cam = 0;

    private Button prevButton;
    private Button nextButton;
    private Label currentLabel;

    private InfoMenu infoMenu;

    private void Start()
    {
        infoMenu = GetComponent<InfoMenu>();
        var root = GetComponent<UIDocument>().rootVisualElement;

        prevButton = root.Q<Button>("button-prev");
        nextButton = root.Q<Button>("button-next");
        currentLabel = root.Q<Label>("label-current");

        prevButton.clicked += prevCam;
        nextButton.clicked += nextCam;

        updateCams();

    }

    /// <summary>
    /// Increases the value of current_cam by one,
    /// effectively switching to the next camera on the list cam_list.
    /// Loops around when it reaches the end of the list.
    /// </summary>
    private void nextCam()
    {
        current_cam++;
        current_cam = current_cam % cam_list.Count;
        updateCams();
    }

    /// <summary>
    /// Decreases the value of current_cam by one,
    /// effectively switching to the previous camera on the list cam_list.
    /// Loops around when it reaches the beginning of the list.
    /// </summary>
    private void prevCam()
    {
        current_cam--;
        int Ccount = cam_list.Count;
        current_cam = (current_cam % Ccount + Ccount) % Ccount; //Removes option for negative numbers
        updateCams();
    }

    /// <summary>
    /// Called at the start and whenever nextCam() or prevCam() is called.
    /// Updates the camera priorities based on the value of current_cam.
    /// This is done by setting the priority of all cameras in cam_list
    /// but the one in the "current_cam" position to zero, and setting the
    /// one in "current_cam" to one.
    /// 
    /// It also updates the DataContainer on infoMenu, based on the correcponding targetObjectReference
    /// </summary>
    private void updateCams()
    {
        //for each element > if list[i] = current_cam set priority 1, else set as 0
        for (int i = 0; i < cam_list.Count; i++)
        {
            if (i != current_cam) 
            {
                cam_list[i].camera.Priority = 0;
            } else
            {
                cam_list[i].camera.Priority = 1;
                currentLabel.text = cam_list[i].name;
                infoMenu.setDataContainer(getCurrentTargetDataContainer());
            }
        }
    }

    /// <summary>
    /// Takes the gameObject referenced in the list cam_list, on the current_cam position.
    /// If this object is not null, it attempts to return the DisplayDataContainer related to it
    /// </summary>
    /// <returns>The DisplayDataContainer of the currently focused object, if it's not null</returns>
    public DisplayDataContainer getCurrentTargetDataContainer()
    {
        GameObject obj = cam_list[current_cam].targetObjectReference;
        if(obj == null)
        {
            return null;
        } else
        {
            return obj.GetComponentInChildren<DisplayDataContainer>();
        }
    }

}
