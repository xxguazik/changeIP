using System;

using System.Collections.Generic;

using System.ComponentModel;

using System.Data;

using System.Drawing;

using System.Linq;

using System.Text;

using System.Windows.Forms;

using System.Management;

using System.IO;

namespace 设置ip
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            getwk();
            getip();
        }


        public void getwk()
        {

            string carName = "";

            ManagementObjectSearcher search = new ManagementObjectSearcher("SELECT * FROM Win32_NetWorkAdapterConfiguration");

            foreach (ManagementObject sear in search.Get())

            {

                if (sear["IPAddress"] != null)

                {

                    carName = sear["Description"].ToString().Trim();

                    comboBox1.Items.Add(carName);

                }

            }

            comboBox1.SelectedIndex = 0;
        }
        private void getip()

        {

            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");

            ManagementObjectCollection moc = mc.GetInstances();

            foreach (ManagementObject mo in moc)

            {

                if ((bool)mo["IPEnabled"])

                {

                    if (mo["Description"].ToString() == comboBox1.SelectedItem.ToString().Trim())

                    {

                        ManagementBaseObject newIP = mo.GetMethodParameters("EnableStatic");

                        ManagementBaseObject newGateway = mo.GetMethodParameters("SetGateways");

                        ManagementBaseObject newDNS = mo.GetMethodParameters("SetDNSServerSearchOrder");

                        // 将要修改的目标 IP 地址

                        //   string selectNewIP;
                        textBox1.Text = newIP["IPAddress"].ToString();
                        textBox2.Text = newIP["SubnetMask"].ToString();
                        textBox3.Text = newIP["DefaultIPGateway"].ToString();
                    }

                }

            }

        }
        private void button3_Click(object sender, EventArgs e)

        {

            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");

            ManagementObjectCollection moc = mc.GetInstances();

            foreach (ManagementObject mo in moc)

            {

                if ((bool)mo["IPEnabled"])

                {

                    if (mo["Description"].ToString() == comboBox1.SelectedItem.ToString().Trim())

                    {

                        //重置DNS为空

                        mo.InvokeMethod("SetDNSServerSearchOrder", null);

                        //开启DHCP

                        mo.InvokeMethod("EnableDHCP", null);

                        MessageBox.Show("自动获取IP成功！");

                        break;

                    }

                }






            }






        }
    }
}

