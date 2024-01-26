using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour {
    public static PlayerManager Instance;
    [HideInInspector]
    public List<PlayerController> players = new();
    PlayerInputManager inputManager;
    public static List<PlayerController> Players => Instance.players;
    public GameObject playerPrefab;
    List<PlayerController> playersDead = new();
    public PlayerData keyboardPlayerData;
    public PlayerData gamepadPlayerData;
    public PlayerController keyboardPlayer;
    public PlayerController gamepadPlayer;


    [SerializeField] MenuHandler menuHandler;
    void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }
    private void Start() {
        inputManager = GetComponent<PlayerInputManager>();
        if (menuHandler != null) {
            menuHandler.onGameStarted += InitializeGame;
            Debug.Log("Menu handler found");
        }
        keyboardPlayerData.Initialize();
        gamepadPlayerData.Initialize();
    }

    public static PlayerController GetClosestPlayer(Vector3 position) {
        if (Instance == null || Players.Count == 0) {
            return null;
        }
        PlayerController closestPlayer = null;
        var closestDistance = float.MaxValue;
        foreach (var player in Players) {
            var distance = Vector3.Distance(player.transform.position, position);
            if (distance < closestDistance) {
                closestDistance = distance;
                closestPlayer = player;
            }
        }
        return closestPlayer;
    }

    public void InitializeGame() {
        Debug.Log("Initializing game");
        SpawnPlayer(keyboardPlayerData);
        SpawnPlayer(gamepadPlayerData);
        SetCameras();
        SetColors();
    }

    void SpawnPlayer(PlayerData playerData) {
        if (!playerData.playerEnabled.isOn) {
            return;
        }
        var player = Instantiate(playerPrefab, new Vector3(-1 + playerData.playerNumber, 1, 0), Quaternion.Euler(0, 0, 0));
        var controller = player.GetComponent<PlayerController>();
        controller.colorInitializer.SetColor(keyboardPlayerData.playerColorPicker.GetColor());
        players.Add(controller);
        controller.virtualCamera.gameObject.layer = playerData.PlayerVirtualCamLayer;
        controller.camera.cullingMask = playerData.cameraCullingMask;
        if (playerData == keyboardPlayerData) {
            keyboardPlayer = controller;
        }
        else {
            gamepadPlayer = controller;
        }
        controller.playerDeath.onPlayerDeath += () => OnPlayerDeath(controller);
    }
    void OnPlayerDeath(PlayerController controller) {
        players.Remove(controller);
        playersDead.Add(controller);
        if (ArePlayersDead()) {
            foreach (var p in playersDead) {
                p.playerDeath.OnAllPlayersDead();
            }
        }
    }
    void ClearPlayer(PlayerController player) {
        Destroy(player.camera.gameObject);
        Destroy(player.virtualCamera.gameObject);
        Destroy(player.gameObject);
    }
    public void RestartGame() {
        foreach (var player in playersDead) {
            ClearPlayer(player);
        }
        playersDead.Clear();
        players.Clear();
        EnemyManager.Instance.Restart();
        InitializeGame();
    }
    public void BackToMenu() {
        foreach (var player in playersDead) {
            ClearPlayer(player);
        }
        Destroy(this);
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
    void SetCameras() {
        var bothPlayers = keyboardPlayer != null && gamepadPlayer != null;
        if (bothPlayers) {
            keyboardPlayer.camera.rect = new Rect(0, 0, 0.5f, 1);
            gamepadPlayer.camera.rect = new Rect(0.5f, 0, 0.5f, 1);
        }
        else if (keyboardPlayer != null) {
            keyboardPlayer.camera.rect = new Rect(0, 0, 1, 1);
        }
        else if (gamepadPlayer != null) {
            gamepadPlayer.camera.rect = new Rect(0, 0, 1, 1);
        }
    }
    void SetColors() {
        if (keyboardPlayer != null) {
            keyboardPlayer.colorInitializer.SetColor(keyboardPlayerData.playerColorPicker.GetColor());
        }
        if (gamepadPlayer != null) {
            gamepadPlayer.colorInitializer.SetColor(gamepadPlayerData.playerColorPicker.GetColor());
        }
    }
    public bool ArePlayersDead() {
        return players.Count == 0;
    }
}

[Serializable]
public class PlayerData {
    public int playerNumber;
    public ColorPicker playerColorPicker;
    public Toggle playerEnabled;
    public LayerMask cameraCullingMask;

    public string PlayerColorKey => $"Player{playerNumber}Color";
    public string PlayerEnabledKey => $"Player{playerNumber}Enabled";
    public LayerMask PlayerVirtualCamLayer => LayerMask.NameToLayer($"P{playerNumber}Cam");
    public void Initialize() {
        playerEnabled.onValueChanged.AddListener(SetPlayerEnabled);
        ColorUtility.TryParseHtmlString("#" + PlayerPrefs.GetString(PlayerColorKey, "ffffff"), out var color);
        playerColorPicker.SetColor(color);
        playerEnabled.isOn = PlayerPrefs.GetInt(PlayerEnabledKey, 0) == 1;
        playerColorPicker.onColorChanged += SetPlayerColor;
    }

    private void SetPlayerColor(Color color) {
        PlayerPrefs.SetString(PlayerColorKey, ColorUtility.ToHtmlStringRGB(color));
    }

    public void SetPlayerEnabled(bool enabled) {
        playerColorPicker.gameObject.SetActive(enabled);
        PlayerPrefs.SetInt(PlayerEnabledKey, enabled ? 1 : 0);
    }

}
