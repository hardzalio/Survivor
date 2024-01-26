using System;

public class RunInterval {
    float delay;
    float runIn = 0f;
    Action action;
    public RunInterval(float delay, Action action) {
        this.delay = delay;
        this.action = action;
    }
    public RunInterval Delayed() {
        runIn = delay;
        return this;
    }
    public void Update(float deltaTime) {
        runIn -= deltaTime;
        if (runIn <= 0) {
            runIn = delay;
            action();
        }
    }
    public bool IsAvaliable() {
        return runIn <= 0;
    }
    public void Restart() {
        runIn = delay;
    }
}
