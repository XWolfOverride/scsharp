//
// SCSharp.UI.Cinematic
//
// Authors:
//	Chris Toshok (toshok@hungry.com)
//
// (C) 2006 The Hungry Programmers (http://www.hungry.com/)
//

//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;
using System.IO;
using System.Threading;

using SdlDotNet;
using System.Drawing;

namespace SCSharp.UI
{
	public class Cinematic : UIScreen
	{
		SmackerPlayer player;
		string resourcePath;

		public Cinematic (Mpq mpq, string resourcePath)
			: base (mpq, null, null)
		{
			this.resourcePath = resourcePath;
		}

		protected override void FirstPaint (object sender, EventArgs args)
		{
			base.FirstPaint (sender, args);

			player = new SmackerPlayer ((Stream)mpq.GetResource (resourcePath));

			player.Finished += PlayerFinished;
			player.FrameReady += PlayerFrameReady;
			player.Play ();
		}

		public override void AddToPainter ()
		{
			base.AddToPainter ();

			Painter.Instance.Add (Layer.Background, VideoPainter);
		}

		public override void RemoveFromPainter ()
		{
			base.RemoveFromPainter ();

			player.Stop ();
			player = null;
			Painter.Instance.Remove (Layer.Background, VideoPainter);
		}

		void PlayerFrameReady ()
		{
			/* signal to the painter to redraw the screen */
			Painter.Instance.Invalidate ();
		}

		void VideoPainter (DateTime dt)
		{
			if (player != null && player.Surface != null) {
				Surface surf = null;

				if (player.Width != Painter.Instance.Width
				    || player.Height != Painter.Instance.Height) {
					double horiz_zoom = (double)Painter.Instance.Width / player.Width;
					double vert_zoom = (double)Painter.Instance.Height / player.Height;
					double zoom;

					if (horiz_zoom < vert_zoom)
						zoom = horiz_zoom;
					else
						zoom = vert_zoom;
					
					surf = new Surface (player.Surface);
					surf.Scale (zoom);
				}
				else
					surf = player.Surface;

				Painter.Instance.Blit (surf,
						       new Point ((Painter.Instance.Width - surf.Width) / 2,
								  (Painter.Instance.Height - surf.Height) / 2));
			}
		}

		public event PlayerEvent Finished;

		void PlayerFinished ()
		{
			if (Finished != null)
				Finished ();
		}

		public override void KeyboardDown (KeyboardEventArgs args)
		{
			if (args.Key == Key.Escape
			    || args.Key == Key.Return
			    || args.Key == Key.Space) {
				player.Stop ();
				PlayerFinished ();
			}
		}
	}
}
