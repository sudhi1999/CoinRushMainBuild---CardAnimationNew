

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class CardDeck : MonoBehaviour
{
    [Header("Grabbing Other GameObject References:")]
    [SerializeField] private GameManager mGameManager;
    [SerializeField] private GameObject mCardHolderParent;
    private int clicks = 0;
    public List<Transform> _playerHandPoints;

    [Space(10)]
    [Header("Cards And Related Lists:")]
    [SerializeField] private List<ScriptedCards> mScriptedCards;
    [SerializeField] private List<Cards> _CardList = new List<Cards>();
    [SerializeField] private List<GameObject> mCardListGameObject;
    private List<Vector3> _PositionList = new List<Vector3>();
   /* private List<Quaternion> _RotationList = new List<Quaternion>();*/
    private List<Vector3> _RotationList = new List<Vector3>();

    [Space(10)]
    [Header("Draw Button And Its States Images with conditions:")]
    [SerializeField] private Image DrawButton;
    [SerializeField] private Sprite drawNormal, drawAutomatic;
    [SerializeField] private RectTransform _drawButtonRectTransform;
    [Space(10)]
    [SerializeField] private int mMaxHoldTime = 5;
    [SerializeField] private float timeForCardAnimation = 3f;
    [SerializeField] private float time = 0, maxTime = 5;
    private bool mAutoCardDraw = false;
    private bool mAutomaticDrawModeOn = false;
    private bool mOnceDone = false;
    private bool canClick = true;
    
    public Image _drawButtonFillerImage;

    [Space(10)]
    [Header("Joker and related things")]
    public List<Cards> _jokerList;
    public bool onceDonee = false;

    public int mHowManyCardSetsAreActive;
    public List<Cards> _cardsThatCanBeReplacedByJoker;

    public List<GameObject> dummyCards;
    int positionNumber = 0;
    int newCardIndex = 0;

    public GameObject cardDeckAnimation2D;
    public GameObject cardDeckAnimation3D;
    bool mMakeDrawBtnEnable=true;

    ScriptedCards cards;
    bool threeCardMatch;
    int threeCardMatchIndex;
    bool hasJoker;
    int numOfPairCards;
    bool jokerFindWithMultiCardPair;
    bool take_Multi_Card_Joker_Pair_Input;
    int[] selectionCards = new int[2]; 
    GameObject flotingJoker;

    //[Range(0, 100)]
    [SerializeField] int jokerProbability;

    private void Start()
    {
        onceDonee = false;
        canClick = true;
        DrawButton.sprite = drawNormal;
        mGameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (GameManager.Instance._SavedCardTypes.Count>0)
        {
            Camera.main.GetComponent<CameraController>().DrawButtonClicked();
            foreach (int cardType in GameManager.Instance._SavedCardTypes)
            {
                InstantiateCard(GetScriptedCardWithCardType((CardType)cardType),true);
            }
        }
    }

    private void DestroyCardList()
    {
        foreach (GameObject card in mCardListGameObject)
        {
            Destroy(card);
        }
        _CardList.Clear();
        _jokerList.Clear();
        newCardIndex=0;
        cardDeckAnimation2D.SetActive(false);
        mCardListGameObject.Clear();
        GameManager.Instance._SavedCardTypes.Clear();//Clear the card type list in gameManager
    }

    private void Update()
    {
        if (clicks == 8&&!threeCardMatch)
        {
            clicks = 0;
            Invoke(nameof(DestroyCardList), 2f);
        }
        
        time = Mathf.Clamp(time, 0f, mMaxHoldTime);
        Vector2 localMousePosition = _drawButtonRectTransform.InverseTransformPoint(Input.mousePosition);

        //if(Input.GetKeyDown(KeyCode.Space))
        //{

        //}


        if (take_Multi_Card_Joker_Pair_Input)
        {
            Vector2 selectionCardPosOne =_CardList[selectionCards[0]].gameObject.GetComponent<RectTransform>().InverseTransformPoint(Input.mousePosition);
            Vector2 selectionCardPosTwo = _CardList[selectionCards[1]].gameObject.GetComponent<RectTransform>().InverseTransformPoint(Input.mousePosition);
            if (Input.GetMouseButtonDown(0))
            {
                if (_CardList[selectionCards[0]].gameObject.GetComponent<RectTransform>().rect.Contains(selectionCardPosOne))
                {
                    Debug.Log("oneSelected");
                    _CardList[selectionCards[0] - 1].PlayThreeCardMatchAnim(-350);
                    flotingJoker.GetComponent<Cards>().PlayThreeCardMatchAnim(0, _CardList[selectionCards[0]].gameObject.GetComponent<Image>().sprite);
                    _CardList[selectionCards[0]].PlayThreeCardMatchAnim(350);

                    _CardList[selectionCards[1] - 1].PlayJokerSelectionPairGetBackAnim();
                    _CardList[selectionCards[1]].PlayJokerSelectionPairGetBackAnim();
                    Debug.Log(selectionCards[0]);
                    _CardList.RemoveAt(selectionCards[0] - 1);
                    _CardList.RemoveAt(selectionCards[0] - 1);
                    clicks -= 3;
                    take_Multi_Card_Joker_Pair_Input = false;
                    ReplacementOfCards(true);
                }
                else if (_CardList[selectionCards[1]].gameObject.GetComponent<RectTransform>().rect.Contains(selectionCardPosTwo))
                {
                    Debug.Log("TwoSelected");
                    _CardList[selectionCards[1] - 1].PlayThreeCardMatchAnim(-350);
                    flotingJoker.GetComponent<Cards>().PlayThreeCardMatchAnim(0, _CardList[selectionCards[1]].gameObject.GetComponent<Image>().sprite);
                    _CardList[selectionCards[1]].PlayThreeCardMatchAnim(350);

                    _CardList[selectionCards[0] - 1].PlayJokerSelectionPairGetBackAnim();
                    _CardList[selectionCards[0]].PlayJokerSelectionPairGetBackAnim();
                    //_CardList.RemoveRange(selectionCards[1] - 1, 2);
                    _CardList.RemoveAt(selectionCards[1] - 1);
                    _CardList.RemoveAt(selectionCards[1] - 1);
                    clicks -= 3;
                    Debug.Log(selectionCards[1]);
                    take_Multi_Card_Joker_Pair_Input = false;
                    ReplacementOfCards(true);

                }
            }
        }

        if (canClick == true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (_drawButtonRectTransform.rect.Contains(localMousePosition) && DrawButton.gameObject.activeInHierarchy == true&& mMakeDrawBtnEnable)
                {
                    //BackToNormalState();\
                    mMakeDrawBtnEnable = false;

                    time = 0;                
                    DrawCard();
                }
            }

            if (!mOnceDone)
            {
                if (Input.GetMouseButton(0))
                {
                    if (_drawButtonRectTransform.rect.Contains(localMousePosition))
                    {
                        if (!mAutomaticDrawModeOn)
                        {
                            DrawButton.color = new Color32(200, 200, 200, 255);
                        }

                        time += Time.deltaTime;
                        var displayValue = Mathf.Lerp(0, 1, time / mMaxHoldTime);
                        //CircleOutlineFiller();
                        _drawButtonFillerImage.fillAmount = displayValue;//Mathf.Lerp(0, 1, 3f * Time.fixedDeltaTime);


                        if (time >= mMaxHoldTime)
                        {
                            ChangeSprites();

                            mOnceDone = true;
                            mAutomaticDrawModeOn = true;
                            mAutoCardDraw = true;                            
                            StartCoroutine(AutomaticCardDrawing());
                        }
                    }
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                _drawButtonFillerImage.fillAmount = 0;
                DrawButton.color = new Color32(255, 255, 255, 255);
                if (_drawButtonRectTransform.rect.Contains(localMousePosition))
                {
                    time = 0;
                }

            }
        }
        
    }

    void CircleOutlineFiller()
    {
        _drawButtonFillerImage.DOFillAmount(1, 3);
    }

    /// <summary>
    /// Brings Back Draw Button To Normal State from Automatic State
    /// </summary>
    public void BackToNormalState()
    {
        if (mAutomaticDrawModeOn)
        {
            _drawButtonFillerImage.fillAmount = 0;
            mAutomaticDrawModeOn = false;
            ChangeSprites();
            mOnceDone = false;
            mAutoCardDraw = false;

            StopCoroutine(AutomaticCardDrawing());
        }
    }

    ///// <summary>
    ///// Changes the sprite of Draw Button
    ///// </summary>
    private void ChangeSprites()
    {
        if (DrawButton.sprite == drawNormal)
        {
            DrawButton.color = new Color32(255, 255, 255, 255);
            DrawButton.sprite = drawAutomatic;
        }
        else if (DrawButton.sprite == drawAutomatic)
        {
            DrawButton.sprite = drawNormal;
        }
    }

    /// <summary>
    /// Automates the Card Drawing
    /// </summary>
    /// <returns></returns>
    private IEnumerator AutomaticCardDrawing()
    {
        while (mAutoCardDraw)
        {
            if(canClick)
                DrawCard();
            yield return new WaitForSeconds(timeForCardAnimation);
        }
    }

    /// <summary>
    /// Card Drawing Function
    /// </summary>
    /// 
    private void DrawCard()
    {
        if (_CardList.Count >= 8)
        {
            return;
        }
        mGameManager._energy -= 1;

        Camera.main.GetComponent<CameraController>().DrawButtonClicked();
        if (!hasJoker && Random.Range(0,100) < jokerProbability)
        {
            cards = mScriptedCards[0];//card will be joker if no joker is there and the chance of getting joker is with percentage
            
        }
        else
        {
            cards = mScriptedCards[Random.Range(1, mScriptedCards.Count)];

        }
        cardDeckAnimation3D.GetComponent<Renderer>().material.mainTexture = cards._cardTex;
        cardDeckAnimation3D.SetActive(true);
        Invoke("Instantiate2DCard", 1.01f);

        /*GameObject card = Instantiate(cards._cardModel, _playerHandPoints[clicks].localPosition, _playerHandPoints[clicks].localRotation, mCardHolderParent.transform);
        Cards cardDetails = card.GetComponent<Cards>();

        cardDetails._cardType = cards._cardType;
        cardDetails._cardID = cards._cardID;
        cardDetails._Position = card.transform.position;

        clicks += 1;
        AddNewCard(card.GetComponent<Cards>(), card);
        ReplacementOfCards();
        CardCheckingFunction();*/
    }

    void Instantiate2DCard()
    {
        InstantiateCard(cards);
        cardDeckAnimation3D.SetActive(false);
    }


    public void InstantiateCard(ScriptedCards inCard, bool isSavedCard = false)
    {
        GameObject card = Instantiate(inCard._cardModel, _playerHandPoints[clicks].localPosition + Vector3.left * 1200, _playerHandPoints[clicks].localRotation, mCardHolderParent.transform);
        
        cardDeckAnimation2D.SetActive(true);
        cardDeckAnimation2D.GetComponent<CardDeckAnimation>().cardSprite = card.GetComponent<Image>().sprite;
        cardDeckAnimation2D.GetComponent<CardDeckAnimation>().SpriteChange();

        Cards cardDetails = card.GetComponent<Cards>();
        cardDetails._cardType = inCard._cardType;
        cardDetails._cardID = inCard._cardID;
        cardDetails._Position = card.transform.position;

        clicks += 1;

        AddNewCard(card.GetComponent<Cards>(), card,isSavedCard);
        //Debug.Log(newCardIndex);
        ReplacementOfCards();
        CardCheckingFunction();
    }
    private void AddNewCard(Cards inNewCard, GameObject inCard,bool isSavedCard=false)
    {   
        mCardListGameObject.Add(inCard);
        for (int i = 0; i < _CardList.Count; i++)
        {
            if (_CardList[i]._cardType == inNewCard._cardType && _CardList[i]._cardType != CardType.JOKER)
            {
                Invoke("TwoMatchCardAnimation", 1.5f);
                if (hasJoker)
                    canClick = false;
                _CardList.Insert(i, inNewCard);
                newCardIndex = i;               
                /*if (!isSavedCard)
                    GameManager.Instance._SavedCardTypes.Insert(i, (int)inNewCard._cardType);//inserting card data to game Manager*/
                return;
            }
        }
        if (inNewCard._cardType == CardType.JOKER)
        {
            _jokerList.Add(inNewCard);
            hasJoker = true;
            if (numOfPairCards == 1)
            {
                int pairIndex = GetTwoPairCardIndex()[0];
                Debug.Log("One Card Pair is there");
                if (hasJoker)
                    canClick = false;
                _CardList.Insert(pairIndex+1, inNewCard);
                newCardIndex = pairIndex+1;
                return;                   
            }
            else if(numOfPairCards == 2)
            {
                Debug.Log("Two Card Pair is there");
                jokerFindWithMultiCardPair = true;
                cardDeckAnimation2D.GetComponent<CardDeckAnimation>().OnJokerChooseAnimation();
                inNewCard.gameObject.SetActive(false);
                inNewCard.gameObject.transform.localPosition = new Vector3(0, 950, 0);
                inNewCard.gameObject.transform.localEulerAngles = Vector3.zero;
                flotingJoker = inNewCard.gameObject;
                Invoke("TwoPairCardWithJoker",1f);

                return;
            }
            else if(numOfPairCards == 3)
            {
                Debug.Log("Three Card Pair is there");
                jokerFindWithMultiCardPair = true;
                GetTwoPairCardIndex();
                return;
            }

        }
       
            newCardIndex = _CardList.Count;
            _CardList.Add(inNewCard);       
       /* if (!isSavedCard)
            GameManager.Instance._SavedCardTypes.Add((int)inNewCard._cardType);//adding new card to gameManager*/
    }
    void TwoPairCardWithJoker()
    {
        flotingJoker.SetActive(true);
        cardDeckAnimation2D.SetActive(false);
        cardDeckAnimation2D.transform.SetAsLastSibling();

        selectionCards[0] = GetTwoPairCardIndex()[0] + 1;
        selectionCards[1] = GetTwoPairCardIndex()[1] + 1;

        _CardList[GetTwoPairCardIndex()[0]].PlayJokerSelectionPairAnim(true);
        _CardList[GetTwoPairCardIndex()[0]+1].PlayJokerSelectionPairAnim(true);
        _CardList[GetTwoPairCardIndex()[1]].PlayJokerSelectionPairAnim(false);
        _CardList[GetTwoPairCardIndex()[1]+1].PlayJokerSelectionPairAnim(false);
        take_Multi_Card_Joker_Pair_Input = true;

    }

    List<int> GetTwoPairCardIndex()
    {
        List<int> pairIndexList = new List<int>();
        for (int i = 0; i < _CardList.Count-1; i++)
        {
            if (_CardList[i]._cardType == _CardList[i + 1]._cardType)
                pairIndexList.Add(i);           
        }
        return pairIndexList;
    }


    void TwoMatchCardAnimation()
    {
        if (hasJoker&&numOfPairCards==0)
        {
            _CardList[newCardIndex].PlayThreeCardMatchAnim(-350);
            _CardList[newCardIndex + 1].PlayThreeCardMatchAnim(350);
            int jokerIndex= FindJokerIndex();
            _CardList[jokerIndex].PlayThreeCardMatchAnim(0, _CardList[newCardIndex].gameObject.GetComponent<Image>().sprite);
            _CardList.RemoveAt(jokerIndex);
            threeCardMatch = true;
            
            if (jokerIndex>newCardIndex)
            {
                _CardList.RemoveRange(newCardIndex, 2);
            }
            else
            {
                _CardList.RemoveRange(newCardIndex-1, 2);
            }
            clicks -= 3;
            Debug.Log(clicks);
            ReplacementOfCards(true);
        }
        else if(!threeCardMatch)
        {
            _CardList[newCardIndex].PlayTwoCardMatchAnim();
            _CardList[newCardIndex + 1].PlayTwoCardMatchAnim();
            //hasSingleTwoPair = true;
            numOfPairCards++;
        }

    }


    int FindJokerIndex()
    {
        for (int i = 0; i < _CardList.Count; i++)
        {
            if (_CardList[i]._cardType == CardType.JOKER)
                return i;
        }
        return 0;
    }

    private void ReplacementOfCards(bool endShuffle=false)
    {
        int medianIndex = _playerHandPoints.Count / 2;
        int incrementValue = 0;
        _PositionList.Clear();
        _RotationList.Clear();

        List<int> drawOrderArrange = new List<int>();

        for (int i = 0; i < _CardList.Count; i++)
        {  
            if ((i % 2 == 0 || i == 0)&& (clicks % 2 == 0))
            {
                if (i<2)
                {
                    drawOrderArrange.Add(medianIndex + incrementValue + 1);

                    incrementValue++;

                }
                else
                {
                    drawOrderArrange.Add(medianIndex + incrementValue + 2);
                    incrementValue += 2;

                }
            }
            else if((i % 2 == 0 || i == 0)&& (clicks % 2 == 1))
            {

                drawOrderArrange.Add(medianIndex + incrementValue);
                incrementValue += 2;
            }
            else
            {
                drawOrderArrange.Add(medianIndex - incrementValue);
            }      
        }

        drawOrderArrange.Sort();
     
        for (int i = 0; i < _CardList.Count; i++)
        {
            _PositionList.Add(_playerHandPoints[drawOrderArrange[i]].transform.localPosition);
            _RotationList.Add(_playerHandPoints[drawOrderArrange[i]].transform.localEulerAngles);
        }

        //Debug.Log(_RotationList[newCardIndex]);
        if (!endShuffle)
        {
            if (!jokerFindWithMultiCardPair)
            {
                cardDeckAnimation2D.GetComponent<CardDeckAnimation>().PlayOnDropAnimation(_PositionList[newCardIndex], _RotationList[newCardIndex].z);
                Invoke("CardShufflingDelay", .6f);
                Invoke("CardGenerationDelay", 1.4f);
            }

            
        }
        else
        {
            // CardShufflingDelay();
            Invoke("EndCardShuffle", .5f);
        }
       
       // CardShufflingDelay();
    }
    void EndCardShuffle()
    {
        Debug.Log("EndSuffle");
        for (int i = 0; i < _CardList.Count; i++)
        {
            _CardList[i]._Position = _PositionList[i];
            _CardList[i].transform.localPosition = _PositionList[i];
            _CardList[i].transform.localEulerAngles = _RotationList[i];
            _CardList[i].transform.SetSiblingIndex(i + 1);
            
        }
    }
    void CardShufflingDelay()
    {
        for (int i = 0; i < _CardList.Count; i++)
        {
            if (i != newCardIndex)
            {
                _CardList[i]._Position = _PositionList[i];
                _CardList[i].transform.localPosition = _PositionList[i];
                _CardList[i].transform.localEulerAngles = _RotationList[i];
                _CardList[i].transform.SetSiblingIndex(i + 1);
                /*StartCoroutine(CardGenerationDelay());*/
            }
            else
            {
                cardDeckAnimation2D.transform.SetSiblingIndex(i + 1);
                //StartCoroutine(CardGenerationDelay(i));
            }

        }
    }

    void CardGenerationDelay()
    {  
        _CardList[newCardIndex]._Position = _PositionList[newCardIndex];
        _CardList[newCardIndex].transform.localPosition = _PositionList[newCardIndex];
        _CardList[newCardIndex].transform.localEulerAngles = _RotationList[newCardIndex];
        _CardList[newCardIndex].transform.SetSiblingIndex(newCardIndex + 1);
        cardDeckAnimation2D.SetActive(false);
        mMakeDrawBtnEnable = true;
 
    }

    private void CardCheckingFunction()
    {
        for (int i = 0; i < _CardList.Count - 2; i++)
        {
            if (!hasJoker && (_CardList[i]._cardType == _CardList[i + 1]._cardType && _CardList[i + 1]._cardType == _CardList[i + 2]._cardType))
            {
                threeCardMatchIndex = i;
                threeCardMatch = true;
                canClick = false;
                CardType matchedCard = _CardList[i]._cardType;
                Invoke("PlayThreeCardAnimation", 1.5f);
               /* GameManager.Instance._SavedCardTypes.RemoveRange(i, 3);//Remove the 3 MatchCard from GameManager*/
                //StartCoroutine(DelayedSceneLoader(matchedCard));
            }
            else if (hasJoker && (_CardList[i]._cardType == _CardList[i + 2]._cardType && _CardList[i + 1]._cardType ==CardType.JOKER))
            {
                Debug.Log("Pair with joker");
                threeCardMatchIndex = i;
                threeCardMatch = true;
                canClick = false;
                CardType matchedCard = _CardList[i]._cardType;
                Invoke("PlayThreeCardAnimation", 1.5f);
            }
        }
    }

    
    
    void PlayThreeCardAnimation()
    {
        _CardList[threeCardMatchIndex].PlayThreeCardMatchAnim(-350);
        _CardList[threeCardMatchIndex + 1].PlayThreeCardMatchAnim(0,hasJoker? _CardList[threeCardMatchIndex].gameObject.GetComponent<Image>().sprite:null);
        _CardList[threeCardMatchIndex + 2].PlayThreeCardMatchAnim(350);
        _CardList.RemoveRange(threeCardMatchIndex, 3);
        clicks -= 3;
        //Debug.Log(clicks);

        ReplacementOfCards(true);
    }



    ScriptedCards GetScriptedCardWithCardType(CardType inCardType)
    {
        foreach (ScriptedCards scriptedCard in mScriptedCards)
        {
            if (scriptedCard._cardType == inCardType) return scriptedCard;
        }
        return null;
    }

    private IEnumerator DelayedSceneLoader(CardType inType)
    {
        yield return new WaitForSeconds(2.5f);
       SceneManager.LoadScene(inType.ToString());
    }

    public void OpenCardAdder()
    {
        if(positionNumber > 4)
        {
            return;
        }
        
        for(int i = 0; i < mGameManager.OpenHandCardsPositions.Length;i++)
        {
            if(mGameManager.OpenHandCardsPositions[positionNumber].GetComponent<OpenCardFilled>().isOpenCardSlotFilled == false)
            {
                Instantiate(dummyCards[Random.Range(0, dummyCards.Count)], mGameManager.OpenHandCardsPositions[positionNumber].position, mGameManager.OpenHandCardsPositions[positionNumber].rotation, mGameManager.OpenHandCardsPositions[positionNumber]);
                break;
            }
        }
        
        positionNumber += 1;
    }
}
//else
//{
//    type = _CardList[j]._cardType;
//    count1 = 1;
//}

