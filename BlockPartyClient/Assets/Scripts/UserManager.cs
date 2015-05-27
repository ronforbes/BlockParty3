using UnityEngine;
using UnityEngine.UI;
using Facebook.MiniJSON;
using System.Collections.Generic;
using System.Collections;

public class UserManager : MonoBehaviour
{
    static UserManager instance;
    public static UserManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<UserManager>();

                // tell unity not to destroy this object when loading a new scene
                DontDestroyOnLoad(instance.gameObject);
            }

            return instance;
        }
    }

    bool initialized;
    bool signedIn;
    public string Name = "Guest";
    public string FacebookId;
    public Texture2D Picture;
    
    public bool Initialized
    {
        get
        {
            return initialized;
        }
    }
    
    public bool SignedIn
    {
        get
        {
            return signedIn;
        }
    }

    void Awake()
    {
        if (instance == null)
        {
            // if i'm the first instance, make me the singleton
            instance = this;
            DontDestroyOnLoad(this);
        } else
        {
            // if a singleton already exists and you find another reference in scene, destroy it
            if (this != instance)
            {
                Destroy(this.gameObject);
            }
        }
    }

    public void Initialize()
    {
        if (!Initialized)
        {
            FB.Init(OnInitComplete);
        }
    }
    
    void OnInitComplete()
    {
        initialized = true;
    }
    
    public void SignIn()
    {
        if (!SignedIn)
        {
            FB.Login("public_profile", OnSignIn);
        }
    }
    
    void OnSignIn(FBResult result)
    {
        signedIn = true;

        // get the user's name
        FB.API("/me", Facebook.HttpMethod.GET, OnGetMe);

        // get the user's profile picture
        StartCoroutine("LoadProfilePicture", Picture);
    }
    
    void OnGetMe(FBResult result)
    {
        var dictionary = Json.Deserialize(result.Text) as Dictionary<string, object>;
        Name = dictionary ["name"] as string;
        FacebookId = dictionary ["id"] as string;
    }
    
    IEnumerator LoadProfilePicture()
    {
        WWW www = new WWW("https://graph.facebook.com/me/picture?access_token=" + FB.AccessToken);
        yield return www;
        Picture = new Texture2D(128, 128, TextureFormat.DXT1, false);
        www.LoadImageIntoTexture(Picture);
    }
}
