using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuController : MonoBehaviour
{
    Button loginButton;
    RawImage userImage;
    Text welcomeText;
    Text userNameText;

    // Use this for initialization
    void Start()
    {
        loginButton = GameObject.Find("Login Button").GetComponent<Button>();
        userImage = GameObject.Find("User Image").GetComponent<RawImage>();
        welcomeText = GameObject.Find("Welcome Text").GetComponent<Text>();
        userNameText = GameObject.Find("User Name Text").GetComponent<Text>();

        if (!UserManager.Instance.Initialized)
        {
            UserManager.Instance.Initialize();
        }
    }
	
    public void Play()
    {
        Application.LoadLevel("Game");
    }

    public void Login()
    {
        UserManager.Instance.Login();
    }

    // Update is called once per frame
    void Update()
    {
        if (UserManager.Instance.LoggedIn)
        {
            loginButton.gameObject.SetActive(false);

            userImage.gameObject.SetActive(true);
            userImage.texture = UserManager.Instance.Picture;

            welcomeText.gameObject.SetActive(true);

            userNameText.gameObject.SetActive(true);
            userNameText.text = UserManager.Instance.Name;
        } else
        {
            loginButton.gameObject.SetActive(true);
            userImage.gameObject.SetActive(false);
            welcomeText.gameObject.SetActive(false);
            userNameText.gameObject.SetActive(false);
        }
    }
}
