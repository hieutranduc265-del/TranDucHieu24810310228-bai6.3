using System;
using System.Windows.Forms;
using ProjectTaskManagement.BUS;
using ProjectTaskManagement.DTO;

namespace ProjectTaskManagement.GUI
{
    public class frmDuAn : Form
    {
        private readonly DuAnBUS bus = new DuAnBUS();
        private int selectedId = 0;

        private TextBox txtTen;
        private DateTimePicker dtpBatDau, dtpKetThuc;
        private ComboBox cboTrangThai;
        private DataGridView dgv;

        public frmDuAn()
        {
            BuildUI();
            LoadData();
        }

        private void BuildUI()
        {
            var lblTen = new Label { Text = "Tên dự án:", Left = 20, Top = 20, Width = 90 };
            txtTen = new TextBox { Left = 115, Top = 17, Width = 260 };

            var lblTT = new Label { Text = "Trạng thái:", Left = 400, Top = 20, Width = 80 };
            cboTrangThai = new ComboBox { Left = 485, Top = 17, Width = 160, DropDownStyle = ComboBoxStyle.DropDownList };
            cboTrangThai.Items.AddRange(new object[] { "Đang chạy", "Hoàn thành", "Tạm dừng" });
            cboTrangThai.SelectedIndex = 0;

            var lblBD = new Label { Text = "Ngày bắt đầu:", Left = 20, Top = 55, Width = 90 };
            dtpBatDau = new DateTimePicker { Left = 115, Top = 52, Width = 160, Format = DateTimePickerFormat.Short };

            var lblKT = new Label { Text = "Ngày kết thúc:", Left = 300, Top = 55, Width = 95 };
            dtpKetThuc = new DateTimePicker { Left = 400, Top = 52, Width = 160, Format = DateTimePickerFormat.Short };

            var btnThem = new Button { Text = "Thêm", Left = 115, Top = 92, Width = 85, Height = 32 };
            var btnSua = new Button { Text = "Sửa", Left = 210, Top = 92, Width = 85, Height = 32 };
            var btnXoa = new Button { Text = "Xóa", Left = 305, Top = 92, Width = 85, Height = 32 };
            var btnLamMoi = new Button { Text = "Làm mới", Left = 400, Top = 92, Width = 95, Height = 32 };

            btnThem.Click += BtnThem_Click;
            btnSua.Click += BtnSua_Click;
            btnXoa.Click += BtnXoa_Click;
            btnLamMoi.Click += (s, e) => ClearForm();

            dgv = new DataGridView
            {
                Left = 20,
                Top = 140,
                Width = 860,
                Height = 460,
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ReadOnly = true
            };
            dgv.CellClick += Dgv_CellClick;

            Controls.AddRange(new Control[]
            {
                lblTen, txtTen, lblTT, cboTrangThai, lblBD, dtpBatDau, lblKT, dtpKetThuc,
                btnThem, btnSua, btnXoa, btnLamMoi, dgv
            });
        }

        private void LoadData()
        {
            dgv.DataSource = null;
            dgv.DataSource = bus.GetAll();
        }

        private void ClearForm()
        {
            selectedId = 0;
            txtTen.Clear();
            cboTrangThai.SelectedIndex = 0;
            dtpBatDau.Value = DateTime.Today;
            dtpKetThuc.Value = DateTime.Today.AddDays(30);
        }

        private void Dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || dgv.CurrentRow == null) return;
            var item = dgv.CurrentRow.DataBoundItem as DuAnDTO;
            if (item == null) return;

            selectedId = item.MaDuAn;
            txtTen.Text = item.TenDuAn;
            cboTrangThai.Text = item.TrangThai;
            if (item.NgayBatDau.HasValue) dtpBatDau.Value = item.NgayBatDau.Value;
            if (item.NgayKetThuc.HasValue) dtpKetThuc.Value = item.NgayKetThuc.Value;
        }

        private void BtnThem_Click(object sender, EventArgs e)
        {
            var da = new DuAnDTO
            {
                TenDuAn = txtTen.Text.Trim(),
                NgayBatDau = dtpBatDau.Value,
                NgayKetThuc = dtpKetThuc.Value,
                TrangThai = cboTrangThai.Text
            };

            var res = bus.Insert(da);
            MessageBox.Show(res.msg);
            if (res.ok) { LoadData(); ClearForm(); }
        }

        private void BtnSua_Click(object sender, EventArgs e)
        {
            var da = new DuAnDTO
            {
                MaDuAn = selectedId,
                TenDuAn = txtTen.Text.Trim(),
                NgayBatDau = dtpBatDau.Value,
                NgayKetThuc = dtpKetThuc.Value,
                TrangThai = cboTrangThai.Text
            };

            var res = bus.Update(da);
            MessageBox.Show(res.msg);
            if (res.ok) { LoadData(); ClearForm(); }
        }

        private void BtnXoa_Click(object sender, EventArgs e)
        {
            if (selectedId <= 0) { MessageBox.Show("Chọn dự án cần xóa."); return; }
            if (MessageBox.Show("Xóa dự án sẽ xóa luôn các task liên quan. Tiếp tục?", "Xác nhận", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            var res = bus.Delete(selectedId);
            MessageBox.Show(res.msg);
            if (res.ok) { LoadData(); ClearForm(); }
        }
    }
}