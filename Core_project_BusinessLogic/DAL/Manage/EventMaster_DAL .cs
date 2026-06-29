using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Core_project_BusinessLogic.Entity;


namespace Core_project_BusinessLogic.DAL
{
public class EventMaster_DAL : DBHelper
{
    public EventMaster_DAL(IConfiguration config) : base(config) { }

    public (List<EventMaster>, int) GetPaged(string search, int page, int pageSize)
    {
        SqlParameter[] p =
        {
            new("@Search", string.IsNullOrEmpty(search) ? DBNull.Value : search),
            new("@Page", page),
            new("@PageSize", pageSize)
        };

        DataSet ds = GetDataSet("EventMaster_GetPaged", p);

        List<EventMaster> list = new();

        foreach (DataRow r in ds.Tables[0].Rows)
        {
            list.Add(new EventMaster
            {
                EventId = Convert.ToInt32(r["EventId"]),
                Title = r["Title"].ToString(),
                EventTypeName = r["EventTypeName"].ToString(),
                EventModeName = r["EventModeName"].ToString(),
                EventDate = r["EventDate"] as DateTime?,
                Status = r["Status"] as int?
            });
        }

        int total = Convert.ToInt32(ds.Tables[1].Rows[0]["TotalCount"]);

        return (list, total);
    }
public EventMaster GetById(int id)
{
    SqlParameter[] p = { new("@EventId", id) };

    DataTable dt = GetDataSet("EventMaster_GetById", p).Tables[0];

    if (dt.Rows.Count == 0) return null;

    var r = dt.Rows[0];

    return new EventMaster
    {
        EventId = Convert.ToInt32(r["EventId"]),
        Title = r["Title"].ToString(),
        EventTypeId = r["EventTypeId"] as int?,
        EventModeId = r["EventModeId"] as int?,
        EventDate = r["EventDate"] as DateTime?,
        EventTime = r["EventTime"].ToString(),
        Intro = r["Intro"]?.ToString(),
        Content = r["Content"]?.ToString(),
        Speakers = r["Speakers"]?.ToString(),
        VideoUrl = r["VideoUrl"]?.ToString(),
        FilePath = r["FilePath"]?.ToString(),
        IsRecurring = Convert.ToBoolean(r["IsRecurring"]),

RecurringAfterDays =
    r["RecurringAfterDays"] == DBNull.Value
        ? null
        : Convert.ToInt32(r["RecurringAfterDays"]),

RecurringEndDate =
    r["RecurringEndDate"] == DBNull.Value
        ? null
        : Convert.ToDateTime(r["RecurringEndDate"]),
    };
}
public int Insert(EventMaster m)
{
    SqlParameter[] p =
    {
        new("@Title", m.Title ?? (object)DBNull.Value),

        new("@EventTypeId",
            m.EventTypeId ?? (object)DBNull.Value),

        new("@EventModeId",
            m.EventModeId ?? (object)DBNull.Value),

        new("@EventDate",
            m.EventDate ?? (object)DBNull.Value),

        new("@EventTime",
            string.IsNullOrEmpty(m.EventTime)
            ? DBNull.Value
            : m.EventTime),

        new("@Intro",
            string.IsNullOrEmpty(m.Intro)
            ? DBNull.Value
            : m.Intro),

        new("@Content",
            string.IsNullOrEmpty(m.Content)
            ? DBNull.Value
            : m.Content),

        new("@Speakers",
            string.IsNullOrEmpty(m.Speakers)
            ? DBNull.Value
            : m.Speakers),

        new("@VideoUrl",
            string.IsNullOrEmpty(m.VideoUrl)
            ? DBNull.Value
            : m.VideoUrl),

        new("@FilePath",
            string.IsNullOrEmpty(m.FilePath)
            ? DBNull.Value
            : m.FilePath),


         new("@IsRecurring", m.IsRecurring),
new("@RecurringAfterDays",
    m.RecurringAfterDays ?? (object)DBNull.Value),

new("@RecurringEndDate",
    m.RecurringEndDate ?? (object)DBNull.Value),
        new("@Created_UserID",
            m.Created_UserID ?? (object)DBNull.Value)
    };

    return SqlInsertReturnIdentity_withSP(
        "EventMaster_Insert",
        "@NewID",
        p);
}
public void Update(EventMaster m)
{
    SqlParameter[] p =
    {
        new("@EventId", m.EventId),

        new("@Title",
            m.Title ?? (object)DBNull.Value),

        new("@EventTypeId",
            m.EventTypeId ?? (object)DBNull.Value),

        new("@EventModeId",
            m.EventModeId ?? (object)DBNull.Value),

        new("@EventDate",
            m.EventDate ?? (object)DBNull.Value),

        new("@EventTime",
            string.IsNullOrEmpty(m.EventTime)
            ? DBNull.Value
            : m.EventTime),

        new("@Intro",
            string.IsNullOrEmpty(m.Intro)
            ? DBNull.Value
            : m.Intro),

        new("@Content",
            string.IsNullOrEmpty(m.Content)
            ? DBNull.Value
            : m.Content),

        new("@Speakers",
            string.IsNullOrEmpty(m.Speakers)
            ? DBNull.Value
            : m.Speakers),

        new("@VideoUrl",
            string.IsNullOrEmpty(m.VideoUrl)
            ? DBNull.Value
            : m.VideoUrl),

        new("@FilePath",
            string.IsNullOrEmpty(m.FilePath)
            ? DBNull.Value
            : m.FilePath),
          new("@IsRecurring", m.IsRecurring),

new("@RecurringAfterDays",
    m.RecurringAfterDays ?? (object)DBNull.Value),

new("@RecurringEndDate",
    m.RecurringEndDate ?? (object)DBNull.Value),
        new("@Updated_UserID",
            m.Updated_UserID ?? (object)DBNull.Value)
    };

    SQLInsert_Update_Delete_Data(
        "EventMaster_Update",
        p);
}


    public void ChangeStatus(int id, int status)
    {
        SqlParameter[] p =
        {
            new("@EventId", id),
            new("@Status", status)
        };

        SQLInsert_Update_Delete_Data("EventMaster_ChangeStatus", p);
    }
}



}