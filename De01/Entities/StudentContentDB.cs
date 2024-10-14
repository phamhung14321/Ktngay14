using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace De01.Entities
{
    public partial class StudentContentDB : DbContext
    {
        public StudentContentDB()
            : base("name=StudentContentDB")
        {
        }

        public virtual DbSet<Lop> Lop { get; set; }
        public virtual DbSet<Sinhvien> SinhVien { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
