using System;
using con = PhotoshopTypeLibrary.PSConstants;
using PSX = Photoshop6OM.PhotoshopProxy;

namespace Photoshop6OM
{


	public class SelectionAPI : ExtensionAPI
	{

		private static void __SetSelection( int item_to_select  )
		{

			// Ref1 
			var Ref1 = PSX.MakeNewReference();

			Ref1.PutProperty(  (int) con.phClassChannel ,  1718838636 ); 

			// Desc1 
			var Desc1 = PSX.MakeNewDescriptor();

			Desc1.PutReference(  (int) con.phKeyNull ,  Ref1 ); 
			Desc1.PutEnumerated(  (int) con.phKeyTo ,  (int) con.phTypeOrdinal ,  item_to_select ); 

			// Play the event in photoshop
			PSX.PlayEvent( (int) con.phEventSet , Desc1 , (int)con.phDialogSilent, PSX.PlayBehavior.checkresult );
		}

		public static void SelectAll( )
		{
			__SetSelection( (int) con.phEnumAll  );
		}

		public static void SelectNone( )
		{
			__SetSelection( (int) con.phEnumNone  );
		}

		public static void SelectPrevious( )
		{
			__SetSelection( (int) con.phEnumPrevious  );
		}

		public static void InvertSelection( )
		{
			var Desc1 = PSX.MakeNewDescriptor();

			// Play the event in photoshop
			PSX.PlayEvent( (int) con.phEventInverse , Desc1 , (int)con.phDialogSilent, PSX.PlayBehavior.checkresult );
		}

		public static void SelectRectangle( int left, int top, int right, int bottom, bool anti_alias )
		{
			__SelectShape( left, top, right, bottom, (int) con.phClassRectangle, anti_alias,(int) con.phEventSet  );
		}

		public static void SelectEllipse( int left, int top, int right, int bottom, bool anti_alias)
		{
			__SelectShape( left, top, right, bottom, (int) con.phClassEllipse, anti_alias, (int) con.phEventSet  );
		}

		public static void AddRectangle( int left, int top, int right, int bottom, bool anti_alias )
		{
			__SelectShape( left, top, right, bottom, (int) con.phClassRectangle, anti_alias,(int) con.phEventAddTo );
		}

		public static void AddEllipse( int left, int top, int right, int bottom, bool anti_alias)
		{
			__SelectShape( left, top, right, bottom, (int) con.phClassEllipse, anti_alias, (int) con.phEventAddTo);
		}

		public static void SubtractRectangle( int left, int top, int right, int bottom, bool anti_alias )
		{
			__SelectShape( left, top, right, bottom, (int) con.phClassRectangle, anti_alias,(int) con.phEventSubtractFrom );
		}

		public static void SubtractEllipse( int left, int top, int right, int bottom, bool anti_alias)
		{
			__SelectShape( left, top, right, bottom, (int) con.phClassEllipse, anti_alias, (int) con.phEventSubtractFrom);
		}

		
		private static void __SelectShape( int left, int top, int right, int bottom, int shape, bool anti_alias, int action_event )
		{

			// WORKITEM: Support con.phEventIntersectWith as the action_event

			// Desc2 
			var Desc2 = PSX.CreateDescriptorForRectangle( left, top, right, bottom );
			
			// Ref1 
			var Ref1 = PSX.MakeNewReference();

			Ref1.PutProperty(  (int) con.phClassChannel ,  1718838636 ); 

			// Desc1 
			var Desc1 = PSX.MakeNewDescriptor();

			Desc1.PutReference(  (int) con.phKeyNull ,  Ref1 ); 
			Desc1.PutObject(  (int) con.phKeyTo ,  shape ,  Desc2 ); 
			Desc1.PutBoolean(  (int) con.phKeyAntiAlias ,  PSX.int_from_bool(anti_alias) ); 

			// Play the event in photoshop
			PSX.PlayEvent( action_event , Desc1 , (int)con.phDialogSilent, PSX.PlayBehavior.checkresult );
		}

		public static void SelectShapeEx( System.Drawing.Rectangle r, int shape, bool anti_alias, int action_event )
		{

			// WORKITEM: Support con.phEventIntersectWith as the action_event

			// Desc2 
			var Desc2 = PSX.CreateDescriptorForRectangle( r );
			
			// Ref1 
			var Ref1 = PSX.MakeNewReference();

			Ref1.PutProperty(  (int) con.phClassChannel ,  1718838636 ); 

			// Desc1 
			var Desc1 = PSX.MakeNewDescriptor();

			Desc1.PutReference(  (int) con.phKeyNull ,  Ref1 ); 
			Desc1.PutObject(  (int) con.phKeyTo ,  shape ,  Desc2 ); 
			Desc1.PutBoolean(  (int) con.phKeyAntiAlias ,  PSX.int_from_bool(anti_alias) ); 

			// Play the event in photoshop
			PSX.PlayEvent( action_event , Desc1 , (int)con.phDialogSilent, PSX.PlayBehavior.checkresult );
		}

	}
}
