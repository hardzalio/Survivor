
/// <summary>
/// Used to handle an action that should only happen after a delay/cooldown
/// </summary>
public class Delay {
    float delay;
    float runIn = 0f;
    public Delay(float delay) {
        this.delay = delay;
    }
    public Delay(float delay, float initialDelay) {
        this.delay = delay;
        this.runIn = initialDelay;
    }
    public void Update(float deltaTime) {
        runIn -= deltaTime;
    }
    public bool IsReady() {
        return runIn <= 0;
    }
    public void Start() {
        runIn = delay;
    }
    public float Remaining() {
        return runIn;
    }
    public void Set(float delay) {
        this.delay = delay;
    }
    static public implicit operator bool(Delay delay) => delay.IsReady();
}
