using System.Collections;
using UnityEngine;

public class SmokeBomb : MonoBehaviour
{
    private float _smokeBombScale = 8f;
    private float _smokeBombMaxTime = 7f;

    [SerializeField]
    GameObject _smokeObject;

    public bool IsActive { get; private set; } = false;

    private void Start()
    {
        _smokeObject.SetActive(false);
    }

    public void Throw()
    {
        if (IsActive) return;
        StartCoroutine(SmokeAnimation());
    }

    private IEnumerator SmokeAnimation()
    {
        _smokeObject.SetActive(true);
        IsActive = true;
        _smokeObject.transform.localScale = Vector3.one * _smokeBombScale;
        yield return new WaitForSeconds(_smokeBombMaxTime);
        IsActive = false;
        _smokeObject.SetActive(false);
        yield return null;
    }
}
