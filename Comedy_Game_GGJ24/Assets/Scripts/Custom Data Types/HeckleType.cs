using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeckleType 
{
	public string heckle_Str;
	public int inputPattern_Length;	
	public int repeats = 0;
	public float responseTimer;

	public HeckleType()	{ }
	public HeckleType(string heckleStr, int patternLength, int repeats, float respondTimer)
	{
		heckle_Str			= heckleStr;
		inputPattern_Length = patternLength;
		this.repeats		= repeats;
		responseTimer		= respondTimer;
	}
}
