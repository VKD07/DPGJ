using System.Collections;
using UnityEngine;

public class DisableTimer : MonoBehaviour
{
    [SerializeField] private float _timeToDisable = 3;
    private void OnEnable()
    {
        StartCoroutine(DisableTimerCoroutine());
    }

    private IEnumerator DisableTimerCoroutine()
    {
        yield return new WaitForSeconds(_timeToDisable);
        gameObject.SetActive(false);
    }
}
