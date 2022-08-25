//using UnityEngine;

//public class GroundController : MonoBehaviour
//{
//    [SerializeField] private GameObject _bridgePrefab;
//    [SerializeField] private GameObject _landPrefab;

//    public bool SkipPlatform;
//    public bool IsCurrentPlatform;
//    public bool BuildInProgress;
    
//    private float _currentTime;
//    private float _rotationSpeed;
//    private Vector3 _defaultBridgeScale;
//    private Vector3 _buildPoint;
//    private bool _rotationInProgress;
//    private float _leftBoundary = -15;
    
//    // Start is called before the first frame update
//    void Start()
//    {
//        _bridgePrefab.gameObject.SetActive(false);
//        _defaultBridgeScale = _bridgePrefab.transform.localScale;
//        _currentTime = GameManager.SharedInstance.BridgeRotationTime;
//    }

//    // Update is called once per frame
//    void LateUpdate()
//    {
//        if (!GameManager.SharedInstance.IsGameStarted) return;

//        MoveLeft();
//        if (SkipPlatform || !IsCurrentPlatform || !PlayerController.SharedInstance.ReadyToBuildBridge) return;

//        if (Input.GetKeyDown(KeyCode.Mouse0) && !BuildInProgress && !_rotationInProgress)
//        {
//            SetStartBuildSettings();
//        }

//        if (Input.GetKey(KeyCode.Mouse0) && BuildInProgress && _bridgePrefab.transform.localScale.y < GameManager.SharedInstance.BridgeMaxLength)
//        {
//            SetNewBridgeScale();
//        }

//        if (Input.GetKeyUp(KeyCode.Mouse0) && BuildInProgress)
//        {
//            StartBridgeRotation();
//        }

//        if (_currentTime < GameManager.SharedInstance.BridgeRotationTime)
//        {
//            RotateBridge();
//        }

//        if (_currentTime > GameManager.SharedInstance.BridgeRotationTime)
//        {
//            ResetFlagsWhenBuildComplete();
//        }
//    }

//    private void SetStartBuildSettings()
//    {
//        BuildInProgress = true;
//        _bridgePrefab.gameObject.SetActive(true);
//        _buildPoint = _landPrefab.transform.position;
//        _buildPoint.x += _landPrefab.transform.lossyScale.x / 2.0f;
//        _buildPoint.y += _landPrefab.transform.lossyScale.y / 2.0f;
//        var bridgePos = _buildPoint;
//        bridgePos.x += _bridgePrefab.transform.localScale.x / 2.0f;
//        bridgePos.y += _bridgePrefab.transform.localScale.y / 2.0f;
//        _bridgePrefab.transform.position = bridgePos;
//    }

//    private void SetNewBridgeScale()
//    {
//        var scale = new Vector3(0, Time.deltaTime * GameManager.SharedInstance.BuildSpeed);
//        _bridgePrefab.transform.localScale += scale;
//        _bridgePrefab.transform.position += scale / 2.0f;
//    }

//    private void StartBridgeRotation()
//    {
//        _currentTime = 0;
//        BuildInProgress = false;
//        _rotationInProgress = true;
//    }

//    private void RotateBridge()
//    {
//        _rotationSpeed = (90 * Time.deltaTime) / GameManager.SharedInstance.BridgeRotationTime;
//        _currentTime += Time.deltaTime;
//        _bridgePrefab.transform.RotateAround(_buildPoint, -Vector3.forward, _rotationSpeed);
//    }

//    private void ResetFlagsWhenBuildComplete()
//    {
//        _currentTime = GameManager.SharedInstance.BridgeRotationTime;
//        PlayerController.SharedInstance.ReadyToBuildBridge = false;
//        IsCurrentPlatform = false;
//        BuildInProgress = false;
//        _rotationInProgress = false;
//    }

//    private void MoveLeft()
//    {
//        if (PlayerController.SharedInstance.ReadyToBuildBridge) return;

//        transform.Translate(Vector3.left * GameManager.SharedInstance.MoveSpeed * Time.deltaTime);

//        if (transform.position.x < _leftBoundary)
//        {
//            ResetPlatformBeforeReturn();
//        }
//    }

//    private void ResetPlatformBeforeReturn()
//    {
//        _bridgePrefab.transform.rotation = Quaternion.identity;
//        _bridgePrefab.transform.localScale = _defaultBridgeScale;
//        _bridgePrefab.SetActive(false);
//        //GetComponent<Collider2D>().enabled = true;
//        ObjectPool.SharedInstance.PlatformObjectPool.Release(gameObject);
//        IsCurrentPlatform = false;
//        SkipPlatform = false;
//    }

//    public Vector3 GetPlatformScale()
//    {
//        return _landPrefab.transform.lossyScale;
//    }
//}
