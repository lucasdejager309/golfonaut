using UnityEngine;

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


    public void ResetToLast(bool costsRetry, bool resetShots) {
            SetCharge(0);
            if (costsRetry) SetRetries(currentRetries-1);
            if (resetShots) SetShots(lastShotAmount);
    }

    public bool CanShoot() {
        if (currentShots > 0) {
            return true;
        } return false;
    }

    public void Shoot() {
        SetLastShots(currentShots);
        SetShots(currentShots-1);
    }

    public void SetLastShots(int amount) {
        lastShotAmount = amount;
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

    public void AddRetry(int amount) {
        SetRetries(currentRetries+amount);
    }

    public void SetRetries(int amount) {
        currentRetries = amount;
        update();
    }

    public void SetCharge(int amount) {
        Recharge(-currentCharge+amount);
    }
}

