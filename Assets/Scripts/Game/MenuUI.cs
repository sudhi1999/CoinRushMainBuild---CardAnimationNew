using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuUI : MonoBehaviour
{
    public GameObject buildPanelGameObject;
    public GameObject screenItemsUIPanel;
    public GameObject DrawButtonPanelUI;

    public TextMeshProUGUI _coinText;
    public TextMeshProUGUI _energyText;

    private GameManager mGameManager;

    [SerializeField] BuildMenuUI mBuildMenuUI;

    void Start()
    {
        mGameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        //if (buildPanelGameObject.activeInHierarchy == true)
        //{
        //    //To restrict the camera moving when build panel is on
        //    Camera.main.GetComponent<CameraController>()._CameraFreeRoam = false;
        //}
        UpdateCoinAndEnergyTextFields();
    }

    public void BuildButton()
    {
        buildPanelGameObject.SetActive(true);
        screenItemsUIPanel.SetActive(false);
        DrawButtonPanelUI.SetActive(false);
        mBuildMenuUI = FindObjectOfType<BuildMenuUI>();
        mBuildMenuUI.SetUpgradeButtons();
    }

    public void ReturnButton()
    {
        buildPanelGameObject.SetActive(false);
        screenItemsUIPanel.SetActive(true);
        DrawButtonPanelUI.SetActive(true);
    }

    private void UpdateCoinAndEnergyTextFields()
    {
        _coinText.text = mGameManager._coins.ToString();
        _energyText.text = mGameManager._energy.ToString();
    }
}
