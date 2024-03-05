
public class MainMenuDirector : SimpleUi.Director {
    protected override void Awake() {
        base.Awake();
        OpenView<ViewMainMenu>();
    }
}