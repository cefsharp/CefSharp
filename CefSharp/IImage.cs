// Copyright © 2018 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using CefSharp.Enums;

namespace CefSharp
{
    /// <summary>
    /// Container for a single image represented at different scale factors.
    /// All image representations should be the same size in density independent pixel (DIP) units.
    /// For example, if the image at scale factor 1.0 is 100x100 pixels then the image at scale factor 2.0 should be 200x200 pixels -- both images will display with a DIP size of 100x100 units.
    /// The methods of this class must be called on the browser process UI thread. 
    /// </summary>
    public interface IImage
    {
        //Add a bitmap image representation for scaleFactor.	
        //bool AddBitmap(float scaleFactor, int pixel_width, int pixel_height, cef_color_type_t color_type, cef_alpha_type_t alpha_type, const void* pixel_data, size_t pixel_data_size );

        //Create a JPEG image representation for scaleFactor.
        //bool AddJPEG(float scaleFactor, const void* jpeg_data, size_t jpeg_data_size )= 0


        //Add a PNG image representation for scaleFactor.
        //bool AddPNG(float scaleFactor, const void* png_data, size_t png_data_size )= 0

        /// <summary>
        /// Returns the bitmap representation that most closely matches scaleFactor.
        /// </summary>
        /// <param name="scaleFactor">scale factor</param>
        /// <param name="colorType">color type</param>
        /// <param name="alphaType">alpha type</param>
        /// <param name="pixelWidth">pixel width</param>
        /// <param name="pixelHeight">pixel height</param>
        /// <returns>A stream represending the bitmap or null.</returns>
        byte[] GetAsBitmap(float scaleFactor, ColorType colorType, AlphaType alphaType, out int pixelWidth, out int pixelHeight);

        /// <summary>
        /// Returns the JPEG representation that most closely matches scaleFactor.
        /// </summary>
        /// <param name="scaleFactor">scale factor</param>
        /// <param name="quality">image quality</param>
        /// <param name="pixelWidth">pixel width</param>
        /// <param name="pixelHeight">pixel height</param>
        /// <returns>A stream representing the JPEG or null.</returns>
        byte[] GetAsJPEG(float scaleFactor, int quality, out int pixelWidth, out int pixelHeight);

        /// <summary>
        /// Returns the PNG representation that most closely matches scaleFactor.
        /// </summary>
        /// <param name="scaleFactor">scale factor</param>
        /// <param name="withTransparency">is the PNG transparent</param>
        /// <param name="pixelWidth">pixel width</param>
        /// <param name="pixelHeight">pixel height</param>
        /// <returns>A stream represending the PNG or null.</returns>
        byte[] GetAsPNG(float scaleFactor, bool withTransparency, out int pixelWidth, out int pixelHeight);

        /// <summary>
        /// Returns information for the representation that most closely matches scaleFactor.
        /// </summary>
        /// <param name="scaleFactor">scale factor</param>
        /// <param name="actualScaleFactor">actual scale factor</param>
        /// <param name="pixelWidth">pixel width</param>
        /// <param name="pixelHeight">pixel height</param>
        /// <returns>return if information found for scale factor</returns>
        bool GetRepresentationInfo(float scaleFactor, out float actualScaleFactor, out int pixelWidth, out int pixelHeight);

        /// <summary>
        /// Returns the image height in density independent pixel(DIP) units.
        /// </summary>
        int Height { get; }

        /// <summary>
        /// Returns true if this image contains a representation for scaleFactor.
        /// </summary>
        /// <param name="scaleFactor"></param>
        /// <returns></returns>
        bool HasRepresentation(float scaleFactor);

        /// <summary>
        /// Returns true if this Image is empty.
        /// </summary>
        bool IsEmpty { get; }

        /// <summary>
        /// Returns true if this Image and that Image share the same underlying storage.
        /// </summary>
        /// <param name="that">image to compare</param>
        /// <returns>returns true if share same underlying storage</returns>
        bool IsSame(IImage that);

        /// <summary>
        /// Removes the representation for scaleFactor.
        /// </summary>
        /// <param name="scaleFactor"></param>
        /// <returns>true for success</returns>
        bool RemoveRepresentation(float scaleFactor);

        /// <summary>
        /// Returns the image width in density independent pixel(DIP) units.
        /// </summary>
        int Width { get; }
    }
}
