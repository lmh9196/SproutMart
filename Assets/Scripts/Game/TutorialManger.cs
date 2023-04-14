using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManger : MonoBehaviour
{
    public static TutorialManger instance = null;
    public WorldGuideArrow playerGuideArrow;

    private void Awake()
    {
        if (instance == null) { instance = this; }

        DontDestroyOnLoad(gameObject);
    }
    public void ActiveTargetNoticeArrow() { playerGuideArrow.gameObject.SetActive(true); }
    public void DiableTargetNoticeArrow() 
    {
        playerGuideArrow.ChaseOrder(false);
        playerGuideArrow.gameObject.SetActive(false);
    }
    public void ArrowTargetRotate(Transform target)
    {
        Vector3 dir = (playerGuideArrow.transform.position - target.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        playerGuideArrow.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
    }

    public void LockTargetPos(Transform transform)
    {
        Vector3 transformPos = Camera.main.WorldToViewportPoint(transform.position);

        transformPos.x = Mathf.Clamp(transformPos.x, 0.1f, 0.9f);
        transformPos.y = Mathf.Clamp(transformPos.y, 0.1f, 0.9f);

        transform.transform.position = Camera.main.ViewportToWorldPoint(transformPos);
    }
}
