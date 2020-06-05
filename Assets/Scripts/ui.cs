using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class ui : MonoBehaviour
{
    public GameObject game;
    public GameObject settings;
    public GameObject multiplayer;
    public GameObject buttons;
    public GameObject ball_pref;
    public GameObject ball_cont;
    public Text best_score;
    public Dropdown color_dd;
    public Button return_menu;
    public platform main_platform = null;
    public TestNetworkManager nm = null;
    public InputField host_address = null;
    private int current_score = 0;
    private GameObject current_ball = null;

    void Start()
    {
        game.SetActive(false);
        settings.SetActive(false);
        multiplayer.SetActive(false);
        return_menu.gameObject.SetActive(false);
        buttons.SetActive(true);

        nm.canvas = this;
        host_address.text = nm.networkAddress;

        color_dd.value = PlayerPrefs.GetInt("ball_color");

        updateBestResultText();
    }

    public void onExitClick() {
        Application.Quit();
    }

    public void onStartClick(bool is_local_game = true) {
        showPlatforms();

        main_platform.can_control = true;
        main_platform.is_local_game = is_local_game;
        main_platform.gameObject.SetActive(true);

        return_menu.gameObject.SetActive(true);

        current_score = 0;

        game.SetActive(true);
        buttons.SetActive(false);

        if (is_local_game) {
            createBall();
        }
    }

    public void createBall() {
        GameObject ball = Instantiate(ball_pref, new Vector3(0, 0, 0), Quaternion.identity, ball_cont.transform);
        ball bs = ball.GetComponent<ball>();
        bs.canvas = this;

        current_ball = ball;
    }

    public GameObject GetBall() {
        return current_ball;
    }

    public void onSettingsClick() {
        settings.SetActive(true);
        buttons.SetActive(false);
    }

    public void onSettingsBackClick() {
        settings.SetActive(false);
        buttons.SetActive(true);
    }

    public void onColorChangeValue(int val) {
        PlayerPrefs.SetInt("ball_color", color_dd.value);
    }

    public void addScore(int count = 1) {
        current_score += count;

        int best_score = PlayerPrefs.GetInt("best_score");
        if (current_score > best_score) {
            PlayerPrefs.SetInt("best_score", current_score);

            updateBestResultText();
        }
    }

    private void updateBestResultText() {
        this.best_score.text = "Лучший результат: " + PlayerPrefs.GetInt("best_score").ToString();
    }

    public void onReturnToMenu() {
        if (main_platform.gameObject.activeSelf) {
            main_platform.reInit();
            return_menu.gameObject.SetActive(false);
            Destroy(current_ball);
            buttons.SetActive(true);
            game.SetActive(false);
        } else {
            if (nm.IsClientConnected()) {
                nm.StopClient();
            } else {
                nm.StopHost();
            }

            return_menu.gameObject.SetActive(false);
            Destroy(current_ball);
            buttons.SetActive(true);
            game.SetActive(false);
        }
    }

    public void onMultiplayer() {
        multiplayer.SetActive(true);
        buttons.SetActive(false);
    }

    public void onMultiplayerBack() {
        multiplayer.SetActive(false);
        buttons.SetActive(true);
    }

    public void onMultiplayerStartHost() {
        multiplayer.SetActive(false);
        nm.StartHost();
        onStartClick(false);
        hidePlatforms();
    }

    public void onMultiplayerStartClient() {
        multiplayer.SetActive(false);
        nm.networkAddress = host_address.text;
        nm.StartClient();
        onStartClick(false);
        hidePlatforms();
    }

    private void hidePlatforms() {
        main_platform.gameObject.SetActive(false);
        main_platform.dependent_platform.SetActive(false);
    }

    private void showPlatforms() {
        main_platform.gameObject.SetActive(true);
        main_platform.dependent_platform.SetActive(true);
    }
}
