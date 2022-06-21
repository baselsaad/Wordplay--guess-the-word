using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;
#if PLATFORM_ANDROID
using UnityEngine.Android;
#endif

public class AdsManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    //SerializedField:
    [SerializeField] private string m_gameId;
    [SerializeField] private string m_placementId = "Banner_Android";
    [SerializeField] private bool m_testMode = true;
    [SerializeField] private bool m_runAds = false;
    [SerializeField] private BannerPosition m_position = BannerPosition.BOTTOM_CENTER;

    private void Awake()
    {
        if (m_runAds)
        {
            Initialize();
        }
    }

    private void Start()
    {
        if (m_runAds)
        {
           StartCoroutine(ShowBannerWhenReady());
        }
    }

    public void Initialize()
    {
        if (Advertisement.isSupported)
        {
            Debug.Log(Application.platform + " supported by Advertisement");
        }

        Advertisement.Initialize(m_gameId, m_testMode, this);
    }


    public IEnumerator ShowBannerWhenReady()
    {
        while (!Advertisement.isInitialized)
        {
            yield return new WaitForSeconds(0.5f);
        }

        Debug.Log("Banner showed");
        Advertisement.Banner.SetPosition(m_position);
        Advertisement.Banner.Show(m_placementId);
    }

    #region Interface Implementations
    public void OnInitializationComplete()
    {
        Debug.Log("Init Success");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Init Failed: [{error}]: {message}");
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        Debug.Log($"Load Success: {placementId}");
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Load Failed: [{error}:{placementId}] {message}");
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.Log($"OnUnityAdsShowFailure: [{error}]: {message}");
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        Debug.Log($"OnUnityAdsShowStart: {placementId}");
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        Debug.Log($"OnUnityAdsShowClick: {placementId}");
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        Debug.Log($"OnUnityAdsShowComplete: [{showCompletionState}]: {placementId}");
    }
    #endregion
}
