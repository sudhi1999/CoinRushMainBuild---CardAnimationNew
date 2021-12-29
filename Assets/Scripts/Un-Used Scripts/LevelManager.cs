using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelManager : MonoBehaviour
{
    

    /// <summary>
    /// Keeps updating coin and energy text fields
    /// </summary>
    //private void UpdateCoinAndEnergyTextFields()
    //{
    //    _coinText.text = mGameManager._coins.ToString();
    //    _energyText.text = mGameManager._energy.ToString();
    //}
}


//void residue()
//{
//        public List<GameObject> _fiveThousandCoinList;
//public List<GameObject> _twentyFiveThousandCoinList;
//public List<GameObject> _hunderThousandCoinList;
//public List<GameObject> _fiveHundredThousandCoinList;
//public List<GameObject> _OneMillionJackPotCardList;
//public List<GameObject> _AttackCardList;
//public List<GameObject> _StealCardList;
//public List<GameObject> _SheildCardList;
//public List<GameObject> _TenEnergyCardList;
//public List<GameObject> _TwentyFiveEnergyCardList;
//public List<GameObject> _HundredEnergyCardList;


//public GameManager _gameManager;

//public TextMeshProUGUI _coinText;
//public TextMeshProUGUI _energyText;

//public bool isDone = false;


//private void Update()
//{
//    UpdateCoinAndEnergyTextFields();
//    if (!isDone)
//    {
//        if (_fiveThousandCoinList.Count == 3)
//        {
//            _gameManager._coins += 5000;
//            isDone = true;
//            foreach (GameObject c in _fiveThousandCoinList)
//            {
//                //PlayAnimation
//                Destroy(c);
//            }
//            _fiveThousandCoinList.Clear();
//            Invoke("IsDone", .1f);
//        }
//        if (_twentyFiveThousandCoinList.Count == 3)
//        {
//            _gameManager._coins += 25000;
//            isDone = true;
//            foreach (GameObject c in _twentyFiveThousandCoinList)
//            {
//                //PlayAnimation
//                Destroy(c);
//            }
//            _twentyFiveThousandCoinList.Clear();
//            Invoke("IsDone", .1f);
//        }
//        if (_hunderThousandCoinList.Count == 3)
//        {
//            _gameManager._coins += 100000;
//            isDone = true;
//            foreach (GameObject c in _hunderThousandCoinList)
//            {
//                //PlayAnimation
//                Destroy(c);
//            }
//            _hunderThousandCoinList.Clear();
//            Invoke("IsDone", .1f);
//        }
//        if (_fiveHundredThousandCoinList.Count == 3)
//        {
//            _gameManager._coins += 500000;
//            isDone = true;
//            foreach (GameObject c in _fiveHundredThousandCoinList)
//            {
//                //PlayAnimation
//                Destroy(c);
//            }
//            _fiveHundredThousandCoinList.Clear();
//            Invoke("IsDone", .1f);
//        }
//        if (_OneMillionJackPotCardList.Count == 3)
//        {
//            _gameManager._coins += 1000000;
//            isDone = true;
//            foreach (GameObject c in _OneMillionJackPotCardList)
//            {
//                //PlayAnimation
//                Destroy(c);
//            }
//            _OneMillionJackPotCardList.Clear();
//            Invoke("IsDone", .1f);
//        }
//        if (_TenEnergyCardList.Count == 3)
//        {
//            _gameManager._energy += 10;
//            isDone = true;
//            foreach (GameObject c in _TenEnergyCardList)
//            {
//                //PlayAnimation
//                Destroy(c);
//            }
//            _TenEnergyCardList.Clear();
//            Invoke("IsDone", .1f);
//        }
//        if (_TwentyFiveEnergyCardList.Count == 3)
//        {
//            _gameManager._energy += 25;
//            isDone = true;
//            foreach (GameObject c in _TwentyFiveEnergyCardList)
//            {
//                //PlayAnimation
//                Destroy(c);
//            }
//            _TwentyFiveEnergyCardList.Clear();
//            Invoke("IsDone", .1f);
//        }
//        if (_HundredEnergyCardList.Count == 3)
//        {
//            _gameManager._energy += 100;
//            isDone = true;
//            foreach (GameObject c in _HundredEnergyCardList)
//            {
//                //PlayAnimation
//                Destroy(c);
//            }
//            _HundredEnergyCardList.Clear();
//            Invoke("IsDone", .1f);
//        }
//        if (_AttackCardList.Count == 3)
//        {
//            SceneManager.LoadScene("AttackScene");
//            isDone = true;
//            foreach (GameObject c in _AttackCardList)
//            {
//                //PlayAnimation
//                Destroy(c);
//            }
//            _AttackCardList.Clear();
//            Invoke("IsDone", .1f);
//        }
//        if (_SheildCardList.Count == 3)
//        {
//            _gameManager._shield += 3;
//            isDone = true;
//            foreach (GameObject c in _SheildCardList)
//            {
//                //PlayAnimation
//                Destroy(c);
//            }
//            _SheildCardList.Clear();
//            Invoke("IsDone", .1f);
//        }
//        if (_StealCardList.Count == 3)
//        {
//            SceneManager.LoadScene("StealScene");
//            isDone = true;
//            foreach (GameObject c in _StealCardList)
//            {
//                //PlayAnimation
//                Destroy(c);
//            }
//            _StealCardList.Clear();
//            Invoke("IsDone", .1f);
//        }
//    }
//}

//void IsDone()
//{
//    isDone = false;
//}

///// <summary>
///// Keeps updating coin and energy text fields
///// </summary>
//private void UpdateCoinAndEnergyTextFields()
//{
//    _coinText.text = _gameManager._coins.ToString();
//    _energyText.text = _gameManager._energy.ToString();
//}
//}
