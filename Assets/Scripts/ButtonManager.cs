using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ButtonManager : MonoBehaviour
{
    [SerializeField] private RawImage buttonImage;

    private Button btn;
    private int itemID;
    private Sprite buttonTexture;

    public int ItemID
    {
        set { itemID = value; }
    }

    public Sprite ButtonTexture
    {
        set
        {
            buttonTexture = value;
            buttonImage.texture = buttonTexture.texture;
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(SelectObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (UIManager.Instance.OnEntered(gameObject))
        {
            transform.DOScale(Vector3.one * 2, 0.3f);
            SelectObject();
        }
        else
        {
            transform.DOScale(Vector3.one, 0.3f);
        }
    }

    void SelectObject()
    {
        DataHandler.Instance.SetFurniture(itemID);
        DataHandler.Instance.SetPrice(itemID);
    }

}