//if (_jokerList.Count == 1)
//{
//    if (onceDonee == true)
//    {
//        return;
//    }
//    CardType type = _CardList[0]._cardType;
//    int count = 1;
//    for (int j = 1; j < _CardList.Count; j++)
//    {
//        if (_CardList[j]._cardType == type)
//        {
//            count++;
//            if (count == 2)
//            {
//                mHowManyCardSetsAreActive += 1;
//                _cardsThatCanBeReplacedByJoker.Add(_CardList[j]);
//                onceDonee = true;
//            }
//        }
//        else
//        {
//            type = _CardList[j]._cardType;
//            count = 1;
//        }
//    }
//    JokerChecking(mHowManyCardSetsAreActive);
//}

//void JokerChecking(int inHowManyCardSets)
//{
//    switch (inHowManyCardSets)
//    {
//        case 1: //If Only One Set of similar Cards at that time
//            for (int i = 0; i < _cardsThatCanBeReplacedByJoker.Count; i++)
//            {
//                _jokerList[0]._cardType = _cardsThatCanBeReplacedByJoker[0]._cardType;
//                //AddNewCard(_jokerList[0].transform.GetComponent<Cards>(), _jokerList[0].transform.gameObject);
//                ReplacementOfCards();
//                CardCheckingFunction();
//            }
//            break;
//        case 2: //If Two Sets of Similar Card are active at that time
//            int j = 0;
//            for (int i = 0; i < _cardsThatCanBeReplacedByJoker.Count; i++, j += 400)
//            {
//                Cards twoSets = Instantiate(_cardsThatCanBeReplacedByJoker[i], _playerHandPoints[0].position + new Vector3(j, 500, 0), Quaternion.identity, mCardHolderParent.transform);
//                twoSets.transform.gameObject.AddComponent<Button>();
//                twoSets.transform.gameObject.GetComponent<Button>().onClick.AddListener(() => { _jokerList[0]._cardType = twoSets._cardType; /*AddNewCard(_jokerList[0].transform.GetComponent<Cards>(), _jokerList[0].transform.gameObject);*/ ReplacementOfCards(); CardCheckingFunction(); });
//            }
//            break;
//        case 3: //If Three Sets of Similar CardType are Active at that time
//            int k = 0;
//            for (int i = 0; i < _cardsThatCanBeReplacedByJoker.Count; i++, k += 300)
//            {
//                Cards threeSets = Instantiate(_cardsThatCanBeReplacedByJoker[i], _playerHandPoints[0].position + new Vector3(k, 500, 0), Quaternion.identity, mCardHolderParent.transform);
//                threeSets.transform.gameObject.AddComponent<Button>();
//                threeSets.transform.gameObject.GetComponent<Button>().onClick.AddListener(() => { _jokerList[0]._cardType = threeSets._cardType; /*AddNewCard(_jokerList[0].transform.GetComponent<Cards>(), _jokerList[0].transform.gameObject);*/ ReplacementOfCards(); CardCheckingFunction(); });
//            }
//            break;
//        #region FutureCase
//        //case 4:
//        //    int l = 0;
//        //    for (int i = 0; i < _cardsThatCanBeReplacedByJoker.Count; i++, l += 300)
//        //    {
//        //        Cards fourSets = Instantiate(_cardsThatCanBeReplacedByJoker[i], _playerHandPoints[1].position + new Vector3(l, 400, 0), Quaternion.identity, mCardHolderParent.transform);
//        //        fourSets.transform.gameObject.AddComponent<Button>();
//        //        fourSets.transform.gameObject.GetComponent<Button>().onClick.AddListener(() => { _jokerList[0]._cardType = fourSets._cardType; AddNewCard(_jokerList[0].transform.GetComponent<Cards>(), _jokerList[0].transform.gameObject); ReplacementOfCards(); CardCheckingFunction(); });
//        //    }
//        //    break;
//        #endregion
//        default:
//            break;
//    }
//}

