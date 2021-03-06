// --------------------------------------------------------------------------------------
// 本软件是自由软件,使用的是GNU通用公共许可证,许可证应使用第二版本或您所选择的更新的版本.
// 发布本软件的目的是希望它能够在一定程度上帮到您.但我们并不为它提供任何形式的担保，
// 也无法保证它可以在特定用途中得到您希望的结果.请参看GNU GPL许可中的更多细节.
// 使用本软件或者与本软件相关的代码,文档,图标之前,您必须接受本软件的协议及许可.
// 您应该在获取本代码同时获得了GNU GPL协议的副本.
// 如果您没有获得GNU GPL协议的副本的话,请给自由软件基金会写信,地址是:
// 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
// 本软件及其代码的作者是黄锐,(Email:vowstar@gmail.com),Lanzhou University
// --------------------------------------------------------------------------------------

using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace NumericalAnalysis
{
	public class Element
	{
		public double x = 0;
		public double f = 0;
		public double omega = 1;
		public Element (double x, double f, double omega)
		{
			this.x = x;
			this.f = f;
			this.omega = omega;
		}
		public Element (double x, double f)
		{
			this.x = x;
			this.f = f;
		}
		public Element ()
		{
		}
	}

	namespace Analysis
	{
		public class Linear
		{
			public double Sigma_Omega = 0;
			public double Sigma_Omega_x = 0;
			public double Sigma_Omega_x2 = 0;
			public double Sigma_Omega_f = 0;
			public double Sigma_Omega_x_f = 0;
			public double a0 = 0;
			public double a1 = 0;
			public int m = 0;
			public Linear (Element[] Data)
			{
				int i;
				m = Data.Length;
				Console.WriteLine ("Begin linear.");
				Console.WriteLine ("Begin sigma.");
				for (i = 0; i < m; i++) {
					Sigma_Omega += Data[i].omega;
					Sigma_Omega_x += Data[i].omega * Data[i].x;
					Sigma_Omega_x2 += Data[i].omega * Data[i].x * Data[i].x;
					Sigma_Omega_f += Data[i].omega * Data[i].f;
					Sigma_Omega_x_f += Data[i].omega * Data[i].x * Data[i].f;
				}
				Console.WriteLine ("Begin solve.");
				Analysis.Solve.SolveLinear (Sigma_Omega, Sigma_Omega_x, Sigma_Omega_f, Sigma_Omega_x, Sigma_Omega_x2, Sigma_Omega_x_f, out a0, out a1);
			}
			public double f (double x)
			{
				return a0 + a1 * x;
			}
		}

		public class Hyperbolic
		{
			public double a = 0;
			public double b = 0;
			public int m = 0;
			public Hyperbolic (Element[] Data)
			{
				int i;
				m = Data.Length;
				Console.WriteLine ("Begin hyperbolic.");
				Element[] data = new Element[m];
				for (i = 0; i < m; i++) {
					data[i].x = 1.0 / Data[i].x;
					data[i].f = 1.0 / Data[i].f;
					data[i].omega = Data[i].omega;
				}
				NumericalAnalysis.Analysis.Linear liner = new NumericalAnalysis.Analysis.Linear (data);
				a = liner.a0;
				b = liner.a1;
			}
			public double f (double x)
			{
				return x / (a * x + b);
			}
		}
		public class Exponential
		{
			public double a = 0;
			public double b = 0;
			public int m = 0;
			public Exponential (Element[] Data)
			{
				int i;
				m = Data.Length;
				Console.WriteLine ("Begin exponential.");
				Element[] data = new Element[m];
				for (i = 0; i < m; i++) {
					data[i].x = 1.0 / Data[i].x;
					data[i].f = Math.Log (Data[i].f);
					data[i].omega = Data[i].omega;
				}
				NumericalAnalysis.Analysis.Linear liner = new NumericalAnalysis.Analysis.Linear (data);
				a = Math.Exp (liner.a0);
				b = liner.a1;
			}
			public double f (double x)
			{
				return a*Math.Exp(b/x);
			}
		}
		public static class Solve
		{
			public static void SolveLinear (double A, double B, double C, double D, double E, double F, out double X, out double Y)
			{
				X = (F * B - E * C) / (B * D - E * A);
				Y = (F * A - D * C) / (A * E - D * B);
			}
		}
	}
	namespace Graphic
	{
		public class Coordinate
		{
			public Bitmap CoordinateBitmap;
			private double Y_Min;
			private double Y_Max;
			public Coordinate (double[] Data, double Start, double End, int Width, int Height)
			{
				CoordinateBitmap = new Bitmap (Width, Height);
				using (Graphics g = Graphics.FromImage (CoordinateBitmap)) {
					g.Clear (Color.White);
					g.DrawLine (new Pen (Color.Black, 2), new Point (0, Height / 2), new Point (Width, Height / 2));
					g.DrawLine (new Pen (Color.Black, 2), new Point (Width / 2, 0), new Point (Width / 2, Height));
					int i = 0;
					Y_Min = Data[0];
					Y_Max = Data[0];
					for (i = 0; i < Data.Length; i++) {
						if (Y_Max < Data[i])
							Y_Max = Data[i];
						if (Y_Min > Data[i])
							Y_Min = Data[i];
					}
					Console.WriteLine ("{0}>>Y Range:{1}~{2}", this.ToString (), Y_Max, Y_Min);
					int x1 = 0, y1 = 0, x2 = 0, y2 = 0;
					i = 0;
					x2 = (int)(i * Width / Data.Length);
					y2 = (int)(Height - Height * (Data[i] - Y_Min) / (Y_Max - Y_Min));
					for (i = 1; i < Data.Length; i++) {						
						x1 = x2;
						y1 = y2;
						x2 = (int)(i * Width / Data.Length);
						y2 = (int)(Height - Height * (Data[i] - Y_Min) / (Y_Max - Y_Min));
						g.DrawLine (new Pen (Color.Blue, 2), new Point (x1, y1), new Point (x2, y2));						
					}
				}
			}
			private void Draw ()
			{
				
				
			}
			
		}
	}
}
