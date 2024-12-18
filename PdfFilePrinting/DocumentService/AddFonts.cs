using PdfSharp.Fonts;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PdfFilePrinting.DocumentService
{
    public class MyFontResolver : IFontResolver
    {
        
        public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
        {

            var name = familyName.ToLower().TrimEnd('#');
            //Console.WriteLine($"Fontr11 {familyName}");
            switch (name)
            {
                case "times new roman":
                    if (isBold)
                    {
                        if (isItalic)
                            return new FontResolverInfo("Times#bi");
                        return new FontResolverInfo("Times#b");
                    }
                    if (isItalic)
                        return new FontResolverInfo("Times#i");
                    return new FontResolverInfo("Times#");
            }


            var r = PlatformFontResolver.ResolveTypeface(familyName, isBold, isItalic);
            if (r==null)
            {
                Console.WriteLine("font is null");
            }
            return r;
        }


        public byte[] GetFont(string faceName)
        {
            //Console.WriteLine($"Fontr2 {faceName}");
            switch (faceName)
            {
                case "Times#":
                    return FontHelper.Times;

                case "Times#b":
                    return FontHelper.TimesBold;

                case "Times#i":
                    return FontHelper.TimesItalic;

                case "Times#bi":
                    return FontHelper.TimesBoldItalic;
            }

            return null;
        }


        internal static MyFontResolver OurGlobalFontResolver = null;


        internal static void Apply()
        {

            if (OurGlobalFontResolver == null || GlobalFontSettings.FontResolver == null)
            {
                if (OurGlobalFontResolver == null)
                    OurGlobalFontResolver = new MyFontResolver();

                GlobalFontSettings.FontResolver = OurGlobalFontResolver;
            }
        }
    }



    public static class FontHelper
    {
        public static byte[] Times
        {
            get { return LoadFontData("PdfFilePrinting.Fonts.times.ttf"); }
        }

        public static byte[] TimesBold
        {
            get { return LoadFontData("PdfFilePrinting.Fonts.timesbd.ttf"); }
        }

        public static byte[] TimesItalic
        {
            get { return LoadFontData("PdfFilePrinting.Fonts.timesi.ttf"); }
        }

        public static byte[] TimesBoldItalic
        {
            get { return LoadFontData("PdfFilePrinting.Fonts.timesbi.ttf"); }
        }

        static byte[] LoadFontData(string name)
        {
            //Console.WriteLine($"Fontr3 {name}");
            var assembly = Assembly.GetExecutingAssembly();

            // Test code to find the names of embedded fonts
            //var ourResources = assembly.GetManifestResourceNames();

            using (Stream stream = assembly.GetManifestResourceStream(name))
            {
                if (stream == null)
                    throw new ArgumentException("No resource with name " + name);

                int count = (int)stream.Length;
                byte[] data = new byte[count];
                stream.Read(data, 0, count);
                return data;
            }
        }
    }



}
