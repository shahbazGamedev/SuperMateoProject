using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenuFunctions : MonoBehaviour 
{
	public void mpShowLevelSelectionMenu()
	{
		SceneManager.LoadScene("menuSelection");
	}
}
