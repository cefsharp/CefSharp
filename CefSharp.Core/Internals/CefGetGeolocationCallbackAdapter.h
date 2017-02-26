// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

#include "include\cef_geolocation.h"

using namespace System::Threading::Tasks;

namespace CefSharp
{
    namespace Internals
    {
        private class CefGetGeolocationCallbackAdapter : public CefGetGeolocationCallback
        {
        private:
            gcroot<IGetGeolocationCallback^> _callback;

        public:
            CefGetGeolocationCallbackAdapter(IGetGeolocationCallback^ callback)
            {
                _callback = callback;
            }

            ~CefGetGeolocationCallbackAdapter()
            {
                delete _callback;
                _callback = nullptr;
            }

            virtual void OnLocationUpdate(const CefGeoposition& position) OVERRIDE
            {
                auto p = gcnew Geoposition();
                p->Accuracy = position.accuracy;
                p->Altitude = position.altitude;
                p->AltitudeAccuracy = position.altitude_accuracy;
                p->ErrorCode = (CefGeoPositionErrorCode)position.error_code;
                p->ErrorMessage = StringUtils::ToClr(position.error_message);
                p->Heading = position.heading;
                p->Latitude = position.latitude;
                p->Longitude = position.longitude;
                p->Speed = position.speed;
                p->Timestamp = ConvertCefTimeToDateTime(position.timestamp);

                _callback->OnLocationUpdate(p);
            };

            DateTime ConvertCefTimeToDateTime(CefTime time)
            {
                auto epoch = time.GetDoubleT();
                if (epoch == 0)
                {
                    return DateTime::MinValue;
                }
                return DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(epoch).ToLocalTime();
            }

            IMPLEMENT_REFCOUNTING(CefGetGeolocationCallbackAdapter)
        };
    }
}


