using System;
using con = PhotoshopTypeLibrary.PSConstants;
using PSX = Photoshop6OM.PhotoshopProxy;

namespace Photoshop6OM
{


	public class LayerAPI : ExtensionAPI
	{


		public static void CreateLayer( string layer_name )
		{

			PSX.CheckStringContents( layer_name );

			// Desc2 
			var Desc2 = PSX.MakeNewDescriptor();

			Desc2.PutString(  (int) con.phKeyName ,  layer_name ); 

			// Ref1 
			var Ref1 = PSX.MakeNewReference();

			Ref1.PutClass(  (int) con.phClassLayer ); 

			// Desc1 
			var Desc1 = PSX.MakeNewDescriptor();

			Desc1.PutReference(  (int) con.phKeyNull ,  Ref1 ); 
			Desc1.PutBoolean(  PSX.StrToID( "below" )  ,  PSX.FALSE ); 
			Desc1.PutObject(  (int) con.phKeyUsing ,  (int) con.phClassLayer ,  Desc2 ); 

			int old_layer_count = LayerAPI.GetLayerCount( -1 );
			// Play the event in photoshop
			PSX.PlayEvent( (int) con.phEventMake , Desc1 , (int)con.phDialogSilent, PSX.PlayBehavior.checkresult );

			LayerAPI.CheckLayerCount( -1, old_layer_count + 1);

		}

		public static int GetLayerCount( int doc_index  )
		{
			///
			/// <summary>
			///		Returns the number of documents open
			/// </summary>
			///
			
			return (int) PSX.get_value_from_object( (int) con.phClassDocument, doc_index, (int) con.phKeyNumberOfLayers );
		}

		public static void CheckLayerCount( int doc_index, int expected_count )
		{
			///
			/// <summary>
			/// Raises an exception of the document count is not what is expected
			///	Workitem: Move some of this checking into PSX
			/// </summary>
			///

			int actual_layer_count = LayerAPI.GetLayerCount( doc_index );
			if ( actual_layer_count != expected_count )
			{
				string msg = string.Format( "Expected layer count of {0}, got {1} instead" , actual_layer_count , expected_count );
				throw ( new Photoshop6OM.PhotoshoProxyError(msg));
			}
		}


		
		public static void SelectLayer( )
		{

			// Ref1 
			var Ref1 = PSX.MakeNewReference();

			Ref1.PutEnumerated(  (int) con.phClassChannel ,  (int) con.phTypeChannel ,  (int) con.phEnumMask ); 

			// Desc1 
			var Desc1 = PSX.MakeNewDescriptor();

			Desc1.PutReference(  (int) con.phKeyNull ,  Ref1 ); 

			// Play the event in photoshop
			PSX.PlayEvent( (int) con.phEventSelect , Desc1 , (int)con.phDialogSilent, PSX.PlayBehavior.checknone );
		}
		
		public static void RasterizeLayer( int layer_index )
		{


			// Ref1 
			var Ref1 = LayerAPI.GetReferenceToLayerByIndex( layer_index );

			// Desc1 
			var Desc1 = PSX.MakeNewDescriptor();

			Desc1.PutReference(  (int) con.phKeyNull ,  Ref1 ); 

			// Play the event in photoshop
			PSX.PlayEvent( PSX.StrToID( "rasterizeLayer" ) , Desc1 , (int)con.phDialogSilent, PSX.PlayBehavior.checkresult );
		}


		static public void FlattenImage()
		{

			//PhotoshopTypeLibrary.IActionDescriptor Desc1 = PSX.MakeNewDescriptor();
			PhotoshopTypeLibrary.IActionDescriptor Desc1 = null;
			// Play the event in photoshop
			PSX.PlayEvent( (int) con.phEventFlattenImage , Desc1 , (int)con.phDialogSilent, PSX.PlayBehavior.checkresult );

			LayerAPI.CheckLayerCount( -1, 0 );
		}




		public static void CreateSolidColorFillLayer( string layer_name, int red, int green, int blue)
		{


			// Desc4 
			var Desc4 = PSX.CreateDescriptorForRGBColor( red, green, blue );

			// Desc3 
			var Desc3 = PSX.MakeNewDescriptor();

			Desc3.PutObject(  (int) con.phKeyColor ,  (int) con.phClassRGBColor ,  Desc4 ); 

			// Desc2 
			var Desc2 = PSX.MakeNewDescriptor();

			Desc2.PutString(  (int) con.phKeyName ,  layer_name ); 
			Desc2.PutObject(  (int) con.phKeyType ,  PSX.StrToID( "solidColorLayer" )  ,  Desc3 ); 

			// Ref1 
			var Ref1 = PSX.MakeNewReference();

			Ref1.PutClass(  PSX.StrToID( "contentLayer" )  ); 

			// Desc1 
			var Desc1 = PSX.MakeNewDescriptor();

			Desc1.PutReference(  (int) con.phKeyNull ,  Ref1 ); 
			Desc1.PutObject(  (int) con.phKeyUsing ,  PSX.StrToID( "contentLayer" )  ,  Desc2 ); 

			// Play the event in photoshop
			PSX.PlayEvent( (int) con.phEventMake , Desc1 , (int)con.phDialogSilent, PSX.PlayBehavior.checkresult );
		}

