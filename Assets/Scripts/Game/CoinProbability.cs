using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class Coins
{
    public int _coinAmount;
    [Range(0, 100)] public float _chanceOfObtaining;
    [HideInInspector] public int _index;
    [HideInInspector] public double _toughnessMeter;

    public Coins(int coinAmount, float chanceOfObtaining, int index, double toughnessMeter)
    {
        _coinAmount = coinAmount;
        _chanceOfObtaining = chanceOfObtaining;
        _index = index;
        _toughnessMeter = toughnessMeter;
    }
}

public class CoinProbability : MonoBehaviour
{
    public List<Coins> _coins;

    private double mTotalToughnessMeter;
    private System.Random mRandomValue = new System.Random();

    private Coins mCoin;

    private float mChanceA = 90, mChanceB = 30, mChanceC = 2, mChanceD = 1, mChanceE = .5f;

    private void Awake()
    {
        _coins = new List<Coins>(5);
        _coins.Add(new Coins(5000, mChanceA, 0, 0));
        _coins.Add(new Coins(25000, mChanceB, 0, 0));
        _coins.Add(new Coins(100000, mChanceC, 0, 0));
        _coins.Add(new Coins(500000, mChanceD, 0, 0));
        _coins.Add(new Coins(1000000, mChanceE, 0, 0));
    }

    public void Start()
    {
        CalculateIndexAndTotalToughness();

        #region Example Probability Thing
        //for (int i = 0;i <= 5;i++)
        //{
        //    if(i == 3)
        //    {
        //        continue;
        //    }
        //    Debug.Log(i);
        //}
        #endregion
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            SceneManager.LoadScene(0);
           
        }
    }

    /// <summary>
    /// This function Calculates the total toughness / weight to all elements in energy by accumulation of their chances of obtaining
    /// </summary>
    private void CalculateIndexAndTotalToughness()
    {
        for (int i = 0; i < _coins.Count; i++)
        {
            mCoin = _coins[i];
            mTotalToughnessMeter += mCoin._chanceOfObtaining;
            mCoin._toughnessMeter = mTotalToughnessMeter;
            
            mCoin._index = i;
        }
    }

    /// <summary>
    /// Final Output of what value we will get after all the probability calculations when openin the box
    /// </summary>
    public int DisplayTheFinalElementBasedOnRandomValueGenerated()
    {
        int index = GetRandomEnergyIndexBasedOnProbability();
        mCoin = _coins[index];
        return mCoin._coinAmount;

        #region Future Plan
        //Open Next Scene

        //Getting the index we need another set of gameobject array that has all the chest in it
        //Another for loop that put value to all the gameObjects and assign values and only ignores the value that is being displayed
        //like for Example a function written in start function that skips if i == 3. Same Way here it skips assign if the the given value == the index amount by providing continue
        #endregion
    }

    /// <summary>
    /// Generates a random number and checks the toughness meter of elements
    /// </summary>
    /// <returns></returns>
    private int GetRandomEnergyIndexBasedOnProbability()
    {
        double tempValue = mRandomValue.NextDouble() * mTotalToughnessMeter;
        for (int i = 0; i < _coins.Count; i++)
        {
            if (_coins[i]._toughnessMeter >= tempValue)
            {
                return i;
            }
        }
        return 0;
    }
}


//void residue()
//{
//    //Shuffle();
//    ///// <summary>
//    ///// Shuffles the coins array list everytime and provides boxes their specific values
//    ///// </summary>
//    //private void Shuffle()
//    //{
//    //    int j = 0;
//    //    for (int i = 0; i <= _coinValues.Length - 1; i++, j += 1)
//    //    {
//    //        int randomValue = Random.Range(i, _coinValues.Length);
//    //        mRandomNumber = _coinValues[randomValue];
//    //        _coinValues[randomValue] = _coinValues[i];
//    //        _coinValues[i] = mRandomNumber;
//    //        _coinChests[j].value = _coinValues[i];
//    //    }
//    //}
//}