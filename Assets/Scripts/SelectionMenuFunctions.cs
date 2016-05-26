using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SelectionMenuFunctions : MonoBehaviour 
{
	public void mpLoadIntro()
	{
		SceneManager.LoadScene("Scenes/levelIntro");
	}
	public void mpLoadLevel01()
	{
		print("level 01 loading " + Time.time);
	}
	public void mpLoadLevel02()
	{
		print("level 02 loading " + Time.time);
	}
}
