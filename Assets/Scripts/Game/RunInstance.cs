
public class RunInstance {

    public int Blood;
    public int WaveIndex = 0;


    public void AddBlood(int value = 1) {
        Blood += value;
    }

    public void RemoveBlood(int value = 1) {
        Blood -= value;
    }

    public void NextWave() {
        WaveIndex++;
    }
}