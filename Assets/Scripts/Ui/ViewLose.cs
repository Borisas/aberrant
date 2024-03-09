using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

public class ViewLose : UiView {

    [System.Serializable]
    public class PlayerLimb {
        public Player.Limb Limb;
        public Image Sprite;
    }

    [SerializeField] private TMP_Text _labelWaves = null;
    [SerializeField] private TMP_Text _labelBlood = null;
    [SerializeField] private TMP_Text _labelKills = null;
    [Header("Player Visuals")]
    [SerializeField] Image _prefabLimb = null;
    [SerializeField] List<PlayerLimb> _limbs = new List<PlayerLimb>();


    public void OnEnable() {
        LoadPlayerVisuals();
        LoadNumbers();
    }

    void LoadNumbers() {

        var run = Scene.GameController.GetRunInstance();

        _labelWaves.text = run.WaveIndex.ToString();
        _labelBlood.text = run.TotalBlood.ToString();
        _labelKills.text = run.TotalKills.ToString();
    }


    void LoadPlayerVisuals() {
        var mutations = Scene.Player.GetStats().GetActiveMutations();

        foreach (var limb in _limbs) {
            limb.Sprite.transform.DestroyAllChildren();
        }

        foreach (var mutation in mutations) {

            var cfg = Database.GetInstance().Main.GetMutationConfig(mutation.Id);

            var limb = _limbs.FirstOrDefault(x => x.Limb == cfg.Limb);
            if (limb == null) continue;

            if (cfg.Attach) {
                var go = Instantiate(_prefabLimb, limb.Sprite.transform);
                go.sprite = cfg.Visual;
            }
            else {
                limb.Sprite.sprite = cfg.Visual;
            }
        }
    }

    public void OnReturnButton() {
        SceneManager.LoadScene("MenuScene");
    }
}
