﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class StringParamSet
{
	public Dictionary<string, int> IntParam;
	public Dictionary<string, float> FloatParam;
	public Dictionary<string, double> DoubleParam;
	public Dictionary<string, bool> BoolParam;
	public Dictionary<string, Vector2> V2Param;
	public Dictionary<string, Vector3> V3Param;

	public void DumpParam()
	{
		var builder = new StringBuilder();

		if( IntParam != null )
		{
			builder.Append( "Int Param\n" );

			foreach( var p in IntParam )
			{
				builder.AppendFormat( "{0} : {1}\n", p.Key, p.Value );
			}

			builder.Append( "\n" );
		}

		if( FloatParam != null )
		{
			builder.Append( "Float Param\n" );

			foreach( var p in FloatParam )
			{
				builder.AppendFormat( "{0} : {1}\n", p.Key, p.Value );
			}

			builder.Append( "\n" );
		}

		if( DoubleParam != null )
		{
			builder.Append( "Double Param\n" );

			foreach( var p in DoubleParam )
			{
				builder.AppendFormat( "{0} : {1}\n", p.Key, p.Value );
			}

			builder.Append( "\n" );
		}

		if( BoolParam != null )
		{
			builder.Append( "Bool Param\n" );

			foreach( var p in BoolParam )
			{
				builder.AppendFormat( "{0} : {1}\n", p.Key, p.Value );
			}

			builder.Append( "\n" );
		}

		if( V2Param != null )
		{
			builder.Append( "Vector2 Param\n" );

			foreach( var p in V2Param )
			{
				builder.AppendFormat( "{0} : {1}\n", p.Key, p.Value );
			}

			builder.Append( "\n" );
		}

		if( V3Param != null )
		{
			builder.Append( "Vector3 Param\n" );

			foreach( var p in V3Param )
			{
				builder.AppendFormat( "{0} : {1}\n", p.Key, p.Value );
			}

			builder.Append( "\n" );
		}

		Debug.Log( builder );
	}
}
