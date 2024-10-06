using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using SQLite;


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

    [SerializeField]
    private TextMeshProUGUI treeCounter;
    [SerializeField]
    private TextMeshProUGUI pazzleCounter;
    [SerializeField]
    private TextMeshProUGUI shirtCounter;
    [SerializeField]
    private TextMeshProUGUI leaveCounter;
    public string[] randomAwardCounters = new string[] { "tree", "pazzle", "shirt", "leave", "coin" };

    public GameObject buttonLamp;
    public GameObject buttonPicture;
    public GameObject buttonPot;
    public GameObject buttonChocolate;
    private Animation anim;
    public AnimationClip objanim;


    async void Start()
    {
        UpdateButtonVisibility();
        anim = GetComponent<Animation>();
        
        SQLiteConnection db = DatabaseManager.GetConnection();
        string dbPath = db.DatabasePath;
        var conn = new SQLiteAsyncConnection(dbPath);

        var existingCoins = await conn.Table<Coins>().FirstOrDefaultAsync(c => c.Id == 1);
        var existingCollectionsElems = await conn.Table<Collections>().FirstOrDefaultAsync(c => c.Id == 1);
        var existingInventoryElems = await conn.Table<Inventory>().FirstOrDefaultAsync(c => c.Id == 1);

        int silveraward = existingCoins != null ? existingCoins.Silver : 0;
        silverCounter.text = silveraward.ToString();

        int goldaward = existingCoins != null ? existingCoins.Gold : 0;
        goldCounter.text = goldaward.ToString();

        int treCounterBase = existingCollectionsElems != null ? existingCollectionsElems.Tree : 0;
        int shirtCounterBase = existingCollectionsElems != null ? existingCollectionsElems.Shirt : 0;
        int pazzleCounterBase = existingCollectionsElems != null ? existingCollectionsElems.Pazzle : 0;
        int leafCounterBase = existingCollectionsElems != null ? existingCollectionsElems.Leaf : 0;

        treeCounter.text = treCounterBase.ToString();
        pazzleCounter.text = pazzleCounterBase.ToString();
        shirtCounter.text = shirtCounterBase.ToString();
        leaveCounter.text = leafCounterBase.ToString();

        int[] baseCounters = new int[4];
        if (existingInventoryElems != null)
        {
            baseCounters[0] = existingInventoryElems != null ? existingInventoryElems.BrokenLamp : 0;
            baseCounters[1] = existingInventoryElems != null ? existingInventoryElems.BrokenPicture : 0;
            baseCounters[2] = existingInventoryElems != null ? existingInventoryElems.BrokenPot : 0;
            baseCounters[3] = existingInventoryElems != null ? existingInventoryElems.Chocolate : 0;
        }

        string[] NamesObjects = new string[] { "BrokenLamp", "BrokenPicture", "BrokenPot", "Chocolate" };

        for (int i = 0; i < NamesObjects.Length; i++)
        {
            GameObject container = inventoryContainer.transform.Find(NamesObjects[i]).gameObject;
            GameObject counter = container.transform.Find("Counter" + NamesObjects[i]).gameObject;
            TextMeshProUGUI textComponent = counter.GetComponent<TextMeshProUGUI>();
            
            string text = textComponent.text;
            string[] parts = text.Split('/');
            
            if (parts.Length < 2)
            {
                Debug.LogError($"Неверный формат текста '{text}'. Ожидалось 'число/число'.");
                continue;
            }
            
            int counterBase = baseCounters[i];
            textComponent.text = counterBase.ToString() + "/" + "5";
        }
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

    public async void MoveToInv()
    {
        GarbageSpawn.totalobj -= 1;
        SQLiteConnection db = DatabaseManager.GetConnection();
        string dbPath = db.DatabasePath;
        var conn = new SQLiteAsyncConnection(dbPath);

        var existingEnergy = await conn.Table<Energy>().FirstOrDefaultAsync(c => c.Id == 1);

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
            existingEnergy.Amount = currentEnergyCounter;
            await conn.UpdateAsync(existingEnergy);
            energyCounter.text = existingEnergy.Amount + "/" + 10;
            
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

    async private void UpdateButtonVisibility()
    {
        SQLiteConnection db = DatabaseManager.GetConnection();
        string dbPath = db.DatabasePath;
        var conn = new SQLiteAsyncConnection(dbPath);

        var existingInventoryElems = await conn.Table<Inventory>().FirstOrDefaultAsync(c => c.Id == 1);

        GameObject itemContainer = inventoryContainer.transform.Find(gameObject.tag)?.gameObject;
        if (itemContainer != null)
        {
            GameObject counter = itemContainer.transform.Find("Counter" + gameObject.tag)?.gameObject;
            GameObject button = itemContainer.transform.Find("Button" + gameObject.tag)?.gameObject; 

            if (counter != null && button != null)
            {
                TextMeshProUGUI textComponent = counter.GetComponent<TextMeshProUGUI>();

                if (textComponent != null)
                {
                    int currentCount = 0;

                    switch (gameObject.tag)
                    {
                        case "BrokenLamp":
                            currentCount = existingInventoryElems?.BrokenLamp ?? 0;
                            break;
                        case "BrokenPicture":
                            currentCount = existingInventoryElems?.BrokenPicture ?? 0;
                            break;
                        case "BrokenPot":
                            currentCount = existingInventoryElems?.BrokenPot ?? 0;
                            break;
                        case "Chocolate":
                            currentCount = existingInventoryElems?.Chocolate ?? 0;
                            break;
                        default:
                            currentCount = 0;
                            break;
                    }
                    if (currentCount >= 5) {
                        button.SetActive(true);
                    }else {
                        button.SetActive(false);
                    }
                }
            }
        }
    }

    private async void UpdateCounterAndButton(GameObject counter, GameObject button, bool increase)
    {
        SQLiteConnection db = DatabaseManager.GetConnection();
        string dbPath = db.DatabasePath;
        var conn = new SQLiteAsyncConnection(dbPath);

        var existingInventoryElems = await conn.Table<Inventory>().FirstOrDefaultAsync(c => c.Id == 1);

        if (counter != null && button != null)
        {
            TextMeshProUGUI textComponent = counter.GetComponent<TextMeshProUGUI>();
            if (textComponent != null)
            {
                string[] parts = textComponent.text.Split('/');
                if (parts.Length > 0)
                {
                    int currentCount = int.Parse(parts[0]);

                    string counterName = textComponent.name;

                    if (increase)
                    {
                        currentCount++;
                        if (existingInventoryElems == null)
                        {
                            existingInventoryElems = new Inventory
                            {
                                Id = 1,
                                BrokenLamp = counterName == "CounterBrokenLamp" ? currentCount : 0,
                                BrokenPicture = counterName == "CounterBrokenPicture" ? currentCount : 0,
                                BrokenPot = counterName == "CounterBrokenPot" ? currentCount : 0,
                                Chocolate = counterName == "CounterChocolate" ? currentCount : 0
                            };
                            await conn.InsertAsync(existingInventoryElems);
                        }
                        else
                        {
                            UpdateInventory(ref existingInventoryElems, counterName, currentCount);
                            await conn.UpdateAsync(existingInventoryElems);
                        }
                    }
                    else
                    {
                        if (currentCount >= 5){
                            currentCount -= 5; 
                        }
                        if (existingInventoryElems != null)
                        {
                            UpdateInventory(ref existingInventoryElems, counterName, currentCount);
                            await conn.UpdateAsync(existingInventoryElems);
                        }
                    }

                    textComponent.text = currentCount.ToString() + "/" + "5";

                    if (currentCount >= 5){
                        button.SetActive(true);
                    }else {
                        button.SetActive(false);
                    }
                }
            }
        }
    }
    private void UpdateInventory(ref Inventory inventory, string counterName, int count)
    {
        switch (counterName)
        {
            case "CounterBrokenLamp":
                inventory.BrokenLamp = count;
                break;
            case "CounterBrokenPicture":
                inventory.BrokenPicture = count;
                break;
            case "CounterBrokenPot":
                inventory.BrokenPot = count;
                break;
            case "CounterChocolate":
                inventory.Chocolate = count;
                break;
        }
    }

    private async void UpdateCoinCounter(int silveramount, int goldamount)
    {
        if (goldCounter == null || silverCounter == null)
        {
            return;
        }

        SQLiteConnection db = DatabaseManager.GetConnection();
        string dbPath = db.DatabasePath;
        var conn = new SQLiteAsyncConnection(dbPath);

        var existingCoins = await conn.Table<Coins>().FirstOrDefaultAsync(c => c.Id == 1);

        if (existingCoins != null)
        {
            existingCoins.Silver += silveramount;
            existingCoins.Gold += goldamount;

            silverCounter.text = existingCoins.Silver.ToString();
            goldCounter.text = existingCoins.Gold.ToString();

            await conn.UpdateAsync(existingCoins);
        }
        else
        {
            existingCoins = new Coins { Id = 1, Silver = silveramount, Gold = goldamount };
            await conn.InsertAsync(existingCoins);

            silverCounter.text = existingCoins.Silver.ToString();
            goldCounter.text = existingCoins.Gold.ToString();
        }
    }

    public void OnMouseDown()
    {
        isclickon = true;
    }

    public async void Trade()
    {
        SQLiteConnection db = DatabaseManager.GetConnection();
        string dbPath = db.DatabasePath;
        var conn = new SQLiteAsyncConnection(dbPath);
        var existingCollectionsElems = await conn.Table<Collections>().FirstOrDefaultAsync(c => c.Id == 1);

        string[] partsTree = treeCounter.text.Split('/');
        int currentTreeCount = int.Parse(partsTree[0]);
        
        string[] partsPazzle = pazzleCounter.text.Split('/');
        int currentPazzleCount = int.Parse(partsPazzle[0]);
        
        string[] partsShirt = shirtCounter.text.Split('/');
        int currentShirtCount = int.Parse(partsShirt[0]);

        string[] partsLeaf = leaveCounter.text.Split('/');
        int currentLeafCount = int.Parse(partsLeaf[0]);


        GameObject button = inventoryContainer.transform.Find(gameObject.tag).Find("Button" + gameObject.tag).gameObject;
        if (button != null)
        {
            GameObject counter = inventoryContainer.transform.Find(gameObject.tag).Find("Counter" + gameObject.tag).gameObject;

            if (counter != null)
            {
                UpdateCounterAndButton(counter, button, false);
            }

            UpdateCoinCounter(100, 1);
            
            int randomNumber = Random.Range(0, 100); 

            if (randomNumber < 33){

                int randomIndex = Random.Range(0, randomAwardCounters.Length);
                string selectedAward = randomAwardCounters[randomIndex];

                if (selectedAward == "tree"){
                    currentTreeCount++;
                    treeCounter.text = currentTreeCount.ToString();
                }
                else if (selectedAward == "pazzle"){
                    currentPazzleCount ++;
                    pazzleCounter.text = currentPazzleCount.ToString();
                }
                else if (selectedAward == "shirt"){
                    currentShirtCount ++;
                    shirtCounter.text = currentShirtCount.ToString();
                }
                else if (selectedAward == "leave"){
                    currentLeafCount ++;
                    leaveCounter.text = currentLeafCount.ToString();
                }
                else if (selectedAward == "coin"){
                    UpdateCoinCounter(100, 1);
                }

                if (existingCollectionsElems == null){
                    existingCollectionsElems = new Collections
                    {
                        Id = 1,
                        Tree = selectedAward == "tree" ? currentTreeCount : 0,
                        Pazzle = selectedAward == "pazzle" ? currentPazzleCount : 0,
                        Shirt = selectedAward == "shirt" ? currentShirtCount : 0,
                        Leaf = selectedAward == "leave" ? currentLeafCount : 0
                    };
        
                await conn.InsertAsync(existingCollectionsElems);
                } else {
                    if (selectedAward == "tree")
                        existingCollectionsElems.Tree = currentTreeCount;
                    else if (selectedAward == "pazzle")
                        existingCollectionsElems.Pazzle = currentPazzleCount;
                    else if (selectedAward == "shirt")
                        existingCollectionsElems.Shirt = currentShirtCount;
                    else if (selectedAward == "leave")
                        existingCollectionsElems.Leaf = currentLeafCount;
                    await conn.UpdateAsync(existingCollectionsElems);
                }

            }
        }
    }
}



