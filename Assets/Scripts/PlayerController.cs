using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Animator _playerAnimator;
    [SerializeField] private ParticleSystem _runSystem;
    [SerializeField] private ParticleSystem _smokeSystem;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _buildComplete;
    [SerializeField] private AudioClip _buildInProgress;

    public bool ReadyToBuildBridge;
    
    public static PlayerController SharedInstance;

    void Awake()
    {
        SharedInstance = this;
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
                PlayMoveAnimation(false);
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

    public void PlayMoveAnimation(bool play)
    {
        if (play)
        {
            _playerAnimator.SetFloat("Speed_f", 1);
            _runSystem.Play();
        }
        else
        {
            _playerAnimator.SetFloat("Speed_f", 0);
            _runSystem.Stop();
        }
        
    }

    public void BuildCompleteAnimation()
    {
        _playerAnimator.SetTrigger("BuildComplete");

    }

    public void BuildStarted()
    {
        _audioSource.clip = _buildInProgress;
        _audioSource.Play();
    }

    public void BuildEnded()
    {
        _audioSource.Stop();
        _audioSource.clip = _buildComplete;
        _audioSource.Play();
        _smokeSystem.Play();
    }
}
