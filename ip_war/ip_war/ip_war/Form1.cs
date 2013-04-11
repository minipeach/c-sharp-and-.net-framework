using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Net.NetworkInformation;
using System.Windows.Forms;
using System.Management;

namespace ip_war
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        public static void setip(string ipfront, string iplast, string wg, string ipdns)
        {
            ManagementBaseObject inPar = null;
            ManagementBaseObject outPar = null;
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                if (!(bool)mo["IPEnabled"])
                    continue;

                //设置ip地址和子网掩码 
                inPar = mo.GetMethodParameters("EnableStatic");
                inPar["IPAddress"] = new string[] { ipfront + "." + iplast };
                inPar["SubnetMask"] = new string[] { "255.255.255.0" };
                outPar = mo.InvokeMethod("EnableStatic", inPar, null);

                //设置网关地址 
                inPar = mo.GetMethodParameters("SetGateways");
                inPar["DefaultIPGateway"] = new string[] { wg };
                outPar = mo.InvokeMethod("SetGateways", inPar, null);

                //设置DNS 
                inPar = mo.GetMethodParameters("SetDNSServerSearchOrder");
                inPar["DNSServerSearchOrder"] = new string[] { ipdns };
                outPar = mo.InvokeMethod("SetDNSServerSearchOrder", inPar, null);
                break;
            }
        }

        public static Boolean pingresult(string wg)  //ping 网关 返回布尔值
        {
            Ping pingSender = new Ping();
            PingOptions options = new PingOptions();
            options.DontFragment = true;
            string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            int timeout = 120;
            PingReply reply = pingSender.Send(wg, timeout, buffer, options);
            if (reply.Status == IPStatus.Success)
                return true;
            else
                return false;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string ipfront = textBox1.Text;
            string wg = textBox3.Text;
            string ipdns = textBox6.Text;
            int i;
            Boolean flag = true;
            for (i = Convert.ToInt32(textBox2.Text); i <= Convert.ToInt32(textBox5.Text); i++)
            {
                string iplast = i.ToString();
                setip(ipfront, iplast, wg, ipdns);
                if (pingresult(wg))
                {
                    listBox1.Items.Add(ipfront + "." + iplast + " ping " + wg + " successfully");
                    listBox1.Items.Add("找到可用ip，并设置完毕，测试结束！");
                    listBox1.SelectedIndex = listBox1.Items.Count - 1;//动态显示不可少
                    flag = false;
                    break;
                }
                else
                {
                    listBox1.Items.Add(ipfront + "." + iplast + " ping " + wg + "  failed");
                    listBox1.SelectedIndex = listBox1.Items.Count - 1;//动态显示不可少
                }        
            }
            if (flag)
            {
                listBox1.Items.Add("未找到可用ip，测试结束！");
                listBox1.SelectedIndex = listBox1.Items.Count - 1;//动态显示不可少
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void textBox4_Click(object sender, EventArgs e)
        {
            textBox4.Text = textBox1.Text;
        }
    }
}
