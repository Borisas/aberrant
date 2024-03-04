using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewMutation : UiView {

    [SerializeField] MutationCell[] _cells;

    public static void Open() {

        var view = Scene.UiDirector.GetView<ViewMutation>();
        if (view == null) return;
        view.SetupNewMutations();
        Scene.UiDirector.OpenViewAdditive<ViewMutation>();
    }

    bool SetupNewMutations() {

        var mc = Scene.GameController.GetMutationController();

        var newMutations = mc.GenerateMutationSelection();

        if (newMutations == null || newMutations.Length <= 0) return false;

        //else new mutations : thumbsup, show them.

        for ( int i = 0; i < _cells.Length; i++ ) {
            if ( i >= newMutations.Length ) {
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
        if ( scell == null ) return;//no cell selected;

        Scene.Player.GetStats().AddMutation(scell.GetMutationId());
        gameObject.SetActive(false);
    }

    public void OnCellClicked(MutationCell c) {

        for ( int i = 0; i < _cells.Length; i++ ) {
            if ( _cells[i] == c ) {
                _cells[i].SetSelected(!_cells[i].IsSelected());
            }
            else {
                _cells[i].SetSelected(false);
            }
        }

        SelectedCellChanged();
    }

    void SelectedCellChanged() {

    }

    MutationCell GetSelectedCell() {
        for ( int i = 0; i < _cells.Length; i++ ) {
            if ( _cells[i].IsSelected() ) {
                return _cells[i];
            }
        }
        return null;
    }

}
