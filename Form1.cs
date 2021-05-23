using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Net.Http;
using System.Windows.Forms;
using System.Security.Cryptography.X509Certificates;
using System.Net;
using Microsoft.Win32;
using System.Net.Security;
using System.IO;
using Newtonsoft.Json;
using System.Diagnostics;

namespace lobbycrasher31
{
	public partial class Form1 : Form
    {
		private LCUListener listener = new LCUListener();

		private IContainer components = null;


		public Form1()
		{
			InitializeComponent();
			listener.StartListening();
		}

		private async void pictureBox1_Click(object sender, EventArgs e)
        {
		}

		private async void button1_Click(object sender, EventArgs e)
		{
                if (this.listener.GetGatheredLCUs().Count == 0)
                {
					MessageBox.Show("Please run League of Legends first");
				}
                else
                {
					foreach (LCUClient LCU in this.listener.GetGatheredLCUs())
					{


					await LCU.HttpDelete("/lol-lobby/v2/lobby");
					await LCU.HttpPostJson("/lol-lobby/v2/matchmaking/quick-search", "{\"queueId\":1110}");
				}
				}
		}

		private void Form1_Load(object sender, EventArgs e)
        {
			this.FormClosing += Form1_FormClosing;
		}

		private async void button2_Click_1(object sender, EventArgs e)
        {
			if (this.listener.GetGatheredLCUs().Count == 0)
			{
				MessageBox.Show("Please run League of Legends first");
			}
            else
            {

				foreach (LCUClient LCU in listener.GetGatheredLCUs())
				{
					await LCU.HttpPostForm("/lol-login/v1/session/invoke", new List<KeyValuePair<string, string>>
			{
				new KeyValuePair<string, string>("destination", "gameService"),
				new KeyValuePair<string, string>("method", "quitGame"),
				new KeyValuePair<string, string>("args", "[]")
			});
					await LCU.HttpDelete("/lol-lobby/v2/lobby");
					await LCU.HttpPostJson("/lol-lobby/v2/lobby", "{\"queueId\":430}");
					await LCU.HttpPostJson("/lol-lobby/v2/lobby/matchmaking/search", null);
					await LCU.HttpDelete("/lol-lobby/v2/lobby/matchmaking/search");
				}
			}


		}
		protected override void Dispose(bool disposing)
		{
			bool flag = disposing && this.components != null;
			if (flag)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}
		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			this.listener.StopListening();
		}

    }
}
