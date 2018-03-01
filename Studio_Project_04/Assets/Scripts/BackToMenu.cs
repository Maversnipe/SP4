using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BackToMenu : MonoBehaviour {

	public Image Ref;

	private Color Temp;

	private bool Pressed;

	// Use this for initialization
	void Start () {
		Temp = Ref.color;

		Temp.a = 1;
	}
	
	// Update is called once per frame
	void Update () {
		Ref.color = Temp;
		if (Pressed == true) {
			if (Temp.a < 1) {
				Temp.a += 0.05f;
			} else {
				SceneManager.LoadScene ("SceneMenu");
			}
		} else {
			if (Temp.a > 0) {
				Temp.a -= 0.05f;
			}
		}
	}

	public void GoBackToMenu()
	{
		Pressed = true;
	}

	public void ChangeSceneWin()
	{
		// If win KILL ALL ENEMIES, go back to scene open
		if(PlayerManager.Instance.GetCurrQuest () == 1)
		{
			SceneManager.LoadScene ("SceneOpen");

		}
		else if(PlayerManager.Instance.GetCurrQuest () == 2)
		{
			// If win ProtectThePresident, go back to main menu
			PlayerManager.Instance.SetCurrQuest (0);
			Pressed = true;
		}
	}

	public void ChangeSceneLose()
	{
		if(PlayerManager.Instance.GetCurrQuest () == 1)
		{
			SceneManager.LoadScene ("SceneOpen");
		}
		else if(PlayerManager.Instance.GetCurrQuest () == 2)
		{
			SceneManager.LoadScene ("SceneOpen");
		}
	}
}
