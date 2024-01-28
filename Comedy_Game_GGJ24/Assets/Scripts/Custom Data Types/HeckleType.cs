using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeckleType 
{
	public string heckle_Str, hexID;
	public int inputPattern_Length;	
	public int repeats = 0;
	public float responseTimer;

	public bool alwaysNew_Gen;

	public HeckleType()	{ }
	public HeckleType(string heckleStr, int patternLength, int repeats, float respondTimer, bool genNew)
	{
		heckle_Str			= heckleStr;
		inputPattern_Length = patternLength;
		this.repeats		= repeats;
		responseTimer		= respondTimer;
		alwaysNew_Gen		= genNew;
	}
}
