using AppodealAds.Unity.Api;
using Lean.Localization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public Text totalCoals;
    public Text highScore;
    public GameObject languagesPanel;
    public GameObject loadingPanel;
    public GameObject gdprPanel;
    private bool consentValue;
    private string appID = "e1101fa0666628cf66acbeb240627a6f069c7605988b6090";

    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("ResultGDPR"))
        {
            gdprPanel.SetActive(true);
        }
        else
        {
            CheckConsent();
        }
        if (PlayerPrefsSafe.HasKey("TotalCoals"))
        {
            totalCoals.text = PlayerPrefsSafe.GetFloat("TotalCoals").ToString("0");
        }
        if (PlayerPrefsSafe.HasKey("HighestScore"))
        {
            highScore.text = "HI " + PlayerPrefsSafe.GetFloat("HighestScore").ToString("0");
        }
    }

    public void CheckConsent()
    {
        if (PlayerPrefs.GetInt("ResultGDPR") != 0)
        {
            consentValue = true;
        }
        else
        {
            consentValue = false;
        }
    }

    public void OnExitClick()
    {
        Application.Quit();
    }

    public void OnStartClick()
    {
        Appodeal.setTesting(false);
        Appodeal.disableWriteExternalStoragePermissionCheck();
        Appodeal.muteVideosIfCallsMuted(true);
        Appodeal.setLogLevel(Appodeal.LogLevel.Verbose);
        Appodeal.initialize(appID, Appodeal.INTERSTITIAL | Appodeal.REWARDED_VIDEO, consentValue);
        loadingPanel.SetActive(true);
        SceneManager.LoadSceneAsync("Main");
    }

    public void OnStoreClick()
    {
        loadingPanel.SetActive(true);
        SceneManager.LoadSceneAsync("Store");
    }

    public void SetLanguage(string language)
    {
        FindObjectOfType<LeanLocalization>().SetCurrentLanguage(language);
        LeanLocalization.CurrentLanguage = language;
        languagesPanel.SetActive(false);
    }

    public void OnGlobeClick()
    {
        languagesPanel.SetActive(true);
    }

    public void OnYesClick()
    {
        PlayerPrefs.SetInt("ResultGDPR", 1);
        consentValue = true;
        gdprPanel.SetActive(false);
    }

    public void OnNoClick()
    {
        PlayerPrefs.SetInt("ResultGDPR", 0);
        consentValue = false;
        gdprPanel.SetActive(false);
    }

    public void OnLearnMoreClick()
    {
        Application.OpenURL("https://www.appodeal.com/privacy-policy");
    }

    public void OnWithdrawClick()
    {
        gdprPanel.SetActive(true);
    }

}
