
public class SubBulletController : BulletController {
    BulletController[] subBullets;
    protected override void Update() {

    }
    public override void Start() {
        Destroy(gameObject, data.bulletLifetime);
        subBullets = GetComponentsInChildren<BulletController>();
        foreach (var bullet in subBullets) {
            bullet.data = data;
            bullet.source = source;
        }
    }
}
