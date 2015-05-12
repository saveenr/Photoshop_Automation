using System;
using con = PhotoshopTypeLibrary.PSConstants;
using PSX = Photoshop6OM.PhotoshopProxy;

namespace Photoshop6OM
{


	public class DocumentAPI : ExtensionAPI
	{
		public static void CreateDocument(int pixelwidth, int pixelheight, int ppi, int fill, int mode, string name)
		{
			System.Diagnostics.Debug.Assert( pixelwidth > 0 );
			System.Diagnostics.Debug.Assert( pixelheight > 0 );
			System.Diagnostics.Debug.Assert( ppi > 0 );
			//PSX.CheckEnum( mode, (int) con.phClassRGBColorMode );
			PSX.CheckEnum( fill, (int) con.phEnumTransparent , (int)con.phEnumBackgroundColor , (int)con.phEnumWhite  );
			
			
			// Creates a new document
			
		
			// Create Desc2
			var Desc2 = PSX.MakeNewDescriptor();
			Desc2.PutString( (int) con.phKeyName, name );
			Desc2.PutClass( (int) con.phKeyMode, mode );
			Desc2.PutUnitDouble( (int)con.phKeyWidth, (int)con.phUnitDistance, pixelwidth  );
			Desc2.PutUnitDouble( (int)con.phKeyHeight, (int)con.phUnitDistance, pixelheight );
			Desc2.PutUnitDouble( (int)con.phKeyResolution, (int)con.phUnitDensity, ppi );
			Desc2.PutEnumerated( (int)con.phKeyFill, (int)con.phTypeFill, fill );

			// Create Desc1
			PhotoshopTypeLibrary.IActionDescriptor Desc1 = PSX.MakeNewDescriptor();
			Desc1.PutObject( (int)con.phKeyNew, (int)con.phClassDocument, Desc2 );


			int old_count = DocumentAPI.GetDocumentCount();

			// Play the Event Into Photoshop
			PSX.PlayEvent( (int)con.phEventMake,Desc1, (int)con.phDialogSilent, PSX.PlayBehavior.checknone  );

			DocumentAPI.CheckDocumentCount( old_count + 1);

		}

		public static void CloseAllDocuments()
		{
			int num_docs = DocumentAPI.GetDocumentCount() ;
			for ( int i =0; i< num_docs; i++ )
			{
				DocumentAPI.CloseDocument();

			}
			DocumentAPI.CheckDocumentCount( 0 );

		}

		public static void CloseDocument()
		{
			// NOTE: Those closes without prompting for save

			System.Diagnostics.Debug.Assert( DocumentAPI.GetDocumentCount() > 0 );

			// Desc1 
			var Desc1 = PSX.MakeNewDescriptor();

			Desc1.PutEnumerated(  (int) con.phKeySaving ,  (int) con.phTypeYesNo ,  (int) con.phEnumNo ); 

			int old_count = DocumentAPI.GetDocumentCount();

			// Play the event in photoshop
			PSX.PlayEvent( (int) con.phEventClose , Desc1 , (int)con.phDialogSilent, PSX.PlayBehavior.checknone );

			DocumentAPI.CheckDocumentCount( old_count - 1);
		}


		public static void DuplicateDocument( string new_name , bool merge_layers )
		{

			System.Diagnostics.Debug.Assert( new_name.Length > 0 );

			// Ref1 
			var Ref1 = PSX.MakeNewReference();

			Ref1.PutEnumerated(  (int) con.phClassDocument ,  (int) con.phTypeOrdinal ,  (int) con.phEnumFirst ); 

			// Desc1 
			var Desc1 = PSX.MakeNewDescriptor();

			Desc1.PutReference(  (int) con.phKeyNull ,  Ref1 ); 
			Desc1.PutString(  (int) con.phKeyName ,  new_name ); 
			
			if (merge_layers)
			{  
				Desc1.PutBoolean( (int) con.phKeyMerged , PSX.TRUE ); 
			}
			else				
			{ 
				Desc1.PutBoolean( (int) con.phKeyMerged , PSX.FALSE ); 
			}

			int old_count = DocumentAPI.GetDocumentCount();

			// Play the event in photoshop
			PSX.PlayEvent( (int) con.phEventDuplicate , Desc1 , (int)con.phDialogSilent, PSX.PlayBehavior.checkresult );

			DocumentAPI.CheckDocumentCount( old_count + 1);
		}



