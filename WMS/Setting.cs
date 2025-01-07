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

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                // Define the file path (save in the user's documents folder)
                string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "settings.txt");

                // Collect data from form fields
                string data = $"Server Address: {txtservername.Text}\n" +
                              $"Port: {txtport.Text}\n" +
                              $"Client ID: {txtclientid.Text}\n" +
                              $"Org ID: {txtorgid.Text}\n" +
                              $"Warehouse ID: {txtwarehouseid.Text}\n" +
                              $"Role ID: {txtroleid.Text}\n" +
                              $"Weight Printer Name: {txtweightprintername.Text}\n" +
                              $"Server: {textserver.Text}\n" +
                              $"DataPort: {txtportdata.Text}\n" +
                              $"User: {txtuser.Text}\n" +
                              $"Password: {txtpassword.Text}\n" +
                              $"Database: {txtdatabase.Text}\n" +
                              $"Scale Port: {txtscaleport.Text}\n" +
                              $"Bit: {txtbit.Text}\n" +
                              $"Data Bit: {txtdatabit.Text}\n" +
                              $"Stop Bit: {txtstopbit.Text}\n" +
                              $"Parity: {txtparity.Text}\n";

                // Write data to the file
                File.WriteAllText(filePath, data);

                // Show a success message
                MessageBox.Show($"Data saved successfully to {filePath}!");
            }
            catch (Exception ex)
            {
                // Show an error message in case of failure
                MessageBox.Show($"Failed to save data: {ex.Message}");
            }
          

        }

     
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Setting_Load(object sender, EventArgs e)
        {
            try
            {
                // Define the file path (same location as where the file was saved)
                string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "settings.txt");

                // Check if the file exists
                if (File.Exists(filePath))
                {
                    // Read all lines from the file
                    string[] lines = File.ReadAllLines(filePath);

                    // Parse each line and assign the values to form fields
                    foreach (string line in lines)
                    {
                        if (line.StartsWith("Server Address:"))
                            txtservername.Text = line.Replace("Server Address:", "").Trim();
                        else if (line.StartsWith("Port:"))
                            txtport.Text = line.Replace("Port:", "").Trim();
                        else if (line.StartsWith("Client ID:"))
                            txtclientid.Text = line.Replace("Client ID:", "").Trim();
                        else if (line.StartsWith("Org ID:"))
                            txtorgid.Text = line.Replace("Org ID:", "").Trim();
                        else if (line.StartsWith("Warehouse ID:"))
                            txtwarehouseid.Text = line.Replace("Warehouse ID:", "").Trim();
                        else if (line.StartsWith("Role ID:"))
                            txtroleid.Text = line.Replace("Role ID:", "").Trim();
                        else if (line.StartsWith("Weight Printer Name:"))
                            txtweightprintername.Text = line.Replace("Weight Printer Name:", "").Trim();
                        else if (line.StartsWith("Server:"))
                            textserver.Text = line.Replace("Server:", "").Trim();
                        else if (line.StartsWith("DataPort:"))
                            txtportdata.Text = line.Replace("DataPort:", "").Trim();
                        else if (line.StartsWith("User:"))
                            txtuser.Text = line.Replace("User:", "").Trim();
                        else if (line.StartsWith("Password:"))
                            txtpassword.Text = line.Replace("Password:", "").Trim();
                        else if (line.StartsWith("Database:"))
                            txtdatabase.Text = line.Replace("Database:", "").Trim();
                        else if (line.StartsWith("Scale Port:"))
                            txtscaleport.Text = line.Replace("Scale Port:", "").Trim();
                        else if (line.StartsWith("Bit:"))
                            txtbit.Text = line.Replace("Bit:", "").Trim();
                        else if (line.StartsWith("Data Bit:"))
                            txtdatabit.Text = line.Replace("Data Bit:", "").Trim();
                        else if (line.StartsWith("Stop Bit:"))
                            txtstopbit.Text = line.Replace("Stop Bit:", "").Trim();
                        else if (line.StartsWith("Parity:"))
                            txtparity.Text = line.Replace("Parity:", "").Trim();
                    }
                }
                else
                {
                    MessageBox.Show("Settings file not found. Please save the settings first.");
                }
            }
            catch (Exception ex)
            {
                // Show an error message in case of failure
                MessageBox.Show($"Failed to load settings: {ex.Message}");
            }
        }

      

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            MDI mdi = new MDI();
            mdi.Show();
            this.Hide();
        }
    }
}
