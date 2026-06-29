
using System.Collections.Generic;
using Core_project_BusinessLogic.DAL;
using Core_project_BusinessLogic.Entity;
using Microsoft.Extensions.Configuration;
using System.Data;
using Microsoft.Data.SqlClient;

namespace Core_project_BusinessLogic.BAL
{
   public class EventMaster_BAL
{
    private readonly EventMaster_DAL _dal;

    public EventMaster_BAL(IConfiguration config)
    {
        _dal = new EventMaster_DAL(config);
    }

    public (List<EventMaster>, int) GetPaged(EventMaster e)
    {
        return _dal.GetPaged(e.SearchText, e.PageNumber, e.PageSize);
    }

    public EventMaster GetById(int id)
    {
        return _dal.GetById(id);
    }

public int Save(EventMaster m)
{
    if (string.IsNullOrEmpty(m.Title))
        throw new Exception("Title is required");

    if (m.EventId == 0)
        return _dal.Insert(m);
    else
    {
        _dal.Update(m);
        return m.EventId;
    }
}
    public void ChangeStatus(int id, int status)
    {
        _dal.ChangeStatus(id, status);
    }
}
}