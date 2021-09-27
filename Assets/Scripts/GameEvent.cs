// GameEvent として文字列を送信する
public class GameEvent {
    private static string StringDoReset = "DoReset";

    private GameEvent (string s) {
        this.EventOf = s;
    }

    public string EventOf { set; get; }

    public static GameEvent DoReset () {
        return new GameEvent (StringDoReset);
    }

    public override bool Equals (object obj) {
        if (typeof (GameEvent) != obj.GetType ()) return false;

        return this.EventOf == (obj as GameEvent).EventOf;
    }

    public override int GetHashCode () {
        return base.GetHashCode ();
    }

    public override string ToString () {
        return base.ToString ();
    }
}