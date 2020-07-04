using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public GameObject loadingScreen,
					  menu;
	public Slider loadingSlider;
	public Text progressText;

	private static LevelManager instance;
	public static LevelManager Instance
	{
		get { return instance; }
	}
	
	void Awake()
	{
		instance = this;	
	}

	public void LoadScene (int sceneIndex)
	{
		Debug.Log (sceneIndex + " has been loaded");
		StartCoroutine (LoadAsync (sceneIndex));
	}

	public void Restart ()
	{
		StartCoroutine(LoadAsync(SceneManager.GetActiveScene().buildIndex));
	}

	IEnumerator LoadAsync (int sceneIndex)
	{
		AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

		loadingScreen.SetActive(true);
		progressText.gameObject.SetActive (true);
		menu.SetActive (false);

		while (!operation.isDone)
		{
			float progress = Mathf.Clamp01 (operation.progress / .9f);
			loadingSlider.value = progress;
			progressText.text = Mathf.Round(progress * 100f) + "%";
			yield return null;
		}
	}

	public void QuitGame ()
	{
		Debug.Log ("Quit Game");
		Application.Quit ();
	}

	public void InstantReturnToMenu ()
	{
		Debug.Log ("Returned to the main menu");
		SceneManager.LoadScene ("Main");
	}
}
