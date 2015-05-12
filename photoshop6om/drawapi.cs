using System;
using con = PhotoshopTypeLibrary.PSConstants;
using PSX = Photoshop6OM.PhotoshopProxy;
using Photoshop6OM;

namespace Photoshop6OM
{


	public class DrawAPI : ExtensionAPI
	{

		
		public static void Fill( int content, double opacity, int blend_mode)
		{

			PSX.CheckEnum( content, (int) con.phEnumBackgroundColor , (int) con.phEnumForegroundColor );
			PSX.CheckRange( opacity, 0, 100);
			PSX.CheckEnumEx( blend_mode, PSX.blend_modes);

			// Desc1 
			var Desc1 = PSX.MakeNewDescriptor();

			Desc1.PutEnumerated(  (int) con.phKeyUsing ,  (int) con.phTypeFillContents ,  content ); 
			Desc1.PutUnitDouble(  (int) con.phKeyOpacity ,  (int) con.phUnitPercent ,  opacity ); 
			Desc1.PutEnumerated(  (int) con.phKeyMode ,  (int) con.phTypeBlendMode , blend_mode ); 

			// Play the event in photoshop
			PSX.PlayEvent( (int) con.phEventFill , Desc1 , (int)con.phDialogSilent, PSX.PlayBehavior.checkresult );
		}

		public static void FillRegion( System.Drawing.Rectangle r, System.Drawing.Color c , bool anti_alias, double opacity, int blend_mode )
		{
			int color_class_id;
			double [] color_v ={0,0,0,0};
			ColorAPI.GetColorEx( (int) con.phKeyForegroundColor, out color_class_id, ref color_v);
			
			SelectionAPI.SelectShapeEx( r, (int) con.phClassRectangle, anti_alias, (int) con.phEventSet );
			ColorAPI.SetForegroundColor( c );

			DrawAPI.Fill( (int) con.phEnumForegroundColor , 100, (int) con.phEnumNormal );
			ColorAPI.SetColorEx( (int) con.phKeyForegroundColor, color_class_id, color_v);

		}


