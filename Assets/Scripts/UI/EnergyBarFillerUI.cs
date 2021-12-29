using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBarFillerUI : MonoBehaviour
{
    public Image _energyBarFillerImage;
    public GameManager _gameManager;
    private float mEnergy, mMaxEnergy;
    private float lerpSpeed;

    private void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void Update()
    {
        mEnergy = _gameManager._energy;
        mMaxEnergy = _gameManager._maxEnergy;
        lerpSpeed = 3f * Time.deltaTime;
        HealthBarFiller();
       
    }

    private void HealthBarFiller()
    {
        _energyBarFillerImage.fillAmount = Mathf.Lerp(_energyBarFillerImage.fillAmount, mEnergy / mMaxEnergy, lerpSpeed);
    }
}
