using System;
using System.Xml;
using stdout = System.Console;

namespace pstocode
{


	interface BaseFormatter
	{
		void FileHeader();
		void FileFooter();
		void StartClass( string name );
		void EndClass( string name );
		void StartEvent( XmlNode event_node );
		void EndEvent( XmlNode event_node );
		string GetEventFunctionName( XmlNode event_node );
		void CreateObject( XmlNode event_object_node );
		void StartField( XmlNode event_object_node , XmlNode field_Node);
		void EndField( XmlNode event_object_node , XmlNode field_Node);
		void FieldParam( XmlNode event_object_node , XmlNode field_Node, XmlNode param_node , int count);
		void CreateNullObject( string name );

	}

	class CSFormatter: BaseFormatter
	{
		private System.IO.StreamWriter fp;


		static string libname = "PSX";
		static System.Collections.Hashtable ht ;

		static CSFormatter()
		{
			CSFormatter.ht = new System.Collections.Hashtable();
			CSFormatter.ht.Add( "enum", "Enumerated");
			CSFormatter.ht.Add( "float", "Double");
			CSFormatter.ht.Add( "unitfloat", "UnitDouble");
		}
		
		public CSFormatter ( System.IO.StreamWriter new_fp)
		{
			fp=new_fp;
		}

		public void  FileHeader()
		{
			fp.Write("FILEHEADER");
		}

		public void  FileFooter()
		{
			fp.Write("FILEFOOTER");
			fp.Flush();
		}

		public void StartClass( string name )
		{
			fp.Write( "\n\nclass" );
			fp.WriteLine( "name" );
			fp.WriteLine( "{" );
		}

		public void EndClass( string name )
		{
			fp.WriteLine( "}\n\n" );
		}

		public void StartEvent( XmlNode event_node )
		{
			string func_name = GetEventFunctionName( event_node );

			fp.Write( "\tpublic static void {0}()", func_name );
			fp.WriteLine( "" );
			fp.WriteLine( "\t{" );
		}

		public string get_event_id( XmlNode event_node )
		{
			string v="";
			string iv = Isotope.commoncs.libxml.SelectSingleNodeText( event_node, "eventid/value"  );
			string type_str = commoncs.libxml.SelectSingleNodeText( event_node, "eventid/type" );
			if (type_str=="runtimeid")
			{
				v = string.Format( "PSX.StrToID( \"{0}\" )", iv );
			}
			else
			{
				v= "con.ph" + commoncs.libstring.ToUpperFirstCharacter( commoncs.libxml.SelectSingleNodeText( event_node, "name" ) );
			}

			return v;
		}


		public void EndEvent( XmlNode event_node )
		{

			string event_id = this.get_event_id( event_node );

		//eventid_type = winmsxml.get_query_text( eventid_node, "type" )

		//if ( eventid_type == "runtimeid" ) :	eventid = "psx.StrToID( \"" + eventid + "\" )"
		//else :	eventid = "con.ph" + libstring.upper_first( eventid )

			fp.WriteLine( "" );
			fp.WriteLine( "\t\t// Play the event in photoshop" );
			fp.WriteLine( "\t\t{0}.PlayEvent( (int) {1} , {2} , {3}, {4} );", libname , event_id, "Desc1", "(int)con.phDialogSilent" , libname + ".PlayBehavior.checknone" );
			fp.WriteLine( "\t}" );
			fp.WriteLine( "" );
			fp.WriteLine( "" );
		}

		public string GetEventFunctionName( XmlNode event_node )
		{
			string n = commoncs.libxml.SelectSingleNodeText( event_node, "name" );
			return n;
		}

		public string GetObjectDisplayName( XmlNode event_object_node )
		{
			string n = commoncs.libxml.SelectSingleNodeText( event_object_node, "displayname" );
			return n;
		}

		public void CreateObject( XmlNode event_object_node )
		{
			string dn = GetObjectDisplayName( event_object_node );
			string type = commoncs.libstring.ToUpperFirstCharacter( event_object_node.LocalName ) ;



			fp.WriteLine("");
			fp.WriteLine("\t\t// {0} ", dn );
			fp.WriteLine("\t\tPhotoshopTypeLibrary.IAction{0} {1} = {2}.MakeNew{3}();", type, dn, libname, type );
			fp.WriteLine("");

		}

		public void StartField( XmlNode event_object_node , XmlNode field_node)
		{

			string dn = GetObjectDisplayName( event_object_node );

			string given_type = commoncs.libxml.SelectSingleNodeText( field_node, "type" );
			string new_type ;
			
			if (ht.ContainsKey(given_type))
			{
				new_type = (string) ht[ given_type ];
			}
			else
			{
				new_type = given_type;
			}

			new_type = commoncs.libstring.ToUpperFirstCharacter( new_type );
			
			fp.Write("\t\t{0}.Put{1}( ", dn , new_type );

		}

		public void EndField( XmlNode event_object_node , XmlNode field_node)
		{
			fp.WriteLine(" ); ");

		}

