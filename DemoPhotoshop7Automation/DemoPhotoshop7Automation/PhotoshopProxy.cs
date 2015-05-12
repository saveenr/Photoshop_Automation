using System;
using Photoshop;
using PhotoshopTypeLibrary;
using con = PhotoshopTypeLibrary.PSConstants;
using System.Collections.Generic;
using System.Linq;

namespace PhotoshopAuto
{

    public static class PE
    {
        public static void PutObjects(this PhotoshopTypeLibrary.IActionList list,  int type, IEnumerable<PhotoshopTypeLibrary.IActionDescriptor> DA)
        {
            foreach (var D in DA)
            {
                list.PutObject(type, D);
            }
        }

        public static PhotoshopTypeLibrary.IActionDescriptor GetActionProperty(this PhotoshopTypeLibrary.IActionControl ctrl, IActionReference ref1)
        {
            PhotoshopTypeLibrary.IActionDescriptor result;
            ctrl.GetActionProperty(ref1, out result);
            return result;
        }

    }

    public partial class PhotoshopProxy
    {

        /// <summary>
        ///	This class provides communication services between APIs and the
        ///	PhotoShop COM object
        /// </summary>
        /// 


        protected PhotoshopTypeLibrary.IPhotoshopApplication m_app;
        protected PhotoshopTypeLibrary.IActionControl m_control;
        public static ProxyLog log = new ProxyLog();
        protected Photoshop.Application m_app2;

        public enum PlayBehavior
        {
            checkresult, checknonresult, checknone
        }


        public PhotoshopProxy()
        {
            /*
             * Ensure that the static members are initialized correctly
             * 
             * */

            m_app2 = new Application();
            m_app = new PhotoshopTypeLibrary.PhotoshopApplication();
            m_app.Visible = 1;
            
            m_control = m_app.MakeControlObject();

            //WORKITEM: throw exception if either of these are null
        }


        public int StrToID(string id)
        {
            // Converts the given string ID into a numeric ID

            int v;
            m_control.StringIDToTypeID(id, out v);
            return v;
        }

        public string IDToStr(int id)
        {
            // Converts a numeric ID to a string ID

            var xx = PSConstants.phKeyForegroundColor;

            string text;
            text = PhotoshopTypeLibrary.PSConstants.GetName(xx.GetType(), id);

            // WORKITEM: Why not use m_control.TypeIDToStringID() ?

            return text;
        }

        public string GetNameFromID(int id, string prefix)
        {
            /*
            Converts the given id to a string and capitilizes the first letter

            prefix	- Add a prefix to the string to be returned

            Example:

                s1 = GetNameFromID( con.phKeyMessage )
                # s1 == "Message"
				
                s2 = GetNameFromID( con.phKeyMessage, "phKey" )
                # s2 == "phKeyMessage"

                s3 = GetNameFromID( somewierdnumber, "phClass" )
                # s3 == "phClassUNKNOWN"
			
            */
            string s = IDToStr(id);

            if ((s == null) || (s.Length < 1))
            {
                // If the Photoshop Object comes backwith a zero-length string we give
                //a dummy name
                s = "UNKNOWN";
            }
            string id_str = prefix + s;
            //WORKITEM: Capatilize the first letter of s;

            System.Diagnostics.Debug.Assert(id_str.Length > 0);

            return id_str;
        }

        public string GetNameFromEventID(int id)
        {
            return GetNameFromID(id, "phEvent");
        }

        public string GetNameFromKeyID(int id)
        {
            return GetNameFromID(id, "phKey");
        }

        public string GetNameFromTypeID(int id)
        {
            return GetNameFromID(id, "phType");
        }


