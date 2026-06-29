using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Core_project_BusinessLogic.Entity;

namespace Core_project_BusinessLogic.DAL
{
    public class NABLtest_Master_DAL : DBHelper
    {
        public NABLtest_Master_DAL(IConfiguration configuration) : base(configuration) { }

        // 🔹 GET PAGED
        public (List<MasterEntity> Data, int Total) GetPaged(string type, string search, int page, int pageSize)
        {
            SqlParameter[] p =
            {
                new("@MasterType", type),
                new("@Search", string.IsNullOrEmpty(search) ? DBNull.Value : search),
                new("@Page", page),
                new("@PageSize", pageSize)
            };

            DataSet ds = GetDataSet("NABLtest_master_GetPaged", p);

            List<MasterEntity> list = new();

            foreach (DataRow r in ds.Tables[0].Rows)
            {
                list.Add(new MasterEntity
                {
                    ID = Convert.ToInt32(r["ID"]),
                    Name = r["Name"].ToString(),
                    Sequence = r["Sequence"] == DBNull.Value ? null : Convert.ToInt32(r["Sequence"]),
                    Status = r["Status"] == DBNull.Value ? null : Convert.ToInt32(r["Status"]),
                    MasterType = type
                });
            }

            int total = Convert.ToInt32(ds.Tables[1].Rows[0]["TotalCount"]);

            return (list, total);
        }

        // 🔹 GET BY ID
        public MasterEntity GetById(int id, string type)
        {
            SqlParameter[] p =
            {
                new("@ID", id),
                new("@MasterType", type)
            };

            DataTable dt = GetDataSet("NABLtest_master_GetById", p).Tables[0];

            if (dt.Rows.Count == 0)
                return null;

            DataRow r = dt.Rows[0];

            return new MasterEntity
            {
                ID = Convert.ToInt32(r["ID"]),
                Name = r["Name"].ToString(),
                 Sequence = r["Sequence"] == DBNull.Value ? null : Convert.ToInt32(r["Sequence"]),
                    Status = r["Status"] == DBNull.Value ? null : Convert.ToInt32(r["Status"]),
                MasterType = type
            };
        }

        // 🔹 INSERT
        public int Insert(MasterEntity m)
        {
            SqlParameter[] p =
            {
                new("@Name", m.Name),
                new("@MasterType", m.MasterType),
                new("@Sequence", m.Sequence.HasValue ? (object)m.Sequence.Value : DBNull.Value),
               
            };

            return SqlInsertReturnIdentity_withSP("NABLtest_master_Insert", "@NewID", p);
        }

        // 🔹 UPDATE
        public void Update(MasterEntity m)
        {
            SqlParameter[] p =
            {
                new("@ID", m.ID),
                new("@Name", m.Name),
                new("@Sequence", m.Sequence.HasValue ? (object)m.Sequence.Value : DBNull.Value),
               
                new("@MasterType", m.MasterType)
            };

            SQLInsert_Update_Delete_Data("NABLtest_master_Update", p);
        }

             // 🔹 DEACTIVATE
        public void Deactivate(int id, string type)
        {
            SqlParameter[] p =
            {
                new("@ID", id),
                new("@MasterType", type)
            };

            SQLInsert_Update_Delete_Data("NABLtest_master_Deactivate", p);
        }

        // 🔹 ACTIVATE
        public void Activate(int id, string type)
        {
            SqlParameter[] p =
            {
                new("@ID", id),
                new("@MasterType", type)
            };

            SQLInsert_Update_Delete_Data("NABLtest_master_Activate", p);
        }

        public void ChangeStatus(int id, string type, int status)
        {
            if (status == 1)
                Activate(id, type);
            else
                Deactivate(id, type);
        }

        // 🔹 UPDATE SEQUENCE (BULK)
        public void UpdateSequence(List<MasterEntity> list, string type)
        {
            foreach (var item in list)
            {
                SqlParameter[] p =
                {
                    new("@ID", item.ID),
                    new("@Sequence", item.Sequence),
                    new("@MasterType", type)
                };

                SQLInsert_Update_Delete_Data("NABLtest_master_UpdateSequence", p);
            }
        }

        // 🔹 DELETE
        public void Delete(int id, string type)
        {
            SqlParameter[] p =
            {
                new("@ID", id),
                new("@MasterType", type)
            };

            SQLInsert_Update_Delete_Data("NABLtest_master_Delete", p);
        }
    }
}