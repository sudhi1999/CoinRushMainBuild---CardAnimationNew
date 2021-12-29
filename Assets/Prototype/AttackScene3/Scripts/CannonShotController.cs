using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonShotController : MonoBehaviour
{

    public Rigidbody _bulletPrefab, _bullet;
    public Transform _shotPoint;
    public Transform _TargetTransform;
    public AttackManager _AttackManager;
    //  public Quaternion CameraAttackRotation;
    // public float CameraAttackPositionZ = -665f;
    public Vector3 CannonAttackPosition;
    Quaternion rot;
    public bool fixCameraRot = false;
    public bool Halfwayreached = true;
    public bool ishalfwayreached = true;
    float CannonTargetDistance;
    float BulletSpeed = 0f;
    float deceleration = 125f;
    public GameObject shieldPref;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {


        if (fixCameraRot)
        {
            Camera.main.transform.LookAt(_TargetTransform);
            if (Vector3.Distance(_bullet.transform.position, _TargetTransform.position) < (CannonTargetDistance * .85))
            {
                Debug.Log("Quaterway reached");
                Debug.Log(CannonTargetDistance / 3);

                Camera.main.transform.position += Time.deltaTime * BulletSpeed * Vector3.back;
                BulletSpeed += deceleration * Time.deltaTime;
                Debug.Log(" Speed " + BulletSpeed);

                ////  fixCameraRot = false;
                //if ((Vector3.Distance(_bullet.transform.position, _TargetTransform.position) < (CannonTargetDistance * .5)) && ( Halfwayreached == true))
                //{
                //    Debug.Log("halfway region entered");
                //    Halfwayreached = false;
                //    Debug.Log("Halfway region false");

                //}

            }
            if ((Vector3.Distance(_bullet.transform.position, _TargetTransform.position) < (CannonTargetDistance * .65)) && ishalfwayreached == true)
            {
                Debug.Log("halfway region entered");
                Halfwayreached = false;
                if (_AttackManager._Shield == true)
                { 
                GameObject ShieldPrefab = Instantiate(shieldPref, _TargetTransform.position, Quaternion.identity);
                }
                ishalfwayreached = false;
                Debug.Log(CannonTargetDistance / 2);
                Debug.Log("Halfway region false");

            }

        }
        /* if (Halfwayreached == true)
         {
             Debug.Log("Halfway reached");
             GameObject shieldprefab = Instantiate(shieldPref, _TargetTransform.position, Quaternion.identity);
           //   Camera.main.transform.parent = null;

         }
        */
    }


    /*  void LaunchProjectile()
      {
          Ray camRay = cam.ScreenPointToRay(Input.mousePosition);
          RaycastHit hit;

          if (Physics.Raycast(camRay, out hit, 100f, layer))
          {
              Cursor.SetActive(true);
              Cursor.transform.position = hit.point + Vector3.up * 0.1f;

              Vector3 Vo = CalculateVelocity(hit.point, _shotPoint.position, 1f);

              transform.rotation = Quaternion.LookRotation(Vo);

              if (Input.GetMouseButtonDown(0))
              {
                  Rigidbody obj = Instantiate(_bulletPrefab, _shotPoint.position, Quaternion.identity);
                  obj.velocity = Vo;
              }

          }
          else
          {
              Cursor.SetActive(false);
          }
      } */

    /// <summary>
    ///  Calcuate the Projectile  of the Bullet from from Origin to Target
    /// </summary>
    /// <param name="target"></param>
    /// <param name="origin"></param>
    /// <param name="time"></param>
    /// <returns></returns>
    Vector3 CalculateVelocity(Vector3 target, Vector3 origin, float time)
    {
        //Define 
        Vector3 _distance = target - origin;
        Vector3 _distanceXZ = _distance;
        _distanceXZ.y = 0f;

        //Distance Value

        float sY = _distance.y;
        float sXZ = _distanceXZ.magnitude;

        float Vxz = sXZ / time;
        float Vy = sY / time + 0.5f * Mathf.Abs(Physics.gravity.y) * time;

        Vector3 result = _distanceXZ.normalized;
        result *= Vxz;
        result.y = Vy;
        Debug.Log("Vector3 value" + result);
        return result;

    }

    /// <summary>
    /// Cannon look at the Target and Instantiate the Bullet Prefab
    /// </summary>
    /// <param name="tran"></param>
    public void AssignPos(Transform tran)
    {
        _TargetTransform = tran;
        this.transform.position = new Vector3(_TargetTransform.position.x, CannonAttackPosition.y, CannonAttackPosition.z);
        this.gameObject.SetActive(true);
        Invoke("ShootBullet", 2.5f);
    }

    public void ShootBullet()
    {
        _bullet = Instantiate(_bulletPrefab, _shotPoint.position, Quaternion.identity);
        _bullet.GetComponent<BallLaunch>().target = _TargetTransform;
        Debug.Log("Cannon fired");
        // Camera.main.transform.parent = _bullet.transform;
        Invoke("FollowCamera", .1f);
        // Invoke("DetachCamera",1.5f);
        //Invoke("DestroyBullet", 2.0f);
    }


    public void FollowCamera()
    {
        //  Vector3 velocity = Vector3.zero;

        // rot = Camera.main.transform.rotation;
        Camera.main.transform.parent = _bullet.transform;
        CannonTargetDistance = Vector3.Distance(this.gameObject.transform.position, _TargetTransform.position);
        Debug.Log(this.gameObject.transform.position);
        Debug.Log(_TargetTransform.position);
        Debug.Log(CannonTargetDistance + " display distance values");

        //  Quaternion smooth = Quaternion.Lerp(_bullet.transform.rotation, Camera.main.transform.rotation, Time.deltaTime * 1f);
        //Camera.main.transform.rotation = rot;
        // transform.position = Vector3.SmoothDamp(Camera.main.transform.position, _bullet.transform.position, ref velocity, .3f);
        Camera.main.transform.LookAt(_bullet.transform);
        fixCameraRot = true;
        Camera.main.transform.LookAt(_TargetTransform);
        //-33 0.172 0
        //  67.345  -0.3 -0.27

        //Camera.main.transform.GetComponent<AttackCameraController>()._BulletActive = true;
        //Camera.main.transform.GetComponent<AttackCameraController>().BulletTrans = this.transform;


    }

    /// <summary>
    /// To detach the camera before the ball hits the building 
    /// </summary>
    public void DetachCamera()
    {
        Camera.main.transform.parent = null;
        fixCameraRot = false;

        //Camera.main.transform.GetComponent<AttackCameraController>()._BulletActive = false;
    }
    /*
    public void FollowCamera()
    {
        Camera.main.transform.parent.parent = _bullet.transform;
    }
    /// <summary>
    /// To detach the camera before the ball hits the building 
    /// </summary>
    public void DetachCamera()
    {
       Camera.main.transform.parent = null;
    }*/

    /// <summary>
    /// 1. To destroy the ball when it hits the building 
    /// 2. Activate the Particle effects based on Shield(T/F)
    /// </summary>
    public void DestroyBullet()
    {
        Destroy(_bullet, 0f);

        if (_AttackManager._Shield == true)
        {
            Debug.Log("Shield Activated");
            Debug.Log(_bulletPrefab.transform.GetChild(0).name);
            _bullet.transform.GetChild(0).gameObject.SetActive(true);
            _bullet.transform.GetChild(1).gameObject.SetActive(true);
            //_bullet.transform.GetChild(0).parent = null;
        }
        else
        {
            Debug.Log("Shield Disabled");
            Debug.Log(_bulletPrefab.transform.GetChild(1).name);
            _bullet.transform.GetChild(0).gameObject.SetActive(true);
            _bullet.transform.GetChild(1).gameObject.SetActive(true);
            _bullet.transform.GetChild(2).gameObject.SetActive(true);
            _bullet.transform.GetChild(3).gameObject.SetActive(true);
            _bullet.transform.GetChild(1).parent = null;
        }
    }
}
