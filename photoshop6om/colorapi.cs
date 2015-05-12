using System;
using con = PhotoshopTypeLibrary.PSConstants;
using PSX = Photoshop6OM.PhotoshopProxy;

namespace Photoshop6OM
{


	public class ColorAPI : ExtensionAPI
	{
		public static PhotoshopTypeLibrary.IActionDescriptor  CreateRGBColorDescriptor( System.Drawing.Color c1)
		{
			return ColorAPI.CreateRGBColorDescriptor( c1.R, c1.G, c1.B );
		}


		public static PhotoshopTypeLibrary.IActionDescriptor  CreateRGBColorDescriptor(int red, int green, int blue)
		{

			// Desc2 
			PhotoshopTypeLibrary.IActionDescriptor Desc2 = PSX.MakeNewDescriptor();

			Desc2.PutDouble(  (int) con.phKeyRed ,  red ); 
			Desc2.PutDouble(  (int) con.phKeyGrain ,  green ); 
			Desc2.PutDouble(  (int) con.phKeyBlue ,  blue ); 

			return Desc2;
		}

		public static void SetForegroundColor( int red, int green, int blue)
		{

			// Desc2 
			PhotoshopTypeLibrary.IActionDescriptor Desc2 = ColorAPI.CreateRGBColorDescriptor( red, green, blue );
			
			ColorAPI.__SetAppColor( (int) con.phKeyForegroundColor, Desc2 );
		}

		public static void SetForegroundColor( System.Drawing.Color c1 )
		{

			// Desc2 
			PhotoshopTypeLibrary.IActionDescriptor Desc2 = ColorAPI.CreateRGBColorDescriptor( c1 );
			
			__SetAppColor( (int) con.phKeyForegroundColor, Desc2  );
		}

		public static void SetBackgroundColor( System.Drawing.Color c1 )
		{

			// Desc2 
			var Desc2 = ColorAPI.CreateRGBColorDescriptor( c1 );
			
			ColorAPI.__SetAppColor( (int) con.phKeyBackgroundColor , Desc2 );
		}


		public static void __SetAppColor( int color_id, PhotoshopTypeLibrary.IActionDescriptor RGBCOLORDESC )
		{
			PSX.CheckEnum( color_id, (int) con.phKeyForegroundColor, (int) con.phKeyBackgroundColor );

			// Ref1 
			var Ref1 = PSX.MakeNewReference();

			Ref1.PutProperty(  (int) con.phClassColor ,  color_id ); 

			// Desc1 
			var Desc1 = PSX.MakeNewDescriptor();

			Desc1.PutReference(  (int) con.phKeyNull ,  Ref1 ); 
			Desc1.PutObject(  (int) con.phKeyTo ,  (int) con.phClassRGBColor ,  RGBCOLORDESC ); 

			// Play the event in photoshop
			PSX.PlayEvent( (int) con.phEventSet , Desc1 , (int)con.phDialogSilent, PSX.PlayBehavior.checknone );
		}
		public static void SetColorEx( int color_id, System.Drawing.Color c) 
		{

			double [] v = {c.R ,c.G , c.B , 0};
			SetColorEx( color_id, (int) con.phClassRGBColor , v); 

		}
		public static void SetColorEx( int color_id, int color_class, double [] v ) 
		{
				
			var Desc2 = PSX.MakeNewDescriptor();
				
			if ( color_class == (int) con.phClassHSBColor )
			{	
				Desc2.PutUnitDouble( (int) con.phKeyHue , (int) con.phUnitAngle , v[0] );
				Desc2.PutDouble( (int) con.phKeySaturation , v[1]  );
				Desc2.PutDouble( (int) con.phKeyBrightness , v[2]   );
			}
			else if ( color_class== (int) con.phClassRGBColor )
			{
				Desc2.PutDouble( (int) con.phKeyRed , v[0] );
				Desc2.PutDouble( (int) con.phKeyGrain , v[1]   );
				Desc2.PutDouble( (int) con.phKeyBlue , v[2]  );
			}
			else if ( color_class==(int)con.phClassLabColor )
			{
				Desc2.PutDouble( (int) con.phKeyLuminance , v[0] );
				Desc2.PutDouble( (int) con.phKeyA , v[1] );
				Desc2.PutDouble( (int) con.phKeyB , v[2] );		
			}
			else if ( color_class== (int)con.phClassCMYKColor )
			{
				Desc2.PutDouble( (int) con.phKeyCyan , v[0] );
				Desc2.PutDouble( (int) con.phKeyMagenta , v[1]  );
				Desc2.PutDouble( (int) con.phKeyYellow , v[2]  );		
				Desc2.PutDouble( (int) con.phKeyBlack , v[3]  );		
			}
			else
			{
				var e = new Photoshop6OM.PhotoshoProxyError( "Improper Color Class" );
				throw e;
			}
							
			var Ref1 = PSX.MakeNewReference();
			Ref1.PutProperty( (int) con.phClassColor , color_id );
							
			var Desc1 = PSX.MakeNewDescriptor();
			Desc1.PutReference( (int)con.phKeyNull , Ref1 );
			Desc1.PutObject( (int) con.phKeyTo , color_class , Desc2 );
					
					
			// ----------------------------------------
			// Play the event
			// ----------------------------------------
			PSX.PlayEvent( (int) con.phEventSet , Desc1 , (int) con.phDialogSilent , PSX.PlayBehavior.checkresult );
					
		}

		public static void GetColorEx( int color_id , out int color_class_id, ref double[] v)
		{
			
			PhotoshopTypeLibrary.IActionDescriptor desc,desc2;
			PSX.CheckEnum( color_id, (int) con.phKeyForegroundColor, (int) con.phKeyBackgroundColor );

			desc = PSX.get_descriptor_to_object_property_by_index( (int) con.phClassApplication , -1, color_id );

			int actual_color_class_id;
			desc.GetObject( (int) con.phKeyForegroundColor, out actual_color_class_id, out desc2);

			

			if ( actual_color_class_id == (int) con.phClassRGBColor )
			{
				color_class_id = (int) con.phClassRGBColor ;
				v[0] = (double) PSX.get_value_from_descriptor( desc2, (int) con.phKeyRed );
				v[1] = (double) PSX.get_value_from_descriptor( desc2, (int) con.phKeyGreen );
				v[2] = (double) PSX.get_value_from_descriptor( desc2, (int) con.phKeyBlue );
				v[3] =0;
			}
			else
			{
				string msg = string.Format( "Did not understand color type returned" );
				var e = new Photoshop6OM.PhotoshoProxyError ( msg );
				throw e;
			}
			
		}

		public static void GetColorEx( int color_id , out int color_class_id, out System.Drawing.Color c)
		{
			
			double [] v = new double[4];
			ColorAPI.GetColorEx( (int) con.phKeyForegroundColor, out color_class_id, ref v);
			c = System.Drawing.Color.FromArgb( (int) v[0], (int) v[1], (int) v[2]);
		
		}

	
	
	}


}
