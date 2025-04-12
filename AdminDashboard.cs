using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace pizza_ordering_app
{
    public partial class AdminDashboard : Form
    {
        private int selectedRowIndex = -1;
        private bool isEditing = false;

        public AdminDashboard()
        {
            InitializeComponent();
            InitializeEventHandlers();
            SetupProductGrid();
            LoadProducts();
        }

        private void InitializeEventHandlers()
        {
            this.Load += AdminDashboard_Load;
            btnSales.Click += BtnSales_Click;
            btnInventory.Click += BtnInventory_Click;
            btnAdd.Click += BtnAdd_Click;
            btnSave.Click += BtnSave_Click;
            btnEdit.Click += BtnEdit_Click;
            btnDelete.Click += BtnDelete_Click;
            btnCancel.Click += BtnCancel_Click;
            dgvProducts.CellClick += DgvProducts_CellClick;
        }

        private void AdminDashboard_Load(object sender, EventArgs e)
        {
            panelInventory.Visible = false;
            btnCancel.Visible = false;
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

            var imageCol = new DataGridViewImageColumn
            {
                HeaderText = "Image",
                Name = "image",
                ImageLayout = DataGridViewImageCellLayout.Zoom
            };
            dgvProducts.Columns.Add(imageCol);
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (isEditing)
            {
                MessageBox.Show("Please save or cancel current operation first.");
                return;
            }

            dgvProducts.ReadOnly = false;
            dgvProducts.Rows.Add();
            selectedRowIndex = dgvProducts.Rows.Count - 1;
            dgvProducts.CurrentCell = dgvProducts.Rows[selectedRowIndex].Cells[0];
            isEditing = true;
            btnCancel.Visible = true;
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (isEditing)
            {
                MessageBox.Show("Finish current operation before editing another.");
                return;
            }

            if (selectedRowIndex >= 0 && selectedRowIndex < dgvProducts.Rows.Count)
            {
                dgvProducts.ReadOnly = false;
                isEditing = true;
                btnCancel.Visible = true;
                dgvProducts.CurrentCell = dgvProducts.Rows[selectedRowIndex].Cells[0];
            }
            else
            {
                MessageBox.Show("Please select a product to edit.");
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (selectedRowIndex < 0 || selectedRowIndex >= dgvProducts.Rows.Count) return;

            var row = dgvProducts.Rows[selectedRowIndex];
            if (!ValidateRow(row)) return;

            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    if (ProductExists(row.Cells["name"].Value.ToString(), conn))
                        UpdateProduct(row, conn);
                    else
                        InsertProduct(row, conn);
                }

                ResetEditingState();
                LoadProducts();
                MessageBox.Show("Product saved successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving product: {ex.Message}");
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (isEditing)
            {
                MessageBox.Show("Finish editing before deleting.");
                return;
            }

            if (selectedRowIndex < 0 || selectedRowIndex >= dgvProducts.Rows.Count)
            {
                MessageBox.Show("Please select a product to delete.");
                return;
            }

            var productName = dgvProducts.Rows[selectedRowIndex].Cells["name"].Value?.ToString();
            if (ConfirmDelete(productName))
            {
                DeleteProduct(productName);
                LoadProducts();
                selectedRowIndex = -1;
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            ResetEditingState();
            LoadProducts();
        }

        private void DgvProducts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            selectedRowIndex = e.RowIndex;
            if (dgvProducts.Columns[e.ColumnIndex].Name == "image" && !dgvProducts.ReadOnly)
            {
                HandleImageUpload(e.RowIndex);
            }
        }

        #region Database Operations
        private void LoadProducts()
        {
            dgvProducts.Rows.Clear();
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                using (var cmd = DatabaseHelper.CreateCommand("SELECT name, price, image FROM products", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var imgBytes = (byte[])reader["image"];
                        using (var ms = new MemoryStream(imgBytes))
                        {
                            dgvProducts.Rows.Add(
                                reader["name"],
                                reader["price"],
                                Image.FromStream(ms)
                            );
                        }
                    }
                }
            }
        }

        private void InsertProduct(DataGridViewRow row, MySqlConnection conn)
        {
            var cmd = DatabaseHelper.CreateCommand(
                "INSERT INTO products (name, price, image) VALUES (@name, @price, @image)", conn);
            AddCommonParameters(cmd, row);
            cmd.ExecuteNonQuery();
        }

        private void UpdateProduct(DataGridViewRow row, MySqlConnection conn)
        {
            var cmd = DatabaseHelper.CreateCommand(
                "UPDATE products SET price=@price, image=@image WHERE name=@name", conn);
            AddCommonParameters(cmd, row);
            cmd.ExecuteNonQuery();
        }

        private void DeleteProduct(string productName)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                using (var cmd = DatabaseHelper.CreateCommand(
                    "DELETE FROM products WHERE name = @name", conn))
                {
                    cmd.Parameters.AddWithValue("@name", productName);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        #endregion

        #region Helper Methods
        private bool ValidateRow(DataGridViewRow row)
        {
            if (string.IsNullOrWhiteSpace(row.Cells["name"].Value?.ToString()))
            {
                MessageBox.Show("Product name cannot be empty.");
                return false;
            }

            if (!decimal.TryParse(row.Cells["price"].Value?.ToString(), out _))
            {
                MessageBox.Show("Invalid price format.");
                return false;
            }

            if (row.Cells["image"].Value == null)
            {
                MessageBox.Show("Please select an image.");
                return false;
            }

            return true;
        }

        private bool ConfirmDelete(string productName)
        {
            return MessageBox.Show(
                $"Delete {productName}?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning) == DialogResult.Yes;
        }

        private void ResetEditingState()
        {
            dgvProducts.ReadOnly = true;
            isEditing = false;
            btnCancel.Visible = false;
            selectedRowIndex = -1;
        }

        private bool ProductExists(string name, MySqlConnection conn)
        {
            using (var cmd = DatabaseHelper.CreateCommand(
                "SELECT COUNT(*) FROM products WHERE name = @name", conn))
            {
                cmd.Parameters.AddWithValue("@name", name);
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }

        private void AddCommonParameters(MySqlCommand cmd, DataGridViewRow row)
        {
            cmd.Parameters.AddWithValue("@name", row.Cells["name"].Value);
            cmd.Parameters.AddWithValue("@price", Convert.ToDecimal(row.Cells["price"].Value));
            cmd.Parameters.AddWithValue("@image", ImageToBytes(row.Cells["image"].Value as Image));
        }

        private byte[] ImageToBytes(Image img)
        {
            using (var ms = new MemoryStream())
            {
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                return ms.ToArray();
            }
        }

        private void HandleImageUpload(int rowIndex)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        dgvProducts.Rows[rowIndex].Cells["image"].Value = Image.FromFile(ofd.FileName);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error loading image: {ex.Message}");
                    }
                }
            }
        }
        #endregion

        // Existing sales/inventory toggle methods
        private void BtnSales_Click(object sender, EventArgs e) => panelInventory.Visible = false;
        private void BtnInventory_Click(object sender, EventArgs e) => panelInventory.Visible = true;
        private void btnLogout_Click(object sender, EventArgs e) => ConfirmLogout();

        private void ConfirmLogout()
        {
            if (MessageBox.Show("Logout?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                new Login().Show();
                Close();
            }
        }
    }
}