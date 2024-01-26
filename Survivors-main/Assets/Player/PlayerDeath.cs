using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDeath : MonoBehaviour {
    public GameObject gameOverScreen;
    public HealthComponent healthComponent;
    public PlayerController controller;
    public Action onPlayerDeath;
    public List<Button> deathScreenButtons;
    void Start() {
        healthComponent = GetComponent<HealthComponent>();
        healthComponent.OnDeath += OnGameOver;
    }

    public void OnGameOver() {
        gameOverScreen.SetActive(true);
        controller.playerScore.scoreText.text = $"Score: {controller.playerScore.score}";
        onPlayerDeath?.Invoke();
    }
    public void OnAllPlayersDead() {
        foreach (var button in deathScreenButtons) {
            button.interactable = true;
        }
    }
    public void OnRestart() {
        PlayerManager.Instance.RestartGame();
    }
    public void OnBackToMenu() {
        PlayerManager.Instance.BackToMenu();
    }
}
