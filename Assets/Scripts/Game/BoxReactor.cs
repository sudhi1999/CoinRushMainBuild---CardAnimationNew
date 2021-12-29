using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

public class BoxReactor : MonoBehaviour
{
    [SerializeField] private GameObject RewardDisplayPanel;
    [SerializeField] private TextMeshProUGUI rewardText;

    [SerializeField] private GameObject EnergyCanSmall;
    [SerializeField] private GameObject EnergyCanMedium;
    [SerializeField] private GameObject EnergyCanLarge;

    private GameObject canSpawnLocation;

    EnergySelector energySelector;

    [SerializeField] private float RewardDisplayInvokeTime;

    private bool isCanInstantiated;
    private bool isCollided;

    public GameObject can;

    [Header("Camera Shake Values: ")]
    [SerializeField] private float mDuration;
    [SerializeField] private int mStrength;
    [SerializeField] private int mVibration;
    [SerializeField] private int mRandomness;

    [Header("Can Values And Camera Zoom In Properties: ")]
    [SerializeField] private float mEndGameCanScaleValue;
    [SerializeField] private float mCanYHeight;
    [SerializeField] private float mCameraYHeight;
    [SerializeField] private float mCameraZoomAmount;

    [SerializeField] private float mCanMoveDuration;
    [SerializeField] private float mCameraMoveDuration;

    [SerializeField] private GameObject HitSmokeEffect;
    [SerializeField] private GameObject HitSmokeRingEffect;

    private LevelLoadManager mLevelLoadManagerRef;

    private void Start()
    {
        mLevelLoadManagerRef = GameObject.Find("GameManager").GetComponent<LevelLoadManager>();
        isCollided = false; 
        isCanInstantiated = false;
    }

    void PlayParticleEffects(GameObject inChest)
    {
        Transform CollisionSmokeSpawnPoint = inChest.transform.Find("CollisionSmokeSpawner").transform;
        Transform HitSmokeSpreadLocation = inChest.transform.Find("HitSmokeSpread").transform;

        GameObject Particle1 = Instantiate(HitSmokeEffect, CollisionSmokeSpawnPoint.position,Quaternion.identity);
        GameObject Particle2 = Instantiate(HitSmokeRingEffect, HitSmokeSpreadLocation.position, Quaternion.identity);
        Destroy(Particle1, 1f);
        Destroy(Particle2, 1f);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!isCollided)
        {
            if (other.gameObject.tag == "EnergyChestBox")
            {
                isCollided = true;
                //Destroy(other.gameObject.GetComponent<BoxCollider>());
                

                PlayParticleEffects(other.gameObject);

                energySelector = other.gameObject.GetComponent<EnergySelector>();
                //Stopping Coroutine Just in Case
                energySelector.EnergyFalling = false;
                Camera.main.DOShakePosition(mDuration, mStrength, mVibration, mRandomness, true);

                other.gameObject.GetComponent<Rigidbody>().isKinematic = true;

                //Disabling the particle Effect
                other.transform.Find("Wind_Effect").gameObject.SetActive(false);

                //Getting the can spawn location to spawn the can
                canSpawnLocation = other.transform.Find("CanSpawnLocation").gameObject;

                //Getting references
                Animator crateAnimRef = other.transform.parent.gameObject.GetComponent<Animator>();
                ChestValue crateValueRef = other.gameObject.GetComponent<ChestValue>();

                if (!isCanInstantiated)
                    //Instantiating cans based on chest value
                    switch (other.gameObject.GetComponent<ChestValue>()._value)
                    {
                        case 10:
                            can = Instantiate(EnergyCanSmall, canSpawnLocation.transform.position, Quaternion.identity);
                            crateAnimRef.SetTrigger("isBreaking?");
                            isCanInstantiated = true;
                            rewardText.text = crateValueRef._value.ToString() + " Energies";
                            Invoke("InvokeKinematic", .75f);
                            break;
                        case 25:
                            can = Instantiate(EnergyCanMedium, canSpawnLocation.transform.position, Quaternion.identity);
                            crateAnimRef.SetTrigger("isBreaking?");
                            isCanInstantiated = true;
                            rewardText.text = crateValueRef._value.ToString() + " Energies";
                            Invoke("InvokeKinematic", .75f);
                            break;
                        case 100:
                            can = Instantiate(EnergyCanLarge, canSpawnLocation.transform.position, Quaternion.identity);
                            crateAnimRef.SetTrigger("isBreaking?");
                            isCanInstantiated = true;
                            rewardText.text = crateValueRef._value.ToString() + " Energies";
                            Invoke("InvokeKinematic", .75f);
                            break;
                    }
                StartCoroutine(CanGameObjectZoomIn(can));

                //Starting to activate Reward Panel
                //Invoke("ActiveRewardPanel", 3f);
            }
        }
    }

    IEnumerator CanGameObjectZoomIn(GameObject inCan)
    {
        yield return new WaitForSeconds(.5f);
        //while (true)
        //{
        Vector3 canHeightTargetPosition = new Vector3(inCan.transform.position.x, inCan.transform.position.y + mCanYHeight, inCan.transform.position.z);
        Vector3 cameraTargetPosition = new Vector3(inCan.transform.position.x, inCan.transform.position.y + mCameraYHeight, inCan.transform.position.z - mCameraZoomAmount);
        //inCan.transform.position = Vector3.Lerp(inCan.transform.position, canHeightTargetPosition, 1 * Time.deltaTime);
        inCan.transform.DOMove(canHeightTargetPosition, mCanMoveDuration, false)/*.OnUpdate(()=> can.transform.GetChild(0).gameObject.SetActive(true))*/.OnComplete(() => can.transform.GetChild(0).gameObject.SetActive(true));

        yield return new WaitForSeconds(.3f);
        inCan.transform.DOScale(mEndGameCanScaleValue, 1);
        Camera.main.transform.DOMove(cameraTargetPosition, mCameraMoveDuration, false);

        // Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, targetPosition, 2 * Time.deltaTime);
        Invoke("ActiveRewardPanel", 2f);

        yield return null;
        //}
    }

    void InvokeKinematic()
    {
        can.GetComponent<Rigidbody>().isKinematic = true;
    }

    void ActiveRewardPanel()
    {
        RewardDisplayPanel.SetActive(true);
    }

    public void BackToMainScene()
    {
        //UnityEngine.SceneManagement.SceneManager.LoadScene(1); //Need to change it from zero to some other value. Will be doing that when scene save system is Done.
        GameManager.Instance.GetComponent<LevelLoadManager>().BacktoHome();
    }
}



//other.transform.parent.GetComponent<Animator>().SetTrigger("isBreaking?");
//rewardText.text = other.gameObject.GetComponent<ChestValue>()._value.ToString() + "Energy";