
public class GameSceneDirector : SimpleUi.Director {
    protected override void Awake() {
        base.Awake();
        OpenView<ViewGameplay>();
    }
}