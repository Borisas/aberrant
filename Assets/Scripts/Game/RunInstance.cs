
public class RunInstance {

    public int Blood;
    public int WaveIndex = 0;
    public int PurchasedMutations = 0;
    public int PurchasedRecoveries = 0;
    public int PurchasedMoreLife = 0;


    public void AddBlood(int value = 1) {
        Blood += value;
    }

    public void RemoveBlood(int value = 1) {
        Blood -= value;
    }

    public void NextWave() {
        WaveIndex++;
    }

    public void OnMutationPurchased() => PurchasedMutations++;
    public void OnRecoveryPurchased() => PurchasedRecoveries++;
    public void OnMoreLifePurchased() => PurchasedMoreLife++;

}