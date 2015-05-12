using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using ps = PhotoshopTypeLibrary;
using PhotoshopAuto.Extensions;
using con = PhotoshopTypeLibrary.PSConstants;

namespace DemoPhotoshop7Automation
{

    /*
     * tlbimp "C:\Program Files\Adobe\Adobe Photoshop CS4\TypeLibrary.tlb"
Microsoft (R) .NET Framework Type Library to Assembly Converter 3.5.30729.1
Copyright (C) Microsoft Corporation.  All rights reserved.

Type library imported to PhotoshopTypeLibrary.dll*/

    class Program
    {
        static void Main(string[] args)
        {

            var psp = new PhotoshopAuto.PhotoshopProxy();


            psp.CreateDocument(300, 200, "FOO");
            psp.OpenDocument(@"D:\\test.png");

            psp.GaussianBlur(5);
        }
    }



   
}


