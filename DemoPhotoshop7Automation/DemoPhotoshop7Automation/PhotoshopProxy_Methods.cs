using System;
using System.Collections.Generic;
using PhotoshopTypeLibrary;
using con = PhotoshopTypeLibrary.PSConstants;

namespace PhotoshopAuto
{


    public partial class PhotoshopProxy
    {
        public int GetDocumentCount()
        {
            ///
            /// <summary>
            ///		Returns the number of documents open
            /// </summary>
            ///

            return (int)get_value_from_object((int)con.phClassApplication, -1, (int)con.phKeyNumberOfDocuments);
        }

        public void CreateDocument(int pixelwidth, int pixelheight, string name)
        {

            this.CreateDocument(pixelwidth, pixelheight, 96, (int)con.phEnumWhite, (int)con.phClassRGBColorMode, name);
            
        }

        public void CreateDocument(int pixelwidth, int pixelheight, int ppi, int fill, int mode, string name)
        {
            System.Diagnostics.Debug.Assert(pixelwidth > 0);
            System.Diagnostics.Debug.Assert(pixelheight > 0);
            System.Diagnostics.Debug.Assert(ppi > 0);
            CheckEnum( mode, (int) con.phClassRGBColorMode );
            CheckEnum(fill, (int)con.phEnumTransparent, (int)con.phEnumBackgroundColor, (int)con.phEnumWhite);


            // Creates a new document


            // Create Desc2
            var Desc2 = this.m_app.MakeDescriptor();
            Desc2.PutString((int)con.phKeyName, name);
            Desc2.PutClass((int)con.phKeyMode, mode);
            Desc2.PutUnitDouble((int)con.phKeyWidth, (int)con.phUnitDistance, pixelwidth);
            Desc2.PutUnitDouble((int)con.phKeyHeight, (int)con.phUnitDistance, pixelheight);
            Desc2.PutUnitDouble((int)con.phKeyResolution, (int)con.phUnitDensity, ppi);
            Desc2.PutEnumerated((int)con.phKeyFill, (int)con.phTypeFill, fill);

            // Create Desc1
            PhotoshopTypeLibrary.IActionDescriptor Desc1 = this.m_app.MakeDescriptor();
            Desc1.PutObject((int)con.phKeyNew, (int)con.phClassDocument, Desc2);


            int old_count = GetDocumentCount();

            // Play the Event Into Photoshop
            PlayEvent((int)con.phEventMake, Desc1, (int)con.phDialogSilent, PlayBehavior.checknone);

            //DocumentAPI.CheckDocumentCount(old_count + 1);

        }

        public void CloseAllDocuments()
        {
            int num_docs = GetDocumentCount();
            for (int i = 0; i < num_docs; i++)
            {
                CloseDocument();

            }
            CheckDocumentCount(0);

        }

        public void CloseDocument()
        {
            // NOTE: Those closes without prompting for save

            //System.Diagnostics.Debug.Assert(GetDocumentCount() > 0);

            // Desc1 
            var Desc1 = this.m_app.MakeDescriptor();

            Desc1.PutEnumerated((int)con.phKeySaving, (int)con.phTypeYesNo, (int)con.phEnumNo);

            int old_count = GetDocumentCount();

            // Play the event in photoshop
            PlayEvent((int)con.phEventClose, Desc1, (int)con.phDialogSilent, PlayBehavior.checknone);

            //DocumentAPI.CheckDocumentCount(old_count - 1);
        }


        public void DuplicateDocument(string new_name, bool merge_layers)
        {

            System.Diagnostics.Debug.Assert(new_name.Length > 0);

            // Ref1 
            var Ref1 = this.m_app.MakeReference();

            Ref1.PutEnumerated((int)con.phClassDocument, (int)con.phTypeOrdinal, (int)con.phEnumFirst);

            // Desc1 
            var Desc1 = this.m_app.MakeDescriptor();

            Desc1.PutReference((int)con.phKeyNull, Ref1);
            Desc1.PutString((int)con.phKeyName, new_name);

            if (merge_layers)
            {
                Desc1.PutBoolean((int)con.phKeyMerged, TRUE);
            }
            else
            {
                Desc1.PutBoolean((int)con.phKeyMerged, FALSE);
            }

            int old_count = GetDocumentCount();

            // Play the event in photoshop
            PlayEvent((int)con.phEventDuplicate, Desc1, (int)con.phDialogSilent, PlayBehavior.checkresult);

            //DocumentAPI.CheckDocumentCount(old_count + 1);
        }




        public string GetTitle(int doc_index)
        {
            ///
            /// <summary>
            ///		Returns the title of document with given doc)index
            /// </summary>
            ///
            return (string)get_value_from_object((int)con.phClassDocument, doc_index, (int)con.phKeyTitle);
        }


        public int GetDirtyFlag(int doc_index)
        {
            ///
            /// <summary>
            ///		Returns the title of document with given doc)index
            /// </summary>
            ///
            return (int)get_value_from_object((int)con.phClassDocument, doc_index, (int)con.phKeyIsDirty);
        }

        public bool IsDocumentDirty(int doc_index)
        {
            return (0 != (int)get_value_from_object((int)con.phClassDocument, doc_index, (int)con.phKeyIsDirty));
        }

        public void OpenDocument(string filename)
        {

            if (IsFileLoaded(filename))
            {
                string msg = string.Format("The file \"{0}\" is already open.", filename);
                var e = new PhotoshoProxyError(msg);
                throw e;
            }

            CheckFileExists(filename);

            // Desc1 
            var Desc1 = this.m_app.MakeDescriptor();

            Desc1.PutPath((int)con.phKeyNull, filename);

            int old_count = GetDocumentCount();

            // Play the event in photoshop
            PlayEvent((int)con.phEventOpen, Desc1, (int)con.phDialogSilent, PlayBehavior.checkresult);

            //DocumentAPI.CheckDocumentCount(old_count + 1);
        }


