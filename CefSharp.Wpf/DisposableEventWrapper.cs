﻿using System;
using System.ComponentModel;
using System.Windows;

namespace CefSharp.Wpf
{
	internal class DisposableEventWrapper : IDisposable
	{
		public DependencyObject Source { get; private set; }
		public DependencyProperty Property { get; private set; }
		public EventHandler Handler { get; private set; }

		public DisposableEventWrapper(DependencyObject source, DependencyProperty property, EventHandler handler)
		{
			Source = source;
			Property = property;
			Handler = handler;
			DependencyPropertyDescriptor.FromProperty(Property, Source.GetType()).AddValueChanged(Source, Handler);
		}

		public void Dispose()
		{
			DependencyPropertyDescriptor.FromProperty(Property, Source.GetType()).RemoveValueChanged(Source, Handler);
		}
	}
}