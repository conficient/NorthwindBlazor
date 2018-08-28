using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthwindBlazor.Entities.Util
{
    public static class Converter
    {
        /// <summary>
        /// Extract image from OLE image data
        /// </summary>
        /// <param name="oleImageData">the raw data from a Northwind image column</param>
        /// <returns>image data as byte[]</returns>
        /// <remarks>
        /// Northwind stores images using an OLE image format. We can remove this 
        /// wrapper to get the original image. 
        /// source: https://blogs.msdn.microsoft.com/pranab/2008/07/15/removing-ole-header-from-images-stored-in-ms-access-db-as-ole-object/
        /// </remarks>
        public static byte[] GetImageBytesFromOLEField(byte[] oleFieldBytes)
        {
            const string BITMAP_ID_BLOCK = "BM";
            const string JPG_ID_BLOCK = "\u00FF\u00D8\u00FF";
            const string PNG_ID_BLOCK = "\u0089PNG\r\n\u001a\n";
            const string GIF_ID_BLOCK = "GIF8";
            const string TIFF_ID_BLOCK = "II*\u0000";

            byte[] imageBytes;

            // Get a UTF7 Encoded string version
            Encoding u8 = Encoding.UTF7;
            string strTemp = u8.GetString(oleFieldBytes);

            // Get the first 300 characters from the string
            string strVTemp = strTemp.Substring(0, 300);

            // Search for the block
            int iPos = -1;
            if (strVTemp.IndexOf(BITMAP_ID_BLOCK) != -1)
                iPos = strVTemp.IndexOf(BITMAP_ID_BLOCK);
            else if (strVTemp.IndexOf(JPG_ID_BLOCK) != -1)
                iPos = strVTemp.IndexOf(JPG_ID_BLOCK);
            else if (strVTemp.IndexOf(PNG_ID_BLOCK) != -1)
                iPos = strVTemp.IndexOf(PNG_ID_BLOCK);
            else if (strVTemp.IndexOf(GIF_ID_BLOCK) != -1)
                iPos = strVTemp.IndexOf(GIF_ID_BLOCK);
            else if (strVTemp.IndexOf(TIFF_ID_BLOCK) != -1)
                iPos = strVTemp.IndexOf(TIFF_ID_BLOCK);
            else
                throw new Exception("Unable to determine header size for the OLE Object");

            // From the position above get the new image
            if (iPos == -1)
                throw new Exception("Unable to determine header size for the OLE Object");

            //Array.Copy(
            imageBytes = new byte[oleFieldBytes.LongLength - iPos];
            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(oleFieldBytes, iPos, oleFieldBytes.Length - iPos);
                imageBytes = ms.ToArray();
                ms.Close();
                ms.Dispose();
            }

            return imageBytes;
        }


    }
}
