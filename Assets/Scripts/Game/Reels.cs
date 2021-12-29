using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ReelElement
{
    public GameObject _slotElementGameObject;

    [Range(0, 100)] public float _chanceOfObtaining;
    [HideInInspector] public int _index;
    [HideInInspector] public double _toughnessMeter;
}

public class Reels : MonoBehaviour
{
    public ReelElement[] _reelElements;
    [Range(1, 20)] public int _reelRollDuration = 4;
    public bool _roll = false;

    public bool mSpinOver = false;
    public float accumalatedY;

    private double mTotalToughnessMeter;
    private System.Random mRandomValue = new System.Random();

    [SerializeField]
    private Transform mReelsRollerParent;
    
    [SerializeField]
    private int mSpeed = 5000;  //Will use it later instead of 700 down in update function
    public bool mdisableRoll = false;

    private UnityAction<ReelElement> mOnReelRollEndEvent;
    public int[] _imageFillPosition = new int[4];

    private void Start()
    {
        mSpeed = Random.Range(2000, 4000);
        for (int i = 0; i < _reelElements.Length; i++)
        {
            accumalatedY += _reelElements[i]._slotElementGameObject.GetComponent<RectTransform>().sizeDelta.y;
        }
        CalculateIndexAndTotalToughness();
    }

    void Update()
    {
        if (_roll)
        {
            if (!mdisableRoll)
            {
                for (int i = _reelElements.Length - 1; i >= 0; i--)
                {
                    //Time.timeScale = 0.01f;                                                                          //700 Down is the speed it needs to roll
                    _reelElements[i]._slotElementGameObject.transform.Translate(Vector3.down * Time.smoothDeltaTime * mSpeed, Space.World);
                    if (_reelElements[i]._slotElementGameObject.transform.localPosition.y < -600)
                    {
                        _reelElements[i]._slotElementGameObject.transform.localPosition = new Vector3(_reelElements[i]._slotElementGameObject.transform.localPosition.x, _reelElements[i]._slotElementGameObject.transform.localPosition.y + accumalatedY, _reelElements[i]._slotElementGameObject.transform.localPosition.z);
                        _reelElements[i]._slotElementGameObject.transform.SetSiblingIndex(i);
                    }
                }
            }
        }
    }

    void DoFinalSet(int inIndex)
    {
        int preIndex = inIndex - 1;
        int postIndex = inIndex + 1;

        //Adjust the position the one before and one after the choosen reel
        if (inIndex == 0)
            preIndex = _reelElements.Length - 1;
        if (inIndex == _reelElements.Length - 1)
            postIndex = 0;

        Debug.Log(preIndex + "   " + postIndex);
        Transform preIndexTransform = _reelElements[preIndex]._slotElementGameObject.transform;
        Transform postIndexTransform = _reelElements[postIndex]._slotElementGameObject.transform;

        // 300 being the interval between the icons.
        preIndexTransform.localPosition = new Vector3(preIndexTransform.localPosition.x, _reelElements[inIndex]._slotElementGameObject.transform.localPosition.y - 300, preIndexTransform.localPosition.z);
        postIndexTransform.localPosition = new Vector3(postIndexTransform.localPosition.x, _reelElements[inIndex]._slotElementGameObject.transform.localPosition.y + 300, postIndexTransform.localPosition.z);
    }

    /// <summary>
    /// Event Function
    /// </summary>
    /// <param name="action"></param>
    public void OnReelRollEnd(UnityAction<ReelElement> action)
    {
        mOnReelRollEndEvent = action;
    }

    /// <summary>
    /// Calculates the accumulated overall weights / toughness for each slot elements in reels
    /// </summary>
    private void CalculateIndexAndTotalToughness()
    {
        for (int i = 0; i < _reelElements.Length; i++)
        {
            ReelElement mReel = _reelElements[i];
            mTotalToughnessMeter += mReel._chanceOfObtaining;
            mReel._toughnessMeter = mTotalToughnessMeter;

            mReel._index = i;
        }
    }

    /// <summary>
    /// Gets A random value with a given probability
    /// </summary>
    /// <returns></returns>
    private int GetRandomEnergyIndexBasedOnProbability()
    {
        double tempValue = mRandomValue.NextDouble() * mTotalToughnessMeter;
        for (int i = 0; i < _reelElements.Length; i++)
        {
            if (_reelElements[i]._toughnessMeter >= tempValue)
            {
                return i;
            }
        }
        return 0;
    }

