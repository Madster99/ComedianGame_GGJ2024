using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionPattern_Controller : MonoBehaviour
{
	public enum ActionType		{ LEFT, RIGHT, UP, DOWN };
	public enum GenerateType	{ APPEND, REFRESH }

	[SerializeField] int step_AMT = 2, curr_Length = 0, max_Length = 12;
	[SerializeField] Stack<ActionType> action_Pattern, input_Pattern;
	[SerializeField] GenerateType patternGenType = GenerateType.REFRESH;
    // Start is called before the first frame update
    void Start()
    {
		action_Pattern = GeneratePattern();
		ResetPlayerInput();
	}

	// Update is called once per frame
	void Update()
	{
		//possibly put this in another manager
		if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			CheckInput(ActionType.LEFT);
		}
		if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			CheckInput(ActionType.RIGHT);
		}
		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			CheckInput(ActionType.UP);
		}
		if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			CheckInput(ActionType.DOWN);
		}
	}

	//checks the input against the stack; if we pass we add it to the player's input
	//if we fail we trigger a strike against the player
	public bool CheckInput(ActionType in_Action)
	{
		bool successFind = false;
		try
		{		
			if (action_Pattern.TryPeek(out ActionType a))
			{
				if (in_Action == a)
				{
					input_Pattern.Push(action_Pattern.Pop()); //success!
					successFind = true;
				}
			}

			if (!successFind)
				throw new Exception();
		}
		catch (Exception e)		{
			//failure, give the strike!
			return successFind;
		}
		
		//once we've drained the action stack, we create the new pattern, as an extension of the old pattern
		if (action_Pattern.Count <= 0)
		{
			int newLength = (input_Pattern.Count + step_AMT);
			newLength = Math.Clamp(newLength, 1, max_Length);

			switch (patternGenType)
			{
				case GenerateType.APPEND:
					action_Pattern = GeneratePattern(newLength, input_Pattern);
					break;
				case GenerateType.REFRESH:
				default:
					action_Pattern = GeneratePattern(newLength);
					break;
			}

			ResetPlayerInput();
		}

		return successFind; //this result will always be true due to the catch above
	}

	private Stack<ActionType> GeneratePattern(int length = 2, Stack<ActionType> inputList = null)
	{
		Stack<ActionType> gen_Pattern = new Stack<ActionType>();
		List<ActionType> newPattern = new List<ActionType>();

		if (inputList != null)
		{
			newPattern = new List<ActionType>(inputList.ToArray());
		}

		//if we're using the previous pattern, append onto the end
		if (newPattern.Count > 0)
		{			
			while(newPattern.Count < length)
			{
				int newActionID = UnityEngine.Random.Range(0, 3); //for each type of direction input

				ActionType action = ActionType.UP;
				try { action = (ActionType)Enum.ToObject(typeof(ActionType), newActionID); }
				catch (Exception e) { continue; }
				newPattern.Add(action);
			}

			//so the stack is popped in the order the list is sequenced, cause it comes in in reverse consumed order
			newPattern.Reverse(); 

			foreach (ActionType a in newPattern)
				gen_Pattern.Push(a);
		}
		//else, create a whole new pattern
		else
		{
			while (gen_Pattern.Count < length)
			{
				int newActionID = UnityEngine.Random.Range(0, 3); //for each type of direction input

				ActionType action = ActionType.UP;
				try					{	action = (ActionType)Enum.ToObject(typeof(ActionType), newActionID);	}
				catch (Exception e) {	action = ActionType.UP; }

				gen_Pattern.Push(action);
			}
		}

		curr_Length = gen_Pattern.Count; //update the length we're at
		return gen_Pattern;
	}

	void ResetPlayerInput()
	{
		//reset the input path for new level
		input_Pattern = new Stack<ActionType>();
	}
}
