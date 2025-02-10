using System.Collections;
using UnityEngine;

public class AutoDisabler : MonoBehaviour
{
    [SerializeField] private float disableDelay;

    private void OnEnable()
    {
        StartCoroutine(DisableIt());
    }

    private IEnumerator DisableIt()
    {
        yield return new WaitForSeconds(disableDelay);
        gameObject.SetActive(false);
    }
}
