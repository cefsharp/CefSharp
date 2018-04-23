// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

#include "include\cef_image.h"
#include "CefWrapper.h"

using namespace System::IO;
using namespace CefSharp::Enums;

namespace CefSharp
{
    namespace Internals
    {
        private ref class CefImageWrapper : public IImage, public CefWrapper
        {
        internal:
            MCefRefPtr<CefImage> _image;

            CefImageWrapper::CefImageWrapper(CefRefPtr<CefImage> &image)
                : _image(image)
            {
            }

            !CefImageWrapper()
            {
                _image = NULL;
            }

            ~CefImageWrapper()
            {
                this->!CefImageWrapper();

                _disposed = true;
            }

        public:
            /// <summary>
            /// Returns the bitmap representation that most closely matches scaleFactor.
            /// </summary>
            /// <param name="scaleFactor">scale factor</param>
            /// <param name="colorType">color type</param>
            /// <param name="alphaType">alpha type</param>
            /// <param name="pixelWidth">pixel width</param>
            /// <param name="pixelHeight">pixel height</param>
            /// <returns>A stream represending the bitmap or null.</returns>
            virtual cli::array<Byte>^ GetAsBitmap(float scaleFactor, ColorType colorType, AlphaType alphaType, int% pixelWidth, int% pixelHeight)
            {
                int width;
                int height;

                auto binary = _image->GetAsBitmap(scaleFactor, (cef_color_type_t)colorType, (cef_alpha_type_t) alphaType, width, height);

                if(binary.get())
                {
                    pixelWidth = width;
                    pixelHeight = height;

                    auto binarySize = binary->GetSize();

                    auto buffer = gcnew cli::array<Byte>(binarySize);
                    pin_ptr<Byte> src = &buffer[0]; // pin pointer to first element in arr

                    binary->GetData(static_cast<void*>(src), binarySize, 0);

                    return buffer;
                }

                return nullptr;
            }

            /// <summary>
            /// Returns the JPEG representation that most closely matches scaleFactor.
            /// </summary>
            /// <param name="scaleFactor">scale factor</param>
            /// <param name="quality">image quality</param>
            /// <param name="pixelWidth">pixel width</param>
            /// <param name="pixelHeight">pixel height</param>
            /// <returns>A stream representing the JPEG or null.</returns>
            virtual cli::array<Byte>^ GetAsJPEG(float scaleFactor, int quality, int% pixelWidth, int% pixelHeight)
            {
                int width;
                int height;

                auto binary = _image->GetAsJPEG(scaleFactor, quality, width, height);

                if (binary.get())
                {
                    pixelWidth = width;
                    pixelHeight = height;

                    auto binarySize = binary->GetSize();

                    auto buffer = gcnew cli::array<Byte>(binarySize);
                    pin_ptr<Byte> src = &buffer[0]; // pin pointer to first element in arr

                    binary->GetData(static_cast<void*>(src), binarySize, 0);

                    return buffer;
                }

                return nullptr;
            }

            /// <summary>
            /// Returns the PNG representation that most closely matches scaleFactor.
            /// </summary>
            /// <param name="scaleFactor">scale factor</param>
            /// <param name="withTransparency">is the PNG transparent</param>
            /// <param name="pixelWidth">pixel width</param>
            /// <param name="pixelHeight">pixel height</param>
            /// <returns>A stream represending the PNG or null.</returns>
            virtual cli::array<Byte>^ GetAsPNG(float scaleFactor, bool withTransparency, int% pixelWidth, int% pixelHeight)
            {
                int width;
                int height;

                auto binary = _image->GetAsPNG(scaleFactor, withTransparency, width, height);

                if (binary.get())
                {
                    pixelWidth = width;
                    pixelHeight = height;

                    auto binarySize = binary->GetSize();

                    auto buffer = gcnew cli::array<Byte>(binarySize);
                    pin_ptr<Byte> src = &buffer[0]; // pin pointer to first element in arr

                    binary->GetData(static_cast<void*>(src), binarySize, 0);

                    return buffer;
                }

                return nullptr;
            }

            /// <summary>
            /// Returns information for the representation that most closely matches scaleFactor.
            /// </summary>
            /// <param name="scaleFactor">scale factor</param>
            /// <param name="actualScaleFactor">actual scale factor</param>
            /// <param name="pixelWidth">pixel width</param>
            /// <param name="pixelHeight">pixel height</param>
            /// <returns>return if information found for scale factor</returns>
            virtual bool GetRepresentationInfo(float scaleFactor, float% actualScaleFactor, int% pixelWidth, int% pixelHeight)
            {
                float actualScale;
                int width;
                int height;
                
                auto success = _image->GetRepresentationInfo(scaleFactor, actualScale, width, height);

                actualScaleFactor = actualScale;
                pixelWidth = width;
                pixelHeight = height;

                return success;
            }

            /// <summary>
            /// Returns the image height in density independent pixel(DIP) units.
            /// </summary>
            virtual property int Height
            {
                int get() { return _image->GetHeight(); }
            }

            /// <summary>
            /// Returns true if this image contains a representation for scaleFactor.
            /// </summary>
            /// <param name="scaleFactor"></param>
            /// <returns></returns>
            virtual bool HasRepresentation(float scaleFactor)
            {
                return _image->HasRepresentation(scaleFactor);
            }

            /// <summary>
            /// Returns true if this Image is empty.
            /// </summary>
            /// <returns></returns>
            virtual property bool IsEmpty
            {
                bool get() { return _image->IsEmpty(); }
            }

            /// <summary>
            /// Returns true if this Image and that Image share the same underlying storage.
            /// </summary>
            /// <param name="that">image to compare</param>
            /// <returns>returns true if share same underlying storage</returns>
            virtual bool IsSame(IImage^ that)
            {
                return _image->IsSame(((CefImageWrapper^)that)->_image.get());
            }

            /// <summary>
            /// Removes the representation for scaleFactor.
            /// </summary>
            /// <param name="scaleFactor"></param>
            /// <returns>true for success</returns>
            virtual bool RemoveRepresentation(float scaleFactor)
            {
                return _image->RemoveRepresentation(scaleFactor);
            }

            /// <summary>
            /// Returns the image width in density independent pixel(DIP) units.
            /// </summary>
            virtual property int Width
            {
                int get() { return _image->GetWidth(); }
            }
        };
    }
}