        public void PlayEvent(int event_id, PhotoshopTypeLibrary.IActionDescriptor parameter_desc, int show_ui, PlayBehavior action)
        {

            /*event_id - the event to play
            parameter_descriptor - a descriptor containing the parameters for the event
            dialog_options - whether to show UI or not
            action - the kind of error checking to use. Possilble values are checkresult,checknonresult, and checknone


            checkresult and checknonresult will cause
            ExtensionError exceptions to be thrown if Photoshop
            says there is an error. Regardless of the value used
            for action, a number of ExtensionError exceptions may
            be thrown.

            */

            string event_id_str = GetNameFromEventID(event_id);
            //self.log.Header( "Play %s (0x%d) " % (event_id_str, event_id ) )


            PhotoshopTypeLibrary.IActionDescriptor result_desc = m_control.Play(event_id, parameter_desc, show_ui);

            // phKeyMessage may be present and contain more information about the event
            // store the value in msg
            string msg = null;


            if (result_desc != null)
            {
                object msg_o = get_value_from_descriptor(result_desc, (int)PSConstants.phKeyMessage);
                msg = (string)msg_o; ;

                dump_descriptor(result_desc, 0);
            }

            bool success = false;
            // Depending on the action there are different criterias for success
            if (action == PlayBehavior.checkresult)
            {
                // Succes is one of the following:
                // - Play() did not return a descriptor
                // - Play() did return a descriptor and that did not contain phKeyMessage
                success = ((result_desc == null) || ((result_desc != null) && (msg == null)));
            }
            else if (action == PlayBehavior.checknonresult)
            {
                // Success is:
                // - Play() did return a descriptor and there is a message
                success = ((result_desc != null) && (msg != null));
            }
            else
            {
                // action == self.checknone ) :
                // Whatever happened is success
                // checknone is for temporary code, callers should end up using checkresult or checknonresult
                success = true;
            }

            if (!success)
            {
                string error_msg = "Photoshop ExtensionError: Failure on Event " + event_id_str;
                if (msg != null)
                {
                    error_msg += msg;
                    var e = new PhotoshoProxyError(error_msg);
                    throw (e);
                }
            }

            //self.log.Header( "PLAY" )
            //self.DumpDescriptor( result_desc, 0)

        }


        private double points_to_pixels(float points)
        {
            double pixels = (points * 100.0 / 72.0);
            return pixels;
        }

        private double pixels_to_points(double pixels)
        {

            double points = (pixels * 72.0 / 100.0);
            return points;
        }



        public int get_type_from_descriptor(PhotoshopTypeLibrary.IActionDescriptor desc, int prop_id)
        {
            // Returns the type for a property stored in a descriptor

            int v;
            System.Diagnostics.Debug.Assert(desc != null);
            System.Diagnostics.Debug.Assert(desc_has_key(desc, prop_id));

            desc.GetType(prop_id, out v);
            return v;
        }

        public Object get_value_from_descriptor(PhotoshopTypeLibrary.IActionDescriptor desc, int prop_id)
        {
            ///
            ///<summary>
            ///
            /// Given a descriptor and a propid, will return an Object containing
            /// the value
            /// 
            /// If there is no prop_id, then returns null
            /// 
            /// Workitem: this should really return an exception if the property doesn't
            /// exist or it should return an additional error code.
            ///
            ///</summary>
            ///


            if (!desc_has_key(desc, prop_id))
            {
                // If the descriptor does not contain the key, return null
                return null;
            }

            // Stores the object being returned
            Object o = null;

            // Determine the type of the object
            int type = 0;
            type = get_type_from_descriptor(desc, prop_id);

            if (type == (int)PSConstants.phTypeChar)
            {
                string v;
                desc.GetString(prop_id, out v);
                o = (string)v;
            }
            else if (type == (int)PSConstants.phTypeInteger)
            {
                int v;
                desc.GetInteger(prop_id, out v);
                o = v;
            }
            else if (type == (int)PSConstants.phTypeFloat)
            {
                double v;
                desc.GetDouble(prop_id, out v);
                o = v;
            }
            else if (type == (int)PSConstants.phTypeBoolean)
            {
                int v;
                desc.GetBoolean(prop_id, out v);
                o = v;
            }
            else if (type == (int)PSConstants.phTypeUnitFloat)
            {
                // WORKITEM: Return an array instead
                int unit_id;
                double v;
                desc.GetUnitDouble(prop_id, out unit_id, out v);
                o = v;
            }
            else if (type == (int)PSConstants.phTypeEnumerated)
            {
                // WORKITEM: Return an array instead
                int enum_type;
                int enum_value;
                desc.GetEnumerated(prop_id, out enum_type, out enum_value);
                o = enum_value;
            }
            else if (type == (int)PSConstants.phTypeObject)
            {
                // WORKITEM: Return an array instead
                int class_id;
                PhotoshopTypeLibrary.IActionDescriptor v;
                desc.GetObject(prop_id, out class_id, out v);
                o = v;
            }
            else if (type == (int)PSConstants.phTypePath)
            {
                string v;
                desc.GetPath(prop_id, out v);
                o = v;
            }
            else
            {
                string type_name = GetNameFromTypeID(type);
                string msg = "Unsupported type " + type_name;
                var e = new PhotoshoProxyError(msg);
                throw (e);
            }


            return o;
        }


