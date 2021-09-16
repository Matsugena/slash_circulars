public class PlayerInput : IPlayerInput {
    public Jump Jump () {
        return new Jump { data = 1 };
    }

    public Move Move (float dx) {
        return new Move { axisDx = dx };
    }
}