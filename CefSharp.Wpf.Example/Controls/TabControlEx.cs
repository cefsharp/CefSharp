using System;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace CefSharp.Wpf.Example.Controls
{
	/// <summary>
	/// NOTE: Source directly imported from http://stackoverflow.com/a/9802346
	/// TabControlEx - Extended TabControl which saves the displayed item so you don't get the performance hit of
	/// unloading and reloading the VisualTree when switching tabs
	/// Obtained from http://www.pluralsight-training.net/community/blogs/eburke/archive/2009/04/30/keeping-the-wpf-tab-control-from-destroying-its-children.aspx
	/// and made a some modifications so it reuses a TabItem's ContentPresenter when doing drag/drop operations
	/// </summary>
	[TemplatePart(Name = "PART_ItemsHolder", Type = typeof(Panel))]
	public class TabControlEx : TabControl
	{
		private Panel itemsHolderPanel;

		public TabControlEx()
		{
			// This is necessary so that we get the initial databound selected item
			ItemContainerGenerator.StatusChanged += ItemContainerGeneratorStatusChanged;
		}

		/// <summary>
		/// If containers are done, generate the selected item
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ItemContainerGeneratorStatusChanged(object sender, EventArgs e)
		{
			if (ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
			{
				ItemContainerGenerator.StatusChanged -= ItemContainerGeneratorStatusChanged;
				UpdateSelectedItem();
			}
		}

		/// <summary>
		/// Get the ItemsHolder and generate any children
		/// </summary>
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			itemsHolderPanel = GetTemplateChild("PART_ItemsHolder") as Panel;
			UpdateSelectedItem();
		}

		/// <summary>
		/// When the items change we remove any generated panel children and add any new ones as necessary
		/// </summary>
		/// <param name="e"></param>
		protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
		{
			base.OnItemsChanged(e);

			if (itemsHolderPanel == null)
				return;

			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Reset:
				itemsHolderPanel.Children.Clear();
				break;

				case NotifyCollectionChangedAction.Add:
				case NotifyCollectionChangedAction.Remove:
				if (e.OldItems != null)
				{
					foreach (var item in e.OldItems)
					{
						var cp = FindChildContentPresenter(item);
						if (cp != null)
							itemsHolderPanel.Children.Remove(cp);
					}
				}

				// Don't do anything with new items because we don't want to
				// create visuals that aren't being shown

				UpdateSelectedItem();
				break;

				case NotifyCollectionChangedAction.Replace:
				throw new NotImplementedException("Replace not implemented yet");
			}
		}

		protected override void OnSelectionChanged(SelectionChangedEventArgs e)
		{
			base.OnSelectionChanged(e);
			UpdateSelectedItem();
		}

		private void UpdateSelectedItem()
		{
			if (itemsHolderPanel == null)
				return;

			// Generate a ContentPresenter if necessary
			var item = GetSelectedTabItem();
			if (item != null)
				CreateChildContentPresenter(item);

			// show the right child
			foreach (ContentPresenter child in itemsHolderPanel.Children)
				child.Visibility = ((child.Tag as TabItem).IsSelected) ? Visibility.Visible : Visibility.Collapsed;
		}

		private ContentPresenter CreateChildContentPresenter(object item)
		{
			if (item == null)
				return null;

			var cp = FindChildContentPresenter(item);

			if (cp != null)
				return cp;

			// the actual child to be added.  cp.Tag is a reference to the TabItem
			cp = new ContentPresenter();
			cp.Content = (item is TabItem) ? (item as TabItem).Content : item;
			cp.ContentTemplate = this.SelectedContentTemplate;
			cp.ContentTemplateSelector = this.SelectedContentTemplateSelector;
			cp.ContentStringFormat = this.SelectedContentStringFormat;
			cp.Visibility = Visibility.Collapsed;
			cp.Tag = (item is TabItem) ? item : (this.ItemContainerGenerator.ContainerFromItem(item));
			itemsHolderPanel.Children.Add(cp);
			return cp;
		}

		private ContentPresenter FindChildContentPresenter(object data)
		{
			if (data is TabItem)
				data = (data as TabItem).Content;

			if (data == null)
				return null;

			if (itemsHolderPanel == null)
				return null;

			foreach (ContentPresenter cp in itemsHolderPanel.Children)
			{
				if (cp.Content == data)
					return cp;
			}

			return null;
		}

		protected TabItem GetSelectedTabItem()
		{
			var selectedItem = SelectedItem;
			if (selectedItem == null)
				return null;

			var item = selectedItem as TabItem ?? ItemContainerGenerator.ContainerFromIndex(SelectedIndex) as TabItem;

			return item;
		}
	}
}