        public PhotoshopTypeLibrary.IActionReference get_reference_to_object_property_by_index(int obj_id, int obj_index, int prop_id)
        {
            if (prop_id < 0)
            {
                var e = new Exception("prop_id is invalid");
                throw e;
            }
            var ref1 = this.m_app.MakeReference();
            ref1.PutProperty((int)PSConstants.phClassProperty, prop_id); // NOTEL this must be put before the stuff below

            if (obj_index < 0)
            {
                // Negative numbers designate "no index"
                // If no index is given, this will get the "current" value
                ref1.PutEnumerated(obj_id, (int)PSConstants.phTypeOrdinal, (int)PSConstants.phEnumTarget);
            }
            else if ((obj_index == 0) && (obj_id == (int)PSConstants.phClassLayer))
            {
                // A special case, if the object is a layer then index zero refers to the background layer
                ref1.PutProperty((int)PSConstants.phClassLayer, (int)PSConstants.phKeyBackground);
            }
            else
            {
                // Ok an index is given so use it
                ref1.PutIndex(obj_id, obj_index);
            }

            return ref1;
        }

        public PhotoshopTypeLibrary.IActionDescriptor get_descriptor_to_object_property_by_index(int obj_id, int object_index, int prop_id)
        {

            var ref1 = get_reference_to_object_property_by_index(obj_id, object_index, prop_id);
            PhotoshopTypeLibrary.IActionDescriptor result;
            m_control.GetActionProperty(ref1, out result);
            return result;
        }


        public PhotoshopTypeLibrary.IActionReference get_reference_to_object(int obj_id, int obj_index)
        {

            var ref1 = this.m_app.MakeReference();

            if (obj_index < 0)
            {
                // Negative numbers designate "no index"
                // If no index is given, this will get the "current" value
                ref1.PutEnumerated(obj_id, (int)PSConstants.phTypeOrdinal, (int)PSConstants.phEnumTarget);
            }
            else if ((obj_index == 0) && (obj_id == (int)PSConstants.phClassLayer))
            {
                // A special case, if the object is a layer then index zero refers to the background layer
                ref1.PutProperty((int)PSConstants.phClassLayer, (int)PSConstants.phKeyBackground);
            }
            else
            {
                // Ok an index is given so use it
                ref1.PutIndex(obj_id, obj_index);
            }

            return ref1;
        }


        public void CheckPropID(int prop_id)
        {
            if (prop_id < 0)
            {
                var e = new Exception("prop_id is invalid");
                throw e;
            }
        }

