using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StoreManager : MonoBehaviour
{
    // Values
    public List<GameObject> skins = new List<GameObject>();
    private int currIndex = 0;
    private SafeFloat currPrice = 0;
    private SafeFloat coals = 0;
    public Transform skinSpawnPoint;

    // UI
    public Text selectedText;
    public Button buyButton;
    public Button selectButton;
    public Button nextButton;
    public Button prevButton;
    public Text priceText;
    public Text totalCoals;
    public GameObject notEnoughMoneyPanel;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefsSafe.HasKey("TotalCoals"))
        {
            coals = PlayerPrefsSafe.GetFloat("TotalCoals");
            totalCoals.text = ((float)coals).ToString("0");
        }
        PlayerPrefsSafe.SetFloat("IsBought" + 0, 1);
        for (int i = 1; i < skins.Count; i++)
        {
            if (!PlayerPrefsSafe.HasKey("IsBought" + i))
            {
                PlayerPrefsSafe.SetFloat("IsBought" + i, 0);
            }
        }
        if (PlayerPrefsSafe.HasKey("SelectedSkin"))
        {
            Instantiate(skins[currIndex], skinSpawnPoint.position, Quaternion.identity);
            FindObjectOfType<PlayerMovement>().enabled = false;
            GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<AudioSource>().Stop();
            if (PlayerPrefsSafe.GetFloat("SelectedSkin") != 0)
            {
                selectedText.gameObject.SetActive(false);
                selectButton.gameObject.SetActive(true);
            }
        }
        else
        {
            PlayerPrefsSafe.SetFloat("SelectedSkin", 0f);
            Instantiate(skins[currIndex], skinSpawnPoint.position, Quaternion.identity);
            FindObjectOfType<PlayerMovement>().enabled = false;
            GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<AudioSource>().Stop();
        }
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void SelectSkin()
    {
        PlayerPrefsSafe.SetFloat("SelectedSkin", currIndex);
        selectedText.gameObject.SetActive(true);
        selectButton.gameObject.SetActive(false);
    }

    public void BuySkin()
    {
        if(currPrice <= coals)
        {
            coals -= currPrice;
            PlayerPrefsSafe.SetFloat("TotalCoals", coals);
            PlayerPrefsSafe.SetFloat("IsBought" + currIndex, 1);
            totalCoals.text = ((float)coals).ToString("0");
            selectButton.gameObject.SetActive(true);
            buyButton.gameObject.SetActive(false);
            priceText.gameObject.SetActive(false);
        }
        else
        {
            notEnoughMoneyPanel.SetActive(true);
        }
    }

    public void CloseNotEnoughMoneyPanel()
    {
        notEnoughMoneyPanel.SetActive(false);
    }

    public void NextSkin()
    {
        if (currIndex < skins.Count - 1)
        {
            prevButton.gameObject.SetActive(true);
            currIndex++;
            Destroy(GameObject.FindGameObjectWithTag("Player").gameObject);
            var tmpSkin = Instantiate(skins[currIndex], skinSpawnPoint.position, Quaternion.identity);
            FindObjectOfType<PlayerMovement>().enabled = false;
            tmpSkin.GetComponentInChildren<AudioSource>().Stop();
            if (PlayerPrefsSafe.GetFloat("SelectedSkin") != currIndex)
            {
                if(PlayerPrefsSafe.GetFloat("IsBought" + currIndex) == 0)
                {
                    selectedText.gameObject.SetActive(false);
                    selectButton.gameObject.SetActive(false);
                    buyButton.gameObject.SetActive(true);
                    priceText.gameObject.SetActive(true);
                    currPrice = FindObjectOfType<Skin>().price;
                    priceText.text = currPrice.ToString();
                    
                }
                else
                {
                    buyButton.gameObject.SetActive(false);
                    priceText.gameObject.SetActive(false);
                    selectedText.gameObject.SetActive(false);
                    selectButton.gameObject.SetActive(true);
                }
                
            }
            else
            {
                buyButton.gameObject.SetActive(false);
                priceText.gameObject.SetActive(false);
                selectedText.gameObject.SetActive(true);
                selectButton.gameObject.SetActive(false);
            }
        }
        if (currIndex == skins.Count - 1)
        {
            nextButton.gameObject.SetActive(false);
        }
    }

    public void PrevSkin()
    {
        if(currIndex > 0)
        {
            nextButton.gameObject.SetActive(true);
            currIndex--;
            Destroy(GameObject.FindGameObjectWithTag("Player").gameObject);
            var tmpSkin = Instantiate(skins[currIndex], skinSpawnPoint.position, Quaternion.identity);
            FindObjectOfType<PlayerMovement>().enabled = false;
            tmpSkin.GetComponentInChildren<AudioSource>().Stop();
            if (PlayerPrefsSafe.GetFloat("SelectedSkin") != currIndex)
            {
                if (PlayerPrefsSafe.GetFloat("IsBought" + currIndex) == 0)
                {
                    selectedText.gameObject.SetActive(false);
                    selectButton.gameObject.SetActive(false);
                    buyButton.gameObject.SetActive(true);
                    priceText.gameObject.SetActive(true);
                    currPrice = FindObjectOfType<Skin>().price;
                    priceText.text = currPrice.ToString();

                }
                else
                {
                    buyButton.gameObject.SetActive(false);
                    priceText.gameObject.SetActive(false);
                    selectedText.gameObject.SetActive(false);
                    selectButton.gameObject.SetActive(true);
                }

            }
            else
            {
                buyButton.gameObject.SetActive(false);
                priceText.gameObject.SetActive(false);
                selectedText.gameObject.SetActive(true);
                selectButton.gameObject.SetActive(false);
            }
        }
        if (currIndex == 0)
        {
            prevButton.gameObject.SetActive(false);
        }
    }
}
