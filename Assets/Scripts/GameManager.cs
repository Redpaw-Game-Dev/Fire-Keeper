using Lean.Localization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using AppodealAds.Unity.Api;
using AppodealAds.Unity.Common;

public class GameManager : MonoBehaviour, IRewardedVideoAdListener
{
    // Settings
    public float speedBoost = 0.001f;
    public float speed = 0.05f;
    public float maxSpeed = 0.07f;
    public float sensitivity = 0.01f;
    public float restartTime = 1f;
    public float animSpeed = 1f;
    public float coalValue = 20f;
    public float firePower = 0.01f;
    private float maxMusicVolume;
    private AudioSource musicSource;
    private AudioSource fireSoundsSource;

    // Values
    private SafeFloat score = 0;
    private SafeFloat coals = 0;
    private SafeFloat brightness = 70;
    private SafeFloat highScore = 0;
    private bool isLose = false;
    private bool isAdShown = false;

    // UI

    public GameObject gameOverPanel;
    public GameObject loadingErrorPanel;
    public Text scoreUI;
    public Text coalsUI;
    public Text endScoreUI;
    public Text endCoalsUI;
    public Text highScoreUI;
    public Slider brightnessBar;
    public Text pausePlayText;
    public Button musicButton;
    public Button soundButton;
    public Button mainMenuButtonPausePanel;

    //Player

    private ParticleSystem playerGlow;
    private ParticleSystem fireAlpha;
    private ParticleSystem fireAdditive;
    private ParticleSystem fireGlow;
    private ParticleSystem fireSparks;
    private Light playerLight;

    void Start()
    {
        Appodeal.setRewardedVideoCallbacks(this);
        if (PlayerPrefsSafe.HasKey("HighestScore"))
        {
            highScore = PlayerPrefsSafe.GetFloat("HighestScore");
        }
        highScoreUI.text = "HI " + ((float)highScore).ToString("0");
    }

    private void FixedUpdate()
    {
        if (brightness <= 0)
        {
            FindObjectOfType<PlayerMovement>().enabled = false;
            EndGame();
        }
        if (!isLose)
        {

            brightness -= firePower;
            brightnessBar.value = brightness;
            CheckBrightness();
            score += (SafeFloat)speed;
            scoreUI.text = ((float)score).ToString("0");
        }
        else if(isLose && !isAdShown)
        {
            if (score >= 150)
            {
                ShowInterstitial();
            }
        }
    }

    public void SpeedUp()
    {
        if(speed < maxSpeed)
        {
            speed += speedBoost;
            FindObjectOfType<PlayerMovement>().SpeedUp(speed);
        }
    }

    public void AddBrightness()
    {
        ++coals;
        coalsUI.text = ((float)coals).ToString("0");
        if (brightness + coalValue >= 100f)
        {
            brightness = 100f;
        }
        else
        {
            brightness += coalValue;
        }
        CheckBrightness();
    }

    private void CheckBrightness()
    {
        if (brightness <= 10f)
        {
            if (brightness <= 1f)
            {
                SetPlayerBrightness(0, 5, 2);
                musicSource.volume = maxMusicVolume * 0.1f;
                fireSoundsSource.volume = maxMusicVolume * 0.1f;
            }
            else if (brightness <= 2f)
            {
                SetPlayerBrightness(1, 5, 3);
                musicSource.volume = maxMusicVolume * 0.2f;
                fireSoundsSource.volume = maxMusicVolume * 0.2f;
            }
            else if (brightness <= 4f)
            {
                SetPlayerBrightness(1, 5, 4);
                musicSource.volume = maxMusicVolume * 0.4f;
                fireSoundsSource.volume = maxMusicVolume * 0.4f;
            }
            else if (brightness <= 6f)
            {
                SetPlayerBrightness(2, 10, 5);
                musicSource.volume = maxMusicVolume * 0.8f;
                fireSoundsSource.volume = maxMusicVolume * 0.8f;
            }
            else if (brightness <= 8f)
            {
                SetPlayerBrightness(2, 10, 6);
                musicSource.volume = maxMusicVolume * 0.9f;
                fireSoundsSource.volume = maxMusicVolume * 0.9f;
            }
            else
            {
                SetPlayerBrightness(2, 10, 7);
                musicSource.volume = maxMusicVolume;
                fireSoundsSource.volume = maxMusicVolume;
            }
        }
        else if (brightness <= 20f)
        {
            SetPlayerBrightness(7, 20, 8);
            musicSource.volume = maxMusicVolume;
            fireSoundsSource.volume = maxMusicVolume;
        }
        else if (brightness <= 30f)
        {
            musicSource.volume = maxMusicVolume;
            fireSoundsSource.volume = maxMusicVolume;
            SetPlayerBrightness(15, 30, 9);
        }
        else if (brightness <= 40f)
        {
            SetPlayerBrightness(20, 45, 10);
        }
        else if (brightness <= 50f)
        {
            SetPlayerBrightness(30, 60, 11);
        }
        else if (brightness <= 60f)
        {
            SetPlayerBrightness(40, 75, 12);
        }
        else if (brightness <= 70f)
        {
            SetPlayerBrightness(50, 90, 13);
        }
        else if (brightness <= 80f)
        {
            SetPlayerBrightness(60, 105, 14);
        }
        else if (brightness <= 90f)
        {
            SetPlayerBrightness(70, 125, 15);
        }
        else if (brightness <= 100f)
        {
            SetPlayerBrightness(75, 150, 16);
        }

    }