#region Old CardDeck
//[Header ("Grabbing Other GameObject References:")]
//[SerializeField] private GameManager mGameManager;
//[SerializeField] private GameObject mCardHolderParent;
//private int clicks = 0;
//public List<Transform> _playerHandPoints;

//[Space(10)]
//[Header ("Cards And Related Lists:")]
//[SerializeField] private List<ScriptedCards> mScriptedCards;
//[SerializeField] private List<Cards> _CardList = new List<Cards>();
//[SerializeField] private List<GameObject> mCardListGameObject;
//private List<Vector3> _PositionList = new List<Vector3>();
//private List<Quaternion> _RotationList = new List<Quaternion>();

//[Space(10)]
//[Header ("Draw Button And Its States Images with conditions:")]
//[SerializeField] private Image DrawButton;
//[SerializeField] private Sprite drawNormal, drawAutomatic;
//[SerializeField] private RectTransform _drawButtonRectTransform;
//[Space(10)]
//[SerializeField] private int mMaxHoldTime = 5;
//[SerializeField] private float timeForCardAnimation = 2f;
//private float time = 0;
//private bool mAutoCardDraw = false;
//private bool mAutomaticDrawModeOn = false;
//private bool mOnceDone = false;

//[Space(10)]
//[Header ("Joker and related things")]
//public List<Cards> _jokerList;
//public bool onceDonee = false;


