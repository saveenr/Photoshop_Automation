using System;
using  con = PhotoshopTypeLibrary.PSConstants;
using  docapi = Photoshop6OM.DocumentAPI;
using  layerapi = Photoshop6OM.LayerAPI;
using  drawapi = Photoshop6OM.DrawAPI;
using  colorapi = Photoshop6OM.ColorAPI;
using  appapi = Photoshop6OM.ApplicationAPI;
using  selapi = Photoshop6OM.SelectionAPI;
using  filterapi = Photoshop6OM.FilterAPI;
using  PSX = Photoshop6OM.PhotoshopProxy;


namespace testom
{

    /*

	class PSTest : UnitTesting.Test
	{

		public PSTest()
		{
		}


		public virtual void CreateDefaultDoc( string docname )
		{
			int old_count = docapi.GetDocumentCount();
			docapi.CreateDocument( 300, 200, 100, (int)con.phEnumWhite, (int)con.phClassRGBColorMode, docname );
			int actual_new_count = docapi.GetDocumentCount();
			int expected_new_count = old_count +1;

			ASSERT( actual_new_count == expected_new_count, "Failed to Create Doc" );
		}

		public void CreateDefaultDocs( string docnames )
		{
			char[] sep = {';'};
			string [] names = docnames.Split( sep  );
			foreach ( string docname in names )
			{
				this.CreateDefaultDoc( docname );
			}
		}

		public string GetDocTitles()
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder( );
			for( int doc_index = 1; doc_index <= docapi.GetDocumentCount(); doc_index++ )
			{
				sb.Append( docapi.GetTitle(doc_index) );

				if (doc_index< docapi.GetDocumentCount() )
				{
					sb.Append(";");
				}
			}
			string retval = sb.ToString();
			return retval;
		}

		public void RemoveFile( string fname )
		{
			if ( System.IO.File.Exists( fname ) ) 
			{
				System.IO.File.Delete( fname );
			}
		}

		public void CheckFileExists( string fname )
		{
			string message = "File not created: " + fname;
			ASSERT( System.IO.File.Exists( fname ), message );
		}

	}

	class PSDocTest : PSTest
	{
	
		public override void SetUp()
		{
			docapi.CloseAllDocuments();
			colorapi.SetForegroundColor( System.Drawing.Color.Black );
			colorapi.SetBackgroundColor( System.Drawing.Color.Red );
		}

	}

	class ErrorTest : PSDocTest
	{
	
		public override void Run()
		{
			/*
			 * In this test, every result is a failure except the specific exception we are deliberately
			 * raising.
			 */

/*			test_worked=false;
			try
			{
				//d.TestError();
			}
			catch ( System.E  e)
			{
				test_worked=true;
			}
			*/


