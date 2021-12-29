using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class CoinSelector : MonoBehaviour
{
    [Header ("Coin Scene References: ")]
    [SerializeField] private CoinProbability mCoinProbability;
    [SerializeField] private GameObject[] mCoinChests; //Optional
    [SerializeField] private GameObject transparentBackgroundPlane;
    [SerializeField] private float mFadeTime;

    [Header ("Other References: ")]
    [SerializeField] private GameManager mGameManager;
    [SerializeField] private TextMeshProUGUI rewardText;
    [SerializeField] private GameObject RewardDisplayPanel;
    [SerializeField] private float mTimeToOpenRewardPanel;

    [Header("Hammer Spawn And HammerHit Particles: ")]
    [SerializeField] private GameObject HammerPrefab;
    [SerializeField] private Transform HammerSpawnPoint;
    private Transform HammerHitPigSpawnPoint;    
    [SerializeField] private GameObject HammerHitPigParticle;
    [SerializeField] private GameObject HammerSpawnParticle;

    [Header("Pig Selection And Its Related Particles: ")]
    [SerializeField] private float mHowMiddleToCameraY;
    [SerializeField] private float mHowFarFromCameraZ;
    [SerializeField] private float mTimeBetweenHammerSpawnAndBreakAnimation;
    [SerializeField] private float mTimeBetweenPigBreakAndCoinShower;
    [SerializeField] private GameObject GodRaysParticleSystem;
    private Transform GodRaysSpawnPoint;
    [SerializeField] private GameObject CoinShowerParticleEffect;
    private Transform CoinShowerSpawnPoint;

    private LevelLoadManager mlevelLoadManagerRef;

    private void Start()
    {
        mGameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        mlevelLoadManagerRef = mGameManager.gameObject.GetComponent<LevelLoadManager>();
    }

    private void OnMouseDown()
    {
        int coinValue = mCoinProbability.DisplayTheFinalElementBasedOnRandomValueGenerated();
        
        //Changing the Coinvalue
        rewardText.text = coinValue.ToString();
        mGameManager._coins += coinValue;

        //Assign it to chest which player clicks on and pass the values
        GameObject SelectedPig = this.gameObject;
        Debug.Log(SelectedPig);
        SelectedPig.GetComponent<ChestValue>()._value = coinValue;

        CoinShowerSpawnPoint = SelectedPig.transform.Find("CoinShowerSpawnPoint").transform;
        GodRaysSpawnPoint = SelectedPig.transform.Find("GodRaysParticleEffect").transform;
        HammerHitPigSpawnPoint = SelectedPig.transform.Find("HammerHitParticleEffect").transform;

        StartCoroutine(PiggyBankFocus(SelectedPig));

        //Optional code for disabling Animators
        //for (int i = 0; i < mCoinChests.Length; i++)
        //{
        //    if (mCoinChests[i].transform.name != SelectedPig.name)
        //    {
        //        Destroy(mCoinChests[i].transform.GetComponent<Animator>());
        //    }
        //}
    }

    private IEnumerator PiggyBankFocus(GameObject inPigSelected)
    {
        //On Pig Selected
        Destroy(inPigSelected.GetComponent<BoxCollider>());
        inPigSelected.GetComponent<Animator>().SetTrigger("isSelected?");
        //Movement to target position
        Vector3 targetPosition = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y - mHowMiddleToCameraY, Camera.main.transform.position.z + mHowFarFromCameraZ);
        transparentBackgroundPlane.GetComponent<Renderer>().material.DOFloat(0.8f, "_alpha", mFadeTime);
        inPigSelected.transform.DOMove(targetPosition, 1, false)
        .OnComplete(() =>
        {
            PlayParticleEffects(GodRaysParticleSystem, GodRaysSpawnPoint,1f);
            PlayParticleEffects(HammerSpawnParticle , HammerSpawnPoint,1f);
            GameObject HammerRef = Instantiate(HammerPrefab, HammerSpawnPoint.position, HammerSpawnPoint.rotation);
            //Debug.Log(HammerRef);
            Destroy(HammerRef, 1f);
            Debug.Log("I read all the above lines");
        });
        
        //To Prevent Clicking Other Pigs in the background
        transparentBackgroundPlane.GetComponent<BoxCollider>().enabled = true;

        yield return new WaitForSeconds(mTimeBetweenHammerSpawnAndBreakAnimation);  //Yield to Wait for the Hammer
        inPigSelected.GetComponent<Animator>().SetTrigger("isBreaking?");
        yield return new WaitForSeconds(0.25f);
        PlayParticleEffects(HammerHitPigParticle, HammerHitPigSpawnPoint,2);
        
        yield return new WaitForSeconds(mTimeBetweenPigBreakAndCoinShower);
        PlayParticleEffects(CoinShowerParticleEffect , CoinShowerSpawnPoint,2);
        Invoke("DisplayRewardAndInvokeScene", 2f);
        //Spawn ParticleEffect

    }

    void PlayParticleEffects(GameObject inParticleEffectGameObject, Transform inParticleSpawnPosition , float DestroySeconds)
    {
        GameObject ParticleRef = Instantiate(inParticleEffectGameObject, inParticleSpawnPosition.position, Quaternion.identity);
        Destroy(ParticleRef, DestroySeconds);
        //Play the particles
    }

    void DisplayRewardAndInvokeScene()
    {
        RewardDisplayPanel.SetActive(true);
        //UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void BackToMainScene()
    {
        mlevelLoadManagerRef.BacktoHome(); //Need to change it from zero to some other value. Will be doing that when scene save system is Done.
    }
}


//void Residue()
//{
//float v = 0.0f;
//public float easing = 1f;

//rewardText.text = coinValue.ToString();
//RewardDisplayPanel.SetActive(true);


//    //float t = 0;
//    //while (t <= 1.0)
//    //{
//    //    t += Time.deltaTime / easing;
//    //    transparentBackgroundPlane.GetComponent<Renderer>().material.SetFloat("_alpha", v = Mathf.Lerp(v, 0.8f, Mathf.SmoothStep(0f, 1f, t)));
//    //    //Invoke("DisplayRewardAndInvokeScene", 1.5f);
//    //}
//    //yield return null;
//}