        public Object get_value_from_object(int obj_classid, int object_index, int prop_id)
        {
            /*
            Returns an Object that represents the value in the descriptor.
			
            obj_id - id for object to get prop from
            prop_id - id for prop value to get
            index - (-1) = use current object, else use the object at the given index
            */

            var ref1 = get_reference_to_object_property_by_index(obj_classid, object_index, prop_id);
            PhotoshopTypeLibrary.IActionDescriptor result = m_control.GetActionProperty(ref1);
 
            Object retval = get_value_from_descriptor(result, prop_id);
            return retval;
        }



        public bool desc_has_key(PhotoshopTypeLibrary.IActionDescriptor desc, int key)
        {
            /*
                 * Utility function to quickly check whether a descriptor has a key
                 * 
                 * Usage:
                 * 
                 * if ( desc_has_key( desc1 , (int) con.phKeyForegroundColor ) )
                 * {
                 *		System.Console.WriteLine( "This descriptor has the key" );
                 * }
                */

            int hk;
            desc.HasKey(key, out hk);
            return (hk != 0);
        }

        public int get_count_from_object_collection(int object_id)
        {

            int count = 0;

            if (object_id == (int)PSConstants.phClassDocument)
            {
                count = (int)get_value_from_object((int)PSConstants.phClassApplication, -1, (int)PSConstants.phKeyNumberOfDocuments);
            }
            else if (object_id == (int)PSConstants.phClassLayer)
            {
                count = (int)get_value_from_object((int)PSConstants.phClassDocument, -1, (int)PSConstants.phKeyNumberOfLayers);
            }
            else
            {
                string msg = string.Format("Invalid object_id {0},{1}: only documents and layers are supported", object_id, IDToStr(object_id));
                var e = new PhotoshoProxyError(msg);
                throw e;
            }
            return count;
        }

        public void get_range_from_object_collection(int object_id, out int start, out int end)
        {

            // Given an object_id returns the number of objects of that type contained

            int count = get_count_from_object_collection(object_id);
            start = 1;
            end = count + 1;
        }

        public List<object> query_object_properties(int object_classid, int[] prop_ids)
        {
            int start;
            int end;

            // rowset will store the results
            var rowset = new List<object>();

            // Find out how many objects there are
            get_range_from_object_collection(object_classid, out start, out end);

            // Loop through each object in its collection
            for (int index = start; index < end; index++)
            {
                // Get the properties for this object
                // and then add it to the result set

                var row = get_values_from_object(object_classid, index, prop_ids);
                rowset.Add(row);
            }

            // Done, now return the result

            return rowset;
        }


        public List<object> get_values_from_object(int object_id, int object_index, int[] prop_ids)
        {
            // NOTE: The values are returned in the order that they appear in the prop_ids list

            // Create a new empty row, this will store all the properties
            var row = new List<object>();

            foreach (int prop_id in prop_ids)
            {
                // Add this property's value to the row
                object val = get_value_from_object(object_id, object_index, prop_id);
                row.Add(val);
            }

            // Done, now return the row
            return row;
        }



        public void dump_descriptor(PhotoshopTypeLibrary.IActionDescriptor desc1, int indent)
        {
            string indent_text = "  ";
            int num_items;
            desc1.GetCount(out  num_items);

            log.WriteLine("Descriptor ({0} items)", num_items);
            for (int i = 0; i < num_items; i++)
            {

                int child_key;
                int child_type;

                desc1.GetKey(i, out child_key);
                desc1.GetType(child_key, out child_type);

                string indent_str = " ";///commoncs.libstring.Multiply(indent_text, indent + 1);

                log.Write(indent_str);
                log.Write("[{0}] ", i.ToString());
                log.WriteLine(" ( {0} ) ", IDToStr(child_type));

                log.Write(indent_str + "-");

                if (child_type == (int)PSConstants.phTypeObject)
                {
                    PhotoshopTypeLibrary.IActionDescriptor child_desc;
                    int child_class_id;

                    desc1.GetObject(child_key, out child_class_id, out child_desc);

                    string sss = string.Format(" ( {0} ) ( {1} ) ", IDToStr(child_key), IDToStr(child_class_id));
                    log.WriteLine(sss);

                    dump_descriptor(child_desc, indent + 1);

                }
                else if (child_type == (int)PSConstants.phTypeObjectReference)
                {
                    log.WriteLine("<handled type>");
                }
                else if (child_type == (int)PSConstants.phTypeText)
                {
                    log.WriteLine("<handled type>");
                }
                else if (child_type == (int)PSConstants.phTypeBoolean)
                {
                    log.WriteLine("<handled type>");
                }
                else if (child_type == (int)PSConstants.phTypeEnumerated)
                {
                    log.WriteLine("<handled type>");
                }
                else if (child_type == (int)PSConstants.phTypeType)
                {
                    log.WriteLine("<handled type>");
                }
                else if (child_type == (int)PSConstants.phTypePath)
                {
                    log.WriteLine("<handled type>");
                }
                else
                {
                    log.WriteLine("<unhandled type>");
                }


            }
        }

