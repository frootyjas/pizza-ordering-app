using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace pizza_ordering_app
{
    public partial class AdminDashboard : Form
    {
        private int selectedRowIndex = -1;
        private bool isEditing = false;

        public AdminDashboard()
        {
            InitializeComponent();
            this.Load += AdminDashboard_Load;

            btnSales.Click += BtnSales_Click;
            btnInventory.Click += BtnInventory_Click;
            btnAdd.Click += btnAdd_Click;
            btnSave.Click += btnSave_Click;
            btnEdit.Click += btnEdit_Click;
            btnDelete.Click += btnDelete_Click;
            dgvProducts.CellClick += dgvProducts_CellClick;

            SetupProductGrid();
            LoadProducts();
        }

        private void AdminDashboard_Load(object sender, EventArgs e)
        {
            panelInventory.Visible = false;
        }

        private void BtnSales_Click(object sender, EventArgs e)
        {
            panelInventory.Visible = false;
        }

        private void BtnInventory_Click(object sender, EventArgs e)
        {
            panelInventory.Visible = true;
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Are you sure you want to logout?",
                "Confirm Logout",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                Form1 login = new Form1();
                login.Show();
                this.Close();
            }
        }

        private void SetupProductGrid()
        {
            dgvProducts.Columns.Clear();
            dgvProducts.ReadOnly = true;
            dgvProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProducts.MultiSelect = false;
            dgvProducts.AllowUserToAddRows = false;
            dgvProducts.RowTemplate.Height = 60;

            dgvProducts.Columns.Add("name", "Name");
            dgvProducts.Columns.Add("price", "Price");

            DataGridViewImageColumn imageCol = new DataGridViewImageColumn();
            imageCol.HeaderText = "Image";
            imageCol.Name = "image";
            imageCol.ImageLayout = DataGridViewImageCellLayout.Zoom;
            dgvProducts.Columns.Add(imageCol);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (isEditing)
            {
                MessageBox.Show("Please save the current edit before adding a new product.");
                return;
            }

            dgvProducts.ReadOnly = false;
            dgvProducts.Rows.Add();
            selectedRowIndex = dgvProducts.Rows.Count - 1;
            dgvProducts.CurrentCell = dgvProducts.Rows[selectedRowIndex].Cells[0];
            isEditing = true;

            // Show cancel button
            btnCancel.Visible = true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (selectedRowIndex >= 0 && selectedRowIndex < dgvProducts.Rows.Count)
            {
                dgvProducts.Rows.RemoveAt(selectedRowIndex);
            }

            dgvProducts.ReadOnly = true;
            isEditing = false;
            selectedRowIndex = -1;

            btnCancel.Visible = false;
        }


        private void dgvProducts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                selectedRowIndex = e.RowIndex;

                if (e.ColumnIndex == dgvProducts.Columns["image"].Index && !dgvProducts.ReadOnly)
                {
                    using (OpenFileDialog ofd = new OpenFileDialog())
                    {
                        ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";

                        if (ofd.ShowDialog() == DialogResult.OK)
                        {
                            try
                            {
                                Image img = Image.FromFile(ofd.FileName);
                                dgvProducts.Rows[e.RowIndex].Cells["image"].Value = img;
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Error loading image: " + ex.Message);
                            }
                        }
                    }
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (selectedRowIndex < 0 || selectedRowIndex >= dgvProducts.Rows.Count)
            {
                MessageBox.Show("No row selected for saving.");
                return;
            }

            DataGridViewRow row = dgvProducts.Rows[selectedRowIndex];
            string name = row.Cells["name"].Value?.ToString();
            if (string.IsNullOrWhiteSpace(name)) return;

            if (!decimal.TryParse(row.Cells["price"].Value?.ToString(), out decimal price)) return;

            Image img = row.Cells["image"].Value as Image;
            if (img == null)
            {
                MessageBox.Show("Please make sure the product has an image.");
                return;
            }

            byte[] imgBytes;
            using (MemoryStream ms = new MemoryStream())
            {
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                imgBytes = ms.ToArray();
            }

           // string connStr = "server=localhost;user id=root;password=;database=pizza_app;";
            string connStr = "server=sql12.freesqldatabase.com;user id=sql12772758;password=ZfPWDQ2dFW;database=sql12772758;";

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                string checkSql = "SELECT COUNT(*) FROM products WHERE name = @name";
                using (MySqlCommand checkCmd = new MySqlCommand(checkSql, conn))
                {
                    checkCmd.Parameters.AddWithValue("@name", name);
                    int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                    if (count > 0)
                    {
                        string updateSql = "UPDATE products SET price=@price, image=@image WHERE name=@name";
                        using (MySqlCommand updateCmd = new MySqlCommand(updateSql, conn))
                        {
                            updateCmd.Parameters.AddWithValue("@price", price);
                            updateCmd.Parameters.AddWithValue("@image", imgBytes);
                            updateCmd.Parameters.AddWithValue("@name", name);
                            updateCmd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        string insertSql = "INSERT INTO products (name, price, image) VALUES (@name, @price, @image)";
                        using (MySqlCommand insertCmd = new MySqlCommand(insertSql, conn))
                        {
                            insertCmd.Parameters.AddWithValue("@name", name);
                            insertCmd.Parameters.AddWithValue("@price", price);
                            insertCmd.Parameters.AddWithValue("@image", imgBytes);
                            insertCmd.ExecuteNonQuery();
                        }
                    }
                }

                btnCancel.Visible = false;

            }

            MessageBox.Show("Product saved!");
            dgvProducts.ReadOnly = true;
            isEditing = false;
            LoadProducts();
        }

        private void LoadProducts()
        {
            //string connStr = "server=localhost;user id=root;password=;database=pizza_app;";
            string connStr = "server=sql12.freesqldatabase.com;user id=sql12772758;password=ZfPWDQ2dFW;database=sql12772758;";

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                string query = "SELECT name, price, image FROM products";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                dgvProducts.Rows.Clear();

                while (reader.Read())
                {
                    string name = reader.GetString("name");
                    decimal price = reader.GetDecimal("price");
                    byte[] imgBytes = (byte[])reader["image"];

                    Image img;
                    using (MemoryStream ms = new MemoryStream(imgBytes))
                    {
                        img = Image.FromStream(ms);
                    }

                    dgvProducts.Rows.Add(name, price, img);
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (isEditing)
            {
                MessageBox.Show("Finish editing the current product before editing another.");
                return;
            }

            if (selectedRowIndex >= 0 && selectedRowIndex < dgvProducts.Rows.Count)
            {
                dgvProducts.ReadOnly = false;
                dgvProducts.Rows[selectedRowIndex].ReadOnly = false;
                dgvProducts.CurrentCell = dgvProducts.Rows[selectedRowIndex].Cells[0];
                isEditing = true;
            }
            else
            {
                MessageBox.Show("Please select a row to edit.");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (isEditing)
            {
                MessageBox.Show("Finish editing before deleting.");
                return;
            }

            if (selectedRowIndex >= 0 && selectedRowIndex < dgvProducts.Rows.Count)
            {
                string productName = dgvProducts.Rows[selectedRowIndex].Cells["name"].Value?.ToString();

                DialogResult confirm = MessageBox.Show(
                    $"Are you sure you want to delete '{productName}'?",
                    "Confirm Delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (confirm == DialogResult.Yes)
                {
                    //string connStr = "server=localhost;user id=root;password=;database=pizza_app;";
                    string connStr = "server=sql12.freesqldatabase.com;user id=sql12772758;password=ZfPWDQ2dFW;database=sql12772758;";

                    using (MySqlConnection conn = new MySqlConnection(connStr))
                    {
                        conn.Open();
                        string deleteQuery = "DELETE FROM products WHERE name = @name";
                        using (MySqlCommand cmd = new MySqlCommand(deleteQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@name", productName);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    dgvProducts.Rows.RemoveAt(selectedRowIndex);
                    selectedRowIndex = -1;
                    MessageBox.Show("Product deleted successfully.");
                }
            }
            else
            {
                MessageBox.Show("Please select a row to delete.");
            }
        }

        private void dgvProducts_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Placeholder if needed
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}