using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class SpinWheelSpin : MonoBehaviour
{
    public Button _uiSpinButton;
    public GameObject _uiReturnToGame;
    public GameObject _backButton;
    public TextMeshProUGUI _uiCoinValue;
    public TextMeshProUGUI _uiFreeSpinValue;
    public TextMeshProUGUI _uiEnergyValue;
    public TextMeshProUGUI _uiFreeSpinCountValue;
    public Sprite _uiCoinSprite;
    public Sprite _uiFreeSpinSprite;
    public Sprite _uiEnergySprite;
    public GameObject _uiCoinReward;
    public GameObject _uiEnergyReward;
    public GameObject _uiFreeSpinReward;   
    public int coin = 0;
    public int Energy = 0;
    public int FreeSpins = 0;
    public bool DoFreeSpins = false;
    public bool disablePanel = false;
    [SerializeField] private SpinWheel spinWheel;
    private GameManager mGameManager;
    public Animator mLightAnimator;
    private void Start()
    {
         mGameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        _uiSpinButton.onClick.AddListener(() =>
        {
            _uiSpinButton.interactable = false;

            spinWheel.OnSpinEnd(wheelPiece =>
            {
                mLightAnimator.SetBool("Spin",false);

                if (FreeSpins == 0)
                {
                    DoFreeSpins = false;
                }
                
                switch (DoFreeSpins)
                {
                    case false:
                        _uiReturnToGame.SetActive(true);
                        if (wheelPiece._Icon == _uiCoinSprite)
                        {
                            Debug.Log(wheelPiece._Icon.name);
                            _uiCoinReward.SetActive(true);
                            coin += wheelPiece._Amount;
                            mGameManager._coins += wheelPiece._Amount;
                            _uiCoinValue.text = wheelPiece._Amount.ToString();

                        }
                        if (wheelPiece._Icon == _uiEnergySprite)
                        {
                            Debug.Log(wheelPiece._Icon.name);
                            _uiEnergyReward.SetActive(true);
                            Energy += wheelPiece._Amount;
                            mGameManager._energy += wheelPiece._Amount;
                            _uiEnergyValue.text = wheelPiece._Amount.ToString();
                        }
                        if (wheelPiece._Icon == _uiFreeSpinSprite)
                        {
                            Debug.Log(wheelPiece._Icon.name);
                            _uiFreeSpinReward.SetActive(true);
                            FreeSpins += wheelPiece._Amount;
                            _uiFreeSpinValue.text = wheelPiece._Amount.ToString();
                            _backButton.SetActive(false);
                            Invoke("BackToSpinWheel", 0.7f);
                        }
                        break;


                    case true:
                        _uiReturnToGame.SetActive(true);
                        _backButton.SetActive(false);
                        _uiSpinButton.interactable = true;
                        if (wheelPiece._Icon == _uiCoinSprite)
                        {
                            disablePanel = true;
                            _uiCoinReward.SetActive(true);
                            coin += wheelPiece._Amount;
                            mGameManager._coins += wheelPiece._Amount;
                            _uiCoinValue.text = wheelPiece._Amount.ToString();
                        }
                        if (wheelPiece._Icon == _uiEnergySprite)
                        {
                            disablePanel = true;
                            _uiEnergyReward.SetActive(true);
                            Energy += wheelPiece._Amount;
                            mGameManager._energy += wheelPiece._Amount;
                            _uiEnergyValue.text = wheelPiece._Amount.ToString();                                                      
                        }
                        if (wheelPiece._Icon == _uiFreeSpinSprite)
                        {
                            _uiFreeSpinReward.SetActive(true);
                            FreeSpins += wheelPiece._Amount;
                            _uiFreeSpinValue.text = wheelPiece._Amount.ToString();
                            Invoke("BackToSpinWheel", 0.7f);
                        }
                        break;
                }
                _uiSpinButton.interactable = true;
            });
            spinWheel.Spin();

        });
    }
    void BackToSpinWheel()
    {
        _uiReturnToGame.SetActive(false);
        _uiFreeSpinReward.SetActive(false);
        _backButton.SetActive(true);
    }
    void BackToWheelOnFreeSpins()
    {
        _uiReturnToGame.SetActive(false);
        _uiCoinReward.SetActive(false);
        _uiEnergyReward.SetActive(false);
        _uiFreeSpinReward.SetActive(false);
        _backButton.SetActive(true);
        Debug.Log("Goback togame");
    }
    public void BackToGameScene()
    {
        //SceneManager.LoadScene(1);
        mGameManager.GetComponent<LevelLoadManager>().BacktoHome();
    }
    private void Update()
    {    if(FreeSpins<10)
        {
            _uiFreeSpinCountValue.text = FreeSpins.ToString();
        }
        else
        {
            _uiFreeSpinCountValue.text = FreeSpins.ToString();
        }
      
        if(disablePanel)
        {
            Invoke("BackToWheelOnFreeSpins", 0.7f);
            disablePanel = false;
        }
    }
}