        public void CheckDocumentCount(int expected_count)
        {
            ///
            /// <summary>
            /// Raises an exception of the document count is not what is expected
            ///	Workitem: Move some of this checking into PSX
            /// </summary>
            ///

            int actual_document_count = GetDocumentCount();
            if (actual_document_count != expected_count)
            {
                string msg = string.Format("Expected document count of {0}, got {1} instead", actual_document_count, expected_count);
                throw (new PhotoshoProxyError(msg));
            }
        }

        public List<object> GetDocumentProperties(int doc_index, int[] prop_ids)
        {
            ///
            /// <summary> 
            /// Returns the specified document properties
            /// 
            /// </summary>
            /// 



            var values = get_values_from_object((int)con.phClassDocument, doc_index, prop_ids);
            return values;
        }

        public bool IsFileLoaded(string filename)
        {
            string fname1 = filename.ToUpper();
            int[] p = { (int)con.phKeyFileReference };

            foreach (List<object> props in query_object_properties((int)con.phClassDocument, p))
            {

                string fname2 = (string)props[0];
                fname2 = fname2.ToUpper();
                if (fname1.Equals(fname2))
                {
                    return true;
                }

            }
            return false;
        }


        public void eventSave()
        {
            var Desc1 = this.m_app.MakeDescriptor();

            // Play the event in photoshop
            PlayEvent((int)con.phEventSave, Desc1, (int)con.phDialogSilent, PlayBehavior.checknone);
        }

        public void _start_save(string filename)
        {
            CheckValidFilename(filename);
        }

        public void _end_save(string filename, PhotoshopTypeLibrary.IActionDescriptor Desc1)
        {

            // Play the event in photoshop
            PlayEvent((int)con.phEventSave, Desc1, (int)con.phDialogSilent, PlayBehavior.checknone);
            CheckFileExists(filename);

        }

        public void SaveAsPSD(string filename, bool save_as_copy)
        {

            // Desc1 
            _start_save(filename);

            var Desc1 = this.m_app.MakeDescriptor();

            Desc1.PutClass((int)con.phKeyAs, (int)con.phClassPhotoshop35Format);
            Desc1.PutPath((int)con.phKeyIn, filename);
            if (save_as_copy)
            {
                Desc1.PutBoolean((int)con.phKeyCopy, TRUE);
                Desc1.PutBoolean((int)con.phKeyLayers, FALSE);
            }

            _end_save(filename, Desc1);

        }


        public void SaveCopyAsJPEG(string filename, int quality)
        {

            // WORKITEM support matte color

            _start_save(filename);

            CheckRange(quality, 0, 12);

            // Desc2 
            var Desc2 = this.m_app.MakeDescriptor();
            Desc2.PutInteger((int)con.phKeyExtendedQuality, quality);

            // Desc1 
            var Desc1 = this.m_app.MakeDescriptor();

            Desc1.PutObject((int)con.phKeyAs, (int)con.phClassJPEGFormat, Desc2);
            // WORKITEM what if they only provided a path
            Desc1.PutPath((int)con.phKeyIn, filename);
            Desc1.PutBoolean((int)con.phKeyCopy, TRUE);

            _end_save(filename, Desc1);
        }

        public void SaveCopyAsPNG(string filename, int interlace)
        {
            _start_save(filename);
            CheckEnum(interlace, (int)con.phEnumPNGInterlaceNone, (int)con.phEnumPNGInterlaceAdam7);

            // Desc2 
            var Desc2 = this.m_app.MakeDescriptor();

            Desc2.PutEnumerated((int)con.phKeyPNGInterlaceType, (int)con.phTypePNGInterlaceType, interlace);
            Desc2.PutEnumerated((int)con.phKeyPNGFilter, (int)con.phTypePNGFilter, (int)con.phEnumPNGFilterAdaptive);

            // Desc1 
            var Desc1 = this.m_app.MakeDescriptor();

            Desc1.PutObject((int)con.phKeyAs, (int)con.phClassPNGFormat, Desc2);
            Desc1.PutPath((int)con.phKeyIn, filename);
            Desc1.PutBoolean((int)con.phKeyCopy, TRUE);

            _end_save(filename, Desc1);
        }


        // A list of all the document properties for easy eumeration

