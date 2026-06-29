
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Core_project_BusinessLogic.Entity;
namespace Core_project_BusinessLogic.DAL
{
public class TestMaster_DAL : DBHelper
{
    public TestMaster_DAL(IConfiguration config) : base(config) { }

    // 🔹 LIST
    public List<TestMasterEntity> GetAll()
    {
        List<TestMasterEntity> list = new();

        DataTable dt = GetDataSet("TestMaster_List").Tables[0];

        foreach (DataRow r in dt.Rows)
        {
            list.Add(new TestMasterEntity
            {
                TestId = Convert.ToInt32(r["TestId"]),
                NABL_OptionName = r["NABL_OptionName"].ToString(),
                TestName = r["TestName"].ToString(),
                SpecimenName = r["SpecimenName"].ToString(),
                TestTypeName = r["TestTypeName"].ToString(),
                OrganName = r["OrganName"].ToString(),
                DepartmentName = r["DepartmentName"].ToString(),
                Status = r["Status"] as int?
            });
        }

        return list;
    }
public TestMasterEntity GetById(int id)
{
    SqlParameter[] p =
    {
        new("@TestId", id)
    };

    DataTable dt = GetDataSet(
        "TestMaster_GetById",
        p
    ).Tables[0];

    if (dt.Rows.Count == 0)
        return null;

    DataRow r = dt.Rows[0];

    return new TestMasterEntity
{
    TestId = Convert.ToInt32(r["TestId"]),

    NABL_Option = r["NABL_Option"] == DBNull.Value
        ? null
        : Convert.ToInt32(r["NABL_Option"]),

    TestName = r["TestName"]?.ToString(),

    SpecimenId = r["SpecimenId"] == DBNull.Value
        ? null
        : Convert.ToInt32(r["SpecimenId"]),

    TestTypeId = r["TestTypeId"] == DBNull.Value
        ? null
        : Convert.ToInt32(r["TestTypeId"]),

    OrganId = r["OrganId"] == DBNull.Value
        ? null
        : Convert.ToInt32(r["OrganId"]),

    DepartmentId = r["DepartmentId"] == DBNull.Value
        ? null
        : Convert.ToInt32(r["DepartmentId"])
};
}
    // 🔹 INSERT
   public int Insert(TestMasterEntity m)
{
    SqlParameter[] p =
    {
        new("@NABL_Option", m.NABL_Option),

        new("@TestName", m.TestName),

        new("@SpecimenId",
            m.SpecimenId.HasValue
            ? (object)m.SpecimenId.Value
            : DBNull.Value),

        new("@TestTypeId",
            m.TestTypeId.HasValue
            ? (object)m.TestTypeId.Value
            : DBNull.Value),

        new("@OrganId",
            m.OrganId.HasValue
            ? (object)m.OrganId.Value
            : DBNull.Value),

        new("@DepartmentId",
            m.DepartmentId.HasValue
            ? (object)m.DepartmentId.Value
            : DBNull.Value),

        new("@Created_UserID", m.Created_UserID)
    };

    return SqlInsertReturnIdentity_withSP(
        "TestMaster_Insert",
        "@NewID",
        p
    );
}
    // 🔹 UPDATE
public void Update(TestMasterEntity m)
{
    SqlParameter[] p =
    {
        new("@TestId", m.TestId),

        new("@NABL_Option", m.NABL_Option),

        new("@TestName", m.TestName),

        new("@SpecimenId",
            m.SpecimenId.HasValue
            ? (object)m.SpecimenId.Value
            : DBNull.Value),

        new("@TestTypeId",
            m.TestTypeId.HasValue
            ? (object)m.TestTypeId.Value
            : DBNull.Value),

        new("@OrganId",
            m.OrganId.HasValue
            ? (object)m.OrganId.Value
            : DBNull.Value),

        new("@DepartmentId",
            m.DepartmentId.HasValue
            ? (object)m.DepartmentId.Value
            : DBNull.Value)
    };

    SQLInsert_Update_Delete_Data(
        "TestMaster_Update",
        p
    );
}

    // 🔹 DEACTIVATE
    public void Deactivate(int id, int userId)
    {
        SqlParameter[] p =
        {
            new("@TestId", id),
            new("@UserID", userId)
        };

        SQLInsert_Update_Delete_Data("TestMaster_Deactivate", p);
    }
    public List<MasterEntity> GetSpecimen()
{
    List<MasterEntity> list = new();

    DataTable dt = GetDataSet("Specimen_List").Tables[0];

    foreach (DataRow r in dt.Rows)
    {
        list.Add(new MasterEntity
        {
            ID = Convert.ToInt32(r["SpecimenId"]),
            Name = r["SpecimenName"].ToString()
        });
    }

    return list;
}
public List<MasterEntity> GetTestType()
{
    List<MasterEntity> list = new();

    DataTable dt = GetDataSet("TestType_List").Tables[0];

    foreach (DataRow r in dt.Rows)
    {
        list.Add(new MasterEntity
        {
            ID = Convert.ToInt32(r["TestTypeId"]),
            Name = r["TestTypeName"].ToString()
        });
    }

    return list;
}
public List<MasterEntity> GetOrgan()
{
    List<MasterEntity> list = new();

    DataTable dt = GetDataSet("Organ_List").Tables[0];

    foreach (DataRow r in dt.Rows)
    {
        list.Add(new MasterEntity
        {
            ID = Convert.ToInt32(r["OrganId"]),
            Name = r["OrganName"].ToString()
        });
    }

    return list;
}
public List<MasterEntity> GetDepartment()
{
    List<MasterEntity> list = new();

    DataTable dt = GetDataSet("Department_List").Tables[0];

    foreach (DataRow r in dt.Rows)
    {
        list.Add(new MasterEntity
        {
            ID = Convert.ToInt32(r["DepartmentId"]),
            Name = r["DepartmentName"].ToString()
        });
    }

    return list;
}

public void ChangeStatus(int id, int status)
{
    SqlParameter[] p =
    {
        new("@TestId", id),
        new("@Status", status)
    };

    SQLInsert_Update_Delete_Data(
        "TestMaster_ChangeStatus",
        p
    );
}
public (List<TestMasterEntity>, int) GetPaged(
    string search,
    int page,
    int pageSize)
{
    SqlParameter[] p =
    {
        new("@Search",
            string.IsNullOrEmpty(search)
            ? DBNull.Value
            : search),

        new("@Page", page),

        new("@PageSize", pageSize)
    };

    DataSet ds = GetDataSet(
        "TestMaster_List_Paged",
        p
    );

    List<TestMasterEntity> list = new();

    foreach (DataRow r in ds.Tables[0].Rows)
    {
        list.Add(new TestMasterEntity
        {
            TestId = Convert.ToInt32(r["TestId"]),

            NABL_OptionName =
                r["NABL_OptionName"]?.ToString(),

            TestName =
                r["TestName"]?.ToString(),

            SpecimenName =
                r["SpecimenName"]?.ToString(),

            TestTypeName =
                r["TestTypeName"]?.ToString(),

            OrganName =
                r["OrganName"]?.ToString(),

            DepartmentName =
                r["DepartmentName"]?.ToString(),

            Status =
                r["Status"] as int?
        });
    }

    int total =
        Convert.ToInt32(
            ds.Tables[1].Rows[0]["TotalCount"]
        );

    return (list, total);
}
}
}