//private void Start()
//{
//    onceDonee = false;
//    DrawButton.sprite = drawNormal;
//    mGameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
//}

//private void Update()
//{
//    if (clicks == 8)
//    {
//        clicks = 0;
//        foreach (GameObject card in mCardListGameObject)
//        {
//            Destroy(card);
//        }
//        _CardList.Clear();
//        mCardListGameObject.Clear();
//    }

//    if(_jokerList.Count >= 1)
//    {
//        if(onceDonee == true)
//        {
//            return;
//        }
//        CardType type = _CardList[0]._cardType;
//        int count = 1;
//        for (int j = 1; j < _CardList.Count; j++)
//        {
//            if (_CardList[j]._cardType == type)
//            {
//                count++;
//                if (count == 2)
//                {
//                    JokerStuff(j);
//                    onceDonee = true;
//                }
//            }
//            else
//            {
//                type = _CardList[j]._cardType;
//                count = 1;
//            }
//        }
//    }
//    #region Button Function
//    time = Mathf.Clamp(time,0f,5f);
//    Vector2 localMousePosition = _drawButtonRectTransform.InverseTransformPoint(Input.mousePosition);

//    if (Input.GetMouseButtonDown(0))
//    {
//        if (_drawButtonRectTransform.rect.Contains(localMousePosition))
//        {
//            BackToNormalState();
//            time = 0;
//            DrawCard();
//        }
//    }

