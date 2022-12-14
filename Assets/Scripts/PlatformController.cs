using System.Linq;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    [SerializeField] private GameObject _bridgePrefab;
    [SerializeField] private GameObject _landPrefab;

    public bool SkipPlatform;
    public bool IsCurrentPlatform;
    public bool BuildInProgress;
    
    private float _currentTime;
    private float _rotationSpeed;
    private Vector3 _defaultBridgeScale;
    private Vector3 _buildPoint;
    private bool _rotationInProgress;
    
    // Start is called before the first frame update
    void Start()
    {
        _bridgePrefab.gameObject.SetActive(false);
        _defaultBridgeScale = _bridgePrefab.transform.localScale;
        _currentTime = GameManager.SharedInstance.BridgeRotationTime;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!GameManager.SharedInstance.IsGameStarted) return;

        CheckOutOfBoundary();
        BuildLogic();
    }

    private void BuildLogic()
    {
        if (SkipPlatform || !IsCurrentPlatform || !PlayerController.SharedInstance.ReadyToBuildBridge) return;

        if(BridgeBuildStarted() && !BuildInProgress && !_rotationInProgress)
        {
            SetStartBuildSettings();
        }

        if (BridgeBuildInProgress() && BuildInProgress && _bridgePrefab.transform.localScale.y < GameManager.SharedInstance.BridgeMaxLength)
        {
            SetNewBridgeScale();
        }

        if (BridgeBuildComplete() && BuildInProgress)
        {
            StartBridgeRotation();
        }

        if (_currentTime < GameManager.SharedInstance.BridgeRotationTime)
        {
            RotateBridge();
        }

        if (_currentTime > GameManager.SharedInstance.BridgeRotationTime)
        {
            ResetFlagsWhenBuildComplete();
        }
    }

    private void SetStartBuildSettings()
    {
        PlayerController.SharedInstance.BuildStarted();
        BuildInProgress = true;
        _bridgePrefab.gameObject.SetActive(true);
        _buildPoint = _landPrefab.transform.position;
        _buildPoint.x += _landPrefab.transform.lossyScale.x / 2.0f;
        _buildPoint.y += _landPrefab.transform.lossyScale.y / 2.0f;
        var bridgePos = _buildPoint;
        bridgePos.x += _bridgePrefab.transform.localScale.x / 2.0f;
        bridgePos.y += _bridgePrefab.transform.localScale.y / 2.0f;
        _bridgePrefab.transform.position = bridgePos;
    }

    private void SetNewBridgeScale()
    {
        var scale = new Vector3(0, Time.deltaTime * GameManager.SharedInstance.BuildSpeed);
        _bridgePrefab.transform.localScale += scale;
        _bridgePrefab.transform.position += scale / 2.0f;
    }

    private void StartBridgeRotation()
    {
        PlayerController.SharedInstance.BuildEnded();
        _currentTime = 0;
        BuildInProgress = false;
        _rotationInProgress = true;
    }

    private void RotateBridge()
    {
        _rotationSpeed = (90 * Time.deltaTime) / GameManager.SharedInstance.BridgeRotationTime;
        _currentTime += Time.deltaTime;
        _bridgePrefab.transform.RotateAround(_buildPoint, -Vector3.forward, _rotationSpeed);
    }

    private void ResetFlagsWhenBuildComplete()
    {
        _currentTime = GameManager.SharedInstance.BridgeRotationTime;
        PlayerController.SharedInstance.ReadyToBuildBridge = false;
        IsCurrentPlatform = false;
        BuildInProgress = false;
        _rotationInProgress = false;
        PlayerController.SharedInstance.PlayMoveAnimation(true);
        PlayerController.SharedInstance.BuildCompleteAnimation();
    }

    private void CheckOutOfBoundary()
    {
        if (transform.position.x < GameManager.SharedInstance.LeftBoundary)
        {
            ResetPlatformBeforeReturn();
        }
    }

    public void ResetPlatformBeforeReturn()
    {
        _bridgePrefab.transform.rotation = Quaternion.identity;
        _bridgePrefab.transform.localScale = _defaultBridgeScale;
        _bridgePrefab.SetActive(false);
        ObjectPool.SharedInstance.PlatformObjectPool.Release(gameObject);
        IsCurrentPlatform = false;
        SkipPlatform = false;
    }

    public Vector3 GetPlatformScale()
    {
        return _landPrefab.transform.lossyScale;
    }

    private bool BridgeBuildStarted()
    {
        var touchStarted = Input.touchCount > 0 && Input.touches.FirstOrDefault().phase == TouchPhase.Began;
        var leftMouseDown = Input.GetKeyDown(KeyCode.Mouse0);

        return touchStarted || leftMouseDown;
    }

    private bool BridgeBuildInProgress()
    {
        var touchStationary = Input.touchCount > 0 && Input.touches.FirstOrDefault().phase == TouchPhase.Stationary;
        var leftMouseOnHold = Input.GetKey(KeyCode.Mouse0);

        return touchStationary || leftMouseOnHold;
    }

    private bool BridgeBuildComplete()
    {
        var touchEnded = Input.touchCount > 0 && Input.touches.FirstOrDefault().phase == TouchPhase.Ended;
        var leftMouseRelease = Input.GetKeyUp(KeyCode.Mouse0);

        return touchEnded || leftMouseRelease;
    }
}
