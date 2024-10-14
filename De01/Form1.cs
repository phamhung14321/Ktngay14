using De01.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace De01
{
    public partial class frmSinhVien : Form
    {
        private StudentContentDB StudentDB = new StudentContentDB();
        public frmSinhVien()
        {
            InitializeComponent();
        }

        private void frmSinhVien_Load(object sender, EventArgs e)
        {
            List<Sinhvien> listStudent = StudentDB.SinhVien.ToList();
            List<Lop> listFaculty = StudentDB.Lop.ToList();
            FillDataCBB(listFaculty);
            FillDataDGV(listStudent);
        }
        private void FillDataDGV(List<Sinhvien> listStudent)
        {
            dgvSinhVien.Rows.Clear();
            foreach (var student in listStudent)
            {
                int RowNew = dgvSinhVien.Rows.Add();
                dgvSinhVien.Rows[RowNew].Cells[0].Value = student.MaSV;
                dgvSinhVien.Rows[RowNew].Cells[1].Value = student.HotenSV;
                dgvSinhVien.Rows[RowNew].Cells[2].Value = student.NgaySinh;
                dgvSinhVien.Rows[RowNew].Cells[3].Value = student.Lop.TenLop;
            }
        }

        private void FillDataCBB(List<Lop> listClass)
        {
            cmbLop.DataSource = listClass;
            cmbLop.DisplayMember = "TenLop";
            cmbLop.ValueMember = "MaLop";
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (CheckDataInput())
            {
                if (!CheckIdSinhVien(txtMSSV.Text))
                {
                    Sinhvien newStudent = new Sinhvien();
                    newStudent.MaSV = txtMSSV.Text;
                    newStudent.HotenSV = txtHoTen.Text;
                    newStudent.NgaySinh = Convert.ToDateTime(dtNgaySinh.Value);
                    newStudent.MaLop = Convert.ToInt32(cmbLop.SelectedValue.ToString());

                    StudentDB.SinhVien.AddOrUpdate(newStudent);
                    loaddgvSinhVien();
                    loadForm();
                    MessageBox.Show($"Thêm sinh viên {newStudent.HotenSV} vào danh sách thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show($"Sinh viên có mã số {txtMSSV.Text} đã tồn tại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        private bool CheckDataInput()
        {
            if (txtMSSV.Text == "" || txtHoTen.Text == ""  )
            {
                MessageBox.Show("Bạn chưa nhập đúng thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            else if (txtMSSV.TextLength < 5)
            {
                MessageBox.Show("Mã số sinh viên nhập chưa đúng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            return true;
        }
        private void loadForm()
        {
            txtMSSV.Clear();
            txtHoTen.Clear();
        }
        private void loaddgvSinhVien()
        {
            List<Sinhvien> newListStudent = StudentDB.SinhVien.ToList();
            FillDataDGV(newListStudent);
        }

        private bool CheckIdSinhVien(string idNewStudent)
        {
            int length = dgvSinhVien.Rows.Count;
            for (int i = 0; i < length; i++)
            {
                if (dgvSinhVien.Rows[i].Cells[0].Value != null)
                {
                    if (dgvSinhVien.Rows[i].Cells[0].Value.ToString() == idNewStudent)
                    {
                        return true;
                    }
                }
            }
            return false;
        }



        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dgvSinhVien.SelectedRows.Count > 0)
            {
                if (CheckDataInput())
                {
                    string selectedStudentID = dgvSinhVien.SelectedRows[0].Cells[0].Value.ToString();
                    Sinhvien updateStudent = StudentDB.SinhVien.FirstOrDefault(s => s.MaSV == selectedStudentID);
                    if (updateStudent != null)
                    {
                        DialogResult confirmResult = MessageBox.Show($"Bạn có chắc muốn sửa thông tin của sinh viên {updateStudent.HotenSV}?", "Xác nhận sửa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (confirmResult == DialogResult.Yes)
                        {
                            updateStudent.HotenSV = txtHoTen.Text;
                            updateStudent.NgaySinh = Convert.ToDateTime(dtNgaySinh.Value);
                            updateStudent.MaLop = Convert.ToInt32(cmbLop.SelectedValue.ToString());
                            StudentDB.SinhVien.AddOrUpdate(updateStudent);;
                            loaddgvSinhVien();
                            loadForm();
                            MessageBox.Show($"Chỉnh sửa dữ liệu sinh viên {updateStudent.HotenSV} thành công!",
                                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy sinh viên cần sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            else
            {
                MessageBox.Show("Bạn chưa chọn sinh viên để sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvSinhVien.SelectedRows.Count > 0)
            {
                string selectedStudentID = dgvSinhVien.SelectedRows[0].Cells[0].Value.ToString();
                Sinhvien deleteStudent = StudentDB.SinhVien.FirstOrDefault(s => s.MaSV == selectedStudentID);

                if (deleteStudent != null)
                {
                    DialogResult confirmResult = MessageBox.Show($"Bạn có chắc muốn xóa sinh viên {deleteStudent.HotenSV}?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (confirmResult == DialogResult.Yes)
                    {
                        
                        StudentDB.SinhVien.Remove(deleteStudent);

                        try
                        {
                            StudentDB.SaveChanges();

                            loaddgvSinhVien();

                            MessageBox.Show($"Xóa sinh viên {deleteStudent.HotenSV} thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Có lỗi xảy ra khi xóa sinh viên: {ex.Message}", "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Không tìm thấy sinh viên cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Bạn chưa chọn sinh viên để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        private void btnLuu_Click(object sender, EventArgs e)
        {
            try
            {
                StudentDB.SaveChanges();
                MessageBox.Show("Lưu thay đổi thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                loaddgvSinhVien();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Có lỗi xảy ra khi lưu: {ex.Message}", "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnKoLuu_Click(object sender, EventArgs e)
        {
            try
            {
                var entries = StudentDB.ChangeTracker.Entries().Where(entryState => entryState.State != EntityState.Unchanged).ToList();

                foreach (var entry in entries)
                {
                    switch (entry.State)
                    {
                        case EntityState.Modified:
                            entry.CurrentValues.SetValues(entry.OriginalValues); 
                            entry.State = EntityState.Unchanged;
                            break;
                        case EntityState.Added:
                            entry.State = EntityState.Detached; 
                            break;
                        case EntityState.Deleted:
                            entry.State = EntityState.Unchanged; 
                            break;
                    }
                }

                MessageBox.Show("Bỏ thay đổi thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                loaddgvSinhVien();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Có lỗi xảy ra khi bỏ thay đổi: {ex.Message}", "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }



        private void btnTim_Click(object sender, EventArgs e)
        {
            string keyword = txtTim.Text.Trim(); 

            if (!string.IsNullOrEmpty(keyword))
            {
                var searchResult = StudentDB.SinhVien
                    .Where(s => s.HotenSV.ToLower().Contains(keyword.ToLower()))
                    .ToList();

                if (searchResult.Count > 0)
                {
                    FillDataDGV(searchResult);
                }
                else
                {
                    MessageBox.Show("Không tìm thấy sinh viên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dgvSinhVien.Rows.Clear(); 
                }
            }
            else
            {
                MessageBox.Show("Vui lòng nhập từ khóa để tìm kiếm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void dgvSinhVien_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvSinhVien.Rows[e.RowIndex];

                txtMSSV.Text = row.Cells[0].Value.ToString();  
                txtHoTen.Text = row.Cells[1].Value.ToString();  
                dtNgaySinh.Value = Convert.ToDateTime(row.Cells[2].Value); 
                cmbLop.Text = row.Cells[3].Value.ToString();    
            }
        }
    }
}

