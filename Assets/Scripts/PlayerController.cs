using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Animator _playerAnimator;
    public static bool ReadyToBuildBridge;
    
    public static PlayerController SharedInstance;

    private float _timeScale;
    private int _scorePoints = -1;
    private bool _isGameStarted;

    void Awake()
    {
        SharedInstance = this;
        _timeScale = Time.timeScale;
        //Time.timeScale = 0;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("BuildPoint"))
        {
            GameManager.SharedInstance.AddScorePoints(1);
            var groundController = other.gameObject.GetComponentInParent<PlatformController>();
            if (!groundController.SkipPlatform)
            {
                InterfaceManager.SharedInstance.ShowOrHideHelpImage(true);
                groundController.IsCurrentPlatform = true;
                ReadyToBuildBridge = true;
                PlayMoveAnimation(0);
                _playerAnimator.SetTrigger("ReadyToBuild");
            }
        }
        else if(other.CompareTag("Ground"))
        {
            GameManager.SharedInstance.StopGame();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("BuildPoint"))
        {
            InterfaceManager.SharedInstance.ShowOrHideHelpImage(false);
            other.gameObject.GetComponentInParent<PlatformController>().IsCurrentPlatform = false;
            ReadyToBuildBridge = false;
        }
    }

    public void PlayMoveAnimation(float speed)
    {
        _playerAnimator.SetFloat("Speed_f", speed);
    }

    public void BuildCompleteAnimation()
    {
        _playerAnimator.SetTrigger("BuildComplete");
    }
}
