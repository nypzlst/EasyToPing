using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection.Emit;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        void Button_Click(object sender, RoutedEventArgs e)
        {
            GetSite(textbox1);
        }
         
        void GetSite(TextBox text)
        {
            string? pingText = text.GetLineText(0);
            string longUrlPattern = @"^(https?:\/\/)?[a-z0-9]+([\-\.]{1}[a-z0-9]+)*\.[a-z]{2,}(:[0-9]{1,5})?(\/.*)?$";
            string shortUrlPattern = @"[a-zA-Z]+.+[a-zA-Z]";
            Regex shortUrl = new Regex(shortUrlPattern);
            Regex longUrl = new Regex(longUrlPattern);
            try
            {
                if (longUrl.IsMatch(pingText))
                {
                    Uri url = new(pingText);
                    Ping ping = new();
                    int timeOut = 1000;
                    PingReply pingReply = ping.Send(url.Host, timeOut);
                    CheckResponse(pingReply, url);
                }
                
            }
            catch (UriFormatException)
            {
                if (shortUrl.IsMatch(pingText))
                {
                    int timeOut = 1000;
                    PingReply pingReply = new Ping().Send(pingText, timeOut);
                    Uri uri = null;
                    CheckResponse(pingReply,uri, pingText);
                }
                else
                {
                    throw new Exception();
                }

            }
            catch (Exception ex)
            {
                label1.Content = ex.Message;
            }
        }
        void CheckResponse(PingReply pingReply, Uri? url= null, string text = "")
        {
            if (pingReply.Status == IPStatus.Success)
            {
                if(url!= null)
                {
                    label1.Content = $"{url} ping in milliseconds : {new Ping().Send(url.Host).RoundtripTime.ToString()}";
                }
                else if (text != "")
                {
                    int timeOut = 1000;
                    label1.Content = $"{text} ping in milliseconds : {new Ping().Send(text, timeOut).RoundtripTime.ToString()}";
                }
            }
            else if (pingReply.Status == IPStatus.TimedOut)
            {
                label1.Content = "Response is timeOut";
            }
            else
            {
                label1.Content = "Site not work or found";
            }
        }
    }
}
