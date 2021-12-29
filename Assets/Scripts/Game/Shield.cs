using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Shield : MonoBehaviour
{
    public int _shieldValue;
    public TextMeshProUGUI _uiShieldValue;
    public GameObject _uiShieldFullText;
    public List<GameObject> _Properties = new List<GameObject>();
    public List<GameObject> _ShieldedProperties = new List<GameObject>();
    private GameObject _destroyedProperty;
    private GameManager mGameManager;


    // Start is called before the first frame update
    void Start()
    {
        mGameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //shield value Clamp
        mGameManager._shield = Mathf.Clamp(mGameManager._shield, 0, _shieldValue);
        
        //shield Values are updated in UI
        ShieldValuesUI();

        //if We get Shield Card, add a property from the list randomly to the shielded properties list
        if (Input.GetKeyDown(KeyCode.S))
        {                     
            ApplyShield();            
        }

        //if Shield gets destroyed, Remove the shield from shielded properties list  
        if(Input.GetKeyDown(KeyCode.D))
        {
            RemoveShield();
        }
    }  
    public void ApplyShield()
    {
        mGameManager._shield += 1;
        if (mGameManager._shield > _shieldValue)
        {
            mGameManager._energy += 3;
        }
        else
        {
            return;
        }    
        if (mGameManager._shield  <= _shieldValue)
        {
            int randomIndex = Random.Range(0, _Properties.Count);
            GameObject shieldedProperty = _Properties[randomIndex];
            _ShieldedProperties.Add(shieldedProperty);
            _Properties.RemoveAt(randomIndex);               
        }
    }
    private void RemoveShield()
    {
        mGameManager._shield -= 1;
        int randomIndex = Random.Range(0, _ShieldedProperties.Count);
        _destroyedProperty = _ShieldedProperties[randomIndex];
        _Properties.Add(_destroyedProperty);
        _ShieldedProperties.RemoveAt(randomIndex);
    }

    private void ShieldValuesUI()
    {
        _uiShieldValue.text = mGameManager._shield.ToString();
        if (mGameManager._shield > _shieldValue - 1)
        {
            _uiShieldValue.text = null;
            _uiShieldFullText.SetActive(true);
        }
        else
        {
            _uiShieldFullText.SetActive(false);
        }
    }
}
