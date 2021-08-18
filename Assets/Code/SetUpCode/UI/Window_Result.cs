using UnityEngine;
using UnityEngine.Rendering.Universal;
using VIRA.CameraSystem;
using VIRA.WindowsManager;

public class Window_Result : WindowBase
{
    [Header("SetUp"), Tooltip("In case result fills screen completely you can disable gameplay camera for optimization reasons")]
    [SerializeField] private bool hideGameplay = default;

    [Header("ResultHolders")]
    [SerializeField] private GameObject _winHolder;
    [SerializeField] private UnityEngine.UI.Button _winButton;
    [SerializeField] private GameObject _loseHolder;
    [SerializeField] private UnityEngine.UI.Button _loseButton;
    [SerializeField] private TMPro.TextMeshProUGUI _scoreTextHolder;
    [SerializeField] private string _scoreText = "score";
    [SerializeField] private ParticleSystem[] winParticles = default;
    [SerializeField] private ParticleSystem[] loseParticles = default;

    protected override void Start()
    {
        // Make two base cameras instead of stack
        if (hideGameplay)
        {
            CameraManager.Instance.GameplayCameraData.cameraStack.Clear();
            CameraManager.Instance.ResultCameraData.renderType = CameraRenderType.Base;
        }
        base.Start();
    }

    public override void Hide()
    {
        CameraManager.Instance.ResultCamera.enabled = false;
        if (hideGameplay)
        {
            CameraManager.Instance.GameplayCamera.enabled = true;
        }
        base.Hide();
        if (GameManager.Instance.Won)
        {
            _winButton.onClick.RemoveListener(GoToGamePlay);
        }
        else
        {
            _loseButton.onClick.RemoveListener(GoToGamePlay);
        }
    }

    public override void Show()
    {
        CameraManager.Instance.ResultCamera.enabled = true;
        if (hideGameplay)
        {
            CameraManager.Instance.GameplayCamera.enabled = false;
        }
        base.Show();
        bool won = GameManager.Instance.Won;
        _winHolder.SetActive(won);
        _loseHolder.SetActive(!won);
        if (won)
        {
            _winButton.onClick.AddListener(GoToGamePlay);
            for (int i = 0; i < winParticles.Length; i++)
            {
                winParticles[i].Play(true);
            }
        }
        else
        {
            _loseButton.onClick.AddListener(GoToGamePlay);
            for (int i = 0; i < loseParticles.Length; i++)
            {
                loseParticles[i].Play(true);
            }
        }
        _scoreTextHolder.text = _scoreText + " " + GameManager.Instance.Place.ToString();

    }

    private void GoToGamePlay()
    {
        GameManager.Instance.GoToGamePlay();
        
    }

}

