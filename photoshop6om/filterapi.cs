using System;
using con = PhotoshopTypeLibrary.PSConstants;
using PSX = Photoshop6OM.PhotoshopProxy;

namespace Photoshop6OM
{


	public class FilterAPI : ExtensionAPI
	{

		public static void GaussianBlur( double radius)
		{
			PSX.CheckRange( radius, 0, 250 );

			// Desc1 
			var Desc1 = PSX.MakeNewDescriptor();

			Desc1.PutUnitDouble(  (int) con.phKeyRadius ,  (int) con.phUnitPixels ,  radius ); 

			// Play the event in photoshop
			PSX.PlayEvent( (int) con.phEventGaussianBlur , Desc1 , (int)con.phDialogSilent, PSX.PlayBehavior.checkresult );
		}

		public static void AddNoise( int distortion, double amount, int monochromatic )
		{

			PSX.CheckEnum( distortion, (int) con.phEnumUniformDistribution , (int) con.phEnumGaussianDistribution );
			PSX.CheckRange( amount, 0 , 400 );
			PSX.CheckEnum( monochromatic , PSX.TRUE , PSX.FALSE);
			// Desc1 
			var Desc1 = PSX.MakeNewDescriptor();

			Desc1.PutEnumerated(  (int) con.phKeyDistortion ,  (int) con.phTypeDistribution , distortion ); 
			Desc1.PutUnitDouble(  (int) con.phKeyNoise ,  (int) con.phUnitPercent ,  amount ); 
			Desc1.PutBoolean(  (int) con.phKeyMonochromatic ,  monochromatic ); 

			// Play the event in photoshop
			PSX.PlayEvent( (int) con.phEventAddNoise , Desc1 , (int)con.phDialogSilent, PSX.PlayBehavior.checkresult );
		}

		public static void UnsharpMask( double amount, double radius, int threshold )
		{

			PSX.CheckRange( amount, 0 , 400 );
			PSX.CheckRange( radius, 0 , 250 );
			PSX.CheckRange( threshold , 0 , 255);

			// Desc1 
			var Desc1 = PSX.MakeNewDescriptor();

			Desc1.PutUnitDouble(  (int) con.phKeyAmount ,  (int) con.phUnitPercent ,  amount ); 
			Desc1.PutUnitDouble(  (int) con.phKeyRadius ,  (int) con.phUnitPixels ,  radius ); 
			Desc1.PutInteger(  (int) con.phKeyThreshold ,  threshold ); 

			// Play the event in photoshop
			PSX.PlayEvent( (int) con.phEventUnsharpMask , Desc1 , (int)con.phDialogSilent, PSX.PlayBehavior.checkresult );
		}




	}


}
