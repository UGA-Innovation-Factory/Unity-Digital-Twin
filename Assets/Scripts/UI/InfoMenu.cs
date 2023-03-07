using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UIElements;

/// <summary>
/// Class in charge of collecting the data from a DisplayDataContained instance
/// and displaying it on the UI as a side pannel
/// </summary>
public class InfoMenu : MonoBehaviour
{
    [Tooltip("The corresponding DisplayDataContainer")]
    [SerializeField]
    DisplayDataContainer _DataContainer;

    [Tooltip("The Visual Tree Asset .uxml file that corresponds to the 'template' to be used for each item in the list")]
    [SerializeField]
    private VisualTreeAsset _listElementTemplate;

    /// <summary> ListView that contains all the data to be displayed </summary>
    private ListView infoListView;
    /// <summary> VisualElement containing the ListView 'infoListView'. Used primarily to show/hide on setDataContainer() </summary>
    private VisualElement infoListGroup;
    /// <summary> Small UI element that should pop up when the focused element is considered to be offline </summary>
    private VisualElement offlineBox;

    /// <summary>
    /// Outdated and unused.
    /// Originally this class would've subscribed to the dataPublisher.
    /// That task was delegated to DisplayDataContainer
    /// </summary>
    public void setDataPublisher()
    {

    }


    private void FixedUpdate()
    {
        if (_DataContainer != null)
        {
            infoListView.itemsSource = _DataContainer.getData();
        }

        checkIfOfflineAndUpdateUIAccordingly();
    }

    private void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        infoListView = root.Q<ListView>("main-listview");
        infoListGroup = root.Q<VisualElement>("InfoBackground");

        offlineBox = root.Q<VisualElement>("OfflineBox");

        FillList();
    }

    /// <summary>
    /// Defines the different fucntions required by the ListView infoListView
    /// in order to work properly
    /// </summary>
    void FillList()
    {
        infoListView.makeItem = () =>
        {
            var newItem = _listElementTemplate.Instantiate();
            var ItemController = new ListItemController2();
            newItem.userData = ItemController;
            ItemController.SetVisualElements(newItem);
            return newItem;
        };

        Debug.LogWarning("ListView 'infoListView' definition of bindItem function does NOT consider the case of '_DataContainer' being null. It still works, but something may break for this reason in the future");
        infoListView.bindItem = (item, index) =>
        {
            // ListView 'infoListView' definition of bindItem function does NOT consider the case of '_DataContainer' being null. It still works, but something may break for this reason in the future
            //Debug.Log(index);
            DataStructure dataStruct = _DataContainer.GetDataStructures()[index];
            (item.userData as ListItemController2).InitializeData(dataStruct.m_DataName, dataStruct.m_DataUnit);
            (item.userData as ListItemController2).SetIndex(index);
            (item.userData as ListItemController2).UpdateValue(_DataContainer.getData()[index]);
        };

        infoListView.unbindItem = (item, index) =>
        {
            /*
            _DataPublisher.Unsubscribe((item.userData as ListItemController2).UpdateFromStringArray);
            */
        };

        //infoListView.itemsSource = _DataContainer.data;
    }

    /// <summary>
    /// Sets the current DisplayDataContainer.
    /// Hides or shows the data display depending if there is data to be displayed or not
    /// </summary>
    public void setDataContainer(DisplayDataContainer dataContainer)
    {
        if (dataContainer == null)
        {
            infoListGroup.style.display = DisplayStyle.None;
        } else
        {
            infoListGroup.style.display = DisplayStyle.Flex;    
        }
        _DataContainer = dataContainer;
    }


    /// <summary>
    /// Checks when was the last time data was received, and updates the "Offline"
    /// Visual element to show if 5 seconds have passed since then.
    /// Hides the VisualElement if the curent _DataContainer is null
    /// 
    /// This function was implemented here since this class already was messing
    /// around with the UI.
    /// </summary>
    private void checkIfOfflineAndUpdateUIAccordingly()
    {
        if (_DataContainer == null)
        {
            offlineBox.style.display = DisplayStyle.None;
        }
        else
        {
            int time = _DataContainer.getTimeSinceLatestData();

            if (offlineBox.style.display == DisplayStyle.None)
            {
                if (time > 5)
                {
                    offlineBox.style.display = DisplayStyle.Flex;
                }
            }
            else
            {
                if (time < 5)
                {
                    offlineBox.style.display = DisplayStyle.None;
                }
            }
        }
    }
}

// We dont talk about ListItemController1
/// <summary>
/// Controller class to be used by the ListView in the InfoMenu class
/// </summary>
public class ListItemController2
{
    Label _nameLabel;
    Label _valueLabel;
    Label _unitLabel;
    int _index;

    /// <summary>
    /// "Connects" the different elements of the given VisualElement to this controller class.
    /// It also initialy sets the value label as "start"
    /// </summary>
    public void SetVisualElements(VisualElement listElementInstace)
    {
        _nameLabel = listElementInstace.Q<Label>("label-data-name");
        _valueLabel = listElementInstace.Q<Label>("label-data-value");
        _unitLabel = listElementInstace.Q<Label>("label-data-unit");

        _valueLabel.text = "start";
    }

    public void InitializeData(string name, string unit)
    {
        _nameLabel.text = name + ":";
        _unitLabel.text = unit.Equals(" ") ? "" : "[" + unit + "]";
    }

    public void UpdateValue(string value)
    {
        _valueLabel.text = value;
    }


    public void SetIndex(int index)
    {
        _index = index;
    }
    public void UpdateFromStringArray(string[] str_arr)
    {
        _valueLabel.text = str_arr[_index];
    }
}