    /*
			test_worked=true;
		}

	}

	
	class CreateDocTest : PSDocTest
	{
	
		public override void Run()
		{
			this.CreateDefaultDocs( "d1;d2;d3" );
		}
	}

	class DuplicateDocTest : PSDocTest
	{
	
		public override void Run()
		{
			this.CreateDefaultDocs( "d1;d2;d3" );
			docapi.DuplicateDocument( "dupe of d3", true );
		}
	}

	class OpenDocTest : PSDocTest
	{
	
		public void DumpDocProperties( int doc_index )
		{
			///
			/// <summary>
			/// 
			/// Just prints out all the names and values for each doc property
			/// </summary>
			/// 

			// WORKITEM: Handle Dumps of Complex properties (Enum, etc)
			// WORKITEM: Handle Dumps of Objects
			// WORKITEM: Merge with DumpDescriptor
			System.Collections.ArrayList a = docapi.GetDocumentProperties( -1, docapi.DocumentProperties );
			int i=0;
			foreach (object v in a)
			{
				System.Console.WriteLine( " {0} {1} ",  PSX.IDToStr( docapi.DocumentProperties[i]) , v );
				i++;
			}
		}

		public override void Run()
		{
			this.CreateDefaultDocs( "d1" );
			string filename = "c:\\foobar.psd"; 
			docapi.SaveAsPSD( filename , false );
			docapi.CloseAllDocuments();
			docapi.OpenDocument( filename );

			filename = "c:\\foobar.jpg"; 
			docapi.SaveCopyAsJPEG( filename , 12 );
			docapi.CloseAllDocuments();
			docapi.OpenDocument( filename );

			filename = "c:\\foobar.png"; 
			docapi.SaveCopyAsPNG( filename , (int) con.phEnumPNGInterlaceNone );
			docapi.CloseAllDocuments();
			docapi.OpenDocument( filename );

		}
	}

	class LayerTest : PSDocTest
	{
		public override void Run()
		{
			this.CreateDefaultDocs( "d1" );
			layerapi.CreateLayer( "foo" );
			drawapi.CreateRectangleShape( 10,10,100,100 );
			drawapi.CreateRectangleShape( 100,100,200,200 );
			layerapi.RasterizeLayer( 1 );
			layerapi.RasterizeLayer( 2 );
			layerapi.FlattenImage();
			layerapi.CreateSolidColorFillLayer( "FOO", 0,0,0);


			double [] opacities = { 0, 100};
			PhotoshopTypeLibrary.IActionDescriptor [] opacity_stops = layerapi.CreateOpacityStops( opacities );

			System.Drawing.Color [] colors = { System.Drawing.Color.Blue , System.Drawing.Color.Green , System.Drawing.Color.Yellow };
			PhotoshopTypeLibrary.IActionDescriptor [] color_stops = layerapi.CreateColorStops( colors );
			
			layerapi.CreateGradientFillLayer( "mygradient" , (int) con.phEnumRadial , 80, opacity_stops, color_stops);

			layerapi.CreateLayerFromBackground( "BK1", 100, (int) con.phEnumNormal );
			layerapi.DuplicateLayer( -1 , "fooD1" );
			layerapi.DuplicateLayer( -1 , "fooD2" );

			layerapi.DeleteLayer( -1 );

		}
	}

	class TextTest : PSDocTest
	{
		public override void Run()
		{
			this.CreateDefaultDocs( "d1" );
			colorapi.SetForegroundColor( 0,0,0);
			drawapi.CreateTextLayer( "Hello World", "Arial" , 10 , "Italic" , (int) con.phEnumAntiAliasNone);
			colorapi.SetForegroundColor( System.Drawing.Color.Beige );
			drawapi.CreateRectangleShape( 10, 10, 100,100 );
			colorapi.SetForegroundColor( System.Drawing.Color.Azure );
			drawapi.CreateRectangleShape( 80, 80, 180,180);
		}
	}

	class SelectionTest : PSDocTest
	{
		public override void Run()
		{
			bool anti_alias = true;
			this.CreateDefaultDocs( "d1" );
			selapi.SelectAll();
			selapi.SelectNone();
			selapi.SelectRectangle( 10,10,30,30, anti_alias );
			selapi.InvertSelection();
			selapi.SelectEllipse( 10,10,40,40, anti_alias );
			selapi.AddRectangle( 10,10,30,30, anti_alias );
			selapi.SubtractEllipse( 20,20,30,30, anti_alias  );
			



		}
	}

	class DrawTest : PSDocTest
	{
		public override void Run()
		{
			this.CreateDefaultDocs( "d1" );
			selapi.SelectAll();
			selapi.SelectRectangle( 10,10,30,30, false );
			drawapi.Fill( (int) con.phEnumForegroundColor , 100, (int) con.phEnumNormal );

		}
	}


	class FilterTest : PSDocTest
	{
		public override void Run()
		{
			this.CreateDefaultDocs( "d1" );
			selapi.SelectAll();

			drawapi.FillRegion( new System.Drawing.Rectangle( 10,10,20,20), System.Drawing.Color.Red, false, 100, (int) con.phEnumNormal );
			drawapi.FillRegion( new System.Drawing.Rectangle( 30,30,20,20), System.Drawing.Color.CornflowerBlue , false, 100, (int) con.phEnumNormal );

			selapi.SelectRectangle( 0,0,50,50,false );
			filterapi.GaussianBlur( 5 );
			filterapi.AddNoise( (int) con.phEnumUniformDistribution, 12.5 , PSX.FALSE );
			filterapi.UnsharpMask( 100 , 250 , 0 );
		}
	}

	class ClipboardTest : PSDocTest
	{
		public override void Run()
		{
			this.CreateDefaultDocs( "d1" );

			//selapi.SelectAll();
			//selapi.SelectRectangle( 80,80,200,200 , true );
			//appapi.Copy();
			//appapi.Paste();

		}
	}

	class TestOM
	{
		static void Main(string[] args)
		{
	
			System.Console.WriteLine( "Photoshop6OM Test Suite");

			UnitTesting.TestSuite suite = new UnitTesting.TestSuite( "Suite #1" );
			suite.Add( new CreateDocTest() );
			suite.Add( new OpenDocTest() );
			suite.Add( new LayerTest() );
			suite.Add( new TextTest() );
			//suite.Add( new ClipboardTest() ); clipboard stuff doesnt work
			suite.Add( new SelectionTest() );
			suite.Add( new DrawTest() );
			suite.Add( new FilterTest() );
			
			
			suite.Run();

		}
	}
*/
}
