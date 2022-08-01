using Dapper;
using System;
using System.Data.SqlClient;
using TestingAplikasi.Models;

namespace TestingAplikasi.DAO
{
    public class AccountDAO
    {
        public UserModel getKaryawan(string npp)
        {
            using (SqlConnection conn = new SqlConnection(DBKoneksi.koneksi))
            {
                try
                {
                    string query = @"SELECT TOP 1 a.NPP, a.NAMA_LENGKAP_GELAR AS NAMA, 
                                    a.PASSWORD, c.DESKRIPSI
                                    FROM simka.MST_KARYAWAN a
                                    JOIN siatmax.TBL_USER_ROLE b ON a.NPP = b.NPP
                                    JOIN siatmax.REF_ROLE c ON b.ID_ROLE = c.ID_ROLE
                                    WHERE a.NPP = @username";
                    var param = new { username = npp };
                    var data = conn.QueryFirstOrDefault<UserModel>(query, param);

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
