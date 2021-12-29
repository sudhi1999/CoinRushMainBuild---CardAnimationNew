using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class WheelPiece
{
    public Sprite _Icon;
    public string _Label;
    public int _Amount;
    [Range(0f, 100f)] public float _Chance;
    [HideInInspector]
    public int _Index;
    [HideInInspector]
    public double _Weight;
}

public class SpinWheel : MonoBehaviour
{
    [Header("References :")]
    //WheelPiece requirements
    public Transform _spinWheelTransform;
    public Transform _spinnerParent;
    public GameObject _wheelPiecePrefab;
    public Transform _wheelPiecesParent;

    [Space]
    [Header("Sounds :")]
    //Sound requirements
    public AudioSource _audioSource;
    public AudioClip _tickAudioClip;
    [Range(0f, 1f)] public float _volume = .5f;
    [Range(-3f, 3f)] public float _pitch = 1f;

    [Space]
    [Header("Picker wheel settings :")]
    //Wheel Settings
    [Range(1, 20)] public int _spinDuration = 8;
    [Range(.2f, 2f)] public float _wheelSize = 1f;

    [Space]
    [Header("Picker wheel pieces :")]
    //Calling WheelPiece Script as a Array
    public WheelPiece[] _wheelPieces;

    //bool
    private bool mIsSpinning = false;

    // Events
    private UnityAction<WheelPiece> onSpinEndEvent;

    //Wheel Piece Properties
    private Vector2 mPieceMinSize = new Vector2(81f, 146f);
    private Vector2 mPieceMaxSize = new Vector2(144f, 213f);
    //private int mPiecesMin = 2;
    //private int mPiecesMax = 16;
    public float mPieceAngle;
    public float mHalfPieceAngle;
    public float mHalfPieceAngleWithPaddings;
    public Vector3 newPos;

    //Probablity Calculation requirements
    private double mAccumulatedWeight;
    private System.Random rand = new System.Random();

    //List for NonZeroChancesIndices
    private List<int> mNonZeroChancesIndices = new List<int>();

    //SpinWheel Needle Animation
    public Animator mNeedleAnim;

    public Animator mLightAnimator;

    private void Start()
    {
        WheelDivider();
        SetupAudio();
        PieceGenerator();
        CalculateWeightsAndIndices();
    }

    /// <summary>
    /// Spin Wheel dividing into number of parts user needs 
    /// </summary>
    private void WheelDivider()
    {
        mPieceAngle = 360 / _wheelPieces.Length;
        mHalfPieceAngle = mPieceAngle / 2f;
        mHalfPieceAngleWithPaddings = mHalfPieceAngle - (mHalfPieceAngle / 4f);
    }

    /// <summary>
    /// Audio Components Link Up
    /// </summary>
    private void SetupAudio()
    {
        _audioSource.clip = _tickAudioClip;
        _audioSource.volume = _volume;
        _audioSource.pitch = _pitch;
    }

    /// <summary>
    /// Generates piece according to images and values given in inspector
    /// </summary>
    private void PieceGenerator()
    {
        for (int i = 0; i < _wheelPieces.Length; i++)
        {
            GameObject wheelPiece = Instantiate(_wheelPiecePrefab, _wheelPiecesParent.position, Quaternion.identity, _wheelPiecesParent);
            WheelPiece piece = _wheelPieces[i];
            Transform pieceTrns = wheelPiece.transform.GetChild(0);

            if (i % 2 == 0 || i == 0)
            {
                pieceTrns.GetChild(0).GetComponent<Text>().text = piece._Label;
            }
            else
            {
                pieceTrns.GetChild(0).GetComponent<Text>().text = piece._Label;
                pieceTrns.GetChild(0).GetComponent<Text>().color = Color.yellow;
            }
            pieceTrns.GetChild(1).GetComponent<Image>().sprite = piece._Icon;
            //pieceTrns.GetChild(1).GetComponent<Text>().text = piece._Label;

            ResizePiece(pieceTrns);
            pieceTrns.RotateAround(_wheelPiecesParent.position, Vector3.back, mPieceAngle * i);
        }
    }

