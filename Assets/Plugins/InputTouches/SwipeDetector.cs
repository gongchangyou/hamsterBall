﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SwipeDetector : MonoBehaviour {

	private enum _SwipeState{None, Start, Swiping, End}

	private List<int> fingerIndex=new List<int>();
	private List<int> mouseIndex=new List<int>();
	
	
	public float maxSwipeDuration=0.25f;
	public float minSpeed=150;
	public float minDistance=15;
	public float maxDirectionChange=35;
	public bool onlyFireWhenLiftFinger=false;

	public bool IsDispSwipeFailureCause { get; set; }

	// Use this for initialization
	void Start () {
		IsDispSwipeFailureCause = false;
	}
	
	// Update is called once per frame
	void Update () {
		
		if(Input.touchCount>0){
			//foreach(Touch touch in Input.touches){
			for(int i=0; i<Input.touches.Length; i++){
				Touch touch=Input.touches[i];
				if(fingerIndex.Count==0 || !fingerIndex.Contains(touch.fingerId)){
					StartCoroutine(TouchSwipeRoutine(touch.fingerId));
				}
			}
		}
		else if(Input.touchCount==0){
			if(Input.GetMouseButtonDown(0)){
				if(!mouseIndex.Contains(0)) StartCoroutine(MouseSwipeRoutine(0)); 
			}
			else if(Input.GetMouseButtonUp(0)){
				if(mouseIndex.Contains(0)) mouseIndex.Remove(0); 
			}
			
			if(Input.GetMouseButtonDown(1)){
				if(!mouseIndex.Contains(1)) StartCoroutine(MouseSwipeRoutine(1)); 
			}
			else if(Input.GetMouseButtonUp(1)){
				if(mouseIndex.Contains(1)) mouseIndex.Remove(1); 
			}
		}
		
	}
	
	
	IEnumerator MouseSwipeRoutine(int index){
		//GameMessage.DisplayMessage("swipe routine started");
		mouseIndex.Add(index);
		
		float timeStartSwipe=Time.realtimeSinceStartup;
		Vector2 startPos;
		Vector2 initVector=Vector2.zero;
		Vector2 lastPos=Vector2.zero;
		_SwipeState swipeState=_SwipeState.None;
		
		lastPos=Input.mousePosition;
		startPos=lastPos;
		
		yield return null;
		
		while(mouseIndex.Contains(index)){
			
			Vector2 curPos=Input.mousePosition;
			Vector2 curVector=curPos-lastPos;
			
			float mag=curVector.magnitude;
			
			if(swipeState==_SwipeState.None && mag>0){
				timeStartSwipe=Time.realtimeSinceStartup;
				startPos=curPos;
				swipeState=_SwipeState.Swiping;
				initVector=curVector;
				
				SwipeStart(startPos, curPos, timeStartSwipe, index, true);
			}
			else if(swipeState==_SwipeState.Swiping){
				if(mag>0){
					Swiping(startPos, curPos, timeStartSwipe, index, true);
					
					if(curPos.x<0 || curPos.x>Screen.width || curPos.y<0 || curPos.y>Screen.height){
						if (IsDispSwipeFailureCause)
						{
							GameMessage.DisplayMessage("out of bound");
						}
						swipeState=_SwipeState.None;
						SwipeEnd(startPos, curPos, timeStartSwipe, index, true);
					}
					if(Time.realtimeSinceStartup-timeStartSwipe>maxSwipeDuration){
						if (IsDispSwipeFailureCause)
						{
							GameMessage.DisplayMessage("duration due");
						}
						swipeState=_SwipeState.None;
						SwipeEnd(startPos, curPos, timeStartSwipe, index, true);
					}
					//check angle
					if(Mathf.Abs(Vector2.Angle(initVector, curVector))>maxDirectionChange){
						if (IsDispSwipeFailureCause)
						{
							GameMessage.DisplayMessage("angle too wide");
						}
						swipeState=_SwipeState.None;
						SwipeEnd(startPos, curPos, timeStartSwipe, index, true);
					}
					//check speed
					if(Mathf.Abs((curPos-startPos).magnitude/(Time.realtimeSinceStartup-timeStartSwipe))<minSpeed){
						if (IsDispSwipeFailureCause)
						{
							GameMessage.DisplayMessage("too slow");
						}
						swipeState=_SwipeState.None;
						SwipeEnd(startPos, curPos, timeStartSwipe, index, true);
					}
				}
			}
			
			lastPos=curPos;
			
			yield return null;
		}
		
		if(swipeState==_SwipeState.Swiping){
			swipeState=_SwipeState.None;
			SwipeEnd(startPos, lastPos, timeStartSwipe, index, true);
		}
		
		//GameMessage.DisplayMessage("swipe routine ended");
	}
	
	
	IEnumerator TouchSwipeRoutine(int index){
		//GameMessage.DisplayMessage("swipe routine started");
		fingerIndex.Add(index);
		
		float timeStartSwipe=Time.realtimeSinceStartup;
		Vector2 startPos;
		Vector2 initVector=Vector2.zero;
		Vector2 lastPos=Vector2.zero;
		_SwipeState swipeState=_SwipeState.None;
		
		lastPos=Gesture.GetTouch(index).position;
		startPos=lastPos;
		
		yield return null;
		
		while(Input.touchCount>0){
			Touch touch=Gesture.GetTouch(index);
			
			if(touch.position==Vector2.zero) break;
			
			Vector2 curPos=touch.position;
			Vector2 curVector=curPos-lastPos;
			
			float mag=curVector.magnitude;
			
			if(swipeState==_SwipeState.None && mag>0){
				timeStartSwipe=Time.realtimeSinceStartup;
				startPos=curPos;
				swipeState=_SwipeState.Swiping;
				initVector=curVector;
				
				SwipeStart(startPos, curPos, timeStartSwipe, index, false);
			}
			else if(swipeState==_SwipeState.Swiping){
				if(mag>0){
					Swiping(startPos, curPos, timeStartSwipe, index, false);
					
					if(curPos.x<0 || curPos.x>Screen.width || curPos.y<0 || curPos.y>Screen.height){
						if (IsDispSwipeFailureCause)
						{
							GameMessage.DisplayMessage("out of bound");
						}
						swipeState=_SwipeState.None;
						SwipeEnd(startPos, curPos, timeStartSwipe, index, true);
					}
					if(Time.realtimeSinceStartup-timeStartSwipe>maxSwipeDuration){
						if (IsDispSwipeFailureCause)
						{
							GameMessage.DisplayMessage("duration due");
						}
						swipeState=_SwipeState.None;
						SwipeEnd(startPos, curPos, timeStartSwipe, index);
					}
					//check angle
					float d = Mathf.Abs(Vector2.Angle(initVector, curVector));
					if (d > maxDirectionChange)
					{
						if (IsDispSwipeFailureCause)
						{
							//GameMessage.DisplayMessage("angle is too wide "+ initVector + "   " + curVector + "   " + d);
							GameMessage.DisplayMessage("angle is too wide " + d);
						}
						swipeState=_SwipeState.None;
						SwipeEnd(startPos, curPos, timeStartSwipe, index);
					}
					//check speed
					if(Mathf.Abs((curPos-startPos).magnitude/(Time.realtimeSinceStartup-timeStartSwipe))<minSpeed){
						if (IsDispSwipeFailureCause)
						{
							GameMessage.DisplayMessage("too slow");
						}
						swipeState=_SwipeState.None;
						SwipeEnd(startPos, curPos, timeStartSwipe, index);
					}
				}
			}
			
			lastPos=curPos;
			
			yield return null;
		}
		
		if(swipeState==_SwipeState.Swiping){
			swipeState=_SwipeState.None;
			SwipeEnd(startPos, lastPos, timeStartSwipe, index);
		}
		
		fingerIndex.Remove(index);
		//GameMessage.DisplayMessage("swipe routine ended");
	}
	
	
	void SwipeStart(Vector2 startPos, Vector2 endPos, float timeStartSwipe, int index, bool isMouse){
		Vector2 swipeDir=endPos-startPos;
		SwipeInfo sw=new SwipeInfo(startPos, endPos, swipeDir, timeStartSwipe, index, isMouse);
		Gesture.SwipeStart(sw);
	}
	
	void Swiping(Vector2 startPos, Vector2 endPos, float timeStartSwipe, int index, bool isMouse){
		Vector2 swipeDir=endPos-startPos;
		SwipeInfo sw=new SwipeInfo(startPos, endPos, swipeDir, timeStartSwipe, index, isMouse);
		Gesture.Swiping(sw);
	}
	
	
	
	void SwipeEnd(Vector2 startPos, Vector2 endPos, float timeStartSwipe, int index){
		SwipeEnd(startPos, endPos, timeStartSwipe, index, false);
	}
		
	void SwipeEnd(Vector2 startPos, Vector2 endPos, float timeStartSwipe, int index, bool isMouse){
		if(onlyFireWhenLiftFinger){
			if(!isMouse){
				for(int i=0; i<Input.touchCount; i++){
					Touch touch=Input.touches[i];
					if(touch.fingerId==index){
						return;
					}
				}
			}
			else{
				if(mouseIndex.Contains(index)) return;
				if(Time.realtimeSinceStartup-timeStartSwipe>maxSwipeDuration) return;
			}
		}
		
		Vector2 swipeDir=endPos-startPos;
		SwipeInfo sw=new SwipeInfo(startPos, endPos, swipeDir, timeStartSwipe, index, isMouse);
		
		Gesture.SwipeEnd(sw);
		
		if((swipeDir).magnitude<minDistance) {
			if (IsDispSwipeFailureCause)
			{
				GameMessage.DisplayMessage("too short");
			}
			return;
		}
		
		Gesture.Swipe(sw);
		
		//GameMessage.DisplayMessage("swiped");
	}
	
	
}