//    if (!mOnceDone)
//    {
//        if (Input.GetMouseButton(0))
//        {
//            if (_drawButtonRectTransform.rect.Contains(localMousePosition))
//            {
//                time += Time.fixedDeltaTime;
//                if (time >= mMaxHoldTime)
//                {
//                    mOnceDone = true;
//                    mAutomaticDrawModeOn = true;
//                    mAutoCardDraw = true;
//                    ChangeSprites();
//                    StartCoroutine(AutomaticCardDrawing());
//                }
//            }
//        }
//    }

//    if (Input.GetMouseButtonUp(0))
//    {
//        if (_drawButtonRectTransform.rect.Contains(localMousePosition))
//        {
//            time = 0;
//        }
//    }
//    #endregion
//}

//void JokerStuff(int j)
//{
//    _jokerList[0]._cardType = _CardList[j]._cardType;
//    AddNewCard(_jokerList[0].transform.GetComponent<Cards>(), _jokerList[0].transform.gameObject);
//    ReplacementOfCards();
//    CardCheckingFunction();
//}
//public void BackToNormalState()
//{
//    if (mAutomaticDrawModeOn)
//    {
//        mAutomaticDrawModeOn = false;
//        ChangeSprites();
//        mOnceDone = false;
//        mAutoCardDraw = false;
//        StopCoroutine(AutomaticCardDrawing());
//    }
//}

//private void ChangeSprites()
//{
//    if (DrawButton.sprite == drawNormal)
//    {
//        DrawButton.sprite = drawAutomatic;
//    }
//    else if (DrawButton.sprite == drawAutomatic)
//    {
//        DrawButton.sprite = drawNormal;
//    }
//}

//private void DrawCard()
//{
//    if (_CardList.Count >= 8)
//    {
//        return;
//    }
//    mGameManager._energy -= 1;

//    Camera.main.GetComponent<CameraController>().DrawButtonClicked();

//    ScriptedCards cards = mScriptedCards[Random.Range(0, mScriptedCards.Count)];

//    GameObject card = Instantiate(cards._cardModel, _playerHandPoints[clicks].localPosition, _playerHandPoints[clicks].localRotation, mCardHolderParent.transform);
//    Cards cardDetails = card.GetComponent<Cards>();

//    cardDetails._cardType = cards._cardType;
//    cardDetails._cardID = cards._cardID;
//    cardDetails._Position = card.transform.position;

//    clicks += 1;
//    AddNewCard(card.GetComponent<Cards>(),card);
//    ReplacementOfCards();
//    CardCheckingFunction();
//}

//private IEnumerator AutomaticCardDrawing()
//{
//    while (mAutoCardDraw)
//    {
//        DrawCard();
//        yield return new WaitForSeconds(timeForCardAnimation);
//    }
//}

//private void AddNewCard(Cards inNewCard , GameObject inCard)
//{
//    mCardListGameObject.Add(inCard);
//    for (int i = 0; i < _CardList.Count; i++)
//    {

//        if (_CardList[i]._cardType == inNewCard._cardType)
//        {
//            _CardList.Insert(i, inNewCard);
//            return;
//        }

//    }
//    if (inNewCard._cardType == CardType.JOKER)
//    {
//        _jokerList.Add(inNewCard);
//    }
//    _CardList.Add(inNewCard);
//}

//private void ReplacementOfCards()
//{
//    int medianIndex = _playerHandPoints.Count / 2;

//    int incrementValue = 0;
//    _PositionList.Clear();
//    _RotationList.Clear();

//    List<int> drawOrderArrange = new List<int>();

//    for (int i = 0; i < _CardList.Count; i++)
//    {
//        if (i % 2 == 0 || i == 0)
//        {
//            drawOrderArrange.Add(medianIndex + incrementValue);
//            incrementValue++;
//        }
//        else
//        {
//            drawOrderArrange.Add(medianIndex - incrementValue);
//        }
//    }

//    drawOrderArrange.Sort();

//    for (int i = 0; i < _CardList.Count; i++)
//    {
//        _PositionList.Add(_playerHandPoints[drawOrderArrange[i]].transform.position);
//        _RotationList.Add(_playerHandPoints[drawOrderArrange[i]].transform.rotation);
//    }

//    for (int i = 0; i < _CardList.Count; i++)
//    {
//        _CardList[i]._Position = _PositionList[i];
//        _CardList[i].transform.position = _PositionList[i];
//        _CardList[i].transform.rotation = _RotationList[i];
//        _CardList[i].transform.SetSiblingIndex(i + 1);
//    }
//}