    public void SetPlayerBrightness(float fire = 50, float sparks = 75, float fog = 12)
    {
        playerGlow.emissionRate = fire;// min = 2 max = 75 (2, 7, 15, 20, 30, 40, 50, 60, 70, 75)
        fireAlpha.emissionRate = fire;// min = 2 max = 75
        fireAdditive.emissionRate = fire;// min = 2 max = 75
        fireGlow.emissionRate = fire;// min = 2 max = 75
        fireSparks.emissionRate = sparks;// min = 10 max = 150 (10, 20, 30, 45, 60, 75, 90, 105, 125, 150)
        playerLight.range = fog;// min = 7 max = 16
        RenderSettings.fogEndDistance = fog;// min = 7 max = 16
    }

    public void SetGameSettings()
    {
        if (!PlayerPrefs.HasKey("Music"))
        {
            PlayerPrefs.SetInt("Music", 1);
        }
        if (PlayerPrefs.GetInt("Music") == 0)
        {
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioSource>().Stop();
            if (LeanLocalization.CurrentLanguage == "English")
            {
                musicButton.GetComponentInChildren<Text>().text = "MUSIC ON";
            }
            else if (LeanLocalization.CurrentLanguage == "Russian")
            {
                musicButton.GetComponentInChildren<Text>().text = "МУЗЫКА ВКЛ";
            }
        }
        if (!PlayerPrefs.HasKey("Sound"))
        {
            PlayerPrefs.SetInt("Sound", 1);
        }
        if (PlayerPrefs.GetInt("Sound") == 0)
        {
            foreach (var audio in GameObject.FindGameObjectWithTag("Player").GetComponents<AudioSource>())
            {
                audio.enabled = false;
            }
            if (LeanLocalization.CurrentLanguage == "English")
            {
                soundButton.GetComponentInChildren<Text>().text = "SOUND ON";
            }
            else if (LeanLocalization.CurrentLanguage == "Russian")
            {
                soundButton.GetComponentInChildren<Text>().text = "ЗВУКИ ВКЛ";
            }
        }
        playerLight = GameObject.Find("Player Light").GetComponent<Light>();
        playerGlow = GameObject.Find("PS_PlayerGlow").GetComponent<ParticleSystem>();
        fireAlpha = GameObject.Find("PS_Fire_ALPHA").GetComponent<ParticleSystem>();
        fireAdditive = GameObject.Find("PS_Fire_ADD").GetComponent<ParticleSystem>();
        fireGlow = GameObject.Find("PS_Glow").GetComponent<ParticleSystem>();
        fireSparks = GameObject.Find("PS_Sparks").GetComponent<ParticleSystem>();
        SetPlayerBrightness();
        fireSoundsSource = GameObject.FindGameObjectWithTag("Player").GetComponents<AudioSource>()[0];
        musicSource = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioSource>();
        maxMusicVolume = musicSource.volume;
    }

    public float GetScore()
    {
        return (float)score;
    }

    public void OnMainMenuClick()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    public void OnOkClick()
    {
        loadingErrorPanel.SetActive(false);
    }

    public void OnMusicClick()
    {
        if (PlayerPrefs.GetInt("Music") == 1)
        {
            PlayerPrefs.SetInt("Music", 0);
            if(LeanLocalization.CurrentLanguage == "English")
            {
                musicButton.GetComponentInChildren<Text>().text = "MUSIC ON";
            }
            else if (LeanLocalization.CurrentLanguage == "Russian")
            {
                musicButton.GetComponentInChildren<Text>().text = "МУЗЫКА ВКЛ";
            }

            musicSource.Stop();
        }
        else
        {
            PlayerPrefs.SetInt("Music", 1);
            if (LeanLocalization.CurrentLanguage == "English")
            {
                musicButton.GetComponentInChildren<Text>().text = "MUSIC OFF";
            }
            else if (LeanLocalization.CurrentLanguage == "Russian")
            {
                musicButton.GetComponentInChildren<Text>().text = "МУЗЫКА ВЫКЛ";
            }
            musicSource.Play();
        }
    }

