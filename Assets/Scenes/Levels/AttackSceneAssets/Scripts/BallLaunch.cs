using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Parabolic Missile
/// < para > Calculating trajectory and steering </para >
/// <para>ZhangYu 2019-02-27</para>
/// </summary>
public class BallLaunch : MonoBehaviour
{

    public Transform target; //target
    public AttackManager _attackManager;
    public float hight = 16f; // parabolic height
    public float gravity = 9.8f; // gravitational acceleration
    private GameObject _bullet;
    private Vector3 position; //My position
    private Vector3 dest; //Target location
    private Vector3 Velocity; //Motion Velocity
    private float time = 0; // Motion time
    public bool BallFlow = true;
    public bool BallReverse = false;
    public GameObject CrackCanvas;
    private float ShieldCameraDistance;
    // public Vector3 offset = new Vector3 (.4f,34.4f,66.9f);

    public void Awake()
    {
        
    }

    private void Start()
    {
        _attackManager = GameObject.Find("AttackManager").GetComponent<AttackManager>();
        CrackCanvas = GameObject.Find("CrackCanvas");
        dest = target.position;
        position = transform.position;
        Velocity = PhysicsUtil.GetParabolaInitVelocity(position, dest, gravity, hight, 0);
        transform.LookAt(PhysicsUtil.GetParabolaNextPosition(position, Velocity, gravity, Time.deltaTime));
    }

    private void Update()
    {
        if (BallFlow == true)
        {
            // Computational displacement
            float deltaTime = Time.deltaTime;
            position = PhysicsUtil.GetParabolaNextPosition(position, Velocity, gravity, deltaTime);
            transform.position = position;
            time += deltaTime;
            Velocity.y += gravity * deltaTime;

            // Computational steering
            transform.LookAt(PhysicsUtil.GetParabolaNextPosition(position, Velocity, gravity, deltaTime));
        }
        if (BallReverse == true)
        {
        //    Vector3 Newdist = Camera.main.ScreenToWorldPoint(CrackCanvas.transform.GetChild(0).gameObject.transform.position);
          //  Debug.Log(Newdist + "Panel Position");
            this.gameObject.transform.position = Vector3.Lerp(this.gameObject.transform.position, Camera.main.transform.position, Time.deltaTime * 5);

            if (Vector3.Distance(this.gameObject.transform.position, Camera.main.transform.position) < (ShieldCameraDistance * .24))
            {
                BallReverse = false;

                this.gameObject.transform.GetChild(4).gameObject.SetActive(true);
              // // CrackCanvas.gameObject.transform.GetChild(0).gameObject.SetActive(true);
              //  Debug.Log(CrackCanvas);
              ////  Debug.Log(CrackCanvas.gameObject.transform.GetChild(0).gameObject + "child Panel name");
              //  Debug.LogError("Ball Reverse Stopped");
              //  Debug.Log(Camera.main.ScreenToWorldPoint (CrackCanvas.transform.position) + "  Camera Panel ScreenView");
            }
        }

        // Simply simulate collision detection
        // if (position.y <= dest.y) enabled = false;
    }

    public void OnCollisionEnter(Collision col)
    {
        Debug.Log("Collision Entered");
        Debug.Log(col.gameObject.name);
        GameObject.Find("Cannon").GetComponent<CannonShotController>().fixCameraRot = false;
        _bullet = this.gameObject;

        if (_attackManager._Shield == true)
        {
            if (col.gameObject.tag == "Shield Protection")
            {
                BallFlow = false;
                Debug.LogError(col.transform.position + "ball Last Position");
                Debug.LogError(col.transform.localPosition + "ball Last Position");
                
                Camera.main.transform.parent = null;
               // this.gameObject.GetComponent<Rigidbody>().useGravity = false;
                this.gameObject.GetComponent<Rigidbody>().isKinematic = true;
              //  this.gameObject.GetComponent<Rigidbody>().useGravity = false;
                Debug.LogError(col.transform.position + "ball Last Position");
                Debug.Log(this.gameObject.transform.position + "  Ball Last hit position");
                Debug.Log(Camera.main.transform.position + " Camera last position");


                //BallReverse = true;
                //Invoke("BallReturnDelay", .5f);
                BallReverse = true;
                ShieldCameraDistance = Vector3.Distance(this.gameObject.transform.position, Camera.main.transform.position);
                Debug.Log(ShieldCameraDistance + "  Shield Camera Distance");
                Debug.Log(ShieldCameraDistance / 2 + " Shield Camera Halfway Distance");
                Debug.Log(ShieldCameraDistance / 3 + " Shield Camera Quaterway Distance");
                //  this.gameObject.transform.position = Vector3.Lerp(this.gameObject.transform.position, Camera.main.transform.position, Time.deltaTime);

                Debug.Log("SHIELD PROTECTED");
            }
        }
        else if (_attackManager._Shield == false)
        {
            Debug.Log("Shield Disabled");
            Debug.Log(_bullet.transform.childCount + "Child Count");
            for (int i = 0; i < _bullet.transform.childCount - 2; i++)
            {
                _bullet.transform.GetChild(i).gameObject.SetActive(true);
                _bullet.transform.GetChild(i).parent = null;

                Debug.Log(_bullet.transform.GetChild(i).gameObject.name);
            }
            Camera.main.transform.parent = null;

            _bullet.SetActive(false);
        }
        // Camera.main.transform.parent = null;

        // _bullet.SetActive(false);
    }

    public void BallReturnDelay()
    {
        BallReverse = true;
    }

}

//if (_attackManager._Shield == true)
//{
//    Debug.Log("Shield Activated");

//    for (int i = 0; i < _bullet.transform.childCount-2; i++)
//    {
//        _bullet.transform.GetChild(i).gameObject.SetActive(true);
//        _bullet.transform.GetChild(i).parent = null;

//        Debug.Log(_bullet.transform.GetChild(i).gameObject.name);
//    }
//    /* _bullet.transform.GetChild(0).gameObject.SetActive(true);
//     _bullet.transform.GetChild(1).gameObject.SetActive(true);
//     _bullet.transform.GetChild(0).parent = null;
//     _bullet.transform.GetChild(1).parent = null;
//    */
//}
//else
//{
//    Debug.Log("Shield Disabled");
//    Debug.Log(_bullet.transform.childCount + "Child Count");
//    for(int i=0; i < _bullet.transform.childCount; i++)
//    {
//        _bullet.transform.GetChild(i).gameObject.SetActive(true);
//        _bullet.transform.GetChild(i).parent = null;

//        Debug.Log(_bullet.transform.GetChild(i).gameObject.name);
//    }
//    /*  _bullet.transform.GetChild(0).gameObject.SetActive(true);
//      _bullet.transform.GetChild(1).gameObject.SetActive(true);
//      _bullet.transform.GetChild(2).gameObject.SetActive(true);
//      _bullet.transform.GetChild(3).gameObject.SetActive(true);
//      _bullet.transform.GetChild(0).parent = null;
//      _bullet.transform.GetChild(1).parent = null;
//      _bullet.transform.GetChild(2).parent = null;
//      _bullet.transform.GetChild(3).parent = null;
//    */
//}
//Camera.main.transform.parent = null;

//_bullet.SetActive(false);