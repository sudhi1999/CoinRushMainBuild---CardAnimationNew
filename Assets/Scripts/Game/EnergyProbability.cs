using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class Energy
{
    public int _energyAmount;
    [Range(0, 100)] public float _chanceOfObtaining;
    public int _index;
    public double _toughnessMeter;

    public Energy(int energyAmount , float chanceOfObtaining , int index , double toughnessMeter)
    {
        _energyAmount = energyAmount;
        _chanceOfObtaining = chanceOfObtaining;
        _index = index;
        _toughnessMeter = toughnessMeter;
    }
}

public class EnergyProbability : MonoBehaviour
{
    public List<Energy> _energies;
    
    private double mTotalToughnessMeter;
    private System.Random mRandomValue = new System.Random();

    private float mChanceA = 90, mChanceB = 20, mChanceC = 1;

    private Energy mEnergy;

    private void Awake()
    {
        _energies = new List<Energy>(3);
        _energies.Add(new Energy(10, mChanceA, 0, 0));
        _energies.Add(new Energy(25, mChanceB, 0, 0));
        _energies.Add(new Energy(100, mChanceC, 0, 0));
    }

    public void Start()
    {
        AllocateToughnessAndIndex();

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
        if(Input.GetKeyDown(KeyCode.E))
        {
            SceneManager.LoadScene(0);
        }
    }

    /// <summary>
    /// This function Calculates the total toughness / weight to all elements in energy by accumulation of their chances of obtaining
    /// </summary>
    private void AllocateToughnessAndIndex()
    {
        for (int i = 0; i < _energies.Count; i++)
        {
            mEnergy = _energies[i];
            mTotalToughnessMeter += mEnergy._chanceOfObtaining;
            mEnergy._toughnessMeter = mTotalToughnessMeter;

            mEnergy._index = i;
        }
    }

    /// <summary>
    /// Final Output of what value we will get after all the probability calculations when openin the box
    /// </summary>
    public int DisplayTheFinalElementBasedOnRandomValueGenerated()
    {
        int index =  GetRandomEnergyIndexBasedOnProbability();
        mEnergy = _energies[index];
        return mEnergy._energyAmount;

        #region Future Plan
        //Open Next Scene

        //Getting the index we need.. another set of gameobject array that has all the chest in it
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
        for (int i = 0; i < _energies.Count; i++)
        {
            if (_energies[i]._toughnessMeter >= tempValue)
            {
                return i;
            }
        }
        return 0;
    }
}


//StartCoroutine(KeepUpdating());
//IEnumerator KeepUpdating()
//{
//    if (_onceClicked)
//    {
//        _energies[0]._chanceOfObtaining = 1;
//        _energies[1]._chanceOfObtaining = 1;
//        _energies[2]._chanceOfObtaining = 90;
//    }
//    else
//    {
//        _energies[0]._chanceOfObtaining = 90;
//        _energies[1]._chanceOfObtaining = 30;
//        _energies[2]._chanceOfObtaining = 1;
//    }
//    yield return null;
//}
//private void Update()
//{
//    Debug.Log(_onceClicked);
//    if (Input.GetKeyDown(KeyCode.Backspace))
//    {
//        _onceClicked = true;
//        //mChanceA = 1;
//        //mChanceB = 1;
//        //mChanceC = 90;
//    }

//    if(Input.GetKeyDown(KeyCode.Space))
//    {
//        //DisplayTheFinalElementBasedOnRandomValueGenerated();
//        _onceClicked = false;
//        //mChanceA = 90;
//        //mChanceB = 30;
//        //mChanceC = 1;
//    }

//    #region "Possible Future Conditions"
//    //if( Somecondition (Dintgetjackpotforlongtime) )
//    //{
//    //   mChanceA = 1;
//    //   mChanceB = 1;
//    //   mChanceC = 90;
//    //   jackpotGiven = true;
//    //}
//    //if(jackpotGiven = true)
//    //{
//    //    mChanceA = 90;
//    //    mChanceB = 30;
//    //    mChanceC = 1;
//    //}
//    #endregion
//}