// Copyright © 2019 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Windows;
using CefSharp.Enums;

namespace CefSharp.Wpf.Internals
{
    internal static class DragOperationMaskExtensions
    {
        /// <summary>
        /// Converts .NET drag drop effects to CEF Drag Operations
        /// </summary>
        /// <param name="dragDropEffects">The drag drop effects.</param>
        /// <returns>DragOperationsMask.</returns>
        public static DragOperationsMask GetDragOperationsMask(this DragDropEffects dragDropEffects)
        {
            var operations = DragOperationsMask.None;

            if (dragDropEffects.HasFlag(DragDropEffects.All))
            {
                operations |= DragOperationsMask.Every;
            }
            if (dragDropEffects.HasFlag(DragDropEffects.Copy))
            {
                operations |= DragOperationsMask.Copy;
            }
            if (dragDropEffects.HasFlag(DragDropEffects.Move))
            {
                operations |= DragOperationsMask.Move;
            }
            if (dragDropEffects.HasFlag(DragDropEffects.Link))
            {
                operations |= DragOperationsMask.Link;
            }

            return operations;
        }

        /// <summary>
        /// Gets the drag effects.
        /// </summary>
        /// <param name="mask">The mask.</param>
        /// <returns>DragDropEffects.</returns>
        public static DragDropEffects GetDragEffects(this DragOperationsMask mask)
        {
            if ((mask & DragOperationsMask.Every) == DragOperationsMask.Every)
            {
                // return all effects (!= DragDropEffects.All, which doesn't include Link)
                return DragDropEffects.Scroll | DragDropEffects.Copy | DragDropEffects.Move | DragDropEffects.Link;
            }
            if ((mask & DragOperationsMask.Copy) == DragOperationsMask.Copy)
            {
                return DragDropEffects.Copy;
            }
            if ((mask & DragOperationsMask.Move) == DragOperationsMask.Move)
            {
                return DragDropEffects.Move;
            }
            if ((mask & DragOperationsMask.Link) == DragOperationsMask.Link)
            {
                return DragDropEffects.Link;
            }
            return DragDropEffects.None;
        }
    }
}
