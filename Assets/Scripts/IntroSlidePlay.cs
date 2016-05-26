using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class IntroSlidePlay : MonoBehaviour 
{
	private	SlideshowTextManager aSlideManager;

	public	GameObject	aTextboxCenter;
	public	GameObject	aTextboxBottom;
	public	GameObject	aImageSlide;
	private	Image		aSlide;

	private	bool		aUsingBottomBox;

	//image collection
	public	Sprite[]	aStoryImages;

	//total images
	private	int		aImageCount;
	[SerializeField]
	private	int		aCurrentImage;

	//interpolation function attributes
	private	float	aTransitionSpeed;

	//cache variables
	private	Color	aCachedColor;

	void Start()
	{
		aUsingBottomBox	=	false;

		aSlideManager	=	GetComponent<SlideshowTextManager>();
		aSlide			=	aImageSlide.GetComponent<Image>();

		aSlideManager.mpSetTextbox(aTextboxCenter);

		//array cache and index definition
		aCurrentImage 		= 	0;
		aImageCount			= 	aStoryImages.Length;

		//Calculated values for interpolation values
		aTransitionSpeed	=	2.5f;

	}

	void Update()
	{
		if (!aUsingBottomBox)
		{
			if (aSlideManager.aCurrentLine >= 4)
			{
				aUsingBottomBox	=	true;
				StartCoroutine(mcFadeIn());
				aSlideManager.mpSetTextbox(aTextboxBottom);
			}
		}
	}

	IEnumerator mcFadeIn()
	{
		aSlide.sprite	=	aStoryImages[aCurrentImage];

		aCachedColor	=	aSlide.color;

		while (aCachedColor.a < 1.0f)
		{
			aCachedColor.a	=	Utilities.mfApproach(1.0f, aCachedColor.a, aTransitionSpeed * Time.deltaTime);
			aSlide.color	=	aCachedColor;
			yield return null;
		}
	}

	IEnumerator mcFadeNext()
	{
		aCachedColor	= aSlide.color;

		while (aCachedColor.a > 0.0f)
		{
			aCachedColor.a	=	Utilities.mfApproach(0.0f, aCachedColor.a, aTransitionSpeed * Time.deltaTime);
			aSlide.color	=	aCachedColor;
			yield return null;
		}

		aSlide.sprite	=	aStoryImages[aCurrentImage];

		while (aCachedColor.a < 1.0f)
		{
			aCachedColor.a		=	Utilities.mfApproach(1.0f, aCachedColor.a, aTransitionSpeed * Time.deltaTime);
			aSlide.color	=	aCachedColor;
			yield return null;
		}
	}

	public void mpGetNextImage()
	{
		if (aUsingBottomBox)
		{
			aCurrentImage	=	++aCurrentImage % aImageCount;
			StartCoroutine(mcFadeNext());
		}
	}

	public void mpFadeOut()
	{
		StopAllCoroutines();
		UnityEngine.SceneManagement.SceneManager.LoadScene("Scenes/levelTutorial");
	}
}
