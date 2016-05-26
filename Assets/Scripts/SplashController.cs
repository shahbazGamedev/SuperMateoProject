using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SplashController : MonoBehaviour 
{
	private	MenuManager		aMenuManager;

	//image collection
	public	GameObject[]	aSplashImages;

	//total images
	private	int		aImageCount;
	private	int		aCurrentImage;

	//interpolation function attributes
	private	float	aElapsedTime;
	private	float	aTransitionSpeed;

	private	float	aInterpolationCoeff;

	//cache variables
	private	Color	aCachedColor;
	private	Image	aCachedImage;

	void Start()
	{
		aMenuManager		=	transform.parent.GetComponent<MenuManager>();

		//array cache and index definition
		aCurrentImage 		= 	0;
		aImageCount			= 	aSplashImages.Length;

		//Calculated values for interpolation values
		aTransitionSpeed	=	0.35f;
		aInterpolationCoeff	=	100.0f/21.0f;

		mpGetNextImage();
	}

	void Update()
	{
		//cache current image color
		aCachedColor		= aCachedImage.color;
			//interpolate alpha value for fading effect
			aCachedColor.a		= aInterpolationCoeff * aElapsedTime * (1 - aElapsedTime);
		//set current color
		aCachedImage.color	= aCachedColor;

		aElapsedTime	+=	aTransitionSpeed * Time.deltaTime;

		//reset function whenever time reaches 1.0 => Domain of f(x) = AXe[0, 1]
		if (aElapsedTime >= 1.0f)
		{
			aCurrentImage++;
			mpGetNextImage();
		}
	}

	void mpGetNextImage()
	{
		if (aCurrentImage < aImageCount - 1)
		{
			//reset interpolation function
			aElapsedTime = 0.0f;
		}
		//check if we are displaying the last image
		else if (aCurrentImage == aImageCount - 1)
		{
			//next image will be a black background, fadeout!
			aElapsedTime		=	0.5f;
			aTransitionSpeed	*=	2.0f;
		}
		else
		{
			//no more images left, destroy the splash canvas
			aMenuManager.enabled	=	true;
			Destroy(gameObject);
			return;
		}

		//get next image in array
		aCachedImage = aSplashImages[aCurrentImage].GetComponent<Image>();
	}
}
