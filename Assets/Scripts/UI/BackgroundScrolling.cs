using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScrolling : MonoBehaviour
{
    [SerializeField]
    private GameObject[] mImages;

    [SerializeField]
    public float mScrollSpeed;

    public float exceedSpot;
    public float rePositionSpot;

    void Start()
    {
    }

    
    void Update()
    {
        for (int i = 0; i < mImages.Length; i++)
        {
            mImages[i].transform.Translate(Vector3.up * Time.smoothDeltaTime * mScrollSpeed, Space.World);
            if(mImages[i].transform.localPosition.y > exceedSpot)
            {
                mImages[i].transform.position = new Vector3(mImages[i].transform.position.x, mImages[i].transform.position.y - rePositionSpot, mImages[i].transform.position.z);
            }
        }
    }
}