		static public PhotoshopTypeLibrary.IActionDescriptor CreateOpacityStop( double percent, int location, int midpoint  )
		{
			var OpacityDesc = PSX.MakeNewDescriptor();
			OpacityDesc.PutUnitDouble(  (int) con.phKeyOpacity ,  (int) con.phUnitPercent ,  percent ); 
			OpacityDesc.PutInteger(  (int) con.phKeyLocation ,  location ); 
			OpacityDesc.PutInteger(  (int) con.phKeyMidpoint ,  midpoint ); 
			return OpacityDesc;
		}

		static public  PhotoshopTypeLibrary.IActionDescriptor CreateColorStop( System.Drawing.Color c, int location, int midpoint  )
		{
			var Desc5 = PSX.MakeNewDescriptor();
			Desc5.PutObject(  (int) con.phKeyColor ,  (int) con.phClassRGBColor ,  PSX.CreateDescriptorForRGBColor(c)); 
			Desc5.PutEnumerated(  (int) con.phKeyType ,  (int) con.phTypeColorStopType ,  (int) con.phEnumUserStop ); 
			Desc5.PutInteger(  (int) con.phKeyLocation ,  location ); 
			Desc5.PutInteger(  (int) con.phKeyMidpoint ,  midpoint); 
			return Desc5;
		}


		public static PhotoshopTypeLibrary.IActionDescriptor [] CreateOpacityStops( double [] OA )
		{
			const int max_location=4096;
			const int min_location=0;
			System.Diagnostics.Debug.Assert( OA.Length > 1 );

			var opacity_stops = new PhotoshopTypeLibrary.IActionDescriptor [ OA.Length ]; 
			int count=0;
			int step = max_location/(OA.Length-1);
			foreach ( double O in OA )
			{
				System.Diagnostics.Debug.Assert( O <= 100);
				System.Diagnostics.Debug.Assert( O >= 0 );
				
				int location = max_location - (step * count);
				System.Diagnostics.Debug.Assert( location <=max_location  );
				System.Diagnostics.Debug.Assert( location >=min_location  );

				const int midpoint = 50;
				opacity_stops[count]=LayerAPI.CreateOpacityStop( O , location , midpoint);
				count ++;

			}
			return opacity_stops;
		}

		public static PhotoshopTypeLibrary.IActionDescriptor [] CreateColorStops( System.Drawing.Color [] CA )
		{
			const int max_location=4096;
			const int min_location=0;
			System.Diagnostics.Debug.Assert( CA.Length > 1 );

			var color_stops = new PhotoshopTypeLibrary.IActionDescriptor [ CA.Length ]; 
			int count=0;
			int step = max_location/(CA.Length-1);
			foreach ( System.Drawing.Color C in CA )
			{
				int location = max_location - (step * count);
				System.Diagnostics.Debug.Assert( location <=max_location  );
				System.Diagnostics.Debug.Assert( location >=min_location  );

				const int midpoint = 50;
				color_stops[count]=LayerAPI.CreateColorStop( C , location , midpoint);
				count ++;

			}
			return color_stops;
		}