		public static void CreateTextLayer( string text , string fontname, int fontsize, string fontstyle , int antialias )
		{

			PSX.CheckEnum( fontstyle,  "Regular", "Bold Italic", "Italic" , "Bold" );
			PSX.CheckEnum( antialias,  (int) con.phEnumAntiAliasCrisp , (int) con.phEnumAntiAliasStrong, (int) con.phEnumAntiAliasSmooth, (int) con.phEnumAntiAliasNone  );

			// Desc10 
			var Desc10 = PSX.MakeNewDescriptor();

			Desc10.PutEnumerated(  (int) con.phKeyAlignment ,  (int) con.phTypeAlignment ,  (int) con.phEnumLeft ); 
			Desc10.PutUnitDouble(  PSX.StrToID( "firstLineIndent" )  ,  PSX.StrToID( "pointsUnit" )  ,  0 ); 
			Desc10.PutUnitDouble(  PSX.StrToID( "startIndent" )  ,  PSX.StrToID( "pointsUnit" )  ,  0 ); 
			Desc10.PutUnitDouble(  PSX.StrToID( "endIndent" )  ,  PSX.StrToID( "pointsUnit" )  ,  0 ); 
			Desc10.PutUnitDouble(  PSX.StrToID( "spaceBefore" )  ,  PSX.StrToID( "pointsUnit" )  ,  0 ); 
			Desc10.PutUnitDouble(  PSX.StrToID( "spaceAfter" )  ,  PSX.StrToID( "pointsUnit" )  ,  0 ); 
			Desc10.PutBoolean(  PSX.StrToID( "hyphenate" )  ,  PSX.TRUE ); 
			Desc10.PutInteger(  PSX.StrToID( "hyphenateWordSize" )  ,  8 ); 
			Desc10.PutInteger(  PSX.StrToID( "hyphenatePreLength" )  ,  3 ); 
			Desc10.PutInteger(  PSX.StrToID( "hyphenatePostLength" )  ,  3 ); 
			Desc10.PutInteger(  PSX.StrToID( "hyphenateLimit" )  ,  2 ); 
			Desc10.PutDouble(  PSX.StrToID( "hyphenationZone" )  ,  36 ); 
			Desc10.PutBoolean(  PSX.StrToID( "hyphenateCapitalized" )  ,  PSX.TRUE ); 
			Desc10.PutDouble(  PSX.StrToID( "justificationWordMinimum" )  ,  0 ); 
			Desc10.PutDouble(  PSX.StrToID( "justificationWordDesired" )  ,  1 ); 
			Desc10.PutDouble(  PSX.StrToID( "justificationWordMaximum" )  ,  1 ); 
			Desc10.PutDouble(  PSX.StrToID( "justificationLetterMinimum" )  ,  0 ); 
			Desc10.PutDouble(  PSX.StrToID( "justificationLetterDesired" )  ,  0 ); 
			Desc10.PutDouble(  PSX.StrToID( "justificationLetterMaximum" )  ,  0 ); 
			Desc10.PutDouble(  PSX.StrToID( "justificationGlyphMinimum" )  ,  1 ); 
			Desc10.PutDouble(  PSX.StrToID( "justificationGlyphDesired" )  ,  1 ); 
			Desc10.PutDouble(  PSX.StrToID( "justificationGlyphMaximum" )  ,  1 ); 
			Desc10.PutBoolean(  PSX.StrToID( "hangingRoman" )  ,  PSX.FALSE ); 
			Desc10.PutBoolean(  PSX.StrToID( "burasagari" )  ,  PSX.FALSE ); 
			Desc10.PutEnumerated(  PSX.StrToID( "preferredKinsokuOrder" )  ,  PSX.StrToID( "preferredKinsokuOrder" )  ,  PSX.StrToID( "pushIn" )  ); 
			Desc10.PutString(  PSX.StrToID( "mojiKumiName" )  ,  "None" ); 
			Desc10.PutBoolean(  PSX.StrToID( "textEveryLineComposer" )  ,  PSX.FALSE ); 
			Desc10.PutDouble(  PSX.StrToID( "autoLeadingPercentage" )  ,  1 ); 
			Desc10.PutEnumerated(  PSX.StrToID( "leadingType" )  ,  PSX.StrToID( "leadingType" )  ,  PSX.StrToID( "leadingBelow" )  ); 

			// Desc9 
			var Desc9 = PSX.MakeNewDescriptor();

			Desc9.PutInteger(  (int) con.phKeyFrom ,  0 ); 
			Desc9.PutInteger(  (int) con.phKeyTo ,  text.Length + 1 ); 
			Desc9.PutObject(  PSX.StrToID( "paragraphStyle" )  ,  PSX.StrToID( "paragraphStyle" )  ,  Desc10 ); 

			// List3 
			var List3 = PSX.MakeNewList();

			List3.PutObject(  PSX.StrToID( "paragraphStyleRange" )  ,  Desc9 ); 

			// Desc8 
			var Desc8 = PSX.MakeNewDescriptor();

			Desc8.PutDouble(  (int) con.phKeyRed ,  241 ); 
			Desc8.PutDouble(  (int) con.phKeyGrain ,  101 ); 
			Desc8.PutDouble(  (int) con.phKeyBlue ,  34 ); 

			// Desc7 
			var Desc7 = PSX.MakeNewDescriptor();

			//Desc7.PutString(  PSX.StrToID( "fontPostScriptName" )  ,  fontname  ); 
			Desc7.PutString(  (int) con.phKeyFontName ,  fontname  ); 
			Desc7.PutString(  (int) con.phKeyFontStyleName ,  fontstyle  ); 
			Desc7.PutInteger(  (int) con.phKeyFontScript ,  0 ); 
			Desc7.PutInteger(  (int) con.phKeyFontTechnology ,  0 ); 
			//Desc7.PutUnitDouble(  (int) con.phkey,  PSX.StrToID( "pointsUnit" )  ,  fontsize  ); SAVEEN
			Desc7.PutBoolean(  PSX.StrToID( "syntheticBold" )  ,  PSX.FALSE ); 
			Desc7.PutBoolean(  PSX.StrToID( "syntheticItalic" )  ,  PSX.FALSE ); 
			Desc7.PutBoolean(  PSX.StrToID( "autoLeading" )  ,  PSX.TRUE ); 
			Desc7.PutInteger(  (int) con.phKeyHorizontalScale ,  100 ); 
			Desc7.PutInteger(  (int) con.phKeyVerticalScale ,  100 ); 
			Desc7.PutInteger(  (int) con.phKeyTracking ,  0 ); 
			Desc7.PutBoolean(  (int) con.phKeyAutoKern ,  PSX.TRUE ); 
			Desc7.PutUnitDouble(  (int) con.phKeyBaselineShift ,  PSX.StrToID( "pointsUnit" )  ,  0 ); 
			Desc7.PutEnumerated(  PSX.StrToID( "fontCaps" )  ,  PSX.StrToID( "fontCaps" )  ,  (int) con.phEnumNormal ); 
			Desc7.PutEnumerated(  PSX.StrToID( "baseline" )  ,  PSX.StrToID( "baseline" )  ,  (int) con.phEnumNormal ); 
			Desc7.PutBoolean(  (int) con.phKeyUnderline ,  PSX.FALSE ); 
			Desc7.PutBoolean(  PSX.StrToID( "strikethrough" )  ,  PSX.FALSE ); 
			Desc7.PutBoolean(  PSX.StrToID( "ligature" )  ,  PSX.TRUE ); 
			Desc7.PutBoolean(  PSX.StrToID( "oldStyle" )  ,  PSX.FALSE ); 
			Desc7.PutBoolean(  PSX.StrToID( "proportionalNumbers" )  ,  PSX.TRUE ); 
			Desc7.PutBoolean(  (int) con.phKeyRotate ,  PSX.TRUE ); 
			Desc7.PutEnumerated(  PSX.StrToID( "baselineDirection" )  ,  PSX.StrToID( "baselineDirection" )  ,  PSX.StrToID( "withStream" )  ); 
			Desc7.PutDouble(  PSX.StrToID( "mojiZume" )  ,  0 ); 
			Desc7.PutEnumerated(  PSX.StrToID( "gridAlignment" )  ,  PSX.StrToID( "gridAlignment" )  ,  PSX.StrToID( "roman" )  ); 
			Desc7.PutEnumerated(  PSX.StrToID( "hyphenationLanguage" )  ,  PSX.StrToID( "hyphenationLanguage" )  ,  PSX.StrToID( "englishLanguage" )  ); 
			Desc7.PutInteger(  PSX.StrToID( "wariChuCount" )  ,  1 ); 
			Desc7.PutDouble(  PSX.StrToID( "wariChuScale" )  ,  1 ); 
			Desc7.PutInteger(  PSX.StrToID( "wariChuWidow" )  ,  25 ); 
			Desc7.PutInteger(  PSX.StrToID( "wariChuOrphan" )  ,  25 ); 
			Desc7.PutBoolean(  PSX.StrToID( "noBreak" )  ,  PSX.FALSE ); 
			Desc7.PutObject(  (int) con.phKeyColor ,  (int) con.phClassRGBColor ,  Desc8 ); 
			Desc7.PutBoolean(  (int) con.phKeyFill ,  PSX.TRUE ); 
			Desc7.PutBoolean(  1400140395 ,  PSX.FALSE ); 
			Desc7.PutBoolean(  PSX.StrToID( "fillFirst" )  ,  PSX.FALSE ); 
			Desc7.PutEnumerated(  PSX.StrToID( "verticalUnderlinePosition" )  ,  PSX.StrToID( "verticalUnderlinePosition" )  ,  PSX.StrToID( "verticalUnderlineRight" )  ); 

			// Desc6 
			var Desc6 = PSX.MakeNewDescriptor();

			Desc6.PutInteger(  (int) con.phKeyFrom ,  0 ); 
			Desc6.PutInteger(  (int) con.phKeyTo ,  text.Length + 1 ); 
			Desc6.PutObject(  (int) con.phKeyTextStyle ,  (int) con.phClassTextStyle ,  Desc7 ); 

			// List2 
			var List2 = PSX.MakeNewList();

			List2.PutObject(  (int) con.phClassTextStyleRange ,  Desc6 ); 

			// Desc5 
			var Desc5 = PSX.MakeNewDescriptor();

			Desc5.PutEnumerated(  1413830740 ,  (int) con.phTypeChar ,  1349415968 ); 

			// List1 
			var List1 = PSX.MakeNewList();

			List1.PutObject(  PSX.StrToID( "textShape" )  ,  Desc5 ); 

			// Desc4 
			var Desc4 = PSX.MakeNewDescriptor();

			Desc4.PutUnitDouble(  (int) con.phKeyHorizontal ,  (int) con.phUnitPercent ,  13 ); 
			Desc4.PutUnitDouble(  (int) con.phKeyVertical ,  (int) con.phUnitPercent ,  25 ); 

			// Desc3 
			var Desc3 = PSX.MakeNewDescriptor();

			Desc3.PutEnumerated(  PSX.StrToID( "warpStyle" )  ,  PSX.StrToID( "warpStyle" )  ,  PSX.StrToID( "warpNone" )  ); 
			Desc3.PutDouble(  PSX.StrToID( "warpValue" )  ,  0 ); 
			Desc3.PutDouble(  PSX.StrToID( "warpPerspective" )  ,  0 ); 
			Desc3.PutDouble(  PSX.StrToID( "warpPerspectiveOther" )  ,  0 ); 
			Desc3.PutEnumerated(  PSX.StrToID( "warpRotate" )  ,  (int) con.phTypeOrientation ,  (int) con.phEnumHorizontal ); 

			// Desc2 
			var Desc2 = PSX.MakeNewDescriptor();

			Desc2.PutString(  (int) con.phKeyText ,  text ); 
			Desc2.PutObject(  PSX.StrToID( "warp" )  ,  PSX.StrToID( "warp" )  ,  Desc3 ); 
			Desc2.PutObject(  (int) con.phKeyTextClickPoint ,  (int) con.phClassPoint ,  Desc4 ); 
			Desc2.PutEnumerated(  PSX.StrToID( "textGridding" )  ,  PSX.StrToID( "textGridding" )  ,  (int) con.phEnumNone ); 
			Desc2.PutEnumerated(  (int) con.phKeyOrientation ,  (int) con.phTypeOrientation ,  (int) con.phEnumHorizontal ); 
			Desc2.PutEnumerated(  (int) con.phKeyAntiAlias ,  (int) con.phTypeAntiAlias ,  (int) con.phEnumAntiAliasCrisp ); 
			Desc2.PutList(  PSX.StrToID( "textShape" )  ,  List1 ); 
			Desc2.PutList(  (int) con.phKeyTextStyleRange ,  List2 ); 
			Desc2.PutList(  PSX.StrToID( "paragraphStyleRange" )  ,  List3 ); 

			// Ref1 
			var Ref1 = PSX.MakeNewReference();

			Ref1.PutClass(  (int) con.phClassTextLayer ); 

			// Desc1 
			var Desc1 = PSX.MakeNewDescriptor();

			Desc1.PutReference(  (int) con.phKeyNull ,  Ref1 ); 
			Desc1.PutObject(  (int) con.phKeyUsing ,  (int) con.phClassTextLayer ,  Desc2 ); 


			int old_layer_count = LayerAPI.GetLayerCount( -1 );
			// Play the event in photoshop
			PSX.PlayEvent( (int) con.phEventMake , Desc1 , (int)con.phDialogSilent, PSX.PlayBehavior.checkresult );

			LayerAPI.CheckLayerCount( -1, old_layer_count + 1 );
		}


