﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAcessLayer;
namespace BusinessAccessLayer
{
   
    public class DBCustomer
    {
        private DAL dal;
        public DBCustomer()
        { 
            dal = new DAL();
        }
        public DataSet LayKhachHang()
        {
            string query = "SELECT * From Customer";
            return dal.ExecuteQueryDataSet(query, CommandType.Text);
        }
        public DataSet TimKhachHangTheoTen(string ten)
        {
            if (string.IsNullOrEmpty(ten))
            {
                return LayKhachHang(); // Hàm này anh có thể đã có rồi
            }

            string query = "sp_TimKhachHangTheoTen";
            return dal.ExecuteQueryDataSet(query, CommandType.StoredProcedure,
                new SqlParameter[] { new SqlParameter("@Name", ten) });
        }
        public DataSet TimKhachHangTheoDiaChi(string diaChi)
        {
            if (string.IsNullOrEmpty(diaChi))
            {
                return LayKhachHang(); // hoặc hàm load full
            }

            string query = "sp_TimKhachHangTheoDiaChi";
            return dal.ExecuteQueryDataSet(query, CommandType.StoredProcedure,
                new SqlParameter[] { new SqlParameter("@DiaChi", diaChi) });
        }
        // Thêm khách hàng
        public bool ThemKhachHang(string fullName, string customerType, string address, string phone)
        {
            return dal.ExecuteNonQuery("sp_AddCustomer", new SqlParameter[]
            {
                new SqlParameter("@FullName", fullName),
                new SqlParameter("@CustomerType", customerType),
                new SqlParameter("@Address", address),
                new SqlParameter("@Phone", phone)
            }) > 0;
        }

        // Sửa khách hàng
        public bool SuaKhachHang(int customerId, string fullName, string customerType, string address, string phone)
        {
            return dal.ExecuteNonQuery("sp_UpdateCustomer", new SqlParameter[]
            {
                new SqlParameter("@CustomerID", customerId),
                new SqlParameter("@FullName", fullName),
                new SqlParameter("@CustomerType", customerType),
                new SqlParameter("@Address", address),
                new SqlParameter("@Phone", phone)
            }) > 0;
        }

        // Xóa khách hàng
        public bool XoaKhachHang(int customerId)
        {
            return dal.ExecuteNonQuery("sp_DeleteCustomer", new SqlParameter[]
            {
                new SqlParameter("@CustomerID", customerId)
            }) > 0;
        }
        //Gộp cả 3 loại tìm 
        public DataSet TimKhachHangTongHop(string tuKhoa)
        {
            return dal.ExecuteQueryDataSet("sp_TimKhachHangTongHop", CommandType.StoredProcedure,
                new SqlParameter[] { new SqlParameter("@TuKhoa", tuKhoa) });
        }

    }
}
