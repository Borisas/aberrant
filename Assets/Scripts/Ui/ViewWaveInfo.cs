using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ViewWaveInfo : UiView {

    [SerializeField] TMP_Text _labelWave;
    [SerializeField] Animation _animation;

    public static void Open(int wave) {

        var view = Scene.UiDirector.GetView<ViewWaveInfo>();
        if (view == null) return;
        view.SetupLabel(wave);
        Scene.UiDirector.OpenViewAdditive<ViewWaveInfo>();
    }

    void SetupLabel(int widx) {
        _labelWave.text = $"STAGE {widx + 1}";
    }

    void OnEnable() {
        _animation.Stop();
        _animation.Play("wave_info");
    }

    public void OnAnimEnd() {
        gameObject.SetActive(false);
    }
}