		static public void CreateGradientFillLayer( string layer_name, int gradient_type, double angle, PhotoshopTypeLibrary.IActionDescriptor [] opacity_stops,  PhotoshopTypeLibrary.IActionDescriptor [] color_stops)
		{

			PSX.CheckRange( angle , 0, 360 ) ;
			PSX.CheckEnum( gradient_type, (int) con.phEnumLinear , (int) con.phEnumRadial ) ;
			string gradient_name = "Custom Gradient" ;

			// List2 
			var List2 = PSX.MakeNewList();
			PSX.AddDescriptorsToList( List2, opacity_stops , (int) con.phClassTransparencyStop );

			// List1 
			var List1 = PSX.MakeNewList();
			PSX.AddDescriptorsToList( List1, color_stops, (int) con.phClassColorStop );

			// Desc4 
			var Desc4 = PSX.MakeNewDescriptor();


			Desc4.PutString(  (int) con.phKeyName ,  gradient_name); 
			Desc4.PutEnumerated(  1198679110 ,  (int) con.phTypeGradientForm ,  (int) con.phEnumCustomStops ); 
			Desc4.PutDouble(  (int) con.phKeyInterfaceIconFrameDimmed ,  4096 ); 
			Desc4.PutList(  (int) con.phKeyColors ,  List1 ); 
			Desc4.PutList(  (int) con.phKeyTransparency ,  List2 ); 

			// Desc3 
			var Desc3 = PSX.MakeNewDescriptor();


			Desc3.PutUnitDouble(  (int) con.phKeyAngle ,  (int) con.phUnitAngle ,  angle); 
			Desc3.PutEnumerated(  (int) con.phKeyType ,  (int) con.phTypeGradientType ,  gradient_type ); 
			Desc3.PutObject(  (int) con.phKeyGradient ,  (int) con.phClassGradient ,  Desc4 ); 

			// Desc2 
			var Desc2 = PSX.MakeNewDescriptor();


			Desc2.PutString(  (int) con.phKeyName ,  layer_name ); 
			Desc2.PutObject(  (int) con.phKeyType ,  PSX.StrToID( "gradientLayer" )  ,  Desc3 ); 

			// Ref1 
			var Ref1 = PSX.MakeNewReference();

			Ref1.PutClass(  PSX.StrToID( "contentLayer" )  ); 

			// Desc1 
			var Desc1 = PSX.MakeNewDescriptor();

			Desc1.PutReference(  (int) con.phKeyNull ,  Ref1 ); 
			Desc1.PutObject(  (int) con.phKeyUsing ,  PSX.StrToID( "contentLayer" )  ,  Desc2 ); 

			// Play the event in photoshop
			PSX.PlayEvent( (int) con.phEventMake , Desc1 , (int)con.phDialogSilent, PSX.PlayBehavior.checkresult );
		}


		public static void CreateLayerFromBackground( string layer_name, double opacity_percent , int blend_mode)
		{


			// Desc2 
			var Desc2 = PSX.MakeNewDescriptor();

			Desc2.PutString(  (int) con.phKeyName ,  layer_name ); 
			Desc2.PutUnitDouble(  (int) con.phKeyOpacity ,  (int) con.phUnitPercent ,  opacity_percent); 
			Desc2.PutEnumerated(  (int) con.phKeyMode ,  (int) con.phTypeBlendMode ,  blend_mode ); 

			// Ref1 
			var Ref1 = PSX.MakeNewReference();

			Ref1.PutProperty(  (int) con.phClassLayer ,  (int) con.phKeyBackground ); 

			// Desc1 
			var Desc1 = PSX.MakeNewDescriptor();

			Desc1.PutReference(  (int) con.phKeyNull ,  Ref1 ); 
			Desc1.PutObject(  (int) con.phKeyTo ,  (int) con.phClassLayer ,  Desc2 ); 

			// Play the event in photoshop
			PSX.PlayEvent( (int) con.phEventSet , Desc1 , (int)con.phDialogSilent, PSX.PlayBehavior.checkresult );
		}

		// A list of all the document properties for easy eumeration

		public static PhotoshopTypeLibrary.IActionReference GetReferenceToLayerByIndex( int layer_index )
		{
			PhotoshopTypeLibrary.IActionReference Ref1 = PSX.get_reference_to_object( (int) con.phClassLayer , layer_index );
			return Ref1;
		}
		

		public static void DeleteLayer( int layer_index )
		{

			// Ref1 
			PhotoshopTypeLibrary.IActionReference Ref1 = LayerAPI.GetReferenceToLayerByIndex( layer_index );
			
			// Desc1 
			PhotoshopTypeLibrary.IActionDescriptor Desc1 = PSX.MakeNewDescriptor();

			Desc1.PutReference(  (int) con.phKeyNull ,  Ref1 ); 

			// Play the event in photoshop
			PSX.PlayEvent( (int) con.phEventDelete , Desc1 , (int)con.phDialogSilent, PSX.PlayBehavior.checkresult );
		}


		public static void DuplicateLayer( int layer_index, string new_name )
		{

			// Ref1 
			PhotoshopTypeLibrary.IActionReference Ref1 = LayerAPI.GetReferenceToLayerByIndex( layer_index );

			// Desc1 
			PhotoshopTypeLibrary.IActionDescriptor Desc1 = PSX.MakeNewDescriptor();

			Desc1.PutReference(  (int) con.phKeyNull ,  Ref1 ); 
			Desc1.PutString(  (int) con.phKeyName ,  new_name ); 

			// Play the event in photoshop
			PSX.PlayEvent( (int) con.phEventDuplicate , Desc1 , (int)con.phDialogSilent, PSX.PlayBehavior.checkresult );
		}



		public static int [] LayerProperties = {	};


	}
}
