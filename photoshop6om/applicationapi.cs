using System;
using con = PhotoshopTypeLibrary.PSConstants;
using PSX = Photoshop6OM.PhotoshopProxy;

namespace Photoshop6OM
{


	public class ApplicationAPI : ExtensionAPI
	{

		public static void Paste()
		{

			/*
			System.Windows.Forms.IDataObject ido = System.Windows.Forms.Clipboard.GetDataObject();

			if (ido!=null)
			{


				string [] formats = ido.GetFormats();
				foreach (string s in formats)
				{
					System.Console.WriteLine( "FMT {0}",s );
				}
			}

			*/

			// Desc1 
			var Desc1 = PSX.MakeNewDescriptor();

			Desc1.PutEnumerated(  (int) con.phKeyAntiAlias ,  (int) con.phTypeAntiAlias ,  (int) con.phEnumAntiAliasNone ); 

			// Play the event in photoshop
			PSX.PlayEvent( (int) con.phEventPaste , Desc1 , (int)con.phDialogSilent, PSX.PlayBehavior.checkresult );
		}
	

		

		public static void Copy()
		{

			var Desc1 = PSX.MakeNewDescriptor();

			// Play the event in photoshop
			PSX.PlayEvent( (int) con.phEventCopy , Desc1 , (int)con.phDialogSilent, PSX.PlayBehavior.checkresult );

			System.Windows.Forms.IDataObject ido = System.Windows.Forms.Clipboard.GetDataObject();
			if ( ido == null)
			{
				var e = new Photoshop6OM.PhotoshoProxyError( "IDO failed" );
				throw e;
			}

			string [] formats = ido.GetFormats();
			foreach( string format in formats )
			{
				System.Console.WriteLine("F: {0}", format );
			}


		}


		public static void EnterQuickMaskMode()
		{

			// Ref1 
			var Ref1 = PSX.MakeNewReference();

			Ref1.PutProperty(  (int) con.phClassProperty ,  (int) con.phKeyQuickMask ); 
			Ref1.PutEnumerated(  (int) con.phClassDocument ,  (int) con.phTypeOrdinal ,  (int) con.phEnumTarget ); 

			// Desc1 
			var Desc1 = PSX.MakeNewDescriptor();

			Desc1.PutReference(  (int) con.phKeyNull ,  Ref1 ); 

			// Play the event in photoshop
			PSX.PlayEvent( (int) con.phEventSet , Desc1 , (int)con.phDialogSilent, PSX.PlayBehavior.checkresult );
		}


		public static void ExitQuickMaskMode()
		{

			// Ref1 
			var Ref1 = PSX.MakeNewReference();

			Ref1.PutProperty(  (int) con.phClassProperty ,  (int) con.phKeyQuickMask ); 
			Ref1.PutEnumerated(  (int) con.phClassDocument ,  (int) con.phTypeOrdinal ,  (int) con.phEnumTarget ); 

			// Desc1 
			var Desc1 = PSX.MakeNewDescriptor();

			Desc1.PutReference(  (int) con.phKeyNull ,  Ref1 ); 

			// Play the event in photoshop
			PSX.PlayEvent( (int) con.phEventClear , Desc1 , (int)con.phDialogSilent, PSX.PlayBehavior.checkresult );
		}


	}


}