    /// <summary>
    /// Used to resize the instantiated piece according to the divisions size and set its anchor points
    /// </summary>
    /// <param name="inPiece"></param>
    private void ResizePiece(Transform inPiece)
    {
        RectTransform rt = inPiece.GetComponent<RectTransform>();
        float pieceWidth = Mathf.Lerp(mPieceMinSize.x, mPieceMaxSize.x, /*1f - Mathf.InverseLerp(mPiecesMin, mPiecesMax, _wheelPieces.Length)*/ Time.deltaTime);
        float pieceHeight = Mathf.Lerp(mPieceMinSize.y, mPieceMaxSize.y, /*1f - Mathf.InverseLerp(mPiecesMin, mPiecesMax, _wheelPieces.Length)*/Time.deltaTime);
        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, pieceWidth);
        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, pieceHeight);
    }

    /// <summary>
    /// Calculates the accumulated overall weights for probability functions
    /// </summary>
    private void CalculateWeightsAndIndices()
    {
        for (int i = 0; i < _wheelPieces.Length; i++)
        {
            WheelPiece piece = _wheelPieces[i];

            //add weights:
            mAccumulatedWeight += piece._Chance;
            piece._Weight = mAccumulatedWeight;

            //add index :
            piece._Index = i;

            //save non zero chance indices:
            if (piece._Chance > 0)
                mNonZeroChancesIndices.Add(i);
        }
    }

    /// <summary>
    /// All spin functions when spin button is clicked
    /// </summary>
    public void Spin()
    {
        if (mIsSpinning == false)
        {
            if (FindObjectOfType<SpinWheelSpin>().FreeSpins <= 0)
            {
                FindObjectOfType<SpinWheelSpin>().DoFreeSpins = false;
            }
            else
            {
                FindObjectOfType<SpinWheelSpin>().DoFreeSpins = true;
              
                FindObjectOfType<SpinWheelSpin>().FreeSpins -= 1;
            }
            mIsSpinning = true;

            int index = GetRandomPieceIndex();
            WheelPiece piece = _wheelPieces[index];

            if (piece._Chance == 0 && mNonZeroChancesIndices.Count != 0)
            {
                index = mNonZeroChancesIndices[Random.Range(0, mNonZeroChancesIndices.Count)];
                piece = _wheelPieces[index];
            }

            float angle = -(mPieceAngle * index); // -240

            float rightOffset = (angle - mHalfPieceAngleWithPaddings) % 360; // (-240 -45) % 360 = -285
            float leftOffset = (angle + mHalfPieceAngleWithPaddings) % 360; // (-240 + 45) % 360 = -195
            float randomAngle = Random.Range(leftOffset, rightOffset);

            Vector3 targetRotation = Vector3.back * (randomAngle + 2 * 360 * _spinDuration);

            float prevAngle, currentAngle;
            prevAngle = currentAngle = _spinnerParent.eulerAngles.z;

            bool isIndicatorOnTheLine = false;

            mNeedleAnim.SetBool("Spin", true);
            Invoke("StopNeedleAnimation", _spinDuration - 0.8f);

            mLightAnimator.SetBool("Spin", true);

            _spinnerParent.DORotate(targetRotation, _spinDuration)
            //.SetEase(Ease.InOutQuart)
            .OnUpdate(() =>
            { //for increasing audio pitch when indicatior needle touching on the line prefab 

                float diff = Mathf.Abs(prevAngle - currentAngle);
                if (diff >= mHalfPieceAngle)
                {
                    if (isIndicatorOnTheLine)
                    {
                        _audioSource.PlayOneShot(_audioSource.clip);
                    }
                    prevAngle = currentAngle;
                    isIndicatorOnTheLine = !isIndicatorOnTheLine;
                }
                currentAngle = _spinnerParent.eulerAngles.z;
            })
            .OnComplete(() =>
            {
                mIsSpinning = false;
                if (onSpinEndEvent != null)
                {
                    onSpinEndEvent.Invoke(piece);
                }
                onSpinEndEvent = null;
            });
        }
    }
    void StopNeedleAnimation()
    {
        mNeedleAnim.SetBool("Spin", false);
    }

    /// <summary>
    /// Event Declaration
    /// </summary>
    /// <param name="action"></param>
    public void OnSpinEnd(UnityAction<WheelPiece> action)
    {
        onSpinEndEvent = action;
    }

    /// <summary>
    /// Gets A random value with a given probability
    /// </summary>
    /// <returns></returns>
    private int GetRandomPieceIndex()
    {
        double r = rand.NextDouble() * mAccumulatedWeight;
        for (int i = 0; i < _wheelPieces.Length; i++)
        {
            if (_wheelPieces[i]._Weight >= r)
            {
                return i;
            }
        }
        return 0;
    }
    private void Update()
    {
       
    }
}