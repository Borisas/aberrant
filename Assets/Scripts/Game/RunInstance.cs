
public class RunInstance {

    public int Blood;
    public int WaveIndex;


    public void AddBlood(int value = 1) {
        Blood += value;
    }

    public void NextWave() {
        WaveIndex++;
    }
}