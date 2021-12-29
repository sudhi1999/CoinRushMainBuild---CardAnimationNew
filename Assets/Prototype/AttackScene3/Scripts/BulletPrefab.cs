using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPrefab : MonoBehaviour
{

    public GameObject _attackManager;

    /// <summary>
    /// This destroys the Bullet when it collides with the Target
    /// </summary>
    /// <param name="collision"></param>
    public void OnCollisionEnter(Collision collision)
    {
        Camera.main.transform.parent = null;
        Destroy(this.gameObject, .0f);
    }
}
