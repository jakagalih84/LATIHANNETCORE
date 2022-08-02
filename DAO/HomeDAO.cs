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

        public UserModel getKaryawanbyNpp(string npp)
        {
            using (SqlConnection conn = new SqlConnection(DBKoneksi.koneksi))
            {
                try
                {
                    string query = @"SELECT TOP 1 a.*, a.NAMA_LENGKAP_GELAR AS nama
                                    FROM simka.MST_KARYAWAN a WHERE a.NPP = @npp";

                    var data = conn.QueryFirstOrDefault<UserModel>(query, new { npp = npp});

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

        public bool simpanKaryawan(UserModel mdl)
        {
            using (SqlConnection conn = new SqlConnection(DBKoneksi.koneksi))
            {
                try
                {
                    string query = @"INSERT INTO [simka].[MST_KARYAWAN]
                                       ([NPP]
                                       ,[NAMA_LENGKAP_GELAR]
                                       ,[USERNAME]
                                       ,[PASSWORD])
                                 VALUES
                                       (@npp, @nama, @npp, @password)";

                    var data = conn.Execute(query, mdl);

                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
                finally
                {
                    conn.Dispose();
                }
            }
        }

        public bool ubahKaryawan(UserModel mdl)
        {
            using (SqlConnection conn = new SqlConnection(DBKoneksi.koneksi))
            {
                try
                {
                    string query = @"UPDATE [simka].[MST_KARYAWAN] SET
                                       [NAMA_LENGKAP_GELAR] = @nama
                                       ,[PASSWORD] = @password
                                    WHERE NPP = @npp";

                    var data = conn.Execute(query, mdl);

                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
                finally
                {
                    conn.Dispose();
                }
            }
        }

    }
}
