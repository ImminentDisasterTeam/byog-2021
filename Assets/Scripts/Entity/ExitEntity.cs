public class ExitEntity : Entity {
    public bool active = true;

    public override bool CanBeMoved() {
        return false;
    }
}