//private void CardCheckingFunction()
//{
//    for (int i = 0; i < _CardList.Count - 2; i++)
//    {
//        if (_CardList[i]._cardType == _CardList[i + 1]._cardType && _CardList[i + 1]._cardType == _CardList[i + 2]._cardType)
//        {
//            StartCoroutine(DelayedSceneLoader(_CardList[i]._cardType));
//        }
//    }
//}

//private IEnumerator DelayedSceneLoader(CardType inType)
//{
//    yield return new WaitForSeconds(2);
//    SceneManager.LoadScene(inType.ToString());
//}
#endregion


//void UnJokerVersion()
//{
//    [Header("Grabing Other GameObject References")]
//    [SerializeField] private GameManager mGameManager;
//    [SerializeField] private GameObject mCardHolderParent;
//    [SerializeField] private List<ScriptedCards> mScriptedCards;
//    [SerializeField] private List<GameObject> mCardListGameObject;
//    [SerializeField] private int mMaxHoldTime = 5;
//    [SerializeField] private float time = 0;
//    [SerializeField] private Image DrawButton;
//    [SerializeField] private Sprite drawNormal, drawAutomatic;
//    [SerializeField] private RectTransform _drawButtonRectTransform;
//    [SerializeField] private float timeForCardAnimation = 2f;

//    private List<Vector3> _PositionList = new List<Vector3>();
//private List<Quaternion> _RotationList = new List<Quaternion>();
//private bool mAutoCardDraw = false;
//private bool mAutomaticDrawModeOn = false;
//private bool mOnceDone = false;
//private int clicks = 0;

//public List<Cards> _CardList = new List<Cards>();
//public List<Transform> _playerHandPoints;





//private void Start()
//{
//    DrawButton.sprite = drawNormal;
//    mGameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
//}

//private void Update()
//{
//    if (clicks == 8)
//    {
//        clicks = 0;
//        foreach (GameObject card in mCardListGameObject)
//        {
//            Destroy(card);
//        }
//        _CardList.Clear();
//        mCardListGameObject.Clear();
//    }

//    time = Mathf.Clamp(time, 0f, 5f);
//    Vector2 localMousePosition = _drawButtonRectTransform.InverseTransformPoint(Input.mousePosition);

//    if (Input.GetMouseButtonDown(0))
//    {
//        if (_drawButtonRectTransform.rect.Contains(localMousePosition))
//        {
//            BackToNormalState();
//            time = 0;
//            DrawCard();
//        }
//    }

//    if (!mOnceDone)
//    {
//        if (Input.GetMouseButton(0))
//        {
//            if (_drawButtonRectTransform.rect.Contains(localMousePosition))
//            {
//                time += Time.fixedDeltaTime;
//                if (time >= mMaxHoldTime)
//                {
//                    mOnceDone = true;
//                    mAutomaticDrawModeOn = true;
//                    mAutoCardDraw = true;
//                    ChangeSprites();
//                    StartCoroutine(AutomaticCardDrawing());
//                }
//            }
//        }
//    }

//    if (Input.GetMouseButtonUp(0))
//    {
//        if (_drawButtonRectTransform.rect.Contains(localMousePosition))
//        {
//            time = 0;
//        }
//    }
//}

//public void BackToNormalState()
//{
//    if (mAutomaticDrawModeOn)
//    {
//        mAutomaticDrawModeOn = false;
//        ChangeSprites();
//        mOnceDone = false;
//        mAutoCardDraw = false;
//        StopCoroutine(AutomaticCardDrawing());
//    }
//}

//private void ChangeSprites()
//{
//    if (DrawButton.sprite == drawNormal)
//    {
//        DrawButton.sprite = drawAutomatic;
//    }
//    else if (DrawButton.sprite == drawAutomatic)
//    {
//        DrawButton.sprite = drawNormal;
//    }
//}

//private void DrawCard()
//{
//    if (_CardList.Count >= 8)
//    {
//        return;
//    }
//    mGameManager._energy -= 1;

//    Camera.main.GetComponent<CameraController>().DrawButtonClicked();

//    ScriptedCards cards = mScriptedCards[Random.Range(0, mScriptedCards.Count)];

//    GameObject card = Instantiate(cards._cardModel, _playerHandPoints[clicks].localPosition, _playerHandPoints[clicks].localRotation, mCardHolderParent.transform);
//    Cards cardDetails = card.GetComponent<Cards>();

//    cardDetails._cardType = cards._cardType;
//    cardDetails._cardID = cards._cardID;
//    cardDetails._Position = card.transform.position;

//    clicks += 1;
//    AddNewCard(card.GetComponent<Cards>(), card);
//    ReplacementOfCards();
//    //CardCheckingFunction();
//}

//private IEnumerator AutomaticCardDrawing()
//{
//    while (mAutoCardDraw)
//    {
//        DrawCard();
//        yield return new WaitForSeconds(timeForCardAnimation);
//    }
//}

//private void AddNewCard(Cards inNewCard, GameObject inCard)
//{
//    mCardListGameObject.Add(inCard);
//    for (int i = 0; i < _CardList.Count; i++)
//    {
//        if (_CardList[i]._cardType == inNewCard._cardType)
//        {
//            _CardList.Insert(i, inNewCard);
//            return;
//        }
//    }
//    _CardList.Add(inNewCard);
//}

//private void ReplacementOfCards()
//{
//    int medianIndex = _playerHandPoints.Count / 2;

//    int incrementValue = 0;
//    _PositionList.Clear();
//    _RotationList.Clear();

//    List<int> drawOrderArrange = new List<int>();

//    for (int i = 0; i < _CardList.Count; i++)
//    {
//        if (i % 2 == 0 || i == 0)
//        {
//            drawOrderArrange.Add(medianIndex + incrementValue);
//            incrementValue++;
//        }
//        else
//        {
//            drawOrderArrange.Add(medianIndex - incrementValue);
//        }
//    }

//    drawOrderArrange.Sort();

//    for (int i = 0; i < _CardList.Count; i++)
//    {
//        _PositionList.Add(_playerHandPoints[drawOrderArrange[i]].transform.position);
//        _RotationList.Add(_playerHandPoints[drawOrderArrange[i]].transform.rotation);
//    }

//    for (int i = 0; i < _CardList.Count; i++)
//    {
//        _CardList[i]._Position = _PositionList[i];
//        _CardList[i].transform.position = _PositionList[i];
//        _CardList[i].transform.rotation = _RotationList[i];
//        _CardList[i].transform.SetSiblingIndex(i + 1);
//    }
//}

//private void CardCheckingFunction()
//{
//    for (int i = 0; i < _CardList.Count - 2; i++)
//    {
//        if (_CardList[i]._cardType == _CardList[i + 1]._cardType && _CardList[i + 1]._cardType == _CardList[i + 2]._cardType)
//        {
//            StartCoroutine(DelayedSceneLoader(_CardList[i]._cardType));
//        }
//    }
//}

