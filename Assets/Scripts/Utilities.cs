using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Utilities : MonoBehaviour
{
    // This class is to easily reference, both via script and via editor, methods that are used by multiple objects
    public static Utilities instance;

	private void Awake()
	{
		if(instance == null) instance = this;
		else
		{
            Destroy(gameObject);
		}
	}

	#region PUBLIC_METHODS

	public void TogglePopUp(GameObject obj)
    {
        bool status = obj.activeSelf;

        if (status)
        {
            StartCoroutine(ClosePopUp(obj));
        }
        else
        {
            StartCoroutine(OpenPopUp(obj));
        }
    }

    public void OpenPopUpCor(GameObject obj)
    {
        StartCoroutine(OpenPopUp(obj));
    }

    public void ClosePopUpCor(GameObject obj)
    {
        StartCoroutine(ClosePopUp(obj));
    }
    #endregion

    #region PRIVATE_METHODS

    private IEnumerator OpenPopUp(GameObject popUp)
    {
        popUp.SetActive(true);
        // We scale it to vector.one quickly and then apply the tweening "PUNCH" so it creates a nice visual "bounce" effect
        popUp.transform.localScale = Vector3.zero;
        popUp.transform.DOScale(1f, .25f).SetEase(Ease.InOutSine);
        yield return new WaitForSeconds(.25f);
        popUp.transform.DOPunchScale(new Vector3(.1f, .1f, 0f), 1f, 5, 1f);
    }

    private IEnumerator ClosePopUp(GameObject popUp)
    {
        // Scale to zero after bubble's duration and enable interaction for next coroutine
        popUp.transform.DOScale(0f, 1f).SetEase(Ease.InOutSine);
        yield return new WaitForSeconds(1f);
        popUp.SetActive(false);
    }

    #endregion

}
