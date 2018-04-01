using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace Bot_App
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Search(string keyword)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://torrentz.eu/search?f=" + keyword);

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            StreamReader sr = new StreamReader(response.GetResponseStream());

            string response_text = sr.ReadToEnd();

            Regex regex = new Regex("<dt>.*?</dt>");

            MatchCollection match = regex.Matches(response_text);

            foreach (Match ping in match)
            {

                //MessageBox.Show(ping.Value);
                //Clipboard.SetText(ping.Value);

                Regex extract_link_regex = new Regex(@"href=""/.*""");
                Match extract_link_match = extract_link_regex.Match((ping.Value));
                string link = extract_link_match.Value;
                link = link.Replace("href=", "");
                link = link.Replace(@"""", "");
                link = "http://arenabg.ch/" + link;


                Regex extract_title_regex = new Regex(@""">.*</a>");
                Match extract_title_match = extract_title_regex.Match((ping.Value));
                string title = extract_title_match.Value;
                title = title.Replace(@""">", "");
                title = title.Replace(@"<b>", "");
                title = title.Replace(@"</b>", "");
                title = title.Replace(@"</a>", "");


                ListViewItem items = new ListViewItem(new string[] { title, link });
                items.Tag = link;
                lstitem.Items.Add(items);
            }
        }

        private void btnsearch_Click(object sender, EventArgs e)
        {
            Search(txtseach.Text);
        }

        private void lstitem_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //IF YOU STARTED DIFFERENT EXPLORER WRİTE NAME !!!
                Process.Start("chrome.exe", lstitem.SelectedItems[0].Tag.ToString());
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }
    }
}
