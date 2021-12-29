using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSceneButton : MonoBehaviour
{

    public GameObject RevengeButtonPanel;
    public GameObject ScorePanel;
 

    //this function Exit the atatck scene and goes to the player main Scene 
    public void BackButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    /// <summary>
    ///  This enables the panel when Revenge button is Pressed
    /// </summary>

    public void RevengePanel()
    {

        if (RevengeButtonPanel != null)
        {
            RevengeButtonPanel.SetActive(true);
        }
    }

    /// <summary>
    /// This close the Revenge Panel when close button Pressed 
    /// </summary>
    public void RevengeCloseButton()
    {
        if (RevengeButtonPanel != null)
        {
            RevengeButtonPanel.SetActive(false);
        }
    }
}
