using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevengePanelButton : MonoBehaviour
{
    public GameObject RevengeButtonPanel;


    //For Revenge button Panel enable Function
    public void RevengePanel()
    {
       
        if (RevengeButtonPanel != null)
        {
            RevengeButtonPanel.SetActive(true);
        }
    }

    public void CloseButton()
    {
        if (RevengeButtonPanel !=null)
        {
            RevengeButtonPanel.SetActive(false);
        }
    }
}
