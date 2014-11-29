// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.


namespace CefSharp
{
   public enum DragOperationsMask
    {
        DRAG_OPERATION_NONE = 0,
        DRAG_OPERATION_COPY = 1,
        DRAG_OPERATION_LINK = 2,
        DRAG_OPERATION_GENERIC = 4, 
        DRAG_OPERATION_PRIVATE = 8, 
        DRAG_OPERATION_MOVE = 16, 
        DRAG_OPERATION_DELETE = 32
        //DRAG_OPERATION_EVERY = UINT_MAX,
    } 
}
