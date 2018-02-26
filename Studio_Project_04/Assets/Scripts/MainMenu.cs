using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	[SerializeField]
	Image FadeIn;

	Color FadeIn_Color;

	bool Btn_Pressed = false;

	string SceneName;

	void Start()
	{
		FadeIn_Color.a = 1;
	}

	void Update()
	{
		FadeIn.GetComponent<Image> ().color = FadeIn_Color;

		if (Btn_Pressed == true) {
			FadeIn_Color.a += 0.05f;
		} else {
			FadeIn_Color.a -= 0.05f;
		}

		if (Btn_Pressed == false && FadeIn_Color.a <= 0) {
			FadeIn_Color.a = 0;
		}

		if (FadeIn_Color.a >= 1 && Btn_Pressed == true) {
			SceneManager.LoadScene (SceneName);
		}
	}

	public void Pressed(string Works)
	{
		Btn_Pressed = true;
		SceneName = Works;
	}

	public void Exit()
	{
		Application.Quit ();
	}
}