//private IEnumerator DelayedSceneLoader(CardType inType)
//{
//    yield return new WaitForSeconds(2);
//    SceneManager.LoadScene(inType.ToString());
//}
//}























//void OldVersion()
//{
//    [SerializeField] private GameManager mGameManager;
//    [SerializeField] private GameObject mCardHolderParent;
//    private int clicks = 0;
//private int mk;

//[SerializeField] public List<ScriptedCards> mScriptedCards;
//public List<Cards> _CardList = new List<Cards>();
//public List<Transform> _playerHandPoints;
//[HideInInspector] public List<Vector3> _PositionList = new List<Vector3>();
//[HideInInspector] public List<Quaternion> _RotationList = new List<Quaternion>();

//#region CardMarch3 Version-1
//public List<Cards> AttackList;
//public List<Cards> StealList;
//public List<Cards> ShieldList;
//public List<Cards> JokerList;
//public List<Cards> EnergyList;
//public List<Cards> CoinsList;
//public List<Cards> FortuneList;
//public List<Cards> SpinList;
//#endregion



//private void Start()
//{
//    mGameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
//}



///// <summary>
///// This function is responsible for the camera to zoom in to the playing space and the card draw functionality
///// 1.Reduce the Energy.
///// 2.Zoom to the gameplay location
///// 3.Have a way to access the card location and spawn card at their respective positions in an inverted U-Shape
///// </summary>
//public void DrawCard()
//{
//    //ChangeSprites();
//    if (_CardList.Count >= 8)
//        return;

//    mGameManager._energy -= 1;

//    Camera.main.GetComponent<CameraController>().DrawButtonClicked();

//    ScriptedCards cards = mScriptedCards[Random.Range(0, mScriptedCards.Count)];

//    GameObject card = Instantiate(cards._cardModel, _playerHandPoints[clicks].localPosition, _playerHandPoints[clicks].localRotation, mCardHolderParent.transform);
//    Cards cardDetails = card.GetComponent<Cards>();

//    cardDetails._cardType = cards._cardType;
//    cardDetails._cardID = cards._cardID;
//    cardDetails._Position = card.transform.position;

//    clicks += 1;
//    AddNewCard(card.GetComponent<Cards>());
//    ReplacementOfCards();
//    CardCheckingFunction();
//}

//private void Update()
//{
//    if (clicks == 8)
//    {
//        clicks = 0;
//    }
//}

///// <summary>
///// </This function is used to allign the picked cards in sorted order>
//public void AddNewCard(Cards inNewCard)
//{
//    for (int i = 0; i < _CardList.Count; i++)
//    {
//        if (_CardList[i]._cardType == inNewCard._cardType)
//        {
//            _CardList.Insert(i, inNewCard);
//            return;
//        }
//    }
//    _CardList.Add(inNewCard);
//}

//public void ReplacementOfCards()
//{
//    int medianIndex = _playerHandPoints.Count / 2;

//    int incrementValue = 0;
//    _PositionList.Clear();
//    _RotationList.Clear();

//    List<int> drawOrderArrange = new List<int>();

//    for (int i = 0; i < _CardList.Count; i++)
//    {
//        if (i % 2 == 0 || i == 0)
//        {
//            drawOrderArrange.Add(medianIndex + incrementValue);
//            incrementValue++;
//        }
//        else
//        {
//            drawOrderArrange.Add(medianIndex - incrementValue);
//        }
//    }

//    drawOrderArrange.Sort();

//    for (int i = 0; i < _CardList.Count; i++)
//    {
//        _PositionList.Add(_playerHandPoints[drawOrderArrange[i]].transform.position);
//        _RotationList.Add(_playerHandPoints[drawOrderArrange[i]].transform.rotation);
//    }

//    for (int i = 0; i < _CardList.Count; i++)
//    {
//        _CardList[i]._Position = _PositionList[i];
//        _CardList[i].transform.position = _PositionList[i];
//        _CardList[i].transform.rotation = _RotationList[i];
//        _CardList[i].transform.SetSiblingIndex(i + 1);
//    }
//}

//void CardCheckingFunction()
//{
//    for (int i = 0; i < _CardList.Count - 2; i++)
//    {
//        if (_CardList[i]._cardType == _CardList[i + 1]._cardType && _CardList[i + 1]._cardType == _CardList[i + 2]._cardType)
//        {
//            StartCoroutine(DelayedSceneLoader(_CardList[i]._cardType));
//        }
//    }
//}

//IEnumerator DelayedSceneLoader(CardType inType)
//{
//    yield return new WaitForSeconds(2);
//    SceneManager.LoadScene(inType.ToString());
//}
//}



// Algorithm for auto draw
// Method - 1
// have a while which when true does the draw card continously

// But before that we should write an algorithm on how to get the hold working for our button and tap for our button
// And have a timer which when holded down for more than a particular second do auto and if not do normal.

//void drawButtonChange()
//{
//      public Image DrawButton;
//      public Sprite drawNormal, drawAutomatic;

// //DrawButton.sprite = drawNormal;

//    //void ChangeSprites()
//    //{
//    //    if (DrawButton.sprite == drawNormal)
//    //    {
//    //        DrawButton.sprite = drawAutomatic;
//    //    }
//    //    else if (DrawButton.sprite == drawAutomatic)
//    //    {
//    //        DrawButton.sprite = drawNormal;
//    //    }
//    //}
//}


















//void Residue()
//{

/// <summary>
/// This Function will Trigger the Scene if 3 Cards Matches
/// </summary>
//void CardTrigger()
//{
//    switch (_CardList[mk]._cardType)
//    {
//        case CardType.ATTACK:
//            SceneManager.LoadScene(1);
//            break;
//        case CardType.COINS:
//            SceneManager.LoadScene(2);
//            break;
//        case CardType.ENERGY:
//            SceneManager.LoadScene(3);
//            break;
//        case CardType.FORTUNEWHEEL:
//            SceneManager.LoadScene(4);
//            break;
//        case CardType.SLOTMACHINE:
//            SceneManager.LoadScene(5);
//            break;
//        case CardType.JOKER:
//            Debug.Log("Joker");
//            break;
//        case CardType.SHIELD:
//            Debug.Log("Shield + 1");
//            break;
//    }
//}

//#region Try-2 "Understood the code. Seems a better choice(Code Took from Internet)"
//if (_CardList.Count > 2)
//{
//    CardType type = _CardList[0]._cardType;
//    int count = 1;
//    for (int i = 1; i < _CardList.Count; i++)
//    {
//        if (_CardList[i]._cardType == type)
//        {
//            count++;
//            if (count == 3)
//            {
//                StartCoroutine(DelayedSceneLoader(type));
//            }
//        }
//        else
//        {
//            type = _CardList[i]._cardType;
//            count = 1;
//        }
//     }
//}
//#endregion

