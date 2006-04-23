//#define SHOW_ELEMENT_BORDERS

using System;
using System.Collections.Generic;
using System.IO;

using SdlDotNet;
using System.Drawing;

namespace SCSharp.UI
{

	public class UIPainter
	{
		List<UIElement> elements;

		public UIPainter (List<UIElement> elements)
		{
			this.elements = elements;
		}

		public void Paint (Surface surf, DateTime now)
		{
			for (int i = 0; i < elements.Count; i ++) {
				UIElement e = elements[i];

				e.Paint (surf, now);
#if SHOW_ELEMENT_BORDERS
				surf.DrawBox (new Rectangle (new Point (e.X1,e.Y1), new Size (e.Width - 1, e.Height - 1)), e.Visible ? Color.Green : Color.Yellow);
				if (e.Text == "")
					e.Text = i.ToString();
#endif
			}
		}
	}

}
