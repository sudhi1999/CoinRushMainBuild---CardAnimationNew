using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Slots : MonoBehaviour
{
    public Reels[] _reels;
    public Button _uiSpinButton;
    public TextMeshProUGUI _SlotDisplayHeadText;

    public GameObject _rewardPanel;
    public TextMeshProUGUI _rewardText;

    public float spin = 1;

    private GameManager mGameManager;

    public List<ReelElement> _elementsName;
    
    private void Start()
    {
        mGameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _uiSpinButton.onClick.AddListener(()=>
        {
            for (int i = 0; i < _reels.Length; i++)
            {
                _reels[i].mSpinOver = false;
            }
            if (spin > 0)
            {
                spin--;
                _uiSpinButton.interactable = false;
                StartCoroutine(DelayedSpin());
                _elementsName.Clear();
            }
        });
    }

    private void Update()
    {
        _SlotDisplayHeadText.text = spin.ToString();
        if (_reels[2].mSpinOver == true && spin != 0)
        {
            _uiSpinButton.interactable = true;
        }
    }
    
    /// <summary>
    /// Function for spin button to work that contains an IEnumerator / Invoke Method for a Delayed start of Reels Spinning creating a moment of suspension.
    /// </summary>
    /// 
    private IEnumerator DelayedSpin()
    {
        foreach (Reels reel in _reels)
        {
            reel._roll = true;
            //Things to happen when roll ends and stops.                                                                                              
            reel.OnReelRollEnd(reel => 
            {   
                _elementsName.Add(reel);
                ResultChecker(); 
            });
            reel.mdisableRoll = false;
        }
        for (int i = 0; i < _reels.Length; i++)
        {
            //Allow The Reels To Spin For A Random Amout Of Time Then Stop Them using the spin function
            yield return new WaitForSeconds(Random.Range(1f,2.5f));
            if(i==2)
            {
                yield return new WaitForSeconds(1.5f);
            }
            _reels[i].Spin(); // Finds a Gameobject based on probability and stop the reel at appropriate spot
        }
    }

    private void ResultChecker()
    {
        for (int i = 0; i < _elementsName.Count - 2; i++)
        {
            if(_elementsName[i]._slotElementGameObject.name == _elementsName[i + 1]._slotElementGameObject.name && _elementsName[i + 1]._slotElementGameObject.name == _elementsName[i + 2]._slotElementGameObject.name)
            {
                switch (_elementsName[i]._slotElementGameObject.name)
                {
                    case "TradingCards": 
                        _rewardText.text = "Trading Card Pack";
                        Invoke(nameof(RewardPanelInvoke), 2f);
                        break;
                    case "FreeSpins":
                        _rewardText.text = "5 Spins";
                        spin += 5;
                        if (spin == 0)
                        {
                            Invoke(nameof(RewardPanelInvoke), 2f);
                        }
                        //Invoke(nameof(ActiveLevelInvoke), 2f);
                        break;
                    case "Coins":
                        _rewardText.text = "5000 Coins";
                        mGameManager._coins += 5000;
                        Invoke(nameof(RewardPanelInvoke), 2f);
                        break;
                    case "Energy":
                        _rewardText.text = "10 Energy";
                        mGameManager._energy += 10;
                        Invoke(nameof(RewardPanelInvoke), 2f);
                        break;
                    default:
                        break;
                }
            }
            else if(_elementsName[i]._slotElementGameObject.name == _elementsName[i + 1]._slotElementGameObject.name || _elementsName[i]._slotElementGameObject.name == _elementsName[i + 2]._slotElementGameObject.name)
            {
                switch (_elementsName[i]._slotElementGameObject.name)
                {
                    case "TradingCards":
                        _rewardText.text = "Oops Try Again";
                        Invoke(nameof(RewardPanelInvoke), 2f);
                        break;
                    case "FreeSpins":
                        _rewardText.text = "3 Free Spins";
                        //Debug.Log("Spin");
                        spin += 3;
                        if (spin == 0)
                        {
                            Invoke(nameof(RewardPanelInvoke), 2f);
                        }
                        break;
                    case "Coins":
                        _rewardText.text = "3000 Coins";
                        mGameManager._coins += 3000;
                        Invoke(nameof(RewardPanelInvoke), 2f);
                        break;
                    case "Energy":
                        _rewardText.text = "5 Energy";
                        mGameManager._energy += 5;
                        Invoke(nameof(RewardPanelInvoke), 2f);
                        break;
                    default:
                        break;
                }
            }
            else if( _elementsName[i + 1]._slotElementGameObject.name == _elementsName[i + 2]._slotElementGameObject.name)
            {
                switch (_elementsName[i + 1]._slotElementGameObject.name)
                {
                    case "TradingCards":
                        _rewardText.text = "Oops Try Again";
                        Invoke(nameof(RewardPanelInvoke), 2f);
                        break;
                    case "FreeSpins":
                        _rewardText.text = "3 Free Spins";
                        spin += 3;
                        if (spin == 0)
                        {
                            Invoke(nameof(RewardPanelInvoke), 2f);
                        }
                        break;
                    case "Coins":
                        _rewardText.text = "3000 Coins";
                        mGameManager._coins += 3000;
                        Invoke(nameof(RewardPanelInvoke), 2f);
                        break;
                    case "Energy":
                        _rewardText.text = "5 Energy";
                        mGameManager._energy += 5;
                        Invoke(nameof(RewardPanelInvoke), 2f);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                _rewardText.text = "Oopsie Nothing is Identical";
                Invoke(nameof(RewardPanelInvoke), 2f);
            }
        }
        
    }

    public void RewardPanelInvoke()
    {
        if (spin == 0 && _reels[2].mSpinOver == true)
        {
            _uiSpinButton.interactable = false;
            _rewardPanel.SetActive(true);
        }
    }
    public void ActiveLevelInvoke()
    {
        //SceneManager.LoadScene(1);
        mGameManager.GetComponent<LevelLoadManager>().BacktoHome();
    }
}












//#region All identical
//if (elementNames[0] == "5KCoins" && elementNames[1] == "5KCoins" && elementNames[2] == "5KCoins")
//{
//    mGameManager._coins += 5000;
//    Invoke(nameof(MainSceneInvoke), 2f);
//}
//else if (elementNames[0] == "25KCoins" && elementNames[1] == "25KCoins" && elementNames[2] == "25KCoins")
//{
//    mGameManager._coins += 25000;
//    Invoke(nameof(MainSceneInvoke), 2f);
//}
//else if (elementNames[0] == "100KCoins" && elementNames[1] == "100KCoins" && elementNames[2] == "100KCoins")
//{
//    mGameManager._coins += 100000;
//    Invoke(nameof(MainSceneInvoke), 2f);
//}
//else if (elementNames[0] == "500KCoins" && elementNames[1] == "500KCoins" && elementNames[2] == "500KCoins")
//{
//    mGameManager._coins += 500000;
//    Invoke(nameof(MainSceneInvoke), 2f);
//}
//else if (elementNames[0] == "1M" && elementNames[1] == "1M" && elementNames[2] == "1M")
//{
//    mGameManager._coins += 1000000;
//    Invoke(nameof(MainSceneInvoke), 2f);
//}
//else if (elementNames[0] == "10Energy" && elementNames[1] == "10Energy" && elementNames[2] == "10Energy")
//{
//    mGameManager._energy += 10;
//    Invoke(nameof(MainSceneInvoke), 2f);
//}
//else if (elementNames[0] == "25Energy" && elementNames[1] == "25Energy" && elementNames[2] == "25Energy")
//{
//    mGameManager._energy += 25;
//    Invoke(nameof(MainSceneInvoke), 2f);
//}
//else if (elementNames[0] == "100Energy" && elementNames[1] == "100Energy" && elementNames[2] == "100Energy")
//{
//    mGameManager._energy += 100;
//    Invoke(nameof(MainSceneInvoke), 2f);
//}
//#endregion
//#region Two Identical
//else if ((elementNames[0] == "5KCoins" && elementNames[1] == "5KCoins") || (elementNames[1] == "5KCoins" && elementNames[2] == "5KCoins") || (elementNames[0] == "5KCoins" && elementNames[2] == "5KCoins"))
//{
//    mGameManager._coins += 2500;
//    Invoke(nameof(MainSceneInvoke), 2f);

//}
//else if ((elementNames[0] == "25KCoins" && elementNames[1] == "25KCoins") || (elementNames[1] == "25KCoins" && elementNames[2] == "25KCoins") || (elementNames[0] == "25KCoins" && elementNames[2] == "25KCoins"))
//{
//    mGameManager._coins += 12500;
//    Invoke(nameof(MainSceneInvoke), 2f);

//}
//else if ((elementNames[0] == "100KCoins" && elementNames[1] == "100KCoins") || (elementNames[1] == "100KCoins" && elementNames[2] == "100KCoins") || (elementNames[0] == "100KCoins" && elementNames[2] == "100KCoins"))
//{
//    mGameManager._coins += 50000;
//    Invoke(nameof(MainSceneInvoke), 2f);

//}
//else if ((elementNames[0] == "500KCoins" && elementNames[1] == "500KCoins") || (elementNames[1] == "500KCoins" && elementNames[2] == "500KCoins") || (elementNames[0] == "500KCoins" && elementNames[2] == "500KCoins"))
//{
//    mGameManager._coins += 250000;
//    Invoke(nameof(MainSceneInvoke), 2f);

//}
//else if ((elementNames[0] == "1M" && elementNames[1] == "1M") || (elementNames[1] == "1M" && elementNames[2] == "1M") || (elementNames[0] == "1M" && elementNames[2] == "1M"))
//{
//    mGameManager._coins += 500000;
//    Invoke(nameof(MainSceneInvoke), 2f);

//}
//else if ((elementNames[0] == "10Energy" && elementNames[1] == "10Energy") || (elementNames[1] == "10Energy" && elementNames[2] == "10Energy") || (elementNames[0] == "10Energy" && elementNames[2] == "10Energy"))
//{
//    mGameManager._energy += 5;
//    Invoke(nameof(MainSceneInvoke), 2f);

//}
//else if ((elementNames[0] == "25Energy" && elementNames[1] == "25Energy") || (elementNames[1] == "25Energy" && elementNames[2] == "25Energy") || (elementNames[0] == "25Energy" && elementNames[2] == "25Energy"))
//{
//    mGameManager._energy += 12;
//    Invoke(nameof(MainSceneInvoke), 2f);
//}
//else if ((elementNames[0] == "100Energy" && elementNames[1] == "100Energy") || (elementNames[1] == "100Energy" && elementNames[2] == "100Energy") || (elementNames[0] == "100Energy" && elementNames[2] == "100Energy"))
//{
//    mGameManager._energy += 50;
//    Invoke(nameof(MainSceneInvoke), 2f);
//}
//#endregion





















//foreach (string s in elementNames)
//{
//    if (elementNames.Any(s => s.Contains("Diamond")))
//    {
//        Debug.Log("x");
//    }
//}
//elementNames.Clear();
//string localName = inReel.SlotElementsName;
//Debug.Log("Outcome: " + localName);
//if (elementNames[0] == localName && elementNames[1] == localName && elementNames[2] == localName)
//{
//    Debug.Log("Result: " + inReel.SlotElementsName);
//}
//else if ((elementNames[0] == localName && elementNames[1] == localName) || (elementNames[1] == localName && elementNames[2] == localName) || (elementNames[0] == localName && elementNames[2] == localName))
//{
//    Debug.Log("Result: " + inReel.SlotElementsName + "  But Only Two Came");
//}
//else if (elementNames[0] != localName && elementNames[1] != localName && elementNames[2] != localName)
//{
//    Debug.Log("Result: " + "Nothing is Identical");
//}
//else
//{
//    Debug.Log("x");
//}
//for (int i = 0; i < elementNames.Count; i++)
//{
//    if (elementNames[i] == localName)
//    {
//        Debug.Log("Result: " + inReel.SlotElementsName);
//    }
//    //else if ((elementNames[0] == localName && elementNames[1] == localName) || (elementNames[1] == localName && elementNames[2] == localName) || (elementNames[0] == localName && elementNames[2] == localName))
//    //{
//    //    Debug.Log("Result: " + inReel.SlotElementsName + "  But Only Two Came");
//    //}
//    //else if (elementNames[0] != localName && elementNames[1] != localName && elementNames[2] != localName)
//    //{
//    //    Debug.Log("Result: " + "Nothing is Identical");
//    //}
//}
//Residue()
//{
//public void SpinButtonFunctionality()
//{
//    //reels[0].Spin();
//    if (startSpin == false)
//    {
//        startSpin = true;

//    }
//}

//void Update()
//{



//}
//CheckResults();
//Allows The Machine To Be Started Again 
//resultsChecked = false;
//    //reels[i].RandomPosition();
//    //startSpin = false;
//    //yield return null;

//if (!rows[0].rowStopped || !rows[1].rowStopped || !rows[2].rowStopped)
//{
//    resultsChecked = false;
//}

//if (reels[0].rowStopped && reels[1].rowStopped && reels[2].rowStopped && !resultsChecked)
//{
//    CheckResults();
//}
//private void CheckResults()
//{
//    //Jackpot();
//    //Twosame();
//    ///this else if is checking whether anyone of the element is not equal
//    //if ((reels[0].stoppedSlot != reels[1].stoppedSlot) && (reels[0].stoppedSlot != reels[2].stoppedSlot) && (reels[1].stoppedSlot != reels[2].stoppedSlot))
//    //{
//    // //   Losepanel.SetActive(true);
//    //    Debug.Log("Better Luck Next Time");
//    //}

//    resultsChecked = true;
//}


//private void Jackpot()
//{

//    ///if 3 items  are equal then here we should give reward
//    if (reels[0].stoppedSlot == "Diamond" && reels[1].stoppedSlot == "Diamond" && reels[2].stoppedSlot == "Diamond")
//    {
//        // JackpotPanel.SetActive(true);
//        // jackpotTxt.text = "Hurray the jackpot you won is " + rows[0].stoppedSlot;
//        Debug.Log("3 are Diamonds");
//    }

//    else if (reels[0].stoppedSlot == "Crown" && reels[1].stoppedSlot == "Crown" && reels[2].stoppedSlot == "Crown")
//    {
//        //JackpotPanel.SetActive(true);
//        // jackpotTxt.text = "Hurray the jackpot you won is " + rows[0].stoppedSlot;
//        Debug.Log("3 are Crowns");
//    }

//    else if (reels[0].stoppedSlot == "Melon" && reels[1].stoppedSlot == "Melon" && reels[2].stoppedSlot == "Melon")
//    {
//        //  JackpotPanel.SetActive(true);
//        // jackpotTxt.text = "Hurray the jackpot you won is " + rows[0].stoppedSlot;
//        Debug.Log("3 are Melons");
//    }

//    else if (reels[0].stoppedSlot == "Bar" && reels[1].stoppedSlot == "Bar" && reels[2].stoppedSlot == "Bar")
//    {
//        //  JackpotPanel.SetActive(true);
//        // jackpotTxt.text = "Hurray the jackpot you won is " + rows[0].stoppedSlot;
//        Debug.Log("3 are Bars");
//    }

//    else if (reels[0].stoppedSlot == "Seven" && reels[1].stoppedSlot == "Seven" && reels[2].stoppedSlot == "Seven")
//    {
//        // JackpotPanel.SetActive(true);
//        // jackpotTxt.text = "Hurray the jackpot you won is " + rows[0].stoppedSlot;
//        Debug.Log("3 are Sevens");
//    }

//    else if (reels[0].stoppedSlot == "Cherry" && reels[1].stoppedSlot == "Cherry" && reels[2].stoppedSlot == "Cherry")
//    {
//        // JackpotPanel.SetActive(true);
//        // jackpotTxt.text = "Hurray the jackpot you won is " + rows[0].stoppedSlot;
//        Debug.Log("3 are Cherries");
//    }

//    else if (reels[0].stoppedSlot == "Lemon" && reels[1].stoppedSlot == "Lemon" && reels[2].stoppedSlot == "Lemon")
//    {
//        //  JackpotPanel.SetActive(true);
//        // jackpotTxt.text = "Hurray the jackpot you won is " + rows[0].stoppedSlot;
//        Debug.Log("3 are Lemons");
//    }
//}


//private void Twosame()
//{
//    ///If 2 items are same in any slot then reward should be given here

//    if (((reels[0].stoppedSlot == reels[1].stoppedSlot) && (reels[0].stoppedSlot == "Diamond"))
//       || ((reels[0].stoppedSlot == reels[2].stoppedSlot) && (reels[0].stoppedSlot == "Diamond"))
//       || ((reels[1].stoppedSlot == reels[2].stoppedSlot) && (reels[1].stoppedSlot == "Diamond")))
//    {
//        //Debug.Log(reels[0].stoppedSlot);
//        // Twosamepanel.SetActive(true);
//        // SameTxt.text = "You Won " + rows[0].stoppedSlot;
//        Debug.Log("Diamond only");
//    }


//    else if (((reels[0].stoppedSlot == reels[1].stoppedSlot) && (reels[0].stoppedSlot == "Crown"))
//        || ((reels[0].stoppedSlot == reels[2].stoppedSlot) && (reels[0].stoppedSlot == "Crown"))
//        || ((reels[1].stoppedSlot == reels[2].stoppedSlot) && (reels[1].stoppedSlot == "Crown")))
//    {
//        //Debug.Log(reels[0].stoppedSlot);
//        //   Twosamepanel.SetActive(true);
//        //  SameTxt.text = "You Won " + rows[0].stoppedSlot;
//        Debug.Log("Crown only");
//    }


//    else if (((reels[0].stoppedSlot == reels[1].stoppedSlot) && (reels[0].stoppedSlot == "Melon"))
//          || ((reels[0].stoppedSlot == reels[2].stoppedSlot) && (reels[0].stoppedSlot == "Melon"))
//          || ((reels[1].stoppedSlot == reels[2].stoppedSlot) && (reels[1].stoppedSlot == "Melon")))
//    {
//        //Debug.Log(reels[0].stoppedSlot);
//        // Twosamepanel.SetActive(true);
//        // SameTxt.text = "You Won " + rows[0].stoppedSlot;
//        Debug.Log("Melon only");
//    }



//    else if (((reels[0].stoppedSlot == reels[1].stoppedSlot) && (reels[0].stoppedSlot == "Bar"))
//          || ((reels[0].stoppedSlot == reels[2].stoppedSlot) && (reels[0].stoppedSlot == "Bar"))
//          || ((reels[1].stoppedSlot == reels[2].stoppedSlot) && (reels[1].stoppedSlot == "Bar")))
//    {
//        //Debug.Log(reels[0].stoppedSlot);
//        // Twosamepanel.SetActive(true);
//        // SameTxt.text = "You Won " + rows[0].stoppedSlot;
//        Debug.Log("Bar only");
//    }


//    else if (((reels[0].stoppedSlot == reels[1].stoppedSlot) && (reels[0].stoppedSlot == "Sevon"))
//        || ((reels[0].stoppedSlot == reels[2].stoppedSlot) && (reels[0].stoppedSlot == "Sevon"))
//        || ((reels[1].stoppedSlot == reels[2].stoppedSlot) && (reels[1].stoppedSlot == "Sevon")))
//    {
//        //Debug.Log(reels[0].stoppedSlot);
//        // Twosamepanel.SetActive(true);
//        // SameTxt.text = "You Won " + rows[0].stoppedSlot;
//        Debug.Log("Sevon only");
//    }


//    else if (((reels[0].stoppedSlot == reels[1].stoppedSlot) && (reels[0].stoppedSlot == "Cherry"))
//         || ((reels[0].stoppedSlot == reels[2].stoppedSlot) && (reels[0].stoppedSlot == "Cherry"))
//         || ((reels[1].stoppedSlot == reels[2].stoppedSlot) && (reels[1].stoppedSlot == "Cherry")))
//    {
//        //Debug.Log("Cherry only");
//        // Twosamepanel.SetActive(true);
//        // SameTxt.text = "You Won " + rows[0].stoppedSlot;
//        Debug.Log(reels[0].stoppedSlot);
//    }



//    else if (((reels[0].stoppedSlot == reels[1].stoppedSlot) && (reels[0].stoppedSlot == "Lemon"))
//        || ((reels[0].stoppedSlot == reels[2].stoppedSlot) && (reels[0].stoppedSlot == "Lemon"))
//        || ((reels[1].stoppedSlot == reels[2].stoppedSlot) && (reels[1].stoppedSlot == "Lemon")))
//    {
//        //Debug.Log(reels[0].stoppedSlot);
//        //  Twosamepanel.SetActive(true);
//        //  SameTxt.text = "You Won" + rows[0].stoppedSlot;
//        Debug.Log("Lemon only");
//    }

//}
//}