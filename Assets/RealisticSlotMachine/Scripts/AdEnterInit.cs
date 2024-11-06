using UnityEngine;
using UnityEngine.Advertisements;
using Unity.Advertisement.IosSupport;
using UnityEngine.iOS;

public class AdEnterInit : MonoBehaviour, IUnityAdsInitializationListener
{
    [SerializeField] private bool _testMode = false;
    private GiftForAdBehavior _giftForAdBehavior;

    private readonly string _androidGameID = "5726415";
    private readonly string _iphoneGameID = "5726414";

    private string _gameID = null;

    public void Start()
    {
        _giftForAdBehavior = GetComponent<GiftForAdBehavior>();
        CheckTrackingPermissionAndInitializeAds();
    }

    private void CheckTrackingPermissionAndInitializeAds()
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            var trackingStatus = ATTrackingStatusBinding.GetAuthorizationTrackingStatus();

            if (trackingStatus == ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED)
            {
                ATTrackingStatusBinding.RequestAuthorizationTracking();
                Invoke("CheckAndInitializeAdsAfterRequest", 1.0f);
            }
            else
            {
                InitializeAds(trackingStatus == ATTrackingStatusBinding.AuthorizationTrackingStatus.AUTHORIZED);
            }
        }
        else
        {
            InitializeAds(true);
        }
    }

    private void CheckAndInitializeAdsAfterRequest()
    {
        var trackingStatus = ATTrackingStatusBinding.GetAuthorizationTrackingStatus();

        if (trackingStatus == ATTrackingStatusBinding.AuthorizationTrackingStatus.AUTHORIZED)
        {
            InitializeAds(true);
            GetIDForAds();
        }
        else
        {
            InitializeAds(false);
        }
    }

    private void GetIDForAds()
    {
        ATTrackingStatusBinding.AuthorizationTrackingStatus status = ATTrackingStatusBinding.GetAuthorizationTrackingStatus();
        if (status == ATTrackingStatusBinding.AuthorizationTrackingStatus.AUTHORIZED)
        {
            string idResult = Device.advertisingIdentifier;
            PlayerPrefs.SetString("idAdverts", idResult);
        }
    }

    public void InitializeAds(bool personalized)
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            _gameID = _iphoneGameID;
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            _gameID = _androidGameID;
        }
        else
        {
            _gameID = _androidGameID;
        }

        Advertisement.Initialize(_gameID, _testMode, this);

        //MetaData dataInfo = new MetaData("gdpr");
        //dataInfo.Set("consent", personalized ? "true" : "false");
        //Advertisement.SetMetaData(dataInfo);
    }

    public void OnInitializationComplete()
    {
        _giftForAdBehavior.LoadAd();
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {

    }
}