//int j = i;
//int k = i + 1;
//int l = i + 2;
//if (j >= _CardList.Count || k >= _CardList.Count || l >= _CardList.Count)
//{
//    return;
//}

//#region CardMarch3 Version-1
//switch (inNewCard._cardType)
//{
//    case CardType.ATTACK: AttackList.Add(inNewCard);
//        break;
//    case CardType.STEAL: StealList.Add(inNewCard);
//        break;
//    case CardType.SHIELD: ShieldList.Add(inNewCard);
//        break;
//    case CardType.JOKER: JokerList.Add(inNewCard);
//        break;
//    case CardType.ENERGY: EnergyList.Add(inNewCard);
//        break;
//    case CardType.COINS: CoinsList.Add(inNewCard);
//        break;
//    case CardType.FORTUNEWHEEL: FortuneList.Add(inNewCard);
//        break;
//    case CardType.SLOTMACHINE: SpinList.Add(inNewCard);
//        break;
//    default:
//        break;

//}
//#endregion

/// <summary>
/// In this function Checking that any of  three cards are same then its name will be displayed
/// </summary>

//if (_CardList[0]._cardID == _CardList[1]._cardID && _CardList[0]._cardID == _CardList[2]._cardID)
//{
//    mk = 1;
//    Invoke("CardTrigger", 1.5f);
//}
//else if (_CardList[1]._cardID == _CardList[2]._cardID && _CardList[1]._cardID == _CardList[3]._cardID)
//{
//    mk = 2;
//    Invoke("CardTrigger", 1.5f);
//}
//else if (_CardList[2]._cardID == _CardList[3]._cardID && _CardList[2]._cardID == _CardList[4]._cardID)
//{
//    mk = 3;
//    Invoke("CardTrigger", 1.5f);
//}
//else if (_CardList[3]._cardID == _CardList[4]._cardID && _CardList[3]._cardID == _CardList[5]._cardID)
//{
//    mk = 4;
//    Invoke("CardTrigger", 1.5f);
//}
//else if (_CardList[4]._cardID == _CardList[5]._cardID && _CardList[4]._cardID == _CardList[6]._cardID)
//{
//    mk = 5;
//    Invoke("CardTrigger", 1.5f);
//}
//else if (_CardList[5]._cardID == _CardList[6]._cardID && _CardList[5]._cardID == _CardList[7]._cardID)
//{
//    mk = 6;
//    Invoke("CardTrigger", 1.5f);
//}

//void CardTrigger()
//{
//    switch (_CardList[mk]._cardType)
//    {
//        case CardType.ATTACK:
//            SceneManager.LoadScene(1);
//            break;
//        case CardType.COINS:
//            SceneManager.LoadScene(2);
//            break;
//        case CardType.ENERGY:
//            SceneManager.LoadScene(3);
//            break;
//        case CardType.FORTUNEWHEEL:
//            SceneManager.LoadScene(4);
//            break;
//        case CardType.SLOTMACHINE:
//            SceneManager.LoadScene(5);
//            break;
//        case CardType.JOKER:
//            Debug.Log("Joker");
//            break;
//        case CardType.SHIELD:
//            Debug.Log("Shield + 1");
//            break;
//    }
//}
//void CardMatchThreeChecker(Cards inNewCard)
//{
//    //#region CardMarch3 Version-1
//    if (AttackList.Count == 3)
//    {
//        SceneManager.LoadScene(1);
//    }
//    if (CoinsList.Count == 3)
//    {
//        SceneManager.LoadScene(2);
//    }
//    if (EnergyList.Count == 3)
//    {
//        SceneManager.LoadScene(3);
//    }
//    if (FortuneList.Count == 3)
//    {
//        SceneManager.LoadScene(4);
//    }
//    if (SpinList.Count == 3)
//    {
//        SceneManager.LoadScene(5);
//    }
//    //#endregion
//    //if(_CardList.Contains(inNewCard))
//    //{

//    //}
//}

//void LoadScene()
//{
//    SceneManager.LoadScene(_CardList[i]._cardType.ToString());
//}




//[SerializeField] private GameObject mCam;
//[SerializeField] private Transform mDeckCardCamPosition;
//[SerializeField] private LevelManagerUI mlevelManagerUI;

//    [SerializeField] private Camera mCam;
//    [SerializeField] private Transform mDeckCardCamPosition;

//    /// <summary>
//    /// This function is responsible for the camera to zoom in to the playing space and the card draw functionality
//    /// </summary>
//    public void DrawCard()
//    {
//        ZoomCameraToPlayArea();
//    }

//    private void ZoomCameraToPlayArea()
//    {
//        mCam.transform.position = mDeckCardCamPosition.position;
//        mCam.transform.rotation = mDeckCardCamPosition.transform.rotation;
//    }

//GameObject card = Instantiate(mCards.boosterCards[Random.Range(0, mCards.boosterCards.Count)]._cardModel, _handPoints[clicks].transform.position, Quaternion.Euler(0, 180f, 0f));
////_handPoints[clicks].isFilled = true;
//switch (card.tag)
//{
//    case "5K Coins":
//        mlevelManagerUI._fiveThousandCoinList.Add(card);
//        //levelManagerUI.OverAllCards.Add(card);
//        break;
//    case "25K Coins":
//        mlevelManagerUI._twentyFiveThousandCoinList.Add(card);
//        //levelManagerUI.OverAllCards.Add(card);
//        break;
//    case "100K Coins":
//        mlevelManagerUI._hunderThousandCoinList.Add(card);
//        //levelManagerUI.OverAllCards.Add(card);
//        break;
//    case "500K Coins":
//        mlevelManagerUI._fiveHundredThousandCoinList.Add(card);
//        //levelManagerUI.OverAllCards.Add(card);
//        break;
//    case "1M Coins":
//        mlevelManagerUI._OneMillionJackPotCardList.Add(card);
//        //levelManagerUI.OverAllCards.Add(card);
//        break;
//    case "10 EC":
//        mlevelManagerUI._TenEnergyCardList.Add(card);
//        //levelManagerUI.OverAllCards.Add(card);
//        break;
//    case "25 EC":
//        mlevelManagerUI._TwentyFiveEnergyCardList.Add(card);
//        //levelManagerUI.OverAllCards.Add(card);
//        break;
//    case "100 EC":
//        mlevelManagerUI._HundredEnergyCardList.Add(card);
//        //levelManagerUI.OverAllCards.Add(card);
//        break;
//    case "Attack":
//        mlevelManagerUI._AttackCardList.Add(card);
//        break;
//    case "Shield":
//        mlevelManagerUI._SheildCardList.Add(card);
//        break;
//    case "Steal":
//        mlevelManagerUI._StealCardList.Add(card);
//        break;
//}
//clicks += 1;
//}