    /// <summary>
    /// Finds a Gameobject based on probability and stop the reel at appropriate spot 
    /// </summary>
    public void Spin()
    {
        int index = GetRandomEnergyIndexBasedOnProbability();
        ReelElement mReel = _reelElements[index];
        float TargetPosition = -(mReel._slotElementGameObject.transform.localPosition.y);
        mdisableRoll = true;
        DoFinalSet(index);

        mReelsRollerParent.DOLocalMoveY(TargetPosition, _reelRollDuration, false)
        .OnComplete(() =>
        {
            mSpinOver = true;
            _roll = false;
            if (mOnReelRollEndEvent != null)
            {
                mOnReelRollEndEvent(mReel);
            }
            mOnReelRollEndEvent = null;
        });
    }

}




        //If selected Gameobject is below zero then continue roll
        //if (mReel._slotElementGameObject.transform.localPosition.y < 0)
        //{

        //if (mReel._slotElementGameObject.transform.localPosition.y > 0)
        //{

        //StartCoroutine(MoveToTargetPosition(mReel, 1f));
        //}
        //}
        //else
        // {
        //StartCoroutine(MoveToTargetPosition(mReel, 0f));
        // }

        //Should put another condition where if the selected element goes below a certain position in y while being chose by probability we need to make it to do 
        //another roll and chose the probability again
    //}

    /// <summary>
    /// Just an Enumerator to mimic invoke function but with parameters
    /// </summary>
    /// <param name="mReel"></param>
    /// <param name="delayTime"></param>
    /// <returns></returns>
    //IEnumerator MoveToTargetPosition(ReelElement mReel, float delayTime)
    //{
    //    yield return new WaitForSeconds(delayTime);

    //}

//public void Spin()
//{
//    int index = GetRandomEnergyIndexBasedOnProbability();
//    ReelElement mReel = _reelElements[index];
//    float TargetPosition = -(mReel._slotElementGameObject.transform.localPosition.y);
//    mdisableRoll = true;
//    mReelsRollerParent.DOLocalMoveY(TargetPosition, _reelRollDuration, false)
//    .OnComplete(() =>
//    {
//        for (int i = 0; i < _reelElements.Length; i++) //New Addition
//        {
//            if (_reelElements[i]._slotElementGameObject.transform.localPosition.y < -305)
//            {
//                _reelElements[i]._slotElementGameObject.transform.localPosition = new Vector3(_reelElements[i]._slotElementGameObject.transform.localPosition.x, _reelElements[i]._slotElementGameObject.transform.localPosition.y + 1200, _reelElements[i]._slotElementGameObject.transform.localPosition.z);
//            }
//        }
//        _roll = false;
//        if (mOnReelRollEndEvent != null)
//        {
//            mOnReelRollEndEvent(mReel);
//        }
//        mOnReelRollEndEvent = null;
//    });
//    //Should put another condition where if the selected element goes below a certain position in y while being chose by probability we need to make it to do 
//    //another roll and chose the probability again
//}

//for (int i = 0; i < _reelElements.Length; i++)
//{
//    Debug.Log(_reelElements[0]._slotElementGameObject.name + " " + _reelElements[1]._slotElementGameObject.name + " " + _reelElements[2]._slotElementGameObject.name + " " + _reelElements[3]._slotElementGameObject.name);
//    Time.timeScale = 0.1f;
//    //700 Down is the speed it needs to roll
//    _reelElements[i]._slotElementGameObject.transform.Translate(Vector3.down * Time.smoothDeltaTime * mSpeed, Space.World);
//    if (_reelElements[i]._slotElementGameObject.transform.localPosition.y < -600)
//    {
//        _reelElements[i]._slotElementGameObject.transform.localPosition = new Vector3(_reelElements[i]._slotElementGameObject.transform.localPosition.x, _reelElements[i]._slotElementGameObject.transform.localPosition.y + 1200, _reelElements[i]._slotElementGameObject.transform.localPosition.z);
//    }
//}

//void refernce()
//{
//    using System.Collections;
//    using System.Collections.Generic;
//    using UnityEngine;

//public class Reel : MonoBehaviour
//{

//    //This Variable Will Be Changed By The "Slots" Class To Control When The Reel Spins  
//    public bool spin;

//    //Speed That Reel Will Spin
//    int speed;

