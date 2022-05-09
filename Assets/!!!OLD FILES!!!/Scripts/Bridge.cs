using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour
{
    [SerializeField] private BoxCollider2D onHiddenCollider2D;

    [SerializeField] private GameObject onShowCollider2D;

    // Start is called before the first frame update
    public void OnShow()
    {
        if (onHiddenCollider2D != null)
        {
            onHiddenCollider2D.enabled = false;
        }

        if (onShowCollider2D != null)
        {
            onShowCollider2D.SetActive(true);
        }
    }

    public void OnHide()
    {
        if (onHiddenCollider2D != null)
        {
            onHiddenCollider2D.enabled = true;
        }

        if (onShowCollider2D != null)
        {
            onShowCollider2D.SetActive(false);
        }
    }
}