		public static int GetDocumentCount()
		{
			///
			/// <summary>
			///		Returns the number of documents open
			/// </summary>
			///
			
			return (int) PSX.get_value_from_object( (int)con.phClassApplication, -1, (int)con.phKeyNumberOfDocuments );
		}


		public static string GetTitle( int doc_index )
		{
			///
			/// <summary>
			///		Returns the title of document with given doc)index
			/// </summary>
			///
			return (string) PSX.get_value_from_object( (int)con.phClassDocument , doc_index, (int) con.phKeyTitle);
		}


		public static int GetDirtyFlag( int doc_index )
		{
			///
			/// <summary>
			///		Returns the title of document with given doc)index
			/// </summary>
			///
			return (int) PSX.get_value_from_object( (int)con.phClassDocument , doc_index, (int) con.phKeyIsDirty );
		}

		public static bool IsDocumentDirty( int doc_index )
		{
			return ( 0 != (int) PSX.get_value_from_object( (int)con.phClassDocument , doc_index, (int) con.phKeyIsDirty ) );
		}

		public static void OpenDocument( string filename )
		{

			if ( DocumentAPI.IsFileLoaded( filename ) )
			{
				string msg = string.Format( "The file \"{0}\" is already open." , filename );
				var e = new Photoshop6OM.PhotoshoProxyError ( msg );
				throw e;
			}

			PSX.CheckFileExists( filename );
			
			// Desc1 
			var Desc1 = PSX.MakeNewDescriptor();

			Desc1.PutPath(  (int) con.phKeyNull ,  filename ); 

			int old_count = DocumentAPI.GetDocumentCount();

			// Play the event in photoshop
			PSX.PlayEvent( (int) con.phEventOpen , Desc1 , (int)con.phDialogSilent, PSX.PlayBehavior.checkresult );

			DocumentAPI.CheckDocumentCount( old_count + 1);
		}


		public static void CheckDocumentCount( int expected_count )
		{
			///
			/// <summary>
			/// Raises an exception of the document count is not what is expected
			///	Workitem: Move some of this checking into PSX
			/// </summary>
			///

			int actual_document_count = DocumentAPI.GetDocumentCount( );
			if ( actual_document_count != expected_count )
			{
				string msg = string.Format( "Expected document count of {0}, got {1} instead" , actual_document_count , expected_count );
				throw ( new Photoshop6OM.PhotoshoProxyError(msg));
			}
		}

		public static System.Collections.ArrayList GetDocumentProperties( int doc_index, int [] prop_ids )
		{
			///
			/// <summary> 
			/// Returns the specified document properties
			/// 
			/// </summary>
			/// 



			System.Collections.ArrayList values;
			values = PSX.get_values_from_object( (int) con.phClassDocument , doc_index, prop_ids ) ;
			return values;
		}

		public static bool IsFileLoaded( string filename )
		{
			string fname1 = filename.ToUpper();
			int [] p = { (int) con.phKeyFileReference };
            foreach ( System.Collections.ArrayList props in PSX.query_object_properties( (int) con.phClassDocument , p ))
			{
				
				string fname2 = (string) props[0];
				fname2 = fname2.ToUpper();
				if (fname1.Equals(fname2))
				{
					return true;
				}
				
			}
			return false;
		}

		
		public static void eventSave()
		{
			var Desc1 = PSX.MakeNewDescriptor();

			// Play the event in photoshop
			PSX.PlayEvent( (int) con.phEventSave , Desc1 , (int)con.phDialogSilent, PSX.PlayBehavior.checknone );
		}

		public static void _start_save( string filename )
		{
			PSX.CheckValidFilename( filename );
		}