//    // Use this for initialization
//    void Start()
//    {
//        spin = false;
//        speed = 1500;
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        if (spin)
//        {
//            foreach (Transform image in transform)//This Targets All Children Objects Of The Main Parent Object
//            {
//                //Direction And Speed Of Movement
//                image.transform.Translate(Vector3.down * Time.smoothDeltaTime * speed, Space.World);

//                //Once The Image Moves Below A Certain Point, Reset Its Position To The Top 
//                if (image.transform.position.y <= 0) { image.transform.position = new Vector3(image.transform.position.x, image.transform.position.y + 600, image.transform.position.z); }
//            }
//        }
//    }

//    //Once The Reel Finishes Spinning The Images Will Be Placed In A Random Position
//    public void RandomPosition()
//    {
//        List<int> parts = new List<int>();

//        //Add All Of The Values For The Original Y Postions  
//        parts.Add(200);
//        parts.Add(100);
//        parts.Add(0);
//        parts.Add(-100);
//        parts.Add(-200);
//        parts.Add(-300);


//        foreach (Transform image in transform)
//        {
//            int rand = Random.Range(0, parts.Count);

//            //The "transform.parent.GetComponent<RectTransform>().transform.position.y" Allows It To Adjust To The Canvas Y Position
//            image.transform.position = new Vector3(image.transform.position.x, parts[rand] + transform.parent.GetComponent<RectTransform>().transform.position.y, image.transform.position.z);

//            parts.RemoveAt(rand);
//        }
//    }
//}

//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Slots : MonoBehaviour
//{

//    public Reel[] reel;
//    bool startSpin;

//    // Use this for initialization
//    void Start()
//    {
//        startSpin = false;
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        if (!startSpin)//Prevents Interference If The Reels Are Still Spinning
//        {
//            if (Input.GetKeyDown(KeyCode.K))//The Input That Starts The Slot Machine 
//            {
//                startSpin = true;
//                StartCoroutine(Spinning());
//            }
//        }
//    }

//    IEnumerator Spinning()
//    {
//        foreach (Reel spinner in reel)
//        {
//            //Tells Each Reel To Start Spnning
//            spinner.spin = true;
//        }

//        for (int i = 0; i < reel.Length; i++)
//        {
//            //Allow The Reels To Spin For A Random Amout Of Time Then Stop Them
//            yield return new WaitForSeconds(Random.Range(1, 3));
//            reel[i].spin = false;
//
//            
//        }

//        //Allows The Machine To Be Started Again 
//        startSpin = false;
//    }
//}
//}







//void Residue()
//{
//rowStopped = true;

//if (!neverDone)
//{

//}
////neverDone = true;

//else
//{
//    rowStopped = true;
//}
//Debug.Log(disableMove);

//Invoke("Targeter", Random.Range(2f,5f));
//ReelsRollerParent.DOMoveY(TargetPosition, reelRollDuration, true).OnComplete(() => { spin = false; rowStopped = true; });

//rowStopped = false;

//    //if (rowStopped == false)
//    //{

//    //}
//    //if (rowStopped == true) //Checking all If condition once the Spin has stopped
//    //{
//    //    if (Slotelements[0].GetComponent<RectTransform>().localPosition.y == 0)
//    //        stoppedSlot = Slotelements[0].name;
//    //       // Debug.Log(Slotelements[0].name);
//    //    if (Slotelements[1].GetComponent<RectTransform>().localPosition.y == 0)
//    //        stoppedSlot = Slotelements[1].name;
//    //       // Debug.Log(Slotelements[1].name);

//    //    if (Slotelements[2].GetComponent<RectTransform>().localPosition.y == 0)
//    //         stoppedSlot = Slotelements[2].name;
//    //      //  Debug.Log(Slotelements[2].name);

//    //    if (Slotelements[3].GetComponent<RectTransform>().localPosition.y == 0)
//    //          stoppedSlot = Slotelements[3].name;
//    //       // Debug.Log(Slotelements[3].name);

//    //    if (Slotelements[4].GetComponent<RectTransform>().localPosition.y == 0)
//    //         stoppedSlot = Slotelements[4].name;
//    //       //Debug.Log(Slotelements[4].name);

//    //    if (Slotelements[5].GetComponent<RectTransform>().localPosition.y == 0)
//    //          stoppedSlot = Slotelements[5].name;
//    //       // Debug.Log(Slotelements[5].name);

//    //    if (Slotelements[6].GetComponent<RectTransform>().localPosition.y == 0)
//    //        stoppedSlot = Slotelements[6].name;
//    //       // Debug.Log(Slotelements[6].name);

