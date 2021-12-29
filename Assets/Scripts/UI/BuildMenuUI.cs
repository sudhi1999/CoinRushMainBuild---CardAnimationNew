using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class BuildMenuUI : MonoBehaviour
{
    //public GameObject buildPanelGameObject;
    //public GameObject screenItemsUIPanel;
    //public GameObject drawButtonUI;

    public Transform ContentView;
    public BuildingManager buildingManagerRef;

    private GameObject BuildingItemTemplate;

    public List<GameObject> ButtonTemplatesHolder = new List<GameObject>();

    public void SetUpgradeButtons()
    {
        BuildingItemTemplate = ContentView.GetChild(0).gameObject;
        for (int i = 0; i <  buildingManagerRef._buildingData.Count; i++)
        {
            GameObject buildingTemplateRef = Instantiate(BuildingItemTemplate, ContentView);
            buildingTemplateRef.name = buildingManagerRef._buildingData[i]._buildingName + " Button";
            ButtonTemplatesHolder.Add(buildingTemplateRef);
            int BuildingUpgradeNumber = i;
            //Debug.Log(i);
            buildingTemplateRef.transform.GetChild(1).gameObject.AddComponent<Button>().onClick.AddListener(() =>
            {
                buildingManagerRef.GrabElementNumberBasedOnButtonClick(BuildingUpgradeNumber);
                //UpdateBuildingImage(buildingTemplateRef, BuildingUpgradeNumber);
                SetButtonImages(buildingTemplateRef, BuildingUpgradeNumber);
            });
            buildingTemplateRef.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = buildingManagerRef._buildingData[i]._buildingName;

            SetButtonImages(buildingTemplateRef, BuildingUpgradeNumber);
        }
        Destroy(BuildingItemTemplate);
        //AttachButton();
    }

    public void UpdateBuildingImage(GameObject inButton,int inElementNumber)
    {
        if (buildingManagerRef._buildingData[inElementNumber]._buildingLevel < buildingManagerRef._buildingData[inElementNumber]._buildingMaxLevel)
        {
            inButton.transform.GetChild(3).GetComponent<Image>().sprite = buildingManagerRef._buildingData[inElementNumber].NextUpgradeImages[buildingManagerRef._buildingData[inElementNumber]._buildingLevel];
        }
        if(buildingManagerRef._buildingData[inElementNumber]._buildingLevel == buildingManagerRef._buildingData[inElementNumber]._buildingMaxLevel)
        {
            inButton.transform.GetChild(0).GetComponent<Image>().color = Color.black;
        }
    }





    void SetButtonImages(GameObject inButton, int inElementNumber)
    {
        if (buildingManagerRef._buildingData[inElementNumber]._buildingLevel < buildingManagerRef._buildingData[inElementNumber]._buildingMaxLevel)
        {
            inButton.transform.GetChild(3).GetComponent<Image>().sprite = buildingManagerRef._buildingData[inElementNumber].NextUpgradeImages[buildingManagerRef._buildingData[inElementNumber]._buildingLevel];
        }
        else
        {
            inButton.transform.GetChild(3).GetComponent<Image>().sprite = buildingManagerRef._buildingData[inElementNumber].NextUpgradeImages[buildingManagerRef._buildingData[inElementNumber]._buildingMaxLevel - 1];
            inButton.transform.GetChild(1).gameObject.GetComponent<Button>().interactable = false;
            inButton.transform.GetChild(0).GetComponent<Image>().color = Color.gray;
        }
    }
}
