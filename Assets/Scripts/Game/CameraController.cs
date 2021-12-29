using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Camera")]
    public Transform _CameraParent;
    private float mInitialPositionX;
    private float mChangedPositionX;
    private float mInitialPositionY;
    private float mChangedPositionY;

    [Header("Horizontal Panning")]
    //[SerializeField] private Transform mTargetToRotateAround;
    [SerializeField] private float mHorizontalPanSpeed;
    public float _CameraLeftBound = 0;
    public float _CameraRightBound = 0;

    public float _CameraUpBound = 0;
    public float _CameraDownBound = 0;

    [Header("Vertical Zooming")]
    [SerializeField] private float mZoomSpeed;
    [SerializeField] private float mCameraNearBound;
    [SerializeField] private float mCameraFarBound;


    [Header("Camera Views")]
    private Transform _currentView;

    private Vector3 initialVector = Vector3.forward;
    private Vector2 _MouseDownPosition = Vector2.zero;

    public Transform[] _views;
    public float _transitionSpeed;
    public RectTransform _DrawButtonRectTransform;
    public RectTransform _OpenHandRectTransform;

    public bool _DrawButtonClicked = false;
    public bool _CameraFreeRoam = true;
    public bool _isCameraInGamePlayView;
    public GameObject OpenCardRegion;

    public int _RotationLimit = 30;

    private CardDeck mCardDeck;

    private void Start()
    {
        mCardDeck = GameObject.Find("CardDeck").GetComponent<CardDeck>();

        _CameraParent = transform.parent;

        //if (mTargetToRotateAround != null)
        //{
        //    initialVector = transform.position - mTargetToRotateAround.position;
        //    initialVector.y = 0;
        //}
    }

    public void DrawButtonClicked()
    {
        _DrawButtonClicked = true;
        transform.localEulerAngles = Vector3.zero;
        transform.localPosition = Vector3.zero;
        _CameraFreeRoam = false;
    }

    public void SetCameraFreeRoam()
    {
        _CameraFreeRoam = true;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _MouseDownPosition = Input.mousePosition;
            Vector2 localMousePosition = _DrawButtonRectTransform.InverseTransformPoint(Input.mousePosition);
            Vector2 localMousePosition1 = _OpenHandRectTransform.InverseTransformPoint(Input.mousePosition); //New Addition
            if (_isCameraInGamePlayView) //New Addition
            {
                if (!_DrawButtonRectTransform.rect.Contains(localMousePosition) && !_OpenHandRectTransform.rect.Contains(localMousePosition1))
                {
                    _DrawButtonClicked = false;
                    _isCameraInGamePlayView = false;
                    Invoke("SetCameraFreeRoam", 0.11f);
                    mCardDeck.BackToNormalState();
                }
            }
            else
            {
                if (!_DrawButtonRectTransform.rect.Contains(localMousePosition))
                {
                    _DrawButtonClicked = false;
                    _isCameraInGamePlayView = false;
                    Invoke("SetCameraFreeRoam", 0.11f);
                    mCardDeck.BackToNormalState();
                }
            }
        }


        if (_CameraFreeRoam && !Input.GetMouseButton(0))
        {
            _CameraFreeRoam = false;
        }

        if (_DrawButtonClicked)
        {
            _isCameraInGamePlayView = true;
            OpenCardRegion.SetActive(true);

            _currentView = _views[1];

            _CameraParent.position = Vector3.Lerp(_CameraParent.position, _currentView.position, 0.1f);// Time.deltaTime * _transitionSpeed);

            Vector3 currentAngle = new Vector3(
                Mathf.LerpAngle(_CameraParent.rotation.eulerAngles.x, _currentView.transform.rotation.eulerAngles.x, 0.1f),// Time.deltaTime * _transitionSpeed),
                Mathf.LerpAngle(_CameraParent.rotation.eulerAngles.y, _currentView.transform.rotation.eulerAngles.y, 0.1f),//Time.deltaTime * _transitionSpeed),
                Mathf.LerpAngle(_CameraParent.rotation.eulerAngles.z, _currentView.transform.rotation.eulerAngles.z, 0.1f));//Time.deltaTime * _transitionSpeed));

            _CameraParent.eulerAngles = currentAngle;
        }
        else
        {

            if (!_CameraFreeRoam)
            {
                OpenCardRegion.SetActive(false);
                if (Mathf.Floor(_CameraParent.rotation.eulerAngles.x) != _views[0].rotation.eulerAngles.x)
                {
                    _currentView = _views[0];
                    _CameraParent.position = Vector3.Lerp(_CameraParent.position, _currentView.position, Time.deltaTime * _transitionSpeed);

                    Vector3 currentAngle = new Vector3(
                        Mathf.LerpAngle(_CameraParent.rotation.eulerAngles.x, _currentView.transform.rotation.eulerAngles.x, Time.deltaTime * _transitionSpeed),
                        Mathf.LerpAngle(_CameraParent.rotation.eulerAngles.y, _currentView.transform.rotation.eulerAngles.y, Time.deltaTime * _transitionSpeed),
                        Mathf.LerpAngle(_CameraParent.rotation.eulerAngles.z, _currentView.transform.rotation.eulerAngles.z, Time.deltaTime * _transitionSpeed));

                    _CameraParent.eulerAngles = currentAngle;
                }
            }
            HorizontalPanning();
            //HorizontalPanningWithRotation();
            VerticalZooming();
        }
    }

    /// <summary>
    /// Responsible for Making the camera move right & left along with rotation.
    /// 1. We store first touch position as previous position or initial position when we click mouseButtonDown
    /// 2. With mouseButtonDown being true we keep tracking the mouseposition and store it to newPosition and then we take the initial/previous position
    /// and check the differnce and store it in as direction as it says which direction are we moving
    /// </summary>
    //private void HorizontalPanningWithRotation()
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        mInitialPositionX = Input.mousePosition.x;
    //    }

    //    if (_CameraFreeRoam)
    //    {
    //        mChangedPositionX = Input.mousePosition.x;
    //        float rotateDegrees = 0f;
    //        if (mChangedPositionX < mInitialPositionX + 5f)
    //        {
    //            rotateDegrees -= mHorizontalPanSpeed * Time.deltaTime;
    //        }
    //        if (mChangedPositionX > mInitialPositionX - 5f)
    //        {
    //            rotateDegrees += mHorizontalPanSpeed * Time.deltaTime;
    //        }
    //        Vector3 currentVector = transform.position - mTargetToRotateAround.position;
    //        currentVector.y = 0;
    //        float angleBetween = Vector3.Angle(initialVector, currentVector) * (Vector3.Cross(initialVector, currentVector).y > 0 ? 1 : -1);
    //        float newAngle = Mathf.Clamp(angleBetween + rotateDegrees, -_RotationLimit, _RotationLimit);
    //        rotateDegrees = newAngle - angleBetween;

    //        transform.RotateAround(mTargetToRotateAround.position, Vector3.up, rotateDegrees);

    //        mInitialPositionX = mChangedPositionX;


    //    }

    //}

    public void HorizontalPanning()
    {
        //float panSpeed = 0;

        if (Input.GetMouseButtonDown(0))
        {
            mInitialPositionX = Input.mousePosition.x;
        }

        if (_CameraFreeRoam)
        {
            mChangedPositionX = Input.mousePosition.x;

            if (mChangedPositionX == mInitialPositionX)
            {
                return;
            }
            if (mChangedPositionX < mInitialPositionX - 100f)
            {
                //panSpeed = mZoomSpeed * -1f * Time.deltaTime;
                Pan(mHorizontalPanSpeed * Time.deltaTime);
            }
            if (mChangedPositionX > mInitialPositionX + 100f)
            {
                //panSpeed = mZoomSpeed * Time.deltaTime;
                Pan(mHorizontalPanSpeed * -1f * Time.deltaTime);
            }
        }

        if (!Input.GetMouseButton(0))
        {
            //PlayCameraBoundEffectX();
            PlayCameraBoundEffect(_CameraParent.position.x, _CameraRightBound, _CameraLeftBound, new Vector3(_CameraLeftBound, _CameraParent.position.y, _CameraParent.position.z), new Vector3(_CameraRightBound, _CameraParent.position.y, _CameraParent.position.z));
        }

        //if ((transform.position.x <= _CameraRightBound + 30 && panSpeed > 0) || (transform.position.x >= _CameraLeftBound - 2 && panSpeed < 0))
        //{
        //    transform.Translate(panSpeed * transform.right);
        //}

    }

    private void Pan(float inPanSpeed) //New Addition
    {
        if ((_CameraParent.position.x <= _CameraRightBound + 30 && inPanSpeed > 0) || (_CameraParent.position.x >= _CameraLeftBound - 30 && inPanSpeed < 0))
        {
            _CameraParent.Translate(inPanSpeed * _CameraParent.right);
        }
    }


    /// <summary>
    /// We get the input position in y on click.
    /// And keep updating the input.y position as save it to mInitialPosition and keep taking count of whats the changed Position and see if its greater or lesser
    /// and do action accordingly
    /// </summary>
    private void VerticalZooming()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mInitialPositionY = Input.mousePosition.y;
        }

        if (_CameraFreeRoam)
        {
            mChangedPositionY = Input.mousePosition.y;

            if (mChangedPositionY == mInitialPositionY)
            {
                return;
            }
            if (mChangedPositionY < mInitialPositionY - 100f) //Plus and -100 is to restrict the movement diagonally
            {
                Zoom(mZoomSpeed * Time.deltaTime);
            }
            if (mChangedPositionY > mInitialPositionY + 100f)
            {
                Zoom(mZoomSpeed * -1f * Time.deltaTime);
            }
        }

        if (!Input.GetMouseButton(0))
        {
            //PlayCameraBoundEffectZ();
            PlayCameraBoundEffect(_CameraParent.position.z, mCameraNearBound, mCameraFarBound, new Vector3(_CameraParent.position.x, _CameraParent.position.y, mCameraFarBound), new Vector3(_CameraParent.position.x, _CameraParent.position.y, mCameraNearBound));
            PlayCameraBoundEffect(_CameraParent.position.y, _CameraUpBound, _CameraDownBound, new Vector3(_CameraParent.position.x, _CameraDownBound, _CameraParent.position.z), new Vector3(_CameraParent.position.x, _CameraUpBound, _CameraParent.position.z));
        }
    }
    /// <summary>
    /// Zoom Condition
    /// check for camera near and far bounds, check conditions independently
    /// Give a small buffer value to bring in rubber band effect for the camera
    /// Buffervalue being 2
    /// </summary>
    /// <param name="inZoomSpeed"></param>
    private void Zoom(float inZoomSpeed)
    {                                                                                //New Change
        if ((_CameraParent.position.z <= mCameraNearBound + 30 && inZoomSpeed > 0 && _CameraParent.position.y <= _CameraUpBound + 30) || (_CameraParent.position.z >= mCameraFarBound - 30 && inZoomSpeed < 0 && _CameraParent.position.y >= _CameraDownBound - 30))
        {
            _CameraParent.Translate(inZoomSpeed * _CameraParent.forward, Space.World);
        }
    }

    /// <summary>
    /// Reset the camera position when touch is released, to set it back to its closest bound, either far or near. 
    /// </summary>
    //public void PlayCameraBoundEffectZ()
    //{
    //    Vector3 newCameraParentPos = Vector3.zero;


    //    if (_CameraParent.position.z > mCameraNearBound || _CameraParent.position.z < mCameraFarBound)
    //    {

    //        if (Mathf.Abs(mCameraFarBound - _CameraParent.position.z) < Mathf.Abs(mCameraNearBound - _CameraParent.position.z))
    //        {
    //            newCameraParentPos = new Vector3(_CameraParent.position.x, _CameraParent.position.y, mCameraFarBound);
    //        }
    //        else
    //        {
    //            newCameraParentPos = new Vector3(_CameraParent.position.x, _CameraParent.position.y, mCameraNearBound);
    //        }

    //        _CameraParent.position = Vector3.Lerp(_CameraParent.position, newCameraParentPos, 0.1f);
    //    }
    //}

    //public void PlayCameraBoundEffectX()
    //{
    //    Vector3 newCameraParentPos = Vector3.zero;


    //    if (_CameraParent.position.x > _CameraRightBound || _CameraParent.position.x < _CameraLeftBound)
    //    {

    //        if (Mathf.Abs(_CameraLeftBound - _CameraParent.position.x) < Mathf.Abs(_CameraRightBound - _CameraParent.position.x))
    //        {
    //            newCameraParentPos = new Vector3(_CameraLeftBound, _CameraParent.position.y, _CameraParent.position.z);
    //        }
    //        else
    //        {
    //            newCameraParentPos = new Vector3(_CameraRightBound, _CameraParent.position.y, _CameraParent.position.z);
    //        }

    //        _CameraParent.position = Vector3.Lerp(_CameraParent.position, newCameraParentPos, 0.1f);
    //    }
    //}

    //+              //-
    public void PlayCameraBoundEffect(float inCameraDirection, float inBound1, float inBound2, Vector3 inCameraBound1, Vector3 inCameraBound2) //Function Modification
    {
        Vector3 newCameraParentPos = Vector3.zero;
        if (inCameraDirection > inBound1 || inCameraDirection < inBound2)
        {

            if (Mathf.Abs(inBound2 - inCameraDirection) < Mathf.Abs(inBound1 - inCameraDirection))
            {
                newCameraParentPos = inCameraBound1;
            }
            else
            {
                newCameraParentPos = inCameraBound2;
            }

            _CameraParent.position = Vector3.Lerp(_CameraParent.position, newCameraParentPos, 0.1f);
        }
    }
}

//void Panning()
//    {

//[SerializeField] private float mCameraRightBound;
//[SerializeField] private float mCameraLeftBound;

//        //    if (mChangedPositionX == mInitialPositionX)
//        //    {
//        //        return;
//        //    }
//        //    if (mChangedPositionX < mInitialPositionX - 100f) //Plus and -100 is to restrict the movement diagonally
//        //    {
//        //        Pan(mHorizontalPanSpeed  * Time.deltaTime);
//        //    }
//        //    if (mChangedPositionX > mInitialPositionX + 100f)
//        //    {
//        //        Pan(mHorizontalPanSpeed  * -1f * Time.deltaTime);
//        //    }

//        //}
//        //if (!Input.GetMouseButton(0))
//        //{
//        //    PlayCameraBoundEffect();
//        //}

//        private void Pan(float inPanSpeed)
//        {
//            if ((_CameraParent.position.x <= mCameraRightBound + 30 && inPanSpeed > 0) || (_CameraParent.position.x >= mCameraLeftBound - 30 && inPanSpeed < 0))
//            {
//                _CameraParent.Translate(inPanSpeed * _CameraParent.right);
//            }
//        }
//    }