//    //}

//void Targeter()
//{

//}

////Once The Reel Finishes Spinning The Images Will Be Placed In A Random Position
//public void RandomPosition()
//{

//    List<int> parts = new List<int>();

//    //Add All Of The Values For The Original Y Postions  
//    parts.Add(200);
//    parts.Add(100);
//    parts.Add(0);
//    parts.Add(-100);
//    parts.Add(-200);
//    parts.Add(-300);
//    parts.Add(-400);


//    foreach (Transform image in transform)
//    {
//        int rand = Random.Range(0, parts.Count);

//        //The "transform.parent.GetComponent<RectTransform>().transform.position.y" Allows It To Adjust To The Canvas Y Position
//        image.transform.position = new Vector3(image.transform.position.x, parts[rand] + transform.parent.GetComponent<RectTransform>().transform.position.y, image.transform.position.z);

//        parts.RemoveAt(rand);
//    }
//}
//}

//using DG.Tweening;
//using UnityEngine;
//using UnityEngine.Events;

//[System.Serializable]
//public class ReelElement
//{
//    public GameObject _slotElementGameObject;

//    [Range(0, 100)] public float _chanceOfObtaining;
//    [HideInInspector] public int _index;
//    [HideInInspector] public double _toughnessMeter;
//}

//public class Reels : MonoBehaviour
//{
//    public ReelElement[] _reelElements;
//    [Range(1, 20)] public int _reelRollDuration = 4;
//    public bool _roll = false;

//    public bool mSpinOver = false;
//    public float accumalatedY;

//    private double mTotalToughnessMeter;
//    private System.Random mRandomValue = new System.Random();

//    [SerializeField]
//    private Transform mReelsRollerParent;

//    [SerializeField]
//    private int mSpeed = 5000;  //Will use it later instead of 700 down in update function
//    public bool mdisableRoll = false;

//    private UnityAction<ReelElement> mOnReelRollEndEvent;
//    public int[] _imageFillPosition = new int[4];

//    private void Start()
//    {
//        mSpeed = Random.Range(2000, 4000);
//        for (int i = 0; i < _reelElements.Length; i++)
//        {
//            accumalatedY += _reelElements[i]._slotElementGameObject.GetComponent<RectTransform>().sizeDelta.y;
//        }
//        CalculateIndexAndTotalToughness();
//    }

//    void Update()
//    {
//        if (_roll)
//        {
//            if (!mdisableRoll)
//            {
//                for (int i = _reelElements.Length - 1; i >= 0; i--)
//                {
//                    //Time.timeScale = 0.01f;                                                                          //700 Down is the speed it needs to roll
//                    _reelElements[i]._slotElementGameObject.transform.Translate(Vector3.down * Time.smoothDeltaTime * mSpeed, Space.World);
//                    if (_reelElements[i]._slotElementGameObject.transform.localPosition.y < -600)
//                    {
//                        _reelElements[i]._slotElementGameObject.transform.localPosition = new Vector3(_reelElements[i]._slotElementGameObject.transform.localPosition.x, _reelElements[i]._slotElementGameObject.transform.localPosition.y + accumalatedY, _reelElements[i]._slotElementGameObject.transform.localPosition.z);
//                        _reelElements[i]._slotElementGameObject.transform.SetSiblingIndex(i);
//                    }
//                }
//            }
//        }
//    }

//    void DoFinalSet(int inIndex)
//    {
//        int preIndex = inIndex - 1;
//        int postIndex = inIndex + 1;

//        //Adjust the position the one before and one after the choosen reel
//        if (inIndex == 0)
//            preIndex = _reelElements.Length - 1;
//        if (inIndex == _reelElements.Length - 1)
//            postIndex = 0;

//        Debug.Log(preIndex + "   " + postIndex);
//        Transform preIndexTransform = _reelElements[preIndex]._slotElementGameObject.transform;
//        Transform postIndexTransform = _reelElements[postIndex]._slotElementGameObject.transform;

//        // 300 being the interval between the icons.
//        preIndexTransform.localPosition = new Vector3(preIndexTransform.localPosition.x, _reelElements[inIndex]._slotElementGameObject.transform.localPosition.y - 300, preIndexTransform.localPosition.z);
//        postIndexTransform.localPosition = new Vector3(postIndexTransform.localPosition.x, _reelElements[inIndex]._slotElementGameObject.transform.localPosition.y + 300, postIndexTransform.localPosition.z);
//    }

