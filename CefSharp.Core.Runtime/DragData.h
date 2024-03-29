// Copyright © 2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

#include "include/cef_drag_data.h"

#include "Internals\CefWrapper.h"
#include "Internals\CefImageWrapper.h"
#include "Internals\CefWriteHandlerWrapper.h"

using namespace std;
using namespace System::IO;

namespace CefSharp
{
    namespace Core
    {
        [System::ComponentModel::EditorBrowsableAttribute(System::ComponentModel::EditorBrowsableState::Never)]
        public ref class DragData : public IDragData, public CefWrapper
        {
        private:
            MCefRefPtr<CefDragData> _wrappedDragData;

        internal:
            DragData(CefRefPtr<CefDragData> &dragData) :
                _wrappedDragData(dragData)
            {
                IsReadOnly = dragData->IsReadOnly();
                FileName = StringUtils::ToClr(dragData->GetFileName());
                IsFile = dragData->IsFile();
                IsFragment = dragData->IsFragment();
                IsLink = dragData->IsLink();
            }

            !DragData()
            {
                _wrappedDragData = nullptr;
            }

            ~DragData()
            {
                this->!DragData();

                _disposed = true;
            }

            operator CefRefPtr<CefDragData>()
            {
                if (this == nullptr)
                {
                    return nullptr;
                }
                return _wrappedDragData.get();
            }

        public:
            virtual property bool IsReadOnly;
            virtual property String^ FileName;
            virtual property bool IsFile;
            virtual property bool IsFragment;
            virtual property bool IsLink;

            virtual IDragData^ Clone()
            {
                return gcnew DragData(_wrappedDragData->Clone());
            }

            ///
            // Create a new CefDragData object.
            ///
            /*--cef()--*/
            static IDragData^ Create()
            {
                return gcnew DragData(CefDragData::Create());
            }

            //TODO: Vector is a pointer, so can potentially be updated (items may be possibly removed)
            virtual property IList<String^>^ FileNames
            {
                IList<String^>^ get()
                {
                    auto names = vector<CefString>();
                    _wrappedDragData->GetFileNames(names);

                    return StringUtils::ToClr(names);
                }
            }

            //TODO: Vector is a pointer, so can potentially be updated (items may be possibly removed)
            virtual property IList<String^>^ FilePaths
            {
                IList<String^>^ get()
                {
                    auto paths = vector<CefString>();
                    _wrappedDragData->GetFilePaths(paths);

                    return StringUtils::ToClr(paths);
                }
            }

            virtual property String^ FragmentBaseUrl
            {
                String^ get()
                {
                    return StringUtils::ToClr(_wrappedDragData->GetFragmentBaseURL());
                }
                void set(String^ fragmentBaseUrl)
                {
                    _wrappedDragData->SetFragmentBaseURL(StringUtils::ToNative(fragmentBaseUrl));
                }
            }

            virtual property String^ FragmentHtml
            {
                String^ get()
                {
                    return StringUtils::ToClr(_wrappedDragData->GetFragmentHtml());
                }
                void set(String^ fragmentHtml)
                {
                    _wrappedDragData->SetFragmentHtml(StringUtils::ToNative(fragmentHtml));
                }
            }

            virtual property String^ FragmentText
            {
                String^ get()
                {
                    return StringUtils::ToClr(_wrappedDragData->GetFragmentText());
                }
                void set(String^ fragmentText)
                {
                    _wrappedDragData->SetFragmentText(StringUtils::ToNative(fragmentText));
                }
            }

            virtual property bool HasImage
            {
                bool get()
                {
                    return _wrappedDragData->HasImage();
                }
            }

            /// <summary>
            /// Get the image representation of drag data.
            /// May return NULL if no image representation is available.
            /// </summary>
            virtual property IImage^ Image
            {
                IImage^ get()
                {
                    if (_wrappedDragData->HasImage())
                    {
                        return gcnew CefImageWrapper(_wrappedDragData->GetImage());
                    }
                    return nullptr;
                }
            }

            /// <summary>
            /// Get the image hotspot (drag start location relative to image dimensions).
            /// </summary>
            virtual property CefSharp::Structs::Point ImageHotspot
            {
                CefSharp::Structs::Point get()
                {
                    return CefSharp::Structs::Point(0, 0);
                }
            }

            virtual property String^ LinkMetaData
            {
                String^ get()
                {
                    return StringUtils::ToClr(_wrappedDragData->GetLinkMetadata());
                }
                void set(String^ linkMetaData)
                {
                    _wrappedDragData->SetLinkMetadata(StringUtils::ToNative(linkMetaData));
                }
            }

            virtual property String^ LinkTitle
            {
                String^ get()
                {
                    return StringUtils::ToClr(_wrappedDragData->GetLinkTitle());
                }
                void set(String^ linkTitle)
                {
                    _wrappedDragData->SetLinkTitle(StringUtils::ToNative(linkTitle));
                }
            }

            virtual property String^ LinkUrl
            {
                String^ get()
                {
                    return StringUtils::ToClr(_wrappedDragData->GetLinkURL());
                }
                void set(String^ linkUrl)
                {
                    _wrappedDragData->SetLinkURL(StringUtils::ToNative(linkUrl));
                }
            }

            virtual void AddFile(String^ path, String^ displayName)
            {
                _wrappedDragData->AddFile(StringUtils::ToNative(path), StringUtils::ToNative(displayName));
            }

            virtual void ResetFileContents()
            {
                _wrappedDragData->ResetFileContents();
            }

            virtual void ClearFilenames()
            {
                _wrappedDragData->ClearFilenames();
            }

            virtual Int64 GetFileContents(Stream^ stream)
            {
                if (stream == nullptr)
                {
                    return (Int64)_wrappedDragData->GetFileContents(nullptr);
                }

                auto writeHandler = new CefWriteHandlerWrapper(stream);

                auto writer = CefStreamWriter::CreateForHandler(writeHandler);
                return (Int64)_wrappedDragData->GetFileContents(writer);
            }
        };
    }
}
