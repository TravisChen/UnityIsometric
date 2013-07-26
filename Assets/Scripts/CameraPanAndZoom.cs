using UnityEngine;
using System.Collections;

public class CameraPanAndZoom : MonoBehaviour 
{
	public float panSpeed = 1.0f;
	public float MIN_PAN_X = 0.0f;
	public float MAX_PAN_X = 10.0f;
	public float MIN_PAN_Y = 0.0f;
	public float MAX_PAN_Y = 7.0f;
		
    public float zoomSpeed = 1.0f;
    public Camera selectedCamera;
    public float MIN_SIZE = 2.0F;
    public float MAX_SIZE = 5.0F;
    public float minPinchSpeed = 5.0F;
    public float varianceInDistances = 5.0F;
    private float touchDelta = 0.0F;
    private Vector2 prevDist = new Vector2(0,0);
    private Vector2 curDist = new Vector2(0,0);
    private float speedTouch0 = 0.0F;
    private float speedTouch1 = 0.0F;
 
    // Use this for initialization
	void Start () 
	{
	
	}
 
	// Update is called once per frame
	void Update () 
	{
		if( Input.touchCount == 1 )
		{
			PanUpdate();
		}
		
		if( Input.touchCount == 2 )
		{
			ZoomUpdate();
		}
	}
	
	void PanUpdate()
	{
		if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved) 
		{
			Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
			selectedCamera.transform.Translate(	-touchDeltaPosition.x * panSpeed, -touchDeltaPosition.y * panSpeed, 0);
			Vector3 clampPosition = new Vector3( 	Mathf.Clamp( selectedCamera.transform.position.x, MIN_PAN_X, MAX_PAN_X ),
													Mathf.Clamp( selectedCamera.transform.position.y, MIN_PAN_Y, MAX_PAN_Y ),
													selectedCamera.transform.position.z );
			selectedCamera.transform.position = clampPosition;
		}
	}
	
	void ZoomUpdate() 
	{
		if (Input.touchCount == 2 && Input.GetTouch(0).phase == TouchPhase.Moved && Input.GetTouch(1).phase == TouchPhase.Moved) 
		{
			curDist = Input.GetTouch(0).position - Input.GetTouch(1).position;
			prevDist = ((Input.GetTouch(0).position - Input.GetTouch(0).deltaPosition) - (Input.GetTouch(1).position - Input.GetTouch(1).deltaPosition)); 
			touchDelta = curDist.magnitude - prevDist.magnitude;
			speedTouch0 = Input.GetTouch(0).deltaPosition.magnitude / Input.GetTouch(0).deltaTime;
			speedTouch1 = Input.GetTouch(1).deltaPosition.magnitude / Input.GetTouch(1).deltaTime;
			
			if ((touchDelta + varianceInDistances <= 1) && (speedTouch0 > minPinchSpeed) && (speedTouch1 > minPinchSpeed))
			{	
				selectedCamera.orthographicSize = Mathf.Clamp(selectedCamera.orthographicSize + (1.0f * zoomSpeed), MIN_SIZE, MAX_SIZE);
			}
				
			if ((touchDelta + varianceInDistances > 1) && (speedTouch0 > minPinchSpeed) && (speedTouch1 > minPinchSpeed))
			{
				selectedCamera.orthographicSize = Mathf.Clamp(selectedCamera.orthographicSize - (1.0f * zoomSpeed), MIN_SIZE, MAX_SIZE);
			}
		}    
	}
 
}