        public int[] DocumentProperties = {	
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





        public void Fill(int content, double opacity, int blend_mode)
        {

            CheckEnum(content, (int)con.phEnumBackgroundColor, (int)con.phEnumForegroundColor);
            CheckRange(opacity, 0, 100);
            CheckEnumEx(blend_mode, blend_modes);

            // Desc1 
            var Desc1 = this.m_app.MakeDescriptor();

            Desc1.PutEnumerated((int)con.phKeyUsing, (int)con.phTypeFillContents, content);
            Desc1.PutUnitDouble((int)con.phKeyOpacity, (int)con.phUnitPercent, opacity);
            Desc1.PutEnumerated((int)con.phKeyMode, (int)con.phTypeBlendMode, blend_mode);

            // Play the event in photoshop
            PlayEvent((int)con.phEventFill, Desc1, (int)con.phDialogSilent, PlayBehavior.checkresult);
        }



        public void FillRegion(System.Drawing.Rectangle r, System.Drawing.Color c, bool anti_alias, double opacity, int blend_mode)
        {
            int color_class_id;
            double[] color_v = { 0, 0, 0, 0 };
            GetColorEx((int)con.phKeyForegroundColor, out color_class_id, ref color_v);

            SelectShapeEx(r, (int)con.phClassRectangle, anti_alias, (int)con.phEventSet);
            SetForegroundColor(c);

            Fill((int)con.phEnumForegroundColor, 100, (int)con.phEnumNormal);
            SetColorEx((int)con.phKeyForegroundColor, color_class_id, color_v);

        }


        public void CreateTextLayer(string text, string fontname, int fontsize, string fontstyle, int antialias)
        {

            CheckEnum(fontstyle, "Regular", "Bold Italic", "Italic", "Bold");
            CheckEnum(antialias, (int)con.phEnumAntiAliasCrisp, (int)con.phEnumAntiAliasStrong, (int)con.phEnumAntiAliasSmooth, (int)con.phEnumAntiAliasNone);

            // Desc10 
            var Desc10 = this.m_app.MakeDescriptor();

            Desc10.PutEnumerated((int)con.phKeyAlignment, (int)con.phTypeAlignment, (int)con.phEnumLeft);
            Desc10.PutUnitDouble(StrToID("firstLineIndent"), StrToID("pointsUnit"), 0);
            Desc10.PutUnitDouble(StrToID("startIndent"), StrToID("pointsUnit"), 0);
            Desc10.PutUnitDouble(StrToID("endIndent"), StrToID("pointsUnit"), 0);
            Desc10.PutUnitDouble(StrToID("spaceBefore"), StrToID("pointsUnit"), 0);
            Desc10.PutUnitDouble(StrToID("spaceAfter"), StrToID("pointsUnit"), 0);
            Desc10.PutBoolean(StrToID("hyphenate"), TRUE);
            Desc10.PutInteger(StrToID("hyphenateWordSize"), 8);
            Desc10.PutInteger(StrToID("hyphenatePreLength"), 3);
            Desc10.PutInteger(StrToID("hyphenatePostLength"), 3);
            Desc10.PutInteger(StrToID("hyphenateLimit"), 2);
            Desc10.PutDouble(StrToID("hyphenationZone"), 36);
            Desc10.PutBoolean(StrToID("hyphenateCapitalized"), TRUE);
            Desc10.PutDouble(StrToID("justificationWordMinimum"), 0);
            Desc10.PutDouble(StrToID("justificationWordDesired"), 1);
            Desc10.PutDouble(StrToID("justificationWordMaximum"), 1);
            Desc10.PutDouble(StrToID("justificationLetterMinimum"), 0);
            Desc10.PutDouble(StrToID("justificationLetterDesired"), 0);
            Desc10.PutDouble(StrToID("justificationLetterMaximum"), 0);
            Desc10.PutDouble(StrToID("justificationGlyphMinimum"), 1);
            Desc10.PutDouble(StrToID("justificationGlyphDesired"), 1);
            Desc10.PutDouble(StrToID("justificationGlyphMaximum"), 1);
            Desc10.PutBoolean(StrToID("hangingRoman"), FALSE);
            Desc10.PutBoolean(StrToID("burasagari"), FALSE);
            Desc10.PutEnumerated(StrToID("preferredKinsokuOrder"), StrToID("preferredKinsokuOrder"), StrToID("pushIn"));
            Desc10.PutString(StrToID("mojiKumiName"), "None");
            Desc10.PutBoolean(StrToID("textEveryLineComposer"), FALSE);
            Desc10.PutDouble(StrToID("autoLeadingPercentage"), 1);
            Desc10.PutEnumerated(StrToID("leadingType"), StrToID("leadingType"), StrToID("leadingBelow"));

            // Desc9 
            var Desc9 = this.m_app.MakeDescriptor();

            Desc9.PutInteger((int)con.phKeyFrom, 0);
            Desc9.PutInteger((int)con.phKeyTo, text.Length + 1);
            Desc9.PutObject(StrToID("paragraphStyle"), StrToID("paragraphStyle"), Desc10);

            // List3 
            var List3 = this.m_app.MakeList();

            List3.PutObject(StrToID("paragraphStyleRange"), Desc9);

            // Desc8 
            var Desc8 = this.m_app.MakeDescriptor();

            Desc8.PutDouble((int)con.phKeyRed, 241);
            Desc8.PutDouble((int)con.phKeyGrain, 101);
            Desc8.PutDouble((int)con.phKeyBlue, 34);

            // Desc7 
            var Desc7 = this.m_app.MakeDescriptor();

            //Desc7.PutString(  StrToID( "fontPostScriptName" )  ,  fontname  ); 
            Desc7.PutString((int)con.phKeyFontName, fontname);
            Desc7.PutString((int)con.phKeyFontStyleName, fontstyle);
            Desc7.PutInteger((int)con.phKeyFontScript, 0);
            Desc7.PutInteger((int)con.phKeyFontTechnology, 0);
            //Desc7.PutUnitDouble(  (int) con.phkey,  StrToID( "pointsUnit" )  ,  fontsize  ); SAVEEN
            Desc7.PutBoolean(StrToID("syntheticBold"), FALSE);
            Desc7.PutBoolean(StrToID("syntheticItalic"), FALSE);
            Desc7.PutBoolean(StrToID("autoLeading"), TRUE);
            Desc7.PutInteger((int)con.phKeyHorizontalScale, 100);
            Desc7.PutInteger((int)con.phKeyVerticalScale, 100);
            Desc7.PutInteger((int)con.phKeyTracking, 0);
            Desc7.PutBoolean((int)con.phKeyAutoKern, TRUE);
            Desc7.PutUnitDouble((int)con.phKeyBaselineShift, StrToID("pointsUnit"), 0);
            Desc7.PutEnumerated(StrToID("fontCaps"), StrToID("fontCaps"), (int)con.phEnumNormal);
            Desc7.PutEnumerated(StrToID("baseline"), StrToID("baseline"), (int)con.phEnumNormal);
            Desc7.PutBoolean((int)con.phKeyUnderline, FALSE);
            Desc7.PutBoolean(StrToID("strikethrough"), FALSE);
            Desc7.PutBoolean(StrToID("ligature"), TRUE);
            Desc7.PutBoolean(StrToID("oldStyle"), FALSE);
            Desc7.PutBoolean(StrToID("proportionalNumbers"), TRUE);
            Desc7.PutBoolean((int)con.phKeyRotate, TRUE);
            Desc7.PutEnumerated(StrToID("baselineDirection"), StrToID("baselineDirection"), StrToID("withStream"));
            Desc7.PutDouble(StrToID("mojiZume"), 0);
            Desc7.PutEnumerated(StrToID("gridAlignment"), StrToID("gridAlignment"), StrToID("roman"));
            Desc7.PutEnumerated(StrToID("hyphenationLanguage"), StrToID("hyphenationLanguage"), StrToID("englishLanguage"));
            Desc7.PutInteger(StrToID("wariChuCount"), 1);
            Desc7.PutDouble(StrToID("wariChuScale"), 1);
            Desc7.PutInteger(StrToID("wariChuWidow"), 25);
            Desc7.PutInteger(StrToID("wariChuOrphan"), 25);
            Desc7.PutBoolean(StrToID("noBreak"), FALSE);
            Desc7.PutObject((int)con.phKeyColor, (int)con.phClassRGBColor, Desc8);
            Desc7.PutBoolean((int)con.phKeyFill, TRUE);
            Desc7.PutBoolean(1400140395, FALSE);
            Desc7.PutBoolean(StrToID("fillFirst"), FALSE);
            Desc7.PutEnumerated(StrToID("verticalUnderlinePosition"), StrToID("verticalUnderlinePosition"), StrToID("verticalUnderlineRight"));

            // Desc6 
            var Desc6 = this.m_app.MakeDescriptor();

            Desc6.PutInteger((int)con.phKeyFrom, 0);
            Desc6.PutInteger((int)con.phKeyTo, text.Length + 1);
            Desc6.PutObject((int)con.phKeyTextStyle, (int)con.phClassTextStyle, Desc7);

            // List2 
            var List2 = this.m_app.MakeList();

            List2.PutObject((int)con.phClassTextStyleRange, Desc6);

            // Desc5 
            var Desc5 = this.m_app.MakeDescriptor();

            Desc5.PutEnumerated(1413830740, (int)con.phTypeChar, 1349415968);

            // List1 
            var List1 = this.m_app.MakeList();

            List1.PutObject(StrToID("textShape"), Desc5);

            // Desc4 
            var Desc4 = this.m_app.MakeDescriptor();

            Desc4.PutUnitDouble((int)con.phKeyHorizontal, (int)con.phUnitPercent, 13);
            Desc4.PutUnitDouble((int)con.phKeyVertical, (int)con.phUnitPercent, 25);

            // Desc3 
            var Desc3 = this.m_app.MakeDescriptor();

            Desc3.PutEnumerated(StrToID("warpStyle"), StrToID("warpStyle"), StrToID("warpNone"));
            Desc3.PutDouble(StrToID("warpValue"), 0);
            Desc3.PutDouble(StrToID("warpPerspective"), 0);
            Desc3.PutDouble(StrToID("warpPerspectiveOther"), 0);
            Desc3.PutEnumerated(StrToID("warpRotate"), (int)con.phTypeOrientation, (int)con.phEnumHorizontal);

            // Desc2 
            var Desc2 = this.m_app.MakeDescriptor();

            Desc2.PutString((int)con.phKeyText, text);
            Desc2.PutObject(StrToID("warp"), StrToID("warp"), Desc3);
            Desc2.PutObject((int)con.phKeyTextClickPoint, (int)con.phClassPoint, Desc4);
            Desc2.PutEnumerated(StrToID("textGridding"), StrToID("textGridding"), (int)con.phEnumNone);
            Desc2.PutEnumerated((int)con.phKeyOrientation, (int)con.phTypeOrientation, (int)con.phEnumHorizontal);
            Desc2.PutEnumerated((int)con.phKeyAntiAlias, (int)con.phTypeAntiAlias, (int)con.phEnumAntiAliasCrisp);
            Desc2.PutList(StrToID("textShape"), List1);
            Desc2.PutList((int)con.phKeyTextStyleRange, List2);
            Desc2.PutList(StrToID("paragraphStyleRange"), List3);

            // Ref1 
            var Ref1 = this.m_app.MakeReference();

            Ref1.PutClass((int)con.phClassTextLayer);

            // Desc1 
            var Desc1 = this.m_app.MakeDescriptor();

            Desc1.PutReference((int)con.phKeyNull, Ref1);
            Desc1.PutObject((int)con.phKeyUsing, (int)con.phClassTextLayer, Desc2);


            int old_layer_count = GetLayerCount(-1);
            // Play the event in photoshop
            PlayEvent((int)con.phEventMake, Desc1, (int)con.phDialogSilent, PlayBehavior.checkresult);

            CheckLayerCount(-1, old_layer_count + 1);
        }


        public void CreateRectangleShape(double left, double top, double right, double bottom)
        {


            // Desc3 
            var Desc3 = this.m_app.MakeDescriptor();
            Desc3.PutUnitDouble((int)con.phKeyTop, (int)con.phUnitDistance, top);
            Desc3.PutUnitDouble((int)con.phKeyLeft, (int)con.phUnitDistance, left);
            Desc3.PutUnitDouble((int)con.phKeyBottom, (int)con.phUnitDistance, bottom);
            Desc3.PutUnitDouble((int)con.phKeyRight, (int)con.phUnitDistance, right);

            // Desc2 
            var Desc2 = this.m_app.MakeDescriptor();

            Desc2.PutClass((int)con.phKeyType, StrToID("solidColorLayer"));
            Desc2.PutObject((int)con.phKeyShape, (int)con.phClassRectangle, Desc3);

            // Ref1 
            var Ref1 = this.m_app.MakeReference();

            Ref1.PutClass(StrToID("contentLayer"));

            // Desc1 
            var Desc1 = this.m_app.MakeDescriptor();

            Desc1.PutReference((int)con.phKeyNull, Ref1);
            Desc1.PutObject((int)con.phKeyUsing, StrToID("contentLayer"), Desc2);

            // Play the event in photoshop
            PlayEvent((int)con.phEventMake, Desc1, (int)con.phDialogSilent, PlayBehavior.checknone);
        }



        public PhotoshopTypeLibrary.IActionDescriptor CreateRGBColorDescriptor(System.Drawing.Color c1)
        {
            return CreateRGBColorDescriptor(c1.R, c1.G, c1.B);
        }


        public PhotoshopTypeLibrary.IActionDescriptor CreateRGBColorDescriptor(int red, int green, int blue)
        {

            // Desc2 
            var Desc2 = this.m_app.MakeDescriptor();

            Desc2.PutDouble((int)con.phKeyRed, red);
            Desc2.PutDouble((int)con.phKeyGrain, green);
            Desc2.PutDouble((int)con.phKeyBlue, blue);

            return Desc2;
        }

        public void SetForegroundColor(int red, int green, int blue)
        {

            // Desc2 
            PhotoshopTypeLibrary.IActionDescriptor Desc2 = CreateRGBColorDescriptor(red, green, blue);

            __SetAppColor((int)con.phKeyForegroundColor, Desc2);
        }

        public void SetForegroundColor(System.Drawing.Color c1)
        {

            // Desc2 
            PhotoshopTypeLibrary.IActionDescriptor Desc2 = CreateRGBColorDescriptor(c1);

            __SetAppColor((int)con.phKeyForegroundColor, Desc2);
        }

        public void SetBackgroundColor(System.Drawing.Color c1)
        {

            // Desc2 
            var Desc2 = CreateRGBColorDescriptor(c1);

            __SetAppColor((int)con.phKeyBackgroundColor, Desc2);
        }


        public void __SetAppColor(int color_id, PhotoshopTypeLibrary.IActionDescriptor RGBCOLORDESC)
        {
            CheckEnum(color_id, (int)con.phKeyForegroundColor, (int)con.phKeyBackgroundColor);

            // Ref1 
            var Ref1 = this.m_app.MakeReference();

            Ref1.PutProperty((int)con.phClassColor, color_id);

            // Desc1 
            var Desc1 = this.m_app.MakeDescriptor();

            Desc1.PutReference((int)con.phKeyNull, Ref1);
            Desc1.PutObject((int)con.phKeyTo, (int)con.phClassRGBColor, RGBCOLORDESC);

            // Play the event in photoshop
            PlayEvent((int)con.phEventSet, Desc1, (int)con.phDialogSilent, PlayBehavior.checknone);
        }
        public void SetColorEx(int color_id, System.Drawing.Color c)
        {

            double[] v = { c.R, c.G, c.B, 0 };
            SetColorEx(color_id, (int)con.phClassRGBColor, v);

        }
        public void SetColorEx(int color_id, int color_class, double[] v)
        {

            var Desc2 = this.m_app.MakeDescriptor();

            if (color_class == (int)con.phClassHSBColor)
            {
                Desc2.PutUnitDouble((int)con.phKeyHue, (int)con.phUnitAngle, v[0]);
                Desc2.PutDouble((int)con.phKeySaturation, v[1]);
                Desc2.PutDouble((int)con.phKeyBrightness, v[2]);
            }
            else if (color_class == (int)con.phClassRGBColor)
            {
                Desc2.PutDouble((int)con.phKeyRed, v[0]);
                Desc2.PutDouble((int)con.phKeyGrain, v[1]);
                Desc2.PutDouble((int)con.phKeyBlue, v[2]);
            }
            else if (color_class == (int)con.phClassLabColor)
            {
                Desc2.PutDouble((int)con.phKeyLuminance, v[0]);
                Desc2.PutDouble((int)con.phKeyA, v[1]);
                Desc2.PutDouble((int)con.phKeyB, v[2]);
            }
            else if (color_class == (int)con.phClassCMYKColor)
            {
                Desc2.PutDouble((int)con.phKeyCyan, v[0]);
                Desc2.PutDouble((int)con.phKeyMagenta, v[1]);
                Desc2.PutDouble((int)con.phKeyYellow, v[2]);
                Desc2.PutDouble((int)con.phKeyBlack, v[3]);
            }
            else
            {
                var e = new PhotoshoProxyError("Improper Color Class");
                throw e;
            }

            var Ref1 = this.m_app.MakeReference();
            Ref1.PutProperty((int)con.phClassColor, color_id);

            var Desc1 = this.m_app.MakeDescriptor();
            Desc1.PutReference((int)con.phKeyNull, Ref1);
            Desc1.PutObject((int)con.phKeyTo, color_class, Desc2);


            // ----------------------------------------
            // Play the event
            // ----------------------------------------
            PlayEvent((int)con.phEventSet, Desc1, (int)con.phDialogSilent, PlayBehavior.checkresult);

        }

        public void GetColorEx(int color_id, out int color_class_id, ref double[] v)
        {

            PhotoshopTypeLibrary.IActionDescriptor desc2;
            CheckEnum(color_id, (int)con.phKeyForegroundColor, (int)con.phKeyBackgroundColor);

            IActionDescriptor desc = get_descriptor_to_object_property_by_index((int)con.phClassApplication, -1, color_id);

            int actual_color_class_id;
            desc.GetObject((int)con.phKeyForegroundColor, out actual_color_class_id, out desc2);



            if (actual_color_class_id == (int)con.phClassRGBColor)
            {
                color_class_id = (int)con.phClassRGBColor;
                v[0] = (double)get_value_from_descriptor(desc2, (int)con.phKeyRed);
                v[1] = (double)get_value_from_descriptor(desc2, (int)con.phKeyGreen);
                v[2] = (double)get_value_from_descriptor(desc2, (int)con.phKeyBlue);
                v[3] = 0;
            }
            else
            {
                string msg = string.Format("Did not understand color type returned");
                var e = new PhotoshoProxyError(msg);
                throw e;
            }

        }

        public void GetColorEx(int color_id, out int color_class_id, out System.Drawing.Color c)
        {

            double[] v = new double[4];
            GetColorEx((int)con.phKeyForegroundColor, out color_class_id, ref v);
            c = System.Drawing.Color.FromArgb((int)v[0], (int)v[1], (int)v[2]);

        }

        public void GaussianBlur(double radius)
        {
            CheckRange(radius, 0, 250);

            // Desc1 
            var Desc1 = this.m_app.MakeDescriptor();

            Desc1.PutUnitDouble((int)con.phKeyRadius, (int)con.phUnitPixels, radius);

            // Play the event in photoshop
            PlayEvent((int)con.phEventGaussianBlur, Desc1, (int)con.phDialogSilent, PlayBehavior.checkresult);
        }

        public void AddNoise(int distortion, double amount, int monochromatic)
        {

            CheckEnum(distortion, (int)con.phEnumUniformDistribution, (int)con.phEnumGaussianDistribution);
            CheckRange(amount, 0, 400);
            CheckEnum(monochromatic, TRUE, FALSE);
            // Desc1 
            var Desc1 = this.m_app.MakeDescriptor();

            Desc1.PutEnumerated((int)con.phKeyDistortion, (int)con.phTypeDistribution, distortion);
            Desc1.PutUnitDouble((int)con.phKeyNoise, (int)con.phUnitPercent, amount);
            Desc1.PutBoolean((int)con.phKeyMonochromatic, monochromatic);

            // Play the event in photoshop
            PlayEvent((int)con.phEventAddNoise, Desc1, (int)con.phDialogSilent, PlayBehavior.checkresult);
        }

        public void UnsharpMask(double amount, double radius, int threshold)
        {

            CheckRange(amount, 0, 400);
            CheckRange(radius, 0, 250);
            CheckRange(threshold, 0, 255);

            // Desc1 
            var Desc1 = this.m_app.MakeDescriptor();

            Desc1.PutUnitDouble((int)con.phKeyAmount, (int)con.phUnitPercent, amount);
            Desc1.PutUnitDouble((int)con.phKeyRadius, (int)con.phUnitPixels, radius);
            Desc1.PutInteger((int)con.phKeyThreshold, threshold);

            // Play the event in photoshop
            PlayEvent((int)con.phEventUnsharpMask, Desc1, (int)con.phDialogSilent, PlayBehavior.checkresult);
        }



        public void CreateLayer(string layer_name)
        {

            CheckStringContents(layer_name);

            // Desc2 
            var Desc2 = this.m_app.MakeDescriptor();

            Desc2.PutString((int)con.phKeyName, layer_name);

            // Ref1 
            var Ref1 = this.m_app.MakeReference();

            Ref1.PutClass((int)con.phClassLayer);

            // Desc1 
            var Desc1 = this.m_app.MakeDescriptor();

            Desc1.PutReference((int)con.phKeyNull, Ref1);
            Desc1.PutBoolean(StrToID("below"), FALSE);
            Desc1.PutObject((int)con.phKeyUsing, (int)con.phClassLayer, Desc2);

            int old_layer_count = GetLayerCount(-1);
            // Play the event in photoshop
            PlayEvent((int)con.phEventMake, Desc1, (int)con.phDialogSilent, PlayBehavior.checkresult);

            CheckLayerCount(-1, old_layer_count + 1);

        }

        public int GetLayerCount(int doc_index)
        {
            ///
            /// <summary>
            ///		Returns the number of documents open
            /// </summary>
            ///

            return (int)get_value_from_object((int)con.phClassDocument, doc_index, (int)con.phKeyNumberOfLayers);
        }

        public void CheckLayerCount(int doc_index, int expected_count)
        {
            ///
            /// <summary>
            /// Raises an exception of the document count is not what is expected
            ///	Workitem: Move some of this checking into PSX
            /// </summary>
            ///

            int actual_layer_count = GetLayerCount(doc_index);
            if (actual_layer_count != expected_count)
            {
                string msg = string.Format("Expected layer count of {0}, got {1} instead", actual_layer_count, expected_count);
                throw (new PhotoshoProxyError(msg));
            }
        }



        public void SelectLayer()
        {

            // Ref1 
            var Ref1 = this.m_app.MakeReference();

            Ref1.PutEnumerated((int)con.phClassChannel, (int)con.phTypeChannel, (int)con.phEnumMask);

            // Desc1 
            var Desc1 = this.m_app.MakeDescriptor();

            Desc1.PutReference((int)con.phKeyNull, Ref1);

            // Play the event in photoshop
            PlayEvent((int)con.phEventSelect, Desc1, (int)con.phDialogSilent, PlayBehavior.checknone);
        }

        public void RasterizeLayer(int layer_index)
        {


            // Ref1 
            var Ref1 = GetReferenceToLayerByIndex(layer_index);

            // Desc1 
            var Desc1 = this.m_app.MakeDescriptor();

            Desc1.PutReference((int)con.phKeyNull, Ref1);

            // Play the event in photoshop
            PlayEvent(StrToID("rasterizeLayer"), Desc1, (int)con.phDialogSilent, PlayBehavior.checkresult);
        }


        public void FlattenImage()
        {

            //PhotoshopTypeLibrary.IActionDescriptor Desc1 = this.m_app.MakeDescriptor();
            PhotoshopTypeLibrary.IActionDescriptor Desc1 = null;
            // Play the event in photoshop
            PlayEvent((int)con.phEventFlattenImage, Desc1, (int)con.phDialogSilent, PlayBehavior.checkresult);

            CheckLayerCount(-1, 0);
        }




        public void CreateSolidColorFillLayer(string layer_name, int red, int green, int blue)
        {


            // Desc4 
            var Desc4 = CreateDescriptorForRGBColor(red, green, blue);

            // Desc3 
            var Desc3 = this.m_app.MakeDescriptor();

            Desc3.PutObject((int)con.phKeyColor, (int)con.phClassRGBColor, Desc4);

            // Desc2 
            var Desc2 = this.m_app.MakeDescriptor();

            Desc2.PutString((int)con.phKeyName, layer_name);
            Desc2.PutObject((int)con.phKeyType, StrToID("solidColorLayer"), Desc3);

            // Ref1 
            var Ref1 = this.m_app.MakeReference();

            Ref1.PutClass(StrToID("contentLayer"));

            // Desc1 
            var Desc1 = this.m_app.MakeDescriptor();

            Desc1.PutReference((int)con.phKeyNull, Ref1);
            Desc1.PutObject((int)con.phKeyUsing, StrToID("contentLayer"), Desc2);

            // Play the event in photoshop
            PlayEvent((int)con.phEventMake, Desc1, (int)con.phDialogSilent, PlayBehavior.checkresult);
        }

        public PhotoshopTypeLibrary.IActionDescriptor CreateOpacityStop(double percent, int location, int midpoint)
        {
            var OpacityDesc = this.m_app.MakeDescriptor();
            OpacityDesc.PutUnitDouble((int)con.phKeyOpacity, (int)con.phUnitPercent, percent);
            OpacityDesc.PutInteger((int)con.phKeyLocation, location);
            OpacityDesc.PutInteger((int)con.phKeyMidpoint, midpoint);
            return OpacityDesc;
        }

        public PhotoshopTypeLibrary.IActionDescriptor CreateColorStop(System.Drawing.Color c, int location, int midpoint)
        {
            var Desc5 = this.m_app.MakeDescriptor();
            Desc5.PutObject((int)con.phKeyColor, (int)con.phClassRGBColor, CreateDescriptorForRGBColor(c));
            Desc5.PutEnumerated((int)con.phKeyType, (int)con.phTypeColorStopType, (int)con.phEnumUserStop);
            Desc5.PutInteger((int)con.phKeyLocation, location);
            Desc5.PutInteger((int)con.phKeyMidpoint, midpoint);
            return Desc5;
        }


        public PhotoshopTypeLibrary.IActionDescriptor[] CreateOpacityStops(double[] OA)
        {
            const int max_location = 4096;
            const int min_location = 0;
            System.Diagnostics.Debug.Assert(OA.Length > 1);

            var opacity_stops = new PhotoshopTypeLibrary.IActionDescriptor[OA.Length];
            int count = 0;
            int step = max_location / (OA.Length - 1);
            foreach (double O in OA)
            {
                System.Diagnostics.Debug.Assert(O <= 100);
                System.Diagnostics.Debug.Assert(O >= 0);

                int location = max_location - (step * count);
                System.Diagnostics.Debug.Assert(location <= max_location);
                System.Diagnostics.Debug.Assert(location >= min_location);

                const int midpoint = 50;
                opacity_stops[count] = CreateOpacityStop(O, location, midpoint);
                count++;

            }
            return opacity_stops;
        }

        public PhotoshopTypeLibrary.IActionDescriptor[] CreateColorStops(System.Drawing.Color[] CA)
        {
            const int max_location = 4096;
            const int min_location = 0;
            System.Diagnostics.Debug.Assert(CA.Length > 1);

            var color_stops = new PhotoshopTypeLibrary.IActionDescriptor[CA.Length];
            int count = 0;
            int step = max_location / (CA.Length - 1);
            foreach (System.Drawing.Color C in CA)
            {
                int location = max_location - (step * count);
                System.Diagnostics.Debug.Assert(location <= max_location);
                System.Diagnostics.Debug.Assert(location >= min_location);

                const int midpoint = 50;
                color_stops[count] = CreateColorStop(C, location, midpoint);
                count++;

            }
            return color_stops;
        }

        public void CreateGradientFillLayer(string layer_name, int gradient_type, double angle, PhotoshopTypeLibrary.IActionDescriptor[] opacity_stops, PhotoshopTypeLibrary.IActionDescriptor[] color_stops)
        {

            CheckRange(angle, 0, 360);
            CheckEnum(gradient_type, (int)con.phEnumLinear, (int)con.phEnumRadial);
            string gradient_name = "Custom Gradient";

            // List2 
            var List2 = this.m_app.MakeList();
            List2.PutObjects((int)con.phClassTransparencyStop, opacity_stops);

            // List1 
            var List1 = this.m_app.MakeList();
            List1.PutObjects((int)con.phClassColorStop, color_stops);

            // Desc4 
            var Desc4 = this.m_app.MakeDescriptor();


            Desc4.PutString((int)con.phKeyName, gradient_name);
            Desc4.PutEnumerated(1198679110, (int)con.phTypeGradientForm, (int)con.phEnumCustomStops);
            Desc4.PutDouble((int)con.phKeyInterfaceIconFrameDimmed, 4096);
            Desc4.PutList((int)con.phKeyColors, List1);
            Desc4.PutList((int)con.phKeyTransparency, List2);

            // Desc3 
            var Desc3 = this.m_app.MakeDescriptor();


            Desc3.PutUnitDouble((int)con.phKeyAngle, (int)con.phUnitAngle, angle);
            Desc3.PutEnumerated((int)con.phKeyType, (int)con.phTypeGradientType, gradient_type);
            Desc3.PutObject((int)con.phKeyGradient, (int)con.phClassGradient, Desc4);

            // Desc2 
            var Desc2 = this.m_app.MakeDescriptor();


            Desc2.PutString((int)con.phKeyName, layer_name);
            Desc2.PutObject((int)con.phKeyType, StrToID("gradientLayer"), Desc3);

            // Ref1 
            var Ref1 = this.m_app.MakeReference();

            Ref1.PutClass(StrToID("contentLayer"));

            // Desc1 
            var Desc1 = this.m_app.MakeDescriptor();

            Desc1.PutReference((int)con.phKeyNull, Ref1);
            Desc1.PutObject((int)con.phKeyUsing, StrToID("contentLayer"), Desc2);

            // Play the event in photoshop
            PlayEvent((int)con.phEventMake, Desc1, (int)con.phDialogSilent, PlayBehavior.checkresult);
        }


        public void CreateLayerFromBackground(string layer_name, double opacity_percent, int blend_mode)
        {


            // Desc2 
            var Desc2 = this.m_app.MakeDescriptor();

            Desc2.PutString((int)con.phKeyName, layer_name);
            Desc2.PutUnitDouble((int)con.phKeyOpacity, (int)con.phUnitPercent, opacity_percent);
            Desc2.PutEnumerated((int)con.phKeyMode, (int)con.phTypeBlendMode, blend_mode);

            // Ref1 
            var Ref1 = this.m_app.MakeReference();

            Ref1.PutProperty((int)con.phClassLayer, (int)con.phKeyBackground);

            // Desc1 
            var Desc1 = this.m_app.MakeDescriptor();

            Desc1.PutReference((int)con.phKeyNull, Ref1);
            Desc1.PutObject((int)con.phKeyTo, (int)con.phClassLayer, Desc2);

            // Play the event in photoshop
            PlayEvent((int)con.phEventSet, Desc1, (int)con.phDialogSilent, PlayBehavior.checkresult);
        }

        // A list of all the document properties for easy eumeration

        public PhotoshopTypeLibrary.IActionReference GetReferenceToLayerByIndex(int layer_index)
        {
            PhotoshopTypeLibrary.IActionReference Ref1 = get_reference_to_object((int)con.phClassLayer, layer_index);
            return Ref1;
        }


        public void DeleteLayer(int layer_index)
        {

            // Ref1 
            var Ref1 = GetReferenceToLayerByIndex(layer_index);

            // Desc1 
            var Desc1 = this.m_app.MakeDescriptor();

            Desc1.PutReference((int)con.phKeyNull, Ref1);

            // Play the event in photoshop
            PlayEvent((int)con.phEventDelete, Desc1, (int)con.phDialogSilent, PlayBehavior.checkresult);
        }


        public void DuplicateLayer(int layer_index, string new_name)
        {

            // Ref1 
            var Ref1 = GetReferenceToLayerByIndex(layer_index);

            // Desc1 
            var Desc1 = this.m_app.MakeDescriptor();

            Desc1.PutReference((int)con.phKeyNull, Ref1);
            Desc1.PutString((int)con.phKeyName, new_name);

            // Play the event in photoshop
            PlayEvent((int)con.phEventDuplicate, Desc1, (int)con.phDialogSilent, PlayBehavior.checkresult);
        }



        public static int[] LayerProperties = { };


        public void __SetSelection(int item_to_select)
        {

            // Ref1 
            var Ref1 = this.m_app.MakeReference();

            Ref1.PutProperty((int)con.phClassChannel, 1718838636);

            // Desc1 
            var Desc1 = this.m_app.MakeDescriptor();

            Desc1.PutReference((int)con.phKeyNull, Ref1);
            Desc1.PutEnumerated((int)con.phKeyTo, (int)con.phTypeOrdinal, item_to_select);

            // Play the event in photoshop
            PlayEvent((int)con.phEventSet, Desc1, (int)con.phDialogSilent, PlayBehavior.checkresult);
        }

        public void SelectAll()
        {
            __SetSelection((int)con.phEnumAll);
        }

        public void SelectNone()
        {
            __SetSelection((int)con.phEnumNone);
        }

        public void SelectPrevious()
        {
            __SetSelection((int)con.phEnumPrevious);
        }

        public void InvertSelection()
        {
            var Desc1 = this.m_app.MakeDescriptor();

            // Play the event in photoshop
            PlayEvent((int)con.phEventInverse, Desc1, (int)con.phDialogSilent, PlayBehavior.checkresult);
        }

        public void SelectRectangle(int left, int top, int right, int bottom, bool anti_alias)
        {
            __SelectShape(left, top, right, bottom, (int)con.phClassRectangle, anti_alias, (int)con.phEventSet);
        }

        public void SelectEllipse(int left, int top, int right, int bottom, bool anti_alias)
        {
            __SelectShape(left, top, right, bottom, (int)con.phClassEllipse, anti_alias, (int)con.phEventSet);
        }

        public void AddRectangle(int left, int top, int right, int bottom, bool anti_alias)
        {
            __SelectShape(left, top, right, bottom, (int)con.phClassRectangle, anti_alias, (int)con.phEventAddTo);
        }

        public void AddEllipse(int left, int top, int right, int bottom, bool anti_alias)
        {
            __SelectShape(left, top, right, bottom, (int)con.phClassEllipse, anti_alias, (int)con.phEventAddTo);
        }

        public void SubtractRectangle(int left, int top, int right, int bottom, bool anti_alias)
        {
            __SelectShape(left, top, right, bottom, (int)con.phClassRectangle, anti_alias, (int)con.phEventSubtractFrom);
        }

        public void SubtractEllipse(int left, int top, int right, int bottom, bool anti_alias)
        {
            __SelectShape(left, top, right, bottom, (int)con.phClassEllipse, anti_alias, (int)con.phEventSubtractFrom);
        }


        private void __SelectShape(int left, int top, int right, int bottom, int shape, bool anti_alias, int action_event)
        {

            // WORKITEM: Support con.phEventIntersectWith as the action_event

            // Desc2 
            var Desc2 = CreateDescriptorForRectangle(left, top, right, bottom);

            // Ref1 
            var Ref1 = this.m_app.MakeReference();

            Ref1.PutProperty((int)con.phClassChannel, 1718838636);

            // Desc1 
            var Desc1 = this.m_app.MakeDescriptor();

            Desc1.PutReference((int)con.phKeyNull, Ref1);
            Desc1.PutObject((int)con.phKeyTo, shape, Desc2);
            Desc1.PutBoolean((int)con.phKeyAntiAlias, int_from_bool(anti_alias));

            // Play the event in photoshop
            PlayEvent(action_event, Desc1, (int)con.phDialogSilent, PlayBehavior.checkresult);
        }

        public void SelectShapeEx(System.Drawing.Rectangle r, int shape, bool anti_alias, int action_event)
        {

            // WORKITEM: Support con.phEventIntersectWith as the action_event

            // Desc2 
            var Desc2 = CreateDescriptorForRectangle(r);

            // Ref1 
            var Ref1 = this.m_app.MakeReference();

            Ref1.PutProperty((int)con.phClassChannel, 1718838636);

            // Desc1 
            var Desc1 = this.m_app.MakeDescriptor();

            Desc1.PutReference((int)con.phKeyNull, Ref1);
            Desc1.PutObject((int)con.phKeyTo, shape, Desc2);
            Desc1.PutBoolean((int)con.phKeyAntiAlias, int_from_bool(anti_alias));

            // Play the event in photoshop
            PlayEvent(action_event, Desc1, (int)con.phDialogSilent, PlayBehavior.checkresult);
        }


    }


}
