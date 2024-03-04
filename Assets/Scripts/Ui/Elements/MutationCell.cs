using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MutationCell : MonoBehaviour {
    [SerializeField] Image _icon = null;
    [SerializeField] TMP_Text _labelTitle = null;
    [SerializeField] Image _iconActive = null;
    [SerializeField] Image _iconBodyPart = null;
    [SerializeField] Image _iconBodyPartAttachment = null;

    MutationId _id = MutationId.Invalid;
    bool _selected = false;

    public void Setup(MutationId id) {
        var cfg = Database.GetInstance().Main.GetMutationConfig(id);
        if ( cfg == null ) {
            gameObject.SetActive(false);
            Debug.LogError($"Cant setup mutation {id}");
            return;
        }

        _id = id;

        MutationInstance mutation = Scene.Player.GetStats().GetMutationInstance(id);
        int level = mutation == null ? 0 : mutation.Level;

        _icon.sprite = cfg.Icon;
        _labelTitle.text = $"{cfg.Title} {(level+1 >= cfg.MaxLevel ? "MAX" : level+1)}";
        _iconActive.gameObject.SetActive(false);
        _iconBodyPartAttachment.gameObject.SetActive(cfg.Attach);
        _iconBodyPart.sprite = Database.GetInstance().Main.GetSpriteForLimb(cfg.Limb);

        _selected = false;
    }

    public void SetSelected(bool s) {
        _iconActive.gameObject.SetActive(s);
        _selected = s;
    }

    public bool IsSelected() {
        return _selected;
    }

    public MutationId GetMutationId() => _id;
}