		public string StringLiteral( string s )
		{
			string ns;
			string dq = "\"";
			ns = dq + s.Replace( "\\" , "\\\\" ) + dq;
			//ns = dq + System.Text.RegularExpressions.Regex.Replace( s, "\\" , "\\\\\\\\" ) + dq;
			return ns;

		}

		public void FieldParam( XmlNode event_object_node , XmlNode field_Node, XmlNode param_node , int count)
		{
			string param_type;
			param_type = commoncs.libxml.SelectSingleNodeText( param_node , "type" );

			string param_raw_text = commoncs.libxml.SelectSingleNodeText( param_node , "value" );

			string s="!!!ERROR!!!";


			if ( param_type  == "id" )
			{
				s = "(int) con.ph" + commoncs.libstring.ToUpperFirstCharacter( param_raw_text );
			}
			else if ( param_type =="runtimeid" )
			{
				s = libname + ".StrToID( \"" + param_raw_text + "\" ) ";
			}
			else if ( param_type =="idliteral" )
			{
				s = commoncs.libxml.SelectSingleNodeText( param_node, "value/idvalue" );
			}
			else if ( param_type =="boolean" )
			{
				s = libname + "." + param_raw_text.ToUpper();
			}
			else if ( param_type =="string" )
			{
				s = StringLiteral( param_raw_text );
			}
			else if (param_type =="path" )
			{
				//System.Console.WriteLine( param_node.OuterXml );
				s = StringLiteral( param_raw_text );
			}
			else if ( (param_type=="descriptor") || (param_type=="list") || (param_type=="reference" ) )
			{
				string name = commoncs.libxml.SelectSingleNodeText( param_node , "value/name" );
				s = GetDisplayNameForObjectName( name, param_node );
			}
			else
			{
				s = param_raw_text;
			}


			string sep="";
			if ( count>0)
			{
				sep=" , ";
			}

			fp.Write( "{0} {1}", sep, s );
			
		}

		string GetDisplayNameForObjectName( string object_name , XmlNode node ) 
		{

			string query = "//*[isobject][name=\"" + object_name +  "\"]";
			XmlNode object_node = node.SelectSingleNode( query );
			string displayname = commoncs.libxml.SelectSingleNodeText( object_node, "displayname" );
			return displayname;
		}

		public void CreateNullObject( string name )
		{
			// this handles the case when there are no descriptors for the event
			fp.WriteLine("\t\tPhotoshopTypeLibrary.IActionDescriptor Desc1 = {0}.MakeNewDescriptor();", libname);

		}
	}

	class GenericOutput
	{
		public static void Run( XmlDocument dom, BaseFormatter fmt)
		{

			XmlNodeList event_nodes = dom.SelectNodes( "//event" );
			

			foreach( XmlNode event_node in event_nodes )
			{
				fmt.StartEvent( event_node );

				// Find all the objects that are required by the event
				XmlNodeList event_objects = event_node.SelectNodes( ".//isobject/.." );

				// Put the nodes into a list so that they can be reversed
				// Printing the list in reverse makes it easier to read (IMO)
				System.Collections.ArrayList L = commoncs.libxml.NodeListToArrayList( event_objects );
				L.Reverse();

				foreach ( XmlNode event_object in L )
				{

					fmt.CreateObject( event_object );

					XmlNodeList field_nodes = event_object.SelectNodes( "putfield" );
					
					foreach ( XmlNode field_node in field_nodes )
					{
						fmt.StartField( event_object, field_node );

						XmlNodeList param_nodes = field_node.SelectNodes( "params/*" );
						
						int param_count =0;
						foreach( XmlNode param_node in param_nodes )
						{
							fmt.FieldParam( event_object, field_node, param_node , param_count);
							param_count++;

						}
						fmt.EndField( event_object, field_node );
					}

				}

				if (L.Count < 1 )
				{
					fmt.CreateNullObject( "Desc1" );
				}

			
				fmt.EndEvent( event_node );
			}
	
		}
	}


	class MainClass
	{

		static void Main(string[] args)
		{

			stdout.WriteLine( "PSTOCODE" );

			if ( args.Length<1)
			{
				stdout.Write( "Not enough args" );
				return;
			}

			string input_path = args[0];
			string output_path = "";
			string output_filename = System.IO.Path.Combine( output_path, "playevents.xml" );


			string [] input_filenames = System.IO.Directory.GetFiles( input_path , "*.ps6xml" );
			System.Array.Sort( input_filenames );

			System.IO.StreamWriter fpout = new System.IO.StreamWriter ( new System.IO.FileStream( "playevents.csharp" , System.IO.FileMode.Create ) );

			foreach (string input_filename in input_filenames)
			{
				stdout.WriteLine( input_filename );
				XmlDocument dom = new XmlDocument();
				dom.Load( input_filename );
				BaseFormatter fmt = new CSFormatter( fpout );
				GenericOutput.Run( dom, fmt );

			}
			fpout.Close();

		}
	}
}
