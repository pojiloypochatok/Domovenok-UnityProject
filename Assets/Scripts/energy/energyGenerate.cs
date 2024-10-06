using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using SQLite;
using System.Threading.Tasks;

public class energyGenerate : MonoBehaviour
{
    int maxEnergy = 10;
    private float timeToNextEnergy;
    private float energyGenerationInterval = 20f;

    [SerializeField]
    private TextMeshProUGUI energyCounter;

    async void Start()
    {
        SQLiteConnection db = DatabaseManager.GetConnection();
        string dbPath = db.DatabasePath;
        var conn = new SQLiteAsyncConnection(dbPath);

        var existingEnergyElems = await conn.Table<Energy>().FirstOrDefaultAsync(c => c.Id == 1);
        
        if (existingEnergyElems == null)
        {
            existingEnergyElems = new Energy
            {
                Id = 1,
                Amount = maxEnergy,
            };
            await conn.InsertAsync(existingEnergyElems);
        }

        int currentEnergyCounter = existingEnergyElems.Amount;

        timeToNextEnergy = energyGenerationInterval;
        await UpdateEnergyDisplay(currentEnergyCounter);
    }

    async void Update(){
        SQLiteConnection db = DatabaseManager.GetConnection();
        string dbPath = db.DatabasePath;
        var conn = new SQLiteAsyncConnection(dbPath);
        
        var existingEnergyElems = await conn.Table<Energy>().FirstOrDefaultAsync(c => c.Id == 1);

        if (existingEnergyElems == null)
        {
            Debug.LogError("Energy record not found!");
            return;
        }

        string[] parts = energyCounter.text.Split('/');
        int currentEnergyCounter = existingEnergyElems != null ? existingEnergyElems.Amount : 10;

        existingEnergyElems.Amount = currentEnergyCounter;

        timeToNextEnergy -= Time.deltaTime;
        if (timeToNextEnergy <= 0)
        {
            GenerateEnergy(existingEnergyElems);
            await conn.UpdateAsync(existingEnergyElems);
            timeToNextEnergy = energyGenerationInterval;
        }
    }

    async private void GenerateEnergy(Energy existingEnergyElems)
    {
        if (existingEnergyElems.Amount < maxEnergy)
        {
            existingEnergyElems.Amount++;
            await UpdateEnergyDisplay(existingEnergyElems.Amount);
        }
    }

    async private Task UpdateEnergyDisplay(int currentEnergyCounter)
    {
        energyCounter.text = currentEnergyCounter + "/" + maxEnergy;
        
        SQLiteConnection db = DatabaseManager.GetConnection();
        string dbPath = db.DatabasePath;
        var conn = new SQLiteAsyncConnection(dbPath);
        
        var existingEnergyElems = await conn.Table<Energy>().FirstOrDefaultAsync(c => c.Id == 1);
        existingEnergyElems.Amount = currentEnergyCounter;
        
        await conn.UpdateAsync(existingEnergyElems);
    }
}

