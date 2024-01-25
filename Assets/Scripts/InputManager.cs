using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using Unity.XR.CoreUtils;
using UnityEngine.EventSystems;
using UnityEngine.XR.Interaction.Toolkit.AR;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UI;
using UnityEngine.InputSystem.HID;

public class InputManager : ARBaseGestureInteractable
{
    [SerializeField] private Camera arCam;
    [SerializeField] private ARRaycastManager _raycastManager;
    [SerializeField] private GameObject crosshair;
    [SerializeField] private Text totalPriceText;

    private List<ARRaycastHit> _hits = new List<ARRaycastHit>();

    private Touch touch;
    private Pose pose;
    private GameObject selectedObj;
    private double totalPrice = 0.0;

    // Start is called before the first frame update
    void Start()
    {
        totalPriceText.text = "RM 0.00";
    }

    protected override bool CanStartManipulationForGesture(TapGesture gesture)
    {
        if (gesture.targetObject == null)
            return true;

        return false;
    }

    protected override void OnEndManipulation(TapGesture gesture)
    {
        if (gesture.isCanceled)
            return;

        if (gesture.targetObject != null && IsPointerOverUI(gesture))
            return;

        //place object in the position of crosshair
        if (GestureTransformationUtility.Raycast(gesture.startPosition, _hits, xrOrigin, TrackableType.PlaneWithinPolygon))
        {
            GameObject placedObj = Instantiate(DataHandler.Instance.GetFurniture(), pose.position, pose.rotation);
            var anchorObject = new GameObject("PlacementAnchor");
            anchorObject.transform.position = pose.position;
            anchorObject.transform.rotation = pose.rotation;
            placedObj.transform.parent = anchorObject.transform;


            
            totalPrice += DataHandler.Instance.GetPrice();
        }

    }


    // Update is called once per frame
    void FixedUpdate()
    {
        CrosshairCalculation();

        //check for selected object
        IdentifyObjectSelection();
        totalPriceText.text = "RM " + totalPrice.ToString("N2");

    }

    bool IsPointerOverUI(TapGesture touch)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = new Vector2(touch.startPosition.x, touch.startPosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        return results.Count > 0;
    }

    void CrosshairCalculation()
    {
        //center location
        Vector3 origin = arCam.ViewportToScreenPoint(new Vector3(0.5f, 0.5f, 0));

        //spawn object when the screen is touched
        if (GestureTransformationUtility.Raycast(origin, _hits, xrOrigin, TrackableType.PlaneWithinPolygon))
        {
            pose = _hits[0].pose;
            crosshair.transform.position = pose.position;
            crosshair.transform.eulerAngles = new Vector3(90, 0, 0);
        }
    }

    public void DestroyObject()
    {
        
        Destroy(selectedObj.transform.parent.gameObject);
        totalPrice -= DataHandler.Instance.GetPrice();


    }

    void IdentifyObjectSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                BoxCollider bc = hit.collider as BoxCollider;

                if (bc != null)
                {
                    selectedObj = bc.gameObject;
                }
                else
                {
                    selectedObj = null;
                }

            }

        }
    }
}