//    /// <summary>
//    /// Event Function
//    /// </summary>
//    /// <param name="action"></param>
//    public void OnReelRollEnd(UnityAction<ReelElement> action)
//    {
//        mOnReelRollEndEvent = action;
//    }

//    /// <summary>
//    /// Calculates the accumulated overall weights / toughness for each slot elements in reels
//    /// </summary>
//    private void CalculateIndexAndTotalToughness()
//    {
//        for (int i = 0; i < _reelElements.Length; i++)
//        {
//            ReelElement mReel = _reelElements[i];
//            mTotalToughnessMeter += mReel._chanceOfObtaining;
//            mReel._toughnessMeter = mTotalToughnessMeter;

//            mReel._index = i;
//        }
//    }

//    /// <summary>
//    /// Gets A random value with a given probability
//    /// </summary>
//    /// <returns></returns>
//    private int GetRandomEnergyIndexBasedOnProbability()
//    {
//        double tempValue = mRandomValue.NextDouble() * mTotalToughnessMeter;
//        for (int i = 0; i < _reelElements.Length; i++)
//        {
//            if (_reelElements[i]._toughnessMeter >= tempValue)
//            {
//                return i;
//            }
//        }
//        return 0;
//    }

//    /// <summary>
//    /// Finds a Gameobject based on probability and stop the reel at appropriate spot 
//    /// </summary>
    //public void Spin()
    //{
    //    //Chooses the Probability
    //    int index = GetRandomEnergyIndexBasedOnProbability();
    //    ReelElement mReel = _reelElements[index];

    //    mdisableRoll = true;
    //    if (mReel._slotElementGameObject.transform.localPosition.y > 0)
    //    {
    //        float TargetPosition = -(mReel._slotElementGameObject.transform.localPosition.y);

    //        mReelsRollerParent.DOLocalMoveY(TargetPosition, _reelRollDuration, false)
    //        .OnUpdate(() =>
    //        {
    //            int j = 0;
    //            for (int i = 0; i < _reelElements.Length; i++, j += 1) // i = 3 , j = 2
    //            {
    //                if (_reelElements[i]._slotElementGameObject.name != mReel._slotElementGameObject.name)
    //                {
    //                    _reelElements[i]._slotElementGameObject.transform.localPosition = new Vector3(_reelElements[i]._slotElementGameObject.transform.localPosition.x, _imageFillPosition[j] + mReel._slotElementGameObject.transform.localPosition.y, _reelElements[i]._slotElementGameObject.transform.localPosition.z);
    //                }
    //                else
    //                {
    //                    j -= 1;
    //                    continue;
    //                }
    //            }
    //        })
    //        .OnComplete(() =>
    //        {
    //            _roll = false;
    //            if (mOnReelRollEndEvent != null)
    //            {
    //                mOnReelRollEndEvent(mReel);
    //            }
    //            mSpinOver = true;
    //            mOnReelRollEndEvent = null;
    //        });
    //    }
    //    else
    //    {
    //        mReel._slotElementGameObject.transform.localPosition = new Vector3(mReel._slotElementGameObject.transform.localPosition.x, accumalatedY, mReel._slotElementGameObject.transform.localPosition.z);
    //        var newPosition = mReel._slotElementGameObject.transform.localPosition;
    //        float TargetPosition = -(newPosition.y);

    //        mReelsRollerParent.DOLocalMoveY(TargetPosition, _reelRollDuration, false)
    //        .OnUpdate(() =>
    //        {
    //            int j = 0;
    //            for (int i = 0; i < _reelElements.Length; i++, j += 1) // i = 3 , j = 2
    //            {
    //                if (_reelElements[i]._slotElementGameObject.name != mReel._slotElementGameObject.name)
    //                {
    //                    _reelElements[i]._slotElementGameObject.transform.localPosition = new Vector3(_reelElements[i]._slotElementGameObject.transform.localPosition.x, _imageFillPosition[j] + mReel._slotElementGameObject.transform.localPosition.y, _reelElements[i]._slotElementGameObject.transform.localPosition.z);
    //                }
    //                else
    //                {
    //                    j -= 1;
    //                    continue;
    //                }
    //            }
    //        })
    //        .OnComplete(() =>
    //        {
    //            _roll = false;
    //            if (mOnReelRollEndEvent != null)
    //            {
    //                mOnReelRollEndEvent(mReel);
    //            }
    //            mSpinOver = true;
    //            mOnReelRollEndEvent = null;
    //        });
    //    }