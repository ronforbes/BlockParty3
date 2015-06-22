using UnityEngine;
using System.Collections;

public class BackgroundScaler : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
		SpriteRenderer renderer = GetComponent<SpriteRenderer> ();

		if (renderer == null)
			return;
			
		transform.localScale = new Vector3 (1, 1, 1);
			
		float width = renderer.sprite.bounds.size.x;
		float height = renderer.sprite.bounds.size.y;
						
		float worldScreenHeight = Camera.main.orthographicSize * 2f;
		float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;
			
		Vector3 xWidth = transform.localScale;
		xWidth.x = worldScreenWidth / width;
		transform.localScale = xWidth;

		Vector3 yHeight = transform.localScale;
		yHeight.y = worldScreenHeight / height;
		transform.localScale = yHeight;
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}
