using UnityEngine;
[System.Serializable]
public class BulletSource {
    public PlayerController player;
    public BulletSource(PlayerController player) {
        this.player = player;
    }
    public void AddScore(int score) {
        if (player != null) {
            Debug.Log("Adding score");
            player.score.score += score;
        }
        else {
            Debug.Log("Trying to add score, no player");
        }
    }
}
