using UnityEngine;

[System.Serializable]
public class Shots {
    //parameters
    public int startWithShots{ get; private set; } = 4;
    public int startWithRetries{ get; private set; } = 0;
    public int maxRetries{ get; private set; } = 3;
    public int chargePerRetry{ get; private set; } = 4;

    //variables
    public int currentShots { get; private set; }
    public int currentRetries {get; private set; } = 0;
    public int currentCharge {get; private set; } = 0;

    [SerializeField] int lastShotAmount;

    //delegate
    public delegate void UpdateShots();
    public UpdateShots update;

    public Shots(PlayerScript player, int startWithShots, int startWithRetries, int maxRetries, int chargePerRetry) {
        this.startWithShots = startWithShots;
        this.startWithRetries = startWithRetries;
        this.maxRetries = maxRetries;
        this.chargePerRetry = chargePerRetry;

        currentShots = startWithShots;
        currentRetries = startWithRetries;

        update += player.gameManager.UpdateShotsUI;
    }

    public void DebugShots() {
        Debug.Log("shots: " + currentShots + " retries: " + currentRetries + " charge: " + currentCharge);
    }

    public void Restart() {
        SetShots(startWithShots);
        SetRetries(startWithRetries);
        SetCharge(0);
    }


    public void ResetToLast(bool freeShot) {
            SetCharge(0);
            if (freeShot) {
                SetRetries(currentRetries-1);
                SetShots(lastShotAmount);
            }
    }

    public bool Shoot(){
        if (currentShots > 0) {
            lastShotAmount = currentShots;
            SetShots(currentShots-1);
            return true;
        } return false;
    }

    public void Recharge(int amount) {
        currentCharge += amount;
        while (currentCharge >= chargePerRetry && currentRetries < maxRetries) {
            currentRetries++;
            currentCharge -= chargePerRetry;
        }
        
        update();
    }

    public void AddShots(int amount) {
        SetShots(currentShots+amount);
    }

    public void SetShots(int amount) {
        currentShots = amount;
        update();
    }

    public void SetRetries(int amount) {
        currentRetries = amount;
        update();
    }

    public void SetCharge(int amount) {
        Recharge(-currentCharge+amount);
    }
}

