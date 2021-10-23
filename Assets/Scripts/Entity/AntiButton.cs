public class AntiButton : Button {
    public override bool IsActive() {
        return !_isPressed;
    }
}