		public static void _end_save( string filename, PhotoshopTypeLibrary.IActionDescriptor Desc1 )
		{

			// Play the event in photoshop
			PSX.PlayEvent( (int) con.phEventSave , Desc1 , (int)con.phDialogSilent, PSX.PlayBehavior.checknone );
			PSX.CheckFileExists( filename );

		}

		public static void SaveAsPSD( string filename, bool save_as_copy  )
		{

			// Desc1 
			_start_save( filename );
			
			var Desc1 = PSX.MakeNewDescriptor();

			Desc1.PutClass(  (int) con.phKeyAs ,  (int) con.phClassPhotoshop35Format ); 
			Desc1.PutPath(  (int) con.phKeyIn ,  filename ); 
			if (save_as_copy)
			{
				Desc1.PutBoolean(  (int) con.phKeyCopy ,  PSX.TRUE ); 
				Desc1.PutBoolean(  (int) con.phKeyLayers ,  PSX.FALSE ); 
			}

			_end_save( filename, Desc1 );

		}


		public static void SaveCopyAsJPEG( string filename, int quality )
		{

			// WORKITEM support matte color

			_start_save( filename );

			PSX.CheckRange( quality, 0,12 );

			// Desc2 
			var Desc2 = PSX.MakeNewDescriptor();
			Desc2.PutInteger(  (int) con.phKeyExtendedQuality ,  quality ); 

			// Desc1 
			var Desc1 = PSX.MakeNewDescriptor();

			Desc1.PutObject(  (int) con.phKeyAs ,  (int) con.phClassJPEGFormat ,  Desc2 ); 
			// WORKITEM what if they only provided a path
			Desc1.PutPath(  (int) con.phKeyIn ,  filename ); 
			Desc1.PutBoolean(  (int) con.phKeyCopy ,  PSX.TRUE ); 

			_end_save( filename, Desc1 );
		}

		public static void SaveCopyAsPNG( string filename, int interlace )
		{
			_start_save( filename );
			PSX.CheckEnum( interlace, (int) con.phEnumPNGInterlaceNone , (int) con.phEnumPNGInterlaceAdam7 );

			// Desc2 
			var Desc2 = PSX.MakeNewDescriptor();

			Desc2.PutEnumerated(  (int) con.phKeyPNGInterlaceType ,  (int) con.phTypePNGInterlaceType ,  interlace ); 
			Desc2.PutEnumerated(  (int) con.phKeyPNGFilter ,  (int) con.phTypePNGFilter ,  (int) con.phEnumPNGFilterAdaptive ); 

			// Desc1 
			var Desc1 = PSX.MakeNewDescriptor();

			Desc1.PutObject(  (int) con.phKeyAs ,  (int) con.phClassPNGFormat ,  Desc2 ); 
			Desc1.PutPath(  (int) con.phKeyIn ,  filename ); 
			Desc1.PutBoolean(  (int) con.phKeyCopy ,  PSX.TRUE ); 

			_end_save( filename, Desc1 );
		}


		// A list of all the document properties for easy eumeration

		public static int [] DocumentProperties = {	
													  (int) con.phKeyMode , 
													  (int) con.phKeyBigNudgeH, 
													  (int) con.phKeyBigNudgeV, 
													  (int) con.phKeyRulerOriginH,
													  (int) con.phKeyRulerOriginV, 
													  (int) con.phKeyWidth, 
													  (int) con.phKeyHeight, 
													  (int) con.phKeyResolution,
													  (int) con.phKeyTitle, 
													  (int) con.phKeyFileInfo, 
													  (int) con.phKeyNumberOfPaths, 
													  (int) con.phKeyNumberOfChannels,
													  (int) con.phKeyNumberOfLayers, 
													  (int) con.phKeyTargetPathIndex, 
													  (int) con.phKeyWorkPathIndex,
													  (int) con.phKeyClippingPathInfo, 
													  (int) con.phKeyDepth, 
													  (int) con.phKeyFileReference, 
													  (int) con.phKeyDocumentID,
													  (int) con.phKeyCopyright, 
													  (int) con.phKeyWatermark, 
													  (int) con.phKeyIsDirty, 
													  (int) con.phKeyCount, 
													  (int) con.phKeyItemIndex };


	}
}
