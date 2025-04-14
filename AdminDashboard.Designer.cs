using System.Drawing;
using System.Windows.Forms;

namespace pizza_ordering_app
{
    partial class AdminDashboard
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.labelInventory = new System.Windows.Forms.Label();
            this.btnLogout = new System.Windows.Forms.Button();
            this.btnInventory = new System.Windows.Forms.Button();
            this.btnSales = new System.Windows.Forms.Button();
            this.panelInventory = new System.Windows.Forms.Panel();
            this.btnIngCancel = new System.Windows.Forms.Button();
            this.btnIngDelete = new System.Windows.Forms.Button();
            this.btnIngEdit = new System.Windows.Forms.Button();
            this.btnIngSave = new System.Windows.Forms.Button();
            this.btnIngAdd = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.dgvIngredients = new System.Windows.Forms.DataGridView();
            this.ingItem = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ingServingSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ingPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ingStockAvailable = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.dgvProducts = new System.Windows.Forms.DataGridView();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.image = new System.Windows.Forms.DataGridViewImageColumn();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panelInventory.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvIngredients)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProducts)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // labelInventory
            // 
            this.labelInventory.AutoSize = true;
            this.labelInventory.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelInventory.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.labelInventory.Location = new System.Drawing.Point(24, 0);
            this.labelInventory.Name = "labelInventory";
            this.labelInventory.Size = new System.Drawing.Size(112, 25);
            this.labelInventory.TabIndex = 0;
            this.labelInventory.Text = "Products";
            // 
            // btnLogout
            // 
            this.btnLogout.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLogout.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnLogout.Location = new System.Drawing.Point(12, 264);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(124, 47);
            this.btnLogout.TabIndex = 2;
            this.btnLogout.Text = "Logout";
            this.btnLogout.UseVisualStyleBackColor = true;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // btnInventory
            // 
            this.btnInventory.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnInventory.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnInventory.Location = new System.Drawing.Point(12, 211);
            this.btnInventory.Name = "btnInventory";
            this.btnInventory.Size = new System.Drawing.Size(124, 47);
            this.btnInventory.TabIndex = 1;
            this.btnInventory.Text = "Inventory";
            this.btnInventory.UseVisualStyleBackColor = true;
            // 
            // btnSales
            // 
            this.btnSales.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSales.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnSales.Location = new System.Drawing.Point(12, 158);
            this.btnSales.Name = "btnSales";
            this.btnSales.Size = new System.Drawing.Size(124, 47);
            this.btnSales.TabIndex = 0;
            this.btnSales.Text = "Sales";
            this.btnSales.UseVisualStyleBackColor = true;
            // 
            // panelInventory
            // 
            this.panelInventory.Controls.Add(this.btnIngCancel);
            this.panelInventory.Controls.Add(this.btnIngDelete);
            this.panelInventory.Controls.Add(this.btnIngEdit);
            this.panelInventory.Controls.Add(this.btnIngSave);
            this.panelInventory.Controls.Add(this.btnIngAdd);
            this.panelInventory.Controls.Add(this.btnCancel);
            this.panelInventory.Controls.Add(this.dgvIngredients);
            this.panelInventory.Controls.Add(this.btnDelete);
            this.panelInventory.Controls.Add(this.btnEdit);
            this.panelInventory.Controls.Add(this.label1);
            this.panelInventory.Controls.Add(this.btnSave);
            this.panelInventory.Controls.Add(this.btnAdd);
            this.panelInventory.Controls.Add(this.dgvProducts);
            this.panelInventory.Controls.Add(this.labelInventory);
            this.panelInventory.Location = new System.Drawing.Point(155, 12);
            this.panelInventory.Name = "panelInventory";
            this.panelInventory.Size = new System.Drawing.Size(839, 547);
            this.panelInventory.TabIndex = 0;
            // 
            // btnIngCancel
            // 
            this.btnIngCancel.Location = new System.Drawing.Point(457, 274);
            this.btnIngCancel.Name = "btnIngCancel";
            this.btnIngCancel.Size = new System.Drawing.Size(67, 25);
            this.btnIngCancel.TabIndex = 13;
            this.btnIngCancel.Text = "Cancel";
            this.btnIngCancel.UseVisualStyleBackColor = true;
            this.btnIngCancel.Visible = false;
            // 
            // btnIngDelete
            // 
            this.btnIngDelete.BackgroundImage = global::pizza_ordering_app.Properties.Resources.Red_and_Green_Modern_Pizza_Presentation;
            this.btnIngDelete.Font = new System.Drawing.Font("Verdana", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnIngDelete.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnIngDelete.Location = new System.Drawing.Point(749, 274);
            this.btnIngDelete.Name = "btnIngDelete";
            this.btnIngDelete.Size = new System.Drawing.Size(67, 25);
            this.btnIngDelete.TabIndex = 12;
            this.btnIngDelete.Text = "Delete";
            this.btnIngDelete.UseVisualStyleBackColor = true;
            // 
            // btnIngEdit
            // 
            this.btnIngEdit.BackgroundImage = global::pizza_ordering_app.Properties.Resources._591C40BD_C657_44A8_BD3D_676C05E00135_;
            this.btnIngEdit.Font = new System.Drawing.Font("Verdana", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnIngEdit.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnIngEdit.Location = new System.Drawing.Point(603, 274);
            this.btnIngEdit.Name = "btnIngEdit";
            this.btnIngEdit.Size = new System.Drawing.Size(67, 25);
            this.btnIngEdit.TabIndex = 11;
            this.btnIngEdit.Text = "Edit";
            this.btnIngEdit.UseVisualStyleBackColor = true;
            // 
            // btnIngSave
            // 
            this.btnIngSave.BackgroundImage = global::pizza_ordering_app.Properties.Resources._58DE3E7E_5C20_4D21_B9B4_AA9D72C03A49_;
            this.btnIngSave.Font = new System.Drawing.Font("Verdana", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnIngSave.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnIngSave.Location = new System.Drawing.Point(676, 274);
            this.btnIngSave.Name = "btnIngSave";
            this.btnIngSave.Size = new System.Drawing.Size(67, 25);
            this.btnIngSave.TabIndex = 10;
            this.btnIngSave.Text = "Save";
            this.btnIngSave.UseVisualStyleBackColor = true;
            // 
            // btnIngAdd
            // 
            this.btnIngAdd.BackgroundImage = global::pizza_ordering_app.Properties.Resources._CBA85D62_306A_4A06_A6A6_B6023109F900_;
            this.btnIngAdd.Font = new System.Drawing.Font("Verdana", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnIngAdd.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnIngAdd.Location = new System.Drawing.Point(530, 274);
            this.btnIngAdd.Name = "btnIngAdd";
            this.btnIngAdd.Size = new System.Drawing.Size(67, 25);
            this.btnIngAdd.TabIndex = 9;
            this.btnIngAdd.Text = "Add";
            this.btnIngAdd.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(457, 0);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(67, 25);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Visible = false;
            // 
            // dgvIngredients
            // 
            this.dgvIngredients.AllowUserToAddRows = false;
            this.dgvIngredients.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvIngredients.BackgroundColor = System.Drawing.SystemColors.ControlLight;
            this.dgvIngredients.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvIngredients.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ingItem,
            this.ingServingSize,
            this.ingPrice,
            this.ingStockAvailable});
            this.dgvIngredients.Font = new System.Drawing.Font("Arial", 10F);
            this.dgvIngredients.Location = new System.Drawing.Point(29, 305);
            this.dgvIngredients.MultiSelect = false;
            this.dgvIngredients.Name = "dgvIngredients";
            this.dgvIngredients.ReadOnly = true;
            this.dgvIngredients.RowHeadersWidth = 51;
            this.dgvIngredients.RowTemplate.Height = 24;
            this.dgvIngredients.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvIngredients.Size = new System.Drawing.Size(787, 239);
            this.dgvIngredients.TabIndex = 7;
            // 
            // ingItem
            // 
            this.ingItem.HeaderText = "Item";
            this.ingItem.MinimumWidth = 6;
            this.ingItem.Name = "ingItem";
            this.ingItem.ReadOnly = true;
            // 
            // ingServingSize
            // 
            this.ingServingSize.HeaderText = "Serving Size";
            this.ingServingSize.MinimumWidth = 6;
            this.ingServingSize.Name = "ingServingSize";
            this.ingServingSize.ReadOnly = true;
            // 
            // ingPrice
            // 
            this.ingPrice.HeaderText = "Price per Serving";
            this.ingPrice.MinimumWidth = 6;
            this.ingPrice.Name = "ingPrice";
            this.ingPrice.ReadOnly = true;
            // 
            // ingStockAvailable
            // 
            this.ingStockAvailable.HeaderText = "Stock Available";
            this.ingStockAvailable.MinimumWidth = 6;
            this.ingStockAvailable.Name = "ingStockAvailable";
            this.ingStockAvailable.ReadOnly = true;
            // 
            // btnDelete
            // 
            this.btnDelete.BackgroundImage = global::pizza_ordering_app.Properties.Resources.Red_and_Green_Modern_Pizza_Presentation;
            this.btnDelete.Font = new System.Drawing.Font("Verdana", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelete.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnDelete.Location = new System.Drawing.Point(749, 0);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(67, 25);
            this.btnDelete.TabIndex = 6;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            // 
            // btnEdit
            // 
            this.btnEdit.BackgroundImage = global::pizza_ordering_app.Properties.Resources._591C40BD_C657_44A8_BD3D_676C05E00135_;
            this.btnEdit.Font = new System.Drawing.Font("Verdana", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEdit.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnEdit.Location = new System.Drawing.Point(603, 0);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(67, 25);
            this.btnEdit.TabIndex = 5;
            this.btnEdit.Text = "Edit";
            this.btnEdit.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(24, 274);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(143, 25);
            this.label1.TabIndex = 4;
            this.label1.Text = "Ingredients";
            // 
            // btnSave
            // 
            this.btnSave.BackgroundImage = global::pizza_ordering_app.Properties.Resources._58DE3E7E_5C20_4D21_B9B4_AA9D72C03A49_;
            this.btnSave.Font = new System.Drawing.Font("Verdana", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnSave.Location = new System.Drawing.Point(676, 0);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(67, 25);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // btnAdd
            // 
            this.btnAdd.BackgroundImage = global::pizza_ordering_app.Properties.Resources._CBA85D62_306A_4A06_A6A6_B6023109F900_;
            this.btnAdd.Font = new System.Drawing.Font("Verdana", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdd.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnAdd.Location = new System.Drawing.Point(530, 0);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(67, 25);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            // 
            // dgvProducts
            // 
            this.dgvProducts.AllowUserToAddRows = false;
            this.dgvProducts.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvProducts.BackgroundColor = System.Drawing.SystemColors.ControlLight;
            this.dgvProducts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProducts.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.name,
            this.price,
            this.image});
            this.dgvProducts.Font = new System.Drawing.Font("Arial", 10F);
            this.dgvProducts.Location = new System.Drawing.Point(29, 31);
            this.dgvProducts.MultiSelect = false;
            this.dgvProducts.Name = "dgvProducts";
            this.dgvProducts.ReadOnly = true;
            this.dgvProducts.RowHeadersWidth = 51;
            this.dgvProducts.RowTemplate.Height = 24;
            this.dgvProducts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvProducts.Size = new System.Drawing.Size(787, 204);
            this.dgvProducts.TabIndex = 1;
            // 
            // name
            // 
            this.name.HeaderText = "Name";
            this.name.MinimumWidth = 6;
            this.name.Name = "name";
            this.name.ReadOnly = true;
            // 
            // price
            // 
            this.price.HeaderText = "Price";
            this.price.MinimumWidth = 6;
            this.price.Name = "price";
            this.price.ReadOnly = true;
            // 
            // image
            // 
            this.image.HeaderText = "Image";
            this.image.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.image.MinimumWidth = 6;
            this.image.Name = "image";
            this.image.ReadOnly = true;
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog1";
            // 
            // panel1
            // 
            this.panel1.BackgroundImage = global::pizza_ordering_app.Properties.Resources.Red_and_Green_Modern_Pizza_Presentation;
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(147, 571);
            this.panel1.TabIndex = 5;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.pictureBox1.Image = global::pizza_ordering_app.Properties.Resources.Beige_Red_Fun_Food_Pizza_Loyalty_Card__3_3;
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(124, 124);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            // 
            // AdminDashboard
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1006, 571);
            this.Controls.Add(this.panelInventory);
            this.Controls.Add(this.btnLogout);
            this.Controls.Add(this.btnInventory);
            this.Controls.Add(this.btnSales);
            this.Controls.Add(this.panel1);
            this.Name = "AdminDashboard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Dashboard";
            this.panelInventory.ResumeLayout(false);
            this.panelInventory.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvIngredients)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProducts)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSales;
        private System.Windows.Forms.Button btnInventory;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panelInventory;
        private System.Windows.Forms.Label labelInventory;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.DataGridView dgvProducts;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private DataGridViewTextBoxColumn name;
        private DataGridViewTextBoxColumn price;
        private DataGridViewImageColumn image;
        private Label label1;
        private Button btnDelete;
        private Button btnEdit;
        private DataGridView dgvIngredients;
        private Button btnCancel;
        private Button btnIngCancel;
        private Button btnIngDelete;
        private Button btnIngEdit;
        private Button btnIngSave;
        private Button btnIngAdd;
        private DataGridViewTextBoxColumn ingItem;
        private DataGridViewTextBoxColumn ingServingSize;
        private DataGridViewTextBoxColumn ingPrice;
        private DataGridViewTextBoxColumn ingStockAvailable;
    }
}