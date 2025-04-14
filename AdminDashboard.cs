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
        private bool isEditingIngredient = false;

        public AdminDashboard()
        {
            InitializeComponent();
            InitializeEventHandlers();
            SetupProductGrid();
            SetupIngredientGrid();
            LoadProducts();
            LoadIngredients();
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

            btnIngAdd.Click += BtnIngAdd_Click;
            btnIngSave.Click += BtnIngSave_Click;
            btnIngEdit.Click += BtnIngEdit_Click;
            btnIngDelete.Click += BtnIngDelete_Click;
            btnIngCancel.Click += BtnIngCancel_Click;
            dgvIngredients.CellClick += DgvIngredients_CellClick;
        }

        private void AdminDashboard_Load(object sender, EventArgs e)
        {
            panelInventory.Visible = false;
            btnCancel.Visible = false;
            btnIngCancel.Visible = false;
        }

        private void SetupProductGrid()
        {
            dgvProducts.Columns.Clear();
            dgvProducts.ReadOnly = true;
            dgvProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProducts.MultiSelect = false;
            dgvProducts.AllowUserToAddRows = false;
            dgvProducts.RowTemplate.Height = 60;

            dgvProducts.Columns.Add("id", "ID");
            dgvProducts.Columns["id"].Visible = false;

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

        private void SetupIngredientGrid()
        {
            dgvIngredients.Columns.Clear();
            dgvIngredients.ReadOnly = true;
            dgvIngredients.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvIngredients.MultiSelect = false;
            dgvIngredients.AllowUserToAddRows = false;

            dgvIngredients.Columns.Add("id", "ID");
            dgvIngredients.Columns["id"].Visible = false;
            dgvIngredients.Columns.Add("ingItem", "Item");
            dgvIngredients.Columns.Add("ingServingSize", "Serving Size");
            dgvIngredients.Columns.Add("ingPrice", "Price per Serving");
            dgvIngredients.Columns.Add("ingStockAvailable", "Stock Available");
        }

        private void LoadProducts()
        {
            dgvProducts.Rows.Clear();
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                var cmd = DatabaseHelper.CreateCommand("SELECT id, name, price, image FROM products", conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var imgBytes = (byte[])reader["image"];
                        using (var ms = new MemoryStream(imgBytes))
                        {
                            dgvProducts.Rows.Add(
                                reader["id"],
                                reader["name"],
                                reader["price"],
                                Image.FromStream(ms)
                            );
                        }
                    }
                }
            }
        }

        private void LoadIngredients()
        {
            dgvIngredients.Rows.Clear();
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                var cmd = DatabaseHelper.CreateCommand("SELECT * FROM ingredients", conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        dgvIngredients.Rows.Add(
                            reader["id"],
                            reader["name"],
                            reader["serving_size"],
                            reader["price_per_serving"],
                            reader["stock_available"]
                        );
                    }
                }
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (isEditing) return;
            dgvProducts.ReadOnly = false;
            dgvProducts.Rows.Add();
            selectedRowIndex = dgvProducts.Rows.Count - 1;
            dgvProducts.CurrentCell = dgvProducts.Rows[selectedRowIndex].Cells["name"];
            isEditing = true;
            btnCancel.Visible = true;
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (isEditing) return;
            if (dgvProducts.SelectedRows.Count == 0) return;
            selectedRowIndex = dgvProducts.SelectedRows[0].Index;
            dgvProducts.ReadOnly = false;
            isEditing = true;
            btnCancel.Visible = true;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (selectedRowIndex < 0 || selectedRowIndex >= dgvProducts.Rows.Count) return;
            var row = dgvProducts.Rows[selectedRowIndex];
            if (!ValidateRow(row)) return;

            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                if (row.Cells["id"].Value != null)
                    UpdateProduct(row, conn, Convert.ToInt32(row.Cells["id"].Value));
                else
                    InsertProduct(row, conn);
            }
            LoadProducts();
            ResetEditingState();
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (isEditing || dgvProducts.SelectedRows.Count == 0) return;
            var row = dgvProducts.SelectedRows[0];
            if (int.TryParse(row.Cells["id"].Value?.ToString(), out int id))
            {
                DeleteProduct(id);
                LoadProducts();
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            ResetEditingState();
            LoadProducts();
        }

        private void ResetEditingState()
        {
            dgvProducts.ReadOnly = true;
            isEditing = false;
            btnCancel.Visible = false;
            selectedRowIndex = -1;
        }

        private bool ValidateRow(DataGridViewRow row)
        {
            if (string.IsNullOrWhiteSpace(row.Cells["name"].Value?.ToString())) return false;
            if (!decimal.TryParse(row.Cells["price"].Value?.ToString(), out _)) return false;
            if (row.Cells["image"].Value == null) return false;
            return true;
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

        private void HandleImageUpload(int rowIndex)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    dgvProducts.Rows[rowIndex].Cells["image"].Value = Image.FromFile(ofd.FileName);
                }
            }
        }

        private void InsertProduct(DataGridViewRow row, MySqlConnection conn)
        {
            var cmd = DatabaseHelper.CreateCommand("INSERT INTO products (name, price, image) VALUES (@name, @price, @image)", conn);
            AddProductParameters(cmd, row);
            cmd.ExecuteNonQuery();
        }

        private void UpdateProduct(DataGridViewRow row, MySqlConnection conn, int id)
        {
            var cmd = DatabaseHelper.CreateCommand("UPDATE products SET name=@name, price=@price, image=@image WHERE id=@id", conn);
            AddProductParameters(cmd, row);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }

        private void DeleteProduct(int id)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                var cmd = DatabaseHelper.CreateCommand("DELETE FROM products WHERE id = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }

        private void AddProductParameters(MySqlCommand cmd, DataGridViewRow row)
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

        private void BtnIngAdd_Click(object sender, EventArgs e)
        {
            if (isEditingIngredient) return;
            dgvIngredients.ReadOnly = false;
            dgvIngredients.Rows.Add();
            selectedRowIndex = dgvIngredients.Rows.Count - 1;
            dgvIngredients.CurrentCell = dgvIngredients.Rows[selectedRowIndex].Cells["ingItem"];
            isEditingIngredient = true;
            btnIngCancel.Visible = true;
            MessageBox.Show("Ready to add a new ingredient.", "Add Ingredient", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnIngEdit_Click(object sender, EventArgs e)
        {
            if (isEditingIngredient || dgvIngredients.SelectedRows.Count == 0) return;
            selectedRowIndex = dgvIngredients.SelectedRows[0].Index;
            dgvIngredients.ReadOnly = false;
            isEditingIngredient = true;
            btnIngCancel.Visible = true;
            MessageBox.Show("Editing ingredient.", "Edit Ingredient", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnIngSave_Click(object sender, EventArgs e)
        {
            if (selectedRowIndex < 0 || selectedRowIndex >= dgvIngredients.Rows.Count) return;
            var row = dgvIngredients.Rows[selectedRowIndex];
            if (!ValidateIngredientRow(row)) return;
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                if (row.Cells["id"].Value != null)
                    UpdateIngredient(row, conn, Convert.ToInt32(row.Cells["id"].Value));
                else
                    InsertIngredient(row, conn);
            }
            LoadIngredients();
            ResetIngredientState();
            MessageBox.Show("Ingredient saved successfully.", "Save Ingredient", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnIngDelete_Click(object sender, EventArgs e)
        {
            if (isEditingIngredient || dgvIngredients.SelectedRows.Count == 0) return;
            var row = dgvIngredients.SelectedRows[0];
            if (int.TryParse(row.Cells["id"].Value?.ToString(), out int id))
            {
                DeleteIngredient(id);
                LoadIngredients();
                MessageBox.Show("Ingredient deleted.", "Delete Ingredient", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnIngCancel_Click(object sender, EventArgs e)
        {
            ResetIngredientState();
            LoadIngredients();
            MessageBox.Show("Cancelled ingredient action.", "Cancel", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void DgvIngredients_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
                selectedRowIndex = e.RowIndex;
        }

        private void ResetIngredientState()
        {
            dgvIngredients.ReadOnly = true;
            isEditingIngredient = false;
            btnIngCancel.Visible = false;
            selectedRowIndex = -1;
        }

        private bool ValidateIngredientRow(DataGridViewRow row)
        {
            return !string.IsNullOrWhiteSpace(row.Cells["ingItem"].Value?.ToString()) &&
                   !string.IsNullOrWhiteSpace(row.Cells["ingServingSize"].Value?.ToString()) &&
                   decimal.TryParse(row.Cells["ingPrice"].Value?.ToString(), out _) &&
                   decimal.TryParse(row.Cells["ingStockAvailable"].Value?.ToString(), out _);
        }

        private void InsertIngredient(DataGridViewRow row, MySqlConnection conn)
        {
            var cmd = DatabaseHelper.CreateCommand("INSERT INTO ingredients (name, serving_size, price_per_serving, stock_available) VALUES (@name, @size, @price, @stock)", conn);
            AddIngredientParameters(cmd, row);
            cmd.ExecuteNonQuery();
        }

        private void UpdateIngredient(DataGridViewRow row, MySqlConnection conn, int id)
        {
            var cmd = DatabaseHelper.CreateCommand("UPDATE ingredients SET name=@name, serving_size=@size, price_per_serving=@price, stock_available=@stock WHERE id=@id", conn);
            AddIngredientParameters(cmd, row);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }

        private void DeleteIngredient(int id)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                var cmd = DatabaseHelper.CreateCommand("DELETE FROM ingredients WHERE id=@id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }

        private void AddIngredientParameters(MySqlCommand cmd, DataGridViewRow row)
        {
            cmd.Parameters.AddWithValue("@name", row.Cells["ingItem"].Value?.ToString());
            cmd.Parameters.AddWithValue("@size", row.Cells["ingServingSize"].Value?.ToString());
            cmd.Parameters.AddWithValue("@price", Convert.ToDecimal(row.Cells["ingPrice"].Value));
            cmd.Parameters.AddWithValue("@stock", Convert.ToDecimal(row.Cells["ingStockAvailable"].Value));
        }

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
