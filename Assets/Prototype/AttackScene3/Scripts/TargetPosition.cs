using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPosition : MonoBehaviour
{
 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnMouseDown()
    {
      /*  GameObject _RevengeButton = GameObject.Find("RevengeButton");
        _RevengeButton.SetActive(false); */
        GameObject _AttackManager = GameObject.Find("AttackManager");
        _AttackManager.GetComponent<AttackManager>().AssignTarget(this.gameObject.transform);
    } 
}