        /*


        public  void XPutRectangleIntoDescriptor( PhotoshopTypeLibrary.IActionDescriptor d, double top, double left, double bottom, double right )
        {
            int unit = (int)con.phUnitDistance;
            d.PutUnitDouble( (int)con.phKeyTop, unit, top );
            d.PutUnitDouble( (int)con.phKeyLeft, unit, left );
            d.PutUnitDouble( (int)con.phKeyBottom, unit, bottom );
            d.PutUnitDouble( (int)con.phKeyRight, unit, right );
        }

        public  void XPutRectangleIntoDescriptorEx( PhotoshopTypeLibrary.IActionDescriptor d, double top, double left, double bottom, double right, int convert )
        {
            int unit = (int)con.phUnitDistance;
			
            if ( convert==1 )
            {
                // the input is pixels, convert to points
                top = pixels_to_points( top );
                left = pixels_to_points( left );
                bottom = pixels_to_points( bottom );
                right = pixels_to_points( right );
            }
            else
            {
                // convert convert the points
            }
			
            d.PutUnitDouble( (int)con.phKeyTop, unit, top );
            d.PutUnitDouble( (int)con.phKeyLeft, unit, left );
            d.PutUnitDouble( (int)con.phKeyBottom, unit, bottom );
            d.PutUnitDouble( (int)con.phKeyRight, unit, right );
        }



*/



