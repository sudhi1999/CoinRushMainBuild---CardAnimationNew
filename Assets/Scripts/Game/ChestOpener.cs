using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class ChestOpener : MonoBehaviour
{
    [Header("Energy Attributes: ")]
    [SerializeField] private EnergyProbability mEnergyProbability;
    [SerializeField] private List<GameObject> mEnergyChests;
    [SerializeField] private float CameraFocusSpeed;
    [SerializeField] private float dropSpeed;
    

    [SerializeField] private GameObject BackgroundParentRef;
    [SerializeField] private GameObject CloudRef;
    public bool EnergyFalling = true;

    [Header("Coin Attributes: ")]
    [SerializeField] private CoinProbability mCoinProbability;
    [SerializeField] private List<GameObject> mCoinChests;
    [SerializeField] private float ItemFocusSpeed;
    [SerializeField] private GameObject transparentBackgroundPlane;


    [Header("Other References: ")]
    [SerializeField] private GameManager mGameManager;
    [SerializeField] private TextMeshProUGUI rewardText;
    [SerializeField] private GameObject RewardDisplayPanel;

    private LevelLoadManager mlevelLoadManagerRef;

    private void Start()
    {
        EnergyFalling = true;
        mGameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        mlevelLoadManagerRef = mGameManager.gameObject.GetComponent<LevelLoadManager>();
    }


    private void Update()
    {
        

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit raycastHit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray,out raycastHit))
            {
                if (raycastHit.transform.gameObject.tag == "EnergyChestBox")
                {
                    //Get the Energy value of probability
                    int energyValue = mEnergyProbability.DisplayTheFinalElementBasedOnRandomValueGenerated();

                    //Changing the Energy value in Gamemanager
                    mGameManager._energy += energyValue;

                    //rewardText.text = energyValue.ToString();
                    //RewardDisplayPanel.SetActive(true);

                    //Assign it to chest which player clicks on and pass the values
                    GameObject Chest = raycastHit.transform.gameObject;
                    Chest.GetComponent<ChestValue>()._value = energyValue;

                    //EnergyListShuffle(mEnergyProbability._energies);
                    //Chest.transform.Find("WindTrialEffect").gameObject.SetActive(true);
                    StartCoroutine(CameraZoomAndFollowEnergy(Chest));
                    //CameraFocusEnergyCrate(Chest);
                    
                    //Destroy other chests except the ones clicked
                    for (int i = 0; i < mEnergyChests.Count; i++)
                    {
                        if (mEnergyChests[i].transform.GetChild(0).name != Chest.name)
                        {
                            //mEnergyChests[i].GetComponent<BoxCollider>().enabled = false;
                            Destroy(mEnergyChests[i].transform.gameObject , 2f);
                        }
                        else
                        {
                            continue;
                        }
                    }
                    //Invoke(nameof(BackToMainScene), 1f);
                }
                if (raycastHit.transform.gameObject.tag == "CoinChestBox")
                {
                    //Get the Coin value of probability
                    int coinValue = mCoinProbability.DisplayTheFinalElementBasedOnRandomValueGenerated();
                    //rewardText.text = coinValue.ToString();
                    //RewardDisplayPanel.SetActive(true);

                    //Changing the Coinvalue
                    mGameManager._coins += coinValue;

                    //Assign it to chest which player clicks on and pass the values
                    GameObject Chest = raycastHit.transform.gameObject;
                    Chest.GetComponent<ChestValue>()._value = coinValue;

                    StartCoroutine(PiggyBankFocus(Chest));
                    
                }
            }
        }
    }

    //void CameraFocusEnergyCrate(GameObject inChest)
    //{
    //    inChest.transform.GetChild(inChest.transform.childCount - 2).gameObject.SetActive(true);
    //    inChest.transform.rotation = Quaternion.identity;
    //    //BackgroundParentRef.GetComponent<BackgroundScrolling>().mScrollSpeed = 100;
    //    //CloudRef.GetComponent<BackgroundScrolling>().mScrollSpeed = 150;
    //    inChest.transform.parent.GetComponent<Animator>().SetTrigger("isFalling?");
    //    Vector3 targetPosition = new Vector3(inChest.transform.position.x, inChest.transform.position.y + 10, inChest.transform.position.z - 50);
    //    Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, targetPosition, CameraFocusSpeed * Time.deltaTime);
    //    Camera.main.transform.SetParent(inChest.transform);
    //    inChest.GetComponent<Rigidbody>().velocity = new Vector3(0, -(dropSpeed) * Time.deltaTime, 0);
    //}

    public IEnumerator CameraZoomAndFollowEnergy(GameObject inChest)
    {
        while (true)
        {
            //Make the chest rotation to zero
            inChest.transform.rotation = Quaternion.identity;
            
            //Make the camera to zoom-in
            Vector3 targetPosition = new Vector3(inChest.transform.position.x, inChest.transform.position.y + 10, inChest.transform.position.z - 50);
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, targetPosition, CameraFocusSpeed * Time.deltaTime);
            Camera.main.transform.SetParent(inChest.transform);

            

            if (EnergyFalling)
            {
                //Change the Animator
                inChest.transform.parent.GetComponent<Animator>().SetTrigger("isFalling?");

                //Make the camera fall down
                inChest.GetComponent<Rigidbody>().velocity = new Vector3(0, -(dropSpeed), 0);

                //Play the falling particle Effect
                inChest.transform.GetChild(inChest.transform.childCount - 2).gameObject.SetActive(true);
            }



            //Change the Scrolling Speed to make it look faster
            BackgroundParentRef.GetComponent<BackgroundScrolling>().mScrollSpeed = 100;
            CloudRef.GetComponent<BackgroundScrolling>().mScrollSpeed = 150;



            yield return null;
        }
    }

    private IEnumerator PiggyBankFocus(GameObject inChest)
    {
        while (true)
        {
            Vector3 targetPosition = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y + 15f, Camera.main.transform.position.z + 5);
            inChest.transform.position = Vector3.Lerp(inChest.transform.position, targetPosition, ItemFocusSpeed * Time.deltaTime);

            transparentBackgroundPlane.GetComponent<Renderer>().material.SetFloat("_alpha", Mathf.SmoothStep(0,0.8f, 1 * Time.deltaTime));
            
            yield return null;
        }
    }

    void UpdateBackground()
    {
        
    }

    /// <summary>
    /// Loads back to active level
    /// </summary>
    public void BackToMainScene()
    {
        mlevelLoadManagerRef.BacktoHome(); //Need to change it from zero to some other value. Will be doing that when scene save system is Done.
    }
}


