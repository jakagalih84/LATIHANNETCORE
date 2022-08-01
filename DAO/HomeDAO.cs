using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TestingAplikasi.Models;

namespace TestingAplikasi.DAO
{
    public class HomeDAO
    {
        public List<UserModel> getKaryawanAll()
        {
            using (SqlConnection conn = new SqlConnection(DBKoneksi.koneksi))
            {
                try
                {
                    string query = @"SELECT TOP 10 a.*, a.NAMA_LENGKAP_GELAR AS nama
                                    FROM simka.MST_KARYAWAN a";
                    
                    var data = conn.Query<UserModel>(query).AsList();

                    return data;
                }
                catch (Exception ex)
                {
                    return null;
                }
                finally
                {
                    conn.Dispose();
                }
            }
        }
    }
}
