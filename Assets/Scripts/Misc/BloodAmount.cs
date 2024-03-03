
public struct BloodAmount {
    public int Amount;

    public BloodAmount(int a) {
        Amount = a;
    }

    public void Remove() {
        Scene.GameController.GetRunInstance().RemoveBlood(Amount);
    }

    public void Add() {
        Scene.GameController.GetRunInstance().AddBlood(Amount);
    }

    public bool CanPay() {
        return Scene.GameController.GetRunInstance().Blood >= Amount;
    }
}