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
    Text playText;
    bool loadGame, loadLobby;

    // Use this for initialization
    void Awake()
    {
        timer = GameObject.Find("Game State Timer").GetComponent<GameStateTimer>();
        signInButton = GameObject.Find("Sign In Button").GetComponent<Button>();
        userImage = GameObject.Find("User Image").GetComponent<RawImage>();
        greetingText = GameObject.Find("Greeting Text").GetComponent<Text>();
        userNameText = GameObject.Find("User Name Text").GetComponent<Text>();
        playText = GameObject.Find("Play Text").GetComponent<Text>();
        
        if (!UserManager.Instance.Initialized)
        {
            UserManager.Instance.Initialize();
        }
    }

    void Start()
    {
        timer.Playing = false;
    }

    public void Play()
    {
        timer.Playing = true;
        string level = timer.State.ToString();
        Application.LoadLevel(level);
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

        if (NetworkingManager.Instance.Connected)
        {
            playText.text = "Play";
        }
    }
}
