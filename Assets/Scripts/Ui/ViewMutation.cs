using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ViewMutation : UiView {

    public event System.Action OnClose;

    [SerializeField] MutationCell[] _cells;
    [SerializeField] GameObject _buttonSelect = null;

    bool _anyCellSelected = false;

    public static void Open() {

        var view = Scene.UiDirector.GetView<ViewMutation>();
        if (view == null) return;
        view.SetupNewMutations();
        Scene.UiDirector.OpenViewAdditive<ViewMutation>();
    }

    void OnDisable() {
        OnClose?.Invoke();
    }

    void OnEnable() {
        _buttonSelect.SetActive(false);
        _anyCellSelected = false;
    }

    bool SetupNewMutations() {

        var mc = Scene.GameController.GetMutationController();

        var newMutations = mc.GenerateMutationSelection();

        if (newMutations == null || newMutations.Length <= 0) return false;

        //else new mutations : thumbsup, show them.

        for (int i = 0; i < _cells.Length; i++) {
            if (i >= newMutations.Length) {
                _cells[i].gameObject.SetActive(false);
                break;
            }

            _cells[i].gameObject.SetActive(true);
            _cells[i].Setup(newMutations[i]);
        }


        return true;
    }

    public void OnContinue() {
        var scell = GetSelectedCell();
        if (scell == null) return;//no cell selected;

        Scene.Player.GetStats().AddMutation(scell.GetMutationId());
        gameObject.SetActive(false);
    }

    public void OnCellClicked(MutationCell c) {

        for (int i = 0; i < _cells.Length; i++) {
            if (_cells[i] == c) {
                _cells[i].SetSelected(true);
            }
            else {
                _cells[i].SetSelected(false);
            }
        }

        SelectedCellChanged();

        if (!_anyCellSelected) {
            _anyCellSelected = true;
            _buttonSelect.SetActive(true);
        }
    }

    void SelectedCellChanged() {

    }

    MutationCell GetSelectedCell() {
        for (int i = 0; i < _cells.Length; i++) {
            if (_cells[i].IsSelected()) {
                return _cells[i];
            }
        }
        return null;
    }

}