		public static void CreateRectangleShape( double left, double top, double right, double bottom)
		{


			// Desc3 
			var Desc3 = PSX.MakeNewDescriptor();
			Desc3.PutUnitDouble(  (int) con.phKeyTop ,  (int) con.phUnitDistance ,  top); 
			Desc3.PutUnitDouble(  (int) con.phKeyLeft ,  (int) con.phUnitDistance ,  left ); 
			Desc3.PutUnitDouble(  (int) con.phKeyBottom ,  (int) con.phUnitDistance ,  bottom ); 
			Desc3.PutUnitDouble(  (int) con.phKeyRight ,  (int) con.phUnitDistance ,  right ); 
			
			// Desc2 
			var Desc2 = PSX.MakeNewDescriptor();

			Desc2.PutClass(  (int) con.phKeyType ,  PSX.StrToID( "solidColorLayer" )  ); 
			Desc2.PutObject(  (int) con.phKeyShape ,  (int) con.phClassRectangle ,  Desc3 ); 

			// Ref1 
			var Ref1 = PSX.MakeNewReference();

			Ref1.PutClass(  PSX.StrToID( "contentLayer" )  ); 

			// Desc1 
			var Desc1 = PSX.MakeNewDescriptor();

			Desc1.PutReference(  (int) con.phKeyNull ,  Ref1 ); 
			Desc1.PutObject(  (int) con.phKeyUsing ,  PSX.StrToID( "contentLayer" )  ,  Desc2 ); 

			// Play the event in photoshop
			PSX.PlayEvent( (int) con.phEventMake , Desc1 , (int)con.phDialogSilent, PSX.PlayBehavior.checknone );
		}



	}


}
