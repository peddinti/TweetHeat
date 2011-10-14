/* Code Title: Tweet Heat Final
 * Version: 1.0
 * Author: Raghava Viswa Mani Kiran Peddinti
 * Initials: VMK
 * Description:
 *      This Plugin to Bing Maps Provides the User a graphical representation of the Twitter users feeling towards any product.
 *      It takes a Search word, Seed Location, Location Range, Language and a Plot Style.
 *      Search word: this is the keyword used to fetch twitter feeds; DEFAULT VALUE: bing
 *      Seed Location: This is teh central location from where feeds are fetched within the Location Range DEFAULT VALUE:bellevue (47.610020,-122.187549)
 *      Location Range: This is the range in KM from where the Feeds are fetched. MINIMUM VALUE: 1 KM
 *      Language: This is the Language with which tweets are twitted. DEFAULT VALUE: ENG (english)
 *      Plot Style: This is the graphical representation of the tweets. Can be a Heat Map or a Push pin for each tweet or both. DEFAULT STYLE: both
*/
using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Microsoft.Maps.Core;
using Microsoft.Maps.MapControl;
using System.Diagnostics;

namespace TweetHeatFinal
{
	public class HeatMap : UserControl
	{
		private Image _content;
		private WriteableBitmap _bitmap;
		private Color[] _pallette;
		private int _width;
		private int _height;
		private MapContract _mapContract;
		private double[] _posValues;
		private double[] _negValues;
		private double[] _weights;
		private double _falloff;
		private double _minRadius;
		private double _maxRadius;


		internal HeatMap(MapContract mapContract)
		{
			this._minRadius = 5;
			this._maxRadius = 60;
			this._falloff = 1.0f;
			this._mapContract = mapContract;
			this._mapContract.ViewChangeOnFrame += OnViewChange;
			_content = new Image();
			this.Points = new ObservableCollection<HeatMapPoint>();
			this.Points.CollectionChanged += OnPointsChanged;
			this.SizeChanged += OnSizeChanged;
			this.Content = _content;
			this.IsHitTestVisible = false;
			SetPallette(GetRedYellowGreenPallette());
		}

		private static Color[] GetRedYellowGreenPallette()
		{
			Color[] colors = new Color[894];
			int index = 0;
			for (byte i = 0; i < 0xC0; i++)
			{
				colors[index++] = Color.FromArgb(0, 0xFF, (byte)(0xC0 - i), (byte)(0xC0 - i));
			}
			for (byte i = 0; i < 0xFF; i++)
			{
				colors[index++] = Color.FromArgb(0, 0xFF, i, 0);
			}
			for (byte i = 0; i < 0xFF; i++)
			{
				colors[index++] = Color.FromArgb(0, (byte)(0xFF - i), 0xFF, 0);
			}
			for (byte i = 0; i < 0xC0; i++)
			{
				colors[index++] = Color.FromArgb(0, 0, (byte)(0xFF - i), 0);
			}
			Debug.Assert(index == colors.Length);
			return colors;
		}

		private void SetPallette(int[] colors)
		{
			_pallette = new Color[colors.Length];
			for (int i = 0; i < colors.Length; i++)
			{
				int color = colors[i];
				byte r = (byte)((color >> 16) & 0xFF);
				byte g = (byte)((color >> 8) & 0xFF);
				byte b = (byte)((color) & 0xFF);
				_pallette[i] = Color.FromArgb(0, r, g, b);
			}
			Draw();
		}

		private void SetPallette(Color[] colors)
		{
			_pallette = new Color[colors.Length];
			Array.Copy(colors, _pallette, colors.Length);
			Draw();
		}

		private void OnViewChange(object sender, MapEventArgs e)
		{
			Draw();
		}

		private void OnPointsChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			Draw();
		}

		private void OnSizeChanged(object sender, SizeChangedEventArgs e)
		{
			_width = (int)e.NewSize.Width;
			_height = (int)e.NewSize.Height;
			_bitmap = new WriteableBitmap(_width, _height);
			int size = _width * _height;
			_posValues = new double[size];
			_negValues = new double[size];
			_weights = new double[size];
			Draw();
		}

		private int ConvertValueToColor(double weight, double value)
		{
			if (value > 1)
				value = 1;
			if (value < -1)
				value = -1;
			double alphaMult = weight;
			Color color = _pallette[(int)(((value + 1) / 2.0f) * (_pallette.Length - 1))];
			byte a = (byte)(alphaMult * 0xFF);
			byte r = (byte)(color.R * alphaMult);
			byte g = (byte)(color.G * alphaMult);
			byte b = (byte)(color.B * alphaMult);
			return ((a << 24) | (r << 16) | (g << 8) | b);
		}

		private void Draw()
		{
			if (_bitmap == null || _pallette == null)
				return;
			
			// Clear out the values
			for (int i = 0; i < _posValues.Length; i++)
			{
				_posValues[i] = 1;
				_negValues[i] = 1;
				_weights[i] = 1;
			}

			// Draw each point on the image
			for (int i = 0; i < Points.Count; i++)
			{
				HeatMapPoint point = Points[i];
				double radius = ((_maxRadius - _minRadius) * Math.Abs(point.Weight)) + _minRadius;

				Point p = _mapContract.LocationToViewportPoint(point.Location);

				int xMin = (int)Math.Max(0, p.X - radius);
				int xMax = (int)Math.Min(_width, p.X + radius);
				int yMin = (int)Math.Max(0, p.Y - radius);
				int yMax = (int)Math.Min(_height, p.Y + radius);
				for (int x = xMin; x < xMax; x++)
				{
					double dx = x - p.X;
					for (int y = yMin; y < yMax; y++)
					{
						double dy = y - p.Y;
						double distance = Math.Sqrt((dx * dx) + (dy * dy));

						if (distance < radius)
						{
							int index = (y * _width) + x;
							double weight = (-(Math.Pow(distance, _falloff)) / (Math.Pow(radius,_falloff))) + 1;
							if (point.Value >= 0)
								_posValues[index] *= (1 - (weight * point.Value));
							else
								_negValues[index] *= (1 + (weight * point.Value));
							_weights[index] *= 1 - weight;
						}
					}
				}
			}
			for (int i = 0; i < _bitmap.Pixels.Length; i++)
			{
				_bitmap.Pixels[i] = ConvertValueToColor(1 - _weights[i], _negValues[i] - _posValues[i]);
			}
			_bitmap.Invalidate();
			_content.Source = _bitmap;
		}
		
		internal ObservableCollection<HeatMapPoint> Points { get; private set; }
	}

	internal class HeatMapPoint
	{
		public HeatMapPoint(Location location, double value, double weight)
		{
			this.Location = location;
			this.Value = value;
			this.Weight = weight;
		}

		public Location Location { get; private set; }
		public double Value { get; set; }
		public double Weight { get; set; }
	}   
}
