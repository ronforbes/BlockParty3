using UnityEngine;
using System.Collections;

public class BoardRaiserController : MonoBehaviour
{
    float previousClickTime;
    const float doubleClickWindow = 0.5f;
    BoardRaiser raiser;

    // Use this for initialization
    void Start()
    {
        raiser = GetComponent<BoardRaiser>();
    }
	
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Time.time - previousClickTime <= doubleClickWindow)
            {
                raiser.ForceRaise();
            }

            previousClickTime = Time.time;
        }
    }
}
