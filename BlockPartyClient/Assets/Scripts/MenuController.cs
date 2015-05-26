using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using BlockPartyShared;

public class MenuController : MonoBehaviour
{
    GameStateTimer timer;
    Button signInButton;
    RawImage userImage;
    Text greetingText;
    Text userNameText;
    bool loadGame, loadLobby;

    // Use this for initialization
    void Awake()
    {
        timer = GameObject.Find("Game State Timer").GetComponent<GameStateTimer>();
        signInButton = GameObject.Find("Sign In Button").GetComponent<Button>();
        userImage = GameObject.Find("User Image").GetComponent<RawImage>();
        greetingText = GameObject.Find("Greeting Text").GetComponent<Text>();
        userNameText = GameObject.Find("User Name Text").GetComponent<Text>();
        
        if (!UserManager.Instance.Initialized)
        {
            UserManager.Instance.Initialize();
        }
    }

    public void Play()
    {
        timer.Playing = true;
        if (timer.State == GameStateTimer.GameState.Lobby)
        {
            Application.LoadLevel("Lobby");
        }
        if (timer.State == GameStateTimer.GameState.Game)
        {
            Application.LoadLevel("Game");
        }
    }

    public void SignIn()
    {
        UserManager.Instance.SignIn();
    }

    // Update is called once per frame
    void Update()
    {
        if (UserManager.Instance.SignedIn)
        {
            signInButton.gameObject.SetActive(false);
            userImage.gameObject.SetActive(true);
            userImage.texture = UserManager.Instance.Picture;
            greetingText.text = "Welcome back";
            userNameText.text = UserManager.Instance.Name;
        } else
        {
            signInButton.gameObject.SetActive(true);
            userImage.gameObject.SetActive(false);
            greetingText.text = "Hello";
            userNameText.text = "Guest";
        }
    }
}
