using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class AttackManager : MonoBehaviour
{
    [SerializeField] private GameManager mGameManager;
    // public List<GameObject> _TargetPoints = new List<GameObject>();
    public List<GameObject> _spawnedTargetPoints = new List<GameObject>();
    public GameObject _TargetPrefab;
    public GameObject _multiplierPrefab;
    public GameObject _multiplierGameObject;
    public GameObject _Cannon;
    public float _MultiplierSwitchTime = 1.0f;
    public GameObject _ScorePanel;
    public Text _ScoreTextOne;
    public Text _ScoreTextTwo;
    public Text _ScoreTextThree;
    public GameObject _bulletPre;
    public Sprite _Sprite1, _Sprite2, _Sprite3, _Sprite4, _Sprite5;
    public Transform _TargetTransform;
    public bool _Shield = false;
    public Quaternion CameraAttackRotation;
    public Vector3 CameraAttackPosition;
    public bool _AllowInteraction = true;
    public GameObject MultiplierGO;
    public GameObject _CanvasGO;
    public GameObject _TargetButton;
    public float HeightAdjustment = 100;
    public Sprite _TargetSprite;
    public Sprite _MultiplierSprite;
    public GameObject shieldPref;

    private Camera cam;
    private int cachedTargetPoint = -1;
    private int TargetObjectIndex;
    private float transitionDuration = 2.5f;



    private void Awake()
    {
        mGameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        for (int i = 0; i < mGameManager._BuildingDetails.Count; i++)
        {
            Instantiate(mGameManager._BuildingDetails[i], mGameManager._PositionDetails[i], mGameManager._RotationList[i]);
        }

    }

    private void Start()
    {
        cam = Camera.main;
       // Application.targetFrameRate = 60;
        Debug.Log(Application.targetFrameRate + "Target Fram Rate ");
        TargetInstantiation();
        //MultiplierInstantiation();
        InvokeRepeating("DoMultiplierSwitching", 0f, _MultiplierSwitchTime);

    }

    // Update is called once per frame
    void Update()
    {
        TargetButtonPositionUpdate();
    }

    /// <summary>
    /// This Function helps in moving the 2X Multiplier randomly Over the buildings
    /// </summary>
    void DoMultiplierSwitching()
    {
        if (_multiplierGameObject == null)
        {
            // newMultiplier = mGameManager._TargetMarkPost[0];
            _multiplierGameObject = _spawnedTargetPoints[0];
        }

        //if (cachedTargetPoint != -1)
        //  _spawnedTargetPoints[cachedTargetPoint].SetActive(true);
        if (cachedTargetPoint != -1)
            _spawnedTargetPoints[cachedTargetPoint].GetComponent<Image>().sprite = _TargetSprite;
        // int rand = Random.Range(0, mGameManager._TargetMarkPost.Count);
        int rand = Random.Range(0, mGameManager._BuildingDetails.Count);
        cachedTargetPoint = rand;
        _multiplierGameObject = _spawnedTargetPoints[cachedTargetPoint];
        _spawnedTargetPoints[cachedTargetPoint].GetComponent<Image>().sprite = _MultiplierSprite;
        //_multiplierGameObject.transform.localPosition = _spawnedTargetPoints[cachedTargetPoint].transform.localPosition;
        //_multiplierGameObject.transform.localRotation = _spawnedTargetPoints[cachedTargetPoint].transform.localRotation;
    }


    /// <summary>
    /// This helps in Instantiating the Target Mark on the Buildings.
    /// </summary>    
    void TargetInstantiation()
    {
        for (int i = 0; i < mGameManager._BuildingDetails.Count; i++)
        {

            GameObject go = Instantiate(_TargetButton) as GameObject; //GameObject.Instantiate(_Button);//Instantiate(_Button, Vector3.zero, Quaternion.identity) as Button;
            go.transform.SetParent(_CanvasGO.transform);
            Vector3 screenPos = Camera.main.WorldToScreenPoint(mGameManager._BuildingDetails[i].transform.position);
            screenPos.y = screenPos.y + HeightAdjustment;
            screenPos.z = 0;
            go.transform.position = screenPos;


            go.name = i.ToString();
            Debug.Log(i);
            _spawnedTargetPoints.Add(go);
            //this.gameObject.transform.LookAt(Camera.main.transform);
            go.GetComponent<Button>().onClick.AddListener(() =>
            {
                AssignTarget(mGameManager._BuildingDetails[int.Parse(go.name)].transform);
                if (mGameManager._BuildingShield[int.Parse(go.name)] == true)
                    _Shield = mGameManager._BuildingShield[int.Parse(go.name)];
                Debug.LogError(mGameManager._BuildingShield[int.Parse(go.name)] + " shield Yes or No Once selected ");
                TargetObjectIndex = int.Parse(go.name);
                Debug.Log(TargetObjectIndex + "TargetObjectIndex");
                Debug.Log("Target position Details  " + mGameManager._BuildingDetails[int.Parse(go.name)].transform.position);

            });

        }
    }

    public void TargetButtonPositionUpdate()
    {
        for (int i = 0; i < _spawnedTargetPoints.Count; i++)
        {

            GameObject go = _spawnedTargetPoints[i];
            Vector3 screenPos = Camera.main.WorldToScreenPoint(mGameManager._BuildingDetails[i].transform.position);
            screenPos.y = screenPos.y + HeightAdjustment;
            screenPos.z = 0;
            go.transform.position = screenPos;
        }
    }


    /// <summary>
    /// This Helps in Instantiating the 2X Multiplier 
    /// </summary>    
    void MultiplierInstantiation()
    {
        Vector3 newMultiplier = mGameManager._TargetMarkPost[0];
        _multiplierGameObject = Instantiate(_multiplierPrefab, newMultiplier, Quaternion.identity);
        _multiplierGameObject.name = 0.ToString();
    }


    /// <summary>
    /// This gets the Target mark Transform Details during on mouse Down click 
    /// </summary>
    /// <param name="trans"></param>
    public void AssignTarget(Transform trans)
    {
        Debug.LogError("Assign Target Called");

        if (_AllowInteraction == true)
        {
            _AllowInteraction = false;
            _TargetTransform = trans;
            GameObject CamParent = Camera.main.gameObject; //GameObject.Find("CameraParent");
            CamParent.GetComponent<AttackCameraController>()._CameraFreeRoam = false;


            for (int i = 0; i < mGameManager._BuildingCost.Count; i++)
            {
                if (mGameManager._BuildingShield[i] == true)
                {
                    _spawnedTargetPoints[i].transform.GetChild(0).gameObject.SetActive(true);
                    Debug.Log(_TargetTransform.name);
                    if (_multiplierGameObject == _TargetTransform)
                    {
                        //  Debug.Log("multiplier 2x");
                        // _multiplierGameObject.transform.GetChild(0).gameObject.SetActive(true);
                    }
                }

                switch (mGameManager._BuildingCost[i])
                {
                    case 1000:
                        _spawnedTargetPoints[i].GetComponent<Image>().sprite = _Sprite1;
                        break;
                    case 2000:
                        _spawnedTargetPoints[i].GetComponent<Image>().sprite = _Sprite2;
                        break;
                    case 3000:
                        _spawnedTargetPoints[i].GetComponent<Image>().sprite = _Sprite3;
                        break;
                    case 4000:
                        _spawnedTargetPoints[i].GetComponent<Image>().sprite = _Sprite4;
                        break;
                    case 5000:
                        _spawnedTargetPoints[i].GetComponent<Image>().sprite = _Sprite5;
                        break;
                }


            }
            switch (int.Parse(_multiplierGameObject.name))
            {
                case 0:
                    _multiplierGameObject.GetComponent<Image>().sprite = _Sprite1;
                    break;
                case 1:
                    _multiplierGameObject.GetComponent<Image>().sprite = _Sprite2;
                    break;
                case 2:
                    _multiplierGameObject.GetComponent<Image>().sprite = _Sprite3;
                    break;
                case 3:
                    _multiplierGameObject.GetComponent<Image>().sprite = _Sprite4;
                    break;
                case 4:
                    _multiplierGameObject.GetComponent<Image>().sprite = _Sprite5;
                    break;
            }

            Invoke("DisableBuildingCost", 2f);
            CancelInvoke("DoMultiplierSwitching");
            Invoke("PerformTarget", 2.1f);
        }
    }


    public void DisableBuildingCost()
    {
        for (int i = 0; i < _spawnedTargetPoints.Count; i++)
        {
            _spawnedTargetPoints[i].SetActive(false);

        }
        _multiplierGameObject.SetActive(false);
    }


    public void PerformTarget()
    {
        //int transIndex = int.Parse(_TargetTransform.gameObject.name);
        //_Shield = mGameManager._BuildingShield[transIndex];

        for (int i = 0; i < _spawnedTargetPoints.Count; i++)
        {
            //if (i != int.Parse(_TargetTransform.gameObject.name))
            {
                _spawnedTargetPoints[i].SetActive(false);
            }
        }
        // if (_multiplierGameObject.name != _TargetTransform.gameObject.name)
        {
            _multiplierGameObject.SetActive(false);
        }

        Debug.Log("before coroutine");
        StartCoroutine(Transition());
        Camera.main.transform.rotation = CameraAttackRotation;
        Invoke("CannonActivation", 0f);
        //  ScoreCalculation(_TargetTransform);
        StartCoroutine(ScoreCalculation(_TargetTransform));
        if (_Shield == true)
        {
            Debug.Log("shield Activated");
        }
        else
        {
            Debug.Log("shield Not Activated");
        }

    }

    public IEnumerator ScoreCalculation(Transform trans)
    {
        Debug.Log("Scoring Calculation function Entered");

        /*  int RewardValue = mGameManager._BuildingCost[int.Parse(trans.gameObject.name)];
          _ScoreTextOne.text = "Building Cost - " + RewardValue;
          if (trans.gameObject.name == _multiplierGameObject.name)
          {
              _ScoreTextTwo.text = "Multiplier (2x) - " + RewardValue + " * 2";
              RewardValue = RewardValue * 2;

          }
          _ScoreTextThree.text = "Your Score Are - " + RewardValue;  */

        int RewardValue = mGameManager._BuildingCost[TargetObjectIndex];
        mGameManager._coins = mGameManager._coins + mGameManager._BuildingCost[TargetObjectIndex];
        _ScoreTextOne.text = "Building Cost - " + RewardValue;

        yield return new WaitForSeconds(7);
        _ScorePanel.SetActive(true);
       //_ScoreTextThree.transform.parent.gameObject.SetActive(true);
        //Debug.Log("I am Here");

    }

    public void CannonActivation()
    {
        // _Cannon.SetActive(true);
        //float zpos = 700;//_Cannon.transform.position.z - _TargetTransform.position.z; // Difference between last building
        //_Cannon.transform.position = new Vector3(_Cannon.transform.position.x, _Cannon.transform.position.y, _TargetTransform.position.z - zpos);
        //Debug.Log("Cannon Activation function" + _TargetTransform);
        _Cannon.GetComponent<CannonShotController>().AssignPos(_TargetTransform);
    }

    IEnumerator Transition()
    {
        float t = 0.0f;
        Vector3 startingPos = Camera.main.transform.position;
        Vector3 endPos = new Vector3(_TargetTransform.localPosition.x, CameraAttackPosition.y, CameraAttackPosition.z);
        Debug.Log(startingPos);
        Debug.Log(endPos);

        while (t < 1.0f)
        {
            t += Time.deltaTime * (Time.timeScale / transitionDuration);
            Debug.Log("Inside Coroutine");

            Camera.main.transform.position = Vector3.Lerp(startingPos, endPos, t * 3);
            //GameObject temp = new GameObject();
            //temp.transform.LookAt(_TargetTransform);
            //Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, temp.transform.rotation, t*3);
            yield return 0;
        }
    }

    public void BackButton()
    {
        /*UnityEngine.SceneManagement.SceneManager.LoadScene("Dock");*/
        mGameManager.GetComponent<LevelLoadManager>().BacktoHome();
    }
}
