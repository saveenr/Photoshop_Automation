using System;
using  con = PhotoshopTypeLibrary.PSConstants;


namespace commoncsX
{
	class libstring
	{
		public static string Join( System.Collections.ArrayList list )
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder( );
			int count=0;
			foreach ( Object o in list )
			{
				string os = o.ToString();
				sb.Append( os );
				if (count>0)
				{
					sb.Append(";");
				}
			}
			string retval = sb.ToString();
			return retval;
		}

		public static string Multiply( string unit, int length)
		{
			string s = "";
			for (int i=0;i<length;i++)
			{
				s+=unit;
			}
			return s;

		}
	
	}
}


