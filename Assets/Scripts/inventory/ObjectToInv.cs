using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class InventoryCounter : MonoBehaviour
{
    public string itemName;
    public int currentCount;
    public int maxCount;

    private Text counterText;

    void Start()
    {
        counterText = GetComponent<Text>();
        UpdateCounterText();
    }

    public void IncreaseCount()
    {
        if (currentCount < maxCount)
        {
            currentCount++;
            UpdateCounterText();
        }
    }

    private void UpdateCounterText()
    {
        counterText.text = currentCount + "/" + maxCount;
    }
}


public class ObjectToInv : MonoBehaviour
{
    public Vector3 InvPos;
    public GameObject molot_right;
    public GameObject molot_left;
    public float movespeed = 10;
    public GameObject player;
    bool isclickon = false;
    bool isdestory = true;

    [SerializeField]
    private GameObject inventoryContainer;

    [SerializeField]
    private TextMeshProUGUI goldCounter;
    [SerializeField]
    private TextMeshProUGUI silverCounter;

    [SerializeField]
    public GameObject inv;

    [SerializeField]
    private TextMeshProUGUI energyCounter;

    public GameObject buttonLamp;
    public GameObject buttonPicture;
    public GameObject buttonPot;
    public GameObject buttonChocolate;
    private Animation anim;
    public AnimationClip objanim;

    void Start()
    {
        UpdateButtonVisibility();
        anim = GetComponent<Animation>();
    }

    void Update()
    {

        string[] parts = energyCounter.text.Split('/');
        int currentEnergyCounter = int.Parse(parts[0]);

        if (currentEnergyCounter > 0){
        
            if (Mathf.Abs(transform.position.x - player.transform.position.x) <= 46 & isdestory & isclickon)
            {
                if (transform.position.x > player.transform.position.x)
                {
                    SpawnMolotRight();
                    Invoke("PlayAnim", 0.3f);
                    isdestory = false;
                    Invoke("MoveToInv", 0.5f);
                }
                else
                {
                    SpawnMolotLeft();
                    Invoke("PlayAnim", 0.3f);
                    isdestory = false;
                    Invoke("MoveToInv", 0.5f);
                }
            }
        UpdateButtonVisibility();
        }
    }

    public void MoveToInv()
    {

        string[] parts = energyCounter.text.Split('/');
        int currentEnergyCounter = int.Parse(parts[0]);

        if (currentEnergyCounter > 0){
            Destroy(GameObject.Find("Molot_right(Clone)"));
            Destroy(GameObject.Find("Molot_left(Clone)"));
            Destroy(this.gameObject);
            inv.GetComponent<Animation>().Play("inv");

            UpdateCounterAndButton(
                inventoryContainer.transform.Find(gameObject.tag).Find("Counter" + gameObject.tag).gameObject,
                inventoryContainer.transform.Find(gameObject.tag).Find("Button" + gameObject.tag).gameObject,
                true
            );
            currentEnergyCounter -= 1;
            energyCounter.text = currentEnergyCounter + "/" + 10;
        }
    }

    private void PlayAnim()
    {
        anim.Play(objanim.name);
    }

    private void SpawnMolotLeft()
    {
        Instantiate(molot_left, new Vector3(transform.position.x + 10, transform.position.y + 10, transform.position.z), 
            Quaternion.identity);
    }

    private void SpawnMolotRight()
    {
        Instantiate(molot_right, new Vector3(transform.position.x - 10, transform.position.y + 10, transform.position.z), 
            Quaternion.identity);
    }

    private void UpdateButtonVisibility()
    {
        GameObject itemContainer = inventoryContainer.transform.Find(gameObject.tag).gameObject;
        if (itemContainer != null)
        {
            GameObject counter = itemContainer.transform.Find("Counter" + gameObject.tag).gameObject;
            GameObject button = itemContainer.transform.Find("Button" + gameObject.tag).gameObject; 

            if (counter != null && button != null)
            {
                TextMeshProUGUI textComponent = counter.GetComponent<TextMeshProUGUI>();

                if (textComponent != null)
                {
                    string[] parts = textComponent.text.Split('/');
                    if (parts.Length > 0)
                    {
                        int currentCount = int.Parse(parts[0]);

                        if (currentCount >= 5)
                        {
                            button.SetActive(true); 
                        }
                        else
                        {
                            button.SetActive(false); 
                        }
                    }
                }
            }
        }
    }

    private void UpdateCounterAndButton(GameObject counter, GameObject button, bool increase)
    {
        if (counter != null && button != null)
        {
            TextMeshProUGUI textComponent = counter.GetComponent<TextMeshProUGUI>();
            if (textComponent != null)
            {
                string[] parts = textComponent.text.Split('/');
                if (parts.Length > 0)
                {
                    int currentCount = int.Parse(parts[0]);

                    if (increase)
                    {
                        currentCount++;
                    }
                    else
                    {
                        currentCount -= 5;
                    }

                    textComponent.text = currentCount.ToString() + textComponent.text[1..];

                    if (currentCount >= 5)
                    {
                        button.SetActive(true);
                    }
                    else
                    {
                        button.SetActive(false);
                    }
                }
            }
        }
    }

    private void UpdateCoinCounter(int silveramount, int goldamount)
    {
        if (goldCounter != null && silverCounter != null) {
            int silveraward = int.Parse(silverCounter.text) + silveramount;
            silverCounter.text = silveraward.ToString();
            int goldaward = int.Parse(goldCounter.text) + goldamount;
            goldCounter.text = goldaward.ToString();
        }
    }

    public void OnMouseDown()
    {
        isclickon = true;
    }

    public void Trade()
    {
        GameObject button = inventoryContainer.transform.Find(gameObject.tag).Find("Button" + gameObject.tag).gameObject;
        if (button != null)
        {
            GameObject counter = inventoryContainer.transform.Find(gameObject.tag).Find("Counter" + gameObject.tag).gameObject;

            if (counter != null)
            {
                UpdateCounterAndButton(counter, button, false);
            }

            UpdateCoinCounter(100, 1);
        }
    }
}