using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WMS
{
    public partial class Setting : Form
    {
        public Setting()
        {
            InitializeComponent();
        }

     
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

       
      

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            MDI mdi = new MDI();
            mdi.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default["ServerAddress"] =txtservername.Text;
            Properties.Settings.Default["Port"] = int.Parse(txtport.Text);
            Properties.Settings.Default["ClientId"] = int.Parse(txtport.Text);
            Properties.Settings.Default["OrgId"]= int.Parse(txtorgid.Text);
            Properties.Settings.Default["WarehouseId"]=int.Parse(txtwarehouseid.Text);
            Properties.Settings.Default["RoleId"]= int.Parse(txtroleid.Text);
            Properties.Settings.Default["WeightPrinterName"] =txtweightprintername.Text;
            Properties.Settings.Default["scaleport"] =txtscaleport.Text;
            Properties.Settings.Default["Bits"]= int.Parse(txtbit.Text);
            Properties.Settings.Default["Databits"] =int.Parse(txtdatabit.Text);
            Properties.Settings.Default["Stopbits"] =int.Parse(txtstopbit.Text);
            Properties.Settings.Default["Parity"] =txtparity.Text;
            Properties.Settings.Default["Server"] =textserver.Text;
            Properties.Settings.Default["Serverport"] =int.Parse(txtportdata.Text);
            Properties.Settings.Default["User"] =txtuser.Text;
            Properties.Settings.Default["Password"] =txtpassword.Text;
            Properties.Settings.Default["Database"] =txtdatabase.Text;
            Properties.Settings.Default.Save();

            MessageBox.Show("Data is save!");
        }

        private void Setting_Load(object sender, EventArgs e)
        { 
          txtservername.Text = (string)Properties.Settings.Default["ServerAddress"];
            txtport.Text = ((int)Properties.Settings.Default["Port"]).ToString();
            txtclientid.Text= ((int)Properties.Settings.Default["ClientId"]).ToString();
            txtorgid.Text= ((int)Properties.Settings.Default["OrgId"]).ToString();
            txtwarehouseid.Text= ((int)Properties.Settings.Default["WarehouseId"]).ToString();
            txtroleid.Text= ((int)Properties.Settings.Default["RoleId"]).ToString();
            txtweightprintername.Text= (string)Properties.Settings.Default["WeightPrinterName"];
            txtscaleport.Text= (string)Properties.Settings.Default["scaleport"];
            txtbit.Text= ((int)Properties.Settings.Default["Bits"]).ToString();
            txtdatabit.Text= ((int)Properties.Settings.Default["Databits"]).ToString();
            txtstopbit.Text= ((int)Properties.Settings.Default["Stopbits"]).ToString();
            txtparity.Text= (string)Properties.Settings.Default["Parity"];
            textserver.Text = (string)Properties.Settings.Default["Server"];
            txtportdata.Text= ((int)Properties.Settings.Default["Serverport"]).ToString();
            txtuser.Text= (string)Properties.Settings.Default["User"];
            txtpassword.Text = (string)Properties.Settings.Default["Password"];
            txtdatabase.Text = (string)Properties.Settings.Default["Database"];
        }
    }
}
