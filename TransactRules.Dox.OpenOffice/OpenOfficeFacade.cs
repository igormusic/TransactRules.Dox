using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Linq;
using uno;
using uno.util;
using unoidl.com.sun.star.beans;
using unoidl.com.sun.star.container;
using unoidl.com.sun.star.frame;
using unoidl.com.sun.star.lang;
using unoidl.com.sun.star.text;
using unoidl.com.sun.star.uno;
using unoidl.com.sun.star.util;

namespace TransactRules.Dox.OpenOffice
{
    class OpenOfficeFacade
    {

        public static IEnumerable<string> GetDocumentVariableNames(XComponent document) {
            var variableList = new List<string>();

            // get XTextFieldsSupplier and XBookmarksSupplier interfaces from document component
            XTextFieldsSupplier xTextFieldsSupplier = (XTextFieldsSupplier)document;
 
            // access the TextFields and the TextFieldMasters collections
            XNameAccess xNamedFieldMasters = xTextFieldsSupplier.getTextFieldMasters();


            return (from name in xNamedFieldMasters.getElementNames()
                    where name.StartsWith("com.sun.star.text.fieldmaster.User.")
                    select name).ToArray();

        }

        public static void SetUserVariables(XComponent document, IList<KeyValuePair<string,string>> keyValuePairs){

            XTextFieldsSupplier xTextFieldsSupplier = (XTextFieldsSupplier)document;
            XNameAccess xNamedFieldMasters = xTextFieldsSupplier.getTextFieldMasters();
            XEnumerationAccess xEnumeratedFields = xTextFieldsSupplier.getTextFields();

            foreach(var keyValue in keyValuePairs){

                Object fieldMaster = xNamedFieldMasters.getByName(keyValue.Key);
 
                // query the XPropertySet interface, we need to set the Content property
                XPropertySet xPropertySet = (XPropertySet) ((uno.Any) fieldMaster).Value;
 
                // insert the column value into field master
                xPropertySet.setPropertyValue("Content", new Any( keyValue.Value));    
            }

            


            XRefreshable xRefreshable = (XRefreshable) xEnumeratedFields;
            xRefreshable.refresh();        
        
        }

        public static void ConvertToPdf(XComponent document, string sourceFilter, string outputFile)
        {
                // save/export the document
                SaveDocument(sourceFilter, document,  PathConverter(outputFile));
        }

        public static XComponent LoadDocument(string inputFile)
        {
            var aLoader = CreateComponentLoader();
            var xComponent = InitDocument(aLoader, PathConverter(inputFile), "_blank");
            //Wait for loading

            //var retry = 0;

            //while (xComponent == null && retry <10)
            //{
            //    Thread.Sleep(retry * 500 +1000);
            //    retry++;
            //}
            return xComponent;
        }

        private static XComponentLoader CreateComponentLoader()
        {
            //StartOpenOffice();

            //Get a ComponentContext
            var xLocalContext = Bootstrap.bootstrap();
            //Get MultiServiceFactory
            var xRemoteFactory = (XMultiServiceFactory)xLocalContext.getServiceManager();
            //Get a CompontLoader
            var aLoader = (XComponentLoader)xRemoteFactory.createInstance("com.sun.star.frame.Desktop");
            return aLoader;
        }

        private static XComponent InitDocument(XComponentLoader aLoader, string file, string target)
        {
            var openProps = new PropertyValue[1];
            openProps[0] = new PropertyValue { Name = "Hidden", Value = new Any(true) };

            var xComponent = aLoader.loadComponentFromURL(
                file, target, 0,
                openProps);

            return xComponent;
        }
        private static void SaveDocument(string sourceFilter, XComponent xComponent,  string destinationFile)
        {
            var propertyValues = new PropertyValue[2];
            // Setting the flag for overwriting
            propertyValues[1] = new PropertyValue { Name = "Overwrite", Value = new Any(true) };
            //// Setting the filter name
            propertyValues[0] = new PropertyValue
            {
                Name = "FilterName",
                Value = new Any(sourceFilter)
            };
            ((XStorable)xComponent).storeToURL(destinationFile, propertyValues);
        }

        private static void StartOpenOffice()
        {
            var ps = Process.GetProcessesByName("soffice.exe");
            if (ps.Length != 0)
                throw new InvalidProgramException("OpenOffice not found.  Is OpenOffice installed?");
            if (ps.Length > 0)
                return;
            var p = new Process
            {
                StartInfo =
                {
                    Arguments = "-headless -nofirststartwizard",
                    FileName = "soffice.exe",
                    CreateNoWindow = true
                }
            };
            var result = p.Start();

            if (result == false)
                throw new InvalidProgramException("OpenOffice failed to start.");
        }

        private static string PathConverter(string file)
        {
            if (string.IsNullOrEmpty(file))
                throw new NullReferenceException("Null or empty path passed to OpenOffice");

            return String.Format("file:///{0}", file.Replace(@"\", "/"));
        }

        public static string ConvertExtensionToFilterType(string extension)
        {
            switch (extension)
            {
                case ".doc":
                case ".docx":
                case ".txt":
                case ".rtf":
                case ".html":
                case ".htm":
                case ".xml":
                case ".odt":
                case ".wps":
                case ".wpd":
                    return "writer_pdf_Export";
                case ".xls":
                case ".xlsb":
                case ".xlsx":
                case ".ods":
                    return "calc_pdf_Export";
                case ".ppt":
                case ".pptx":
                case ".odp":
                    return "impress_pdf_Export";

                default:
                    return null;
            }
        }
    }
}
