using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuHandler : MonoBehaviour {
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject playerSelectMenu;
    public Action onGameStarted;
    void Start() {
        DontDestroyOnLoad(transform.parent.gameObject);
    }
    public void OnMainMenuSwitch(bool openMain) {
        mainMenu.SetActive(openMain);
        playerSelectMenu.SetActive(!openMain);
    }
    public void InitializeGame() {
        StartCoroutine(OnGameStart());
    }
    public IEnumerator OnGameStart() {
        Debug.Log("Loading");
        var loader = SceneManager.LoadSceneAsync("SampleScene", LoadSceneMode.Additive);
        Debug.Log("Waiting for load");
        yield return new WaitUntil(() => loader.isDone);
        Debug.Log("Game starting <handler>");
        var unloader = SceneManager.UnloadSceneAsync("MainMenu");
        Debug.Log("Unloading scene");
        yield return new WaitUntil(() => unloader.isDone);
        loader.allowSceneActivation = true;
        unloader.allowSceneActivation = true;
        onGameStarted?.Invoke();
        Destroy(transform.parent.gameObject);
    }

}