        public void CheckEnumEx(int v, int[] enum_range)
        {
            bool found = false;
            foreach (int enum_value in enum_range)
            {
                if (v == enum_value)
                {
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                PhotoshoProxyError e = new PhotoshoProxyError("Not in range");
                throw (e);
            }

        }


        public void CheckEnum(int v, params int[] enum_range)
        {
            CheckEnumEx(v, enum_range);

        }

        public void CheckEnum(string v, params string[] enum_range)
        {
            bool found = false;
            foreach (string enum_value in enum_range)
            {
                if (v == enum_value)
                {
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                PhotoshoProxyError e = new PhotoshoProxyError("Not in range");
                throw (e);
            }

        }

        public void CheckFileExists(string fname)
        {
            if (System.IO.File.Exists(fname))
            {
                // do nothing
            }
            else
            {
                string msg = string.Format("File {0} does not exist", fname);
                throw (new PhotoshoProxyError(msg));
            }

        }

        public void CheckValidFilename(string fname)
        {
            if (fname.Length < 1)
            {

                string msg = string.Format("Filename \"{0}\" is not valid", fname);
                throw (new PhotoshoProxyError(msg));
            }

        }

        public void CheckRange(int v, int min, int max)
        {
            if ((v < min) || (v > max))
            {
                string msg = string.Format("Value \"{0}\" is not in range [{1},{2} ]", v, min, max);
                throw (new PhotoshoProxyError(msg));
            }
        }

        public void CheckRange(double v, double min, double max)
        {
            if ((v < min) || (v > max))
            {
                string msg = string.Format("Value \"{0}\" is not in range [{1},{2} ]", v, min, max);
                throw (new PhotoshoProxyError(msg));
            }
        }

        public void CheckStringContents(string s)
        {
            if (s == null)
            {
                string msg = string.Format("String is null");
                throw (new PhotoshoProxyError(msg));
            }

            if (s.Length < 1)
            {
                string msg = string.Format("String has zero length");
                throw (new PhotoshoProxyError(msg));

            }
        }


        public PhotoshopTypeLibrary.IActionDescriptor CreateDescriptorForRectangle(System.Drawing.Rectangle r)
        {
            var Desc = this.m_app.MakeDescriptor();
            Desc.PutUnitDouble((int)PSConstants.phKeyTop, (int)PSConstants.phUnitDistance, r.Top);
            Desc.PutUnitDouble((int)PSConstants.phKeyLeft, (int)PSConstants.phUnitDistance, r.Left);
            Desc.PutUnitDouble((int)PSConstants.phKeyBottom, (int)PSConstants.phUnitDistance, r.Bottom);
            Desc.PutUnitDouble((int)PSConstants.phKeyRight, (int)PSConstants.phUnitDistance, r.Right);
            return Desc;
        }


        public PhotoshopTypeLibrary.IActionDescriptor CreateDescriptorForRectangle(double left, double top, double right, double bottom)
        {
            var Desc = this.m_app.MakeDescriptor();
            Desc.PutUnitDouble((int)PSConstants.phKeyTop, (int)PSConstants.phUnitDistance, top);
            Desc.PutUnitDouble((int)PSConstants.phKeyLeft, (int)PSConstants.phUnitDistance, left);
            Desc.PutUnitDouble((int)PSConstants.phKeyBottom, (int)PSConstants.phUnitDistance, bottom);
            Desc.PutUnitDouble((int)PSConstants.phKeyRight, (int)PSConstants.phUnitDistance, right);
            return Desc;
        }

        public PhotoshopTypeLibrary.IActionDescriptor CreateDescriptorForRectangle(int left, int top, int right, int bottom)
        {
            var Desc = this.m_app.MakeDescriptor();
            Desc.PutUnitDouble((int)PSConstants.phKeyTop, (int)PSConstants.phUnitDistance, top);
            Desc.PutUnitDouble((int)PSConstants.phKeyLeft, (int)PSConstants.phUnitDistance, left);
            Desc.PutUnitDouble((int)PSConstants.phKeyBottom, (int)PSConstants.phUnitDistance, bottom);
            Desc.PutUnitDouble((int)PSConstants.phKeyRight, (int)PSConstants.phUnitDistance, right);
            return Desc;
        }

        public PhotoshopTypeLibrary.IActionDescriptor CreateDescriptorForRGBColor(System.Drawing.Color C)
        {
            var Desc = this.m_app.MakeDescriptor();
            Desc.PutDouble((int)PSConstants.phKeyRed, C.R);
            Desc.PutDouble((int)PSConstants.phKeyGrain, C.G);
            Desc.PutDouble((int)PSConstants.phKeyBlue, C.B);
            return Desc;
        }
        public PhotoshopTypeLibrary.IActionDescriptor CreateDescriptorForRGBColor(int red, int green, int blue)
        {
            var Desc = this.m_app.MakeDescriptor();
            Desc.PutDouble((int)PSConstants.phKeyRed, red);
            Desc.PutDouble((int)PSConstants.phKeyGrain, green);
            Desc.PutDouble((int)PSConstants.phKeyBlue, blue);
            return Desc;
        }



        public static int int_from_bool(bool v)
        {
            if (v) { return TRUE; }
            else { return FALSE; }
        }

        public static int[] blend_modes = { (int)PSConstants.phEnumDifference, (int)PSConstants.phEnumDarken, (int)PSConstants.phEnumNormal };

        public static int FALSE = 0;
        public static int TRUE = 1;

    }

}