    public void OnSoundClick()
    {
        if (PlayerPrefs.GetInt("Sound") == 1)
        {
            PlayerPrefs.SetInt("Sound", 0);
            if (LeanLocalization.CurrentLanguage == "English")
            {
                soundButton.GetComponentInChildren<Text>().text = "SOUND ON";
            }
            else if (LeanLocalization.CurrentLanguage == "Russian")
            {
                soundButton.GetComponentInChildren<Text>().text = "ЗВУКИ ВКЛ";
            }

            foreach (var audio in GameObject.FindGameObjectWithTag("Player").GetComponents<AudioSource>())
            {
                audio.enabled = false;
            }
        }
        else
        {
            PlayerPrefs.SetInt("Sound", 1);
            if (LeanLocalization.CurrentLanguage == "English")
            {
                soundButton.GetComponentInChildren<Text>().text = "SOUND OFF";
            }
            else if (LeanLocalization.CurrentLanguage == "Russian")
            {
                soundButton.GetComponentInChildren<Text>().text = "ЗВУКИ ВЫКЛ";
            }
            foreach (var audio in GameObject.FindGameObjectWithTag("Player").GetComponents<AudioSource>())
            {
                audio.enabled = true;
            }
        }
    }

    public void OnPausePlayClick()
    {
        if (Time.timeScale == 1)
        {
            mainMenuButtonPausePanel.gameObject.SetActive(true);
            soundButton.gameObject.SetActive(true);
            musicButton.gameObject.SetActive(true);
            pausePlayText.text = "GO";
            CheckPausePanelText();
            Time.timeScale = 0;
        }
        else
        {
            mainMenuButtonPausePanel.gameObject.SetActive(false);
            soundButton.gameObject.SetActive(false);
            musicButton.gameObject.SetActive(false);
            pausePlayText.text = "II";
            Time.timeScale = 1;
        }
    }

    public void EndGame()
    {
        SetPlayerBrightness(0, 0, 0);
        fireSoundsSource.Stop();
        musicSource.Stop();
        isLose = true;
        PlayerPrefsSafe.SetFloat("TotalCoals", PlayerPrefsSafe.GetFloat("TotalCoals") + coals);
        Invoke("GameResults", restartTime);
    }

    private void GameResults()
    {
        if (highScore < score)
        {
            PlayerPrefsSafe.SetFloat("HighestScore", score);
        }
        endScoreUI.text = ((float)score).ToString("0");
        endCoalsUI.text = ((float)coals).ToString("0");
        gameOverPanel.SetActive(true);
    }

    public void OnTryAgainClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnCoalsX2Click()
    {
        if (Appodeal.isLoaded(Appodeal.REWARDED_VIDEO))
        {
            Appodeal.show(Appodeal.REWARDED_VIDEO);
        }
        else
        {
            loadingErrorPanel.SetActive(true);
        }
    }

    public void CheckPausePanelText()
    {
        if (PlayerPrefs.GetInt("Music") == 1)
        {
            if (LeanLocalization.CurrentLanguage == "English")
            {
                musicButton.GetComponentInChildren<Text>().text = "MUSIC OFF";
            }
            else if (LeanLocalization.CurrentLanguage == "Russian")
            {
                musicButton.GetComponentInChildren<Text>().text = "МУЗЫКА ВЫКЛ";
            }
        }
        else
        {
            if (LeanLocalization.CurrentLanguage == "English")
            {
                musicButton.GetComponentInChildren<Text>().text = "MUSIC ON";
            }
            else if (LeanLocalization.CurrentLanguage == "Russian")
            {
                musicButton.GetComponentInChildren<Text>().text = "МУЗЫКА ВКЛ";
            }
        }

        if (PlayerPrefs.GetInt("Sound") == 1)
        {
            if (LeanLocalization.CurrentLanguage == "English")
            {
                soundButton.GetComponentInChildren<Text>().text = "SOUND OFF";
            }
            else if (LeanLocalization.CurrentLanguage == "Russian")
            {
                soundButton.GetComponentInChildren<Text>().text = "ЗВУКИ ВЫКЛ";
            }
        }
        else
        {
            if (LeanLocalization.CurrentLanguage == "English")
            {
                soundButton.GetComponentInChildren<Text>().text = "SOUND ON";
            }
            else if (LeanLocalization.CurrentLanguage == "Russian")
            {
                soundButton.GetComponentInChildren<Text>().text = "ЗВУКИ ВКЛ";
            }
        }
    }

    public void ShowInterstitial()
    {
        if (Appodeal.isLoaded(Appodeal.INTERSTITIAL) && !Appodeal.isPrecache(Appodeal.INTERSTITIAL))
        {
            Appodeal.show(Appodeal.INTERSTITIAL);
            isAdShown = true;
        }
        else
        {
            Appodeal.cache(Appodeal.INTERSTITIAL);
        }
    }

    public void onRewardedVideoLoaded(bool precache)
    {
       
    }

    public void onRewardedVideoFailedToLoad()
    {

    }

    public void onRewardedVideoShowFailed()
    {

    }

    public void onRewardedVideoShown()
    {

    }

    public void onRewardedVideoFinished(double amount, string name)
    {
        PlayerPrefsSafe.SetFloat("TotalCoals", PlayerPrefsSafe.GetFloat("TotalCoals") + coals);
        coals *= 2;
        endCoalsUI.text = ((float)coals).ToString("0");
        GameObject.Find("Coals x2 Button").GetComponent<Button>().interactable = false;
    }

    public void onRewardedVideoClosed(bool finished)
    {

    }

    public void onRewardedVideoExpired()
    {

    }

    public void onRewardedVideoClicked()
    {

    }
}