//Coin
//CoinListShuffle(mCoinProbability._coins);

//Assign other values from Coin's Array to Chest Value apart from the Value that we got above
//for (int i = 0; i < mCoinChests.Count; i++)
//{
//    if (mCoinChests[i].GetComponent<ChestValue>()._value == coinValue)
//    {
//        mCoinChests.Remove(mCoinChests[i]);
//    }
//    if (mCoinProbability._coins[i]._coinAmount == coinValue)
//    {
//        mCoinProbability._coins.Remove(mCoinProbability._coins[i]);
//    }
//    mCoinChests[i].GetComponent<ChestValue>()._value = mCoinProbability._coins[i]._coinAmount;
//}
//Invoke(nameof(BackToMainScene), 1f);
//Camera.main.transform.SetParent(inChest.transform);
//inChest.GetComponent<Rigidbody>().velocity = new Vector3(0, dropSpeed * Time.deltaTime, 0);
//Destroy(inChest.transform.parent.GetComponent<Animator>());

//inChest.transform.rotation = Quaternion.identity;


//Energy
//Provides remaining energy to other boxes except the one that was selected by probability

//if (mEnergyChests[i].GetComponent<ChestValue>()._value == energyValue)
//{
//    mEnergyChests.Remove(mEnergyChests[i]);
//}
//if (mEnergyProbability._energies[i]._energyAmount == energyValue)
//{
//    mEnergyProbability._energies.Remove(mEnergyProbability._energies[i]);
//}
//mEnergyChests[i].GetComponent<ChestValue>()._value = mEnergyProbability._energies[i]._energyAmount;

/// <summary>
/// Below both the methods are done for natural looking purposes after clicking it shuffles the list randomly which sometimes makes the higher values to be beside what
/// we clicked creating the anticipation factor.
/// </summary>
/// <param name="inEnergyList"></param>
//private void EnergyListShuffle(List<Energy> inEnergyList)
//{
//    for (int i = 0; i < inEnergyList.Count; i++)
//    {
//        Energy temp = inEnergyList[i];
//        int randomIndex = Random.Range(i, inEnergyList.Count);
//        inEnergyList[i] = inEnergyList[randomIndex];
//        inEnergyList[randomIndex] = temp;
//    }
//}
//private void CoinListShuffle(List<Coins> inCoinList)
//{
//    for (int i = 0; i < inCoinList.Count; i++)
//    {
//        Coins temp = inCoinList[i];
//        int randomIndex = Random.Range(i, inCoinList.Count);
//        inCoinList[i] = inCoinList[randomIndex];
//        inCoinList[randomIndex] = temp;
//    }
//}