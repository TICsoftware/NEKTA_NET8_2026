using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Core_project_BusinessLogic.DAL;
using Core_project_BusinessLogic.Entity;
namespace Core_project_BusinessLogic.BAL
{

public class TestMaster_BAL
{
    private readonly TestMaster_DAL _dal;

    public TestMaster_BAL(IConfiguration config)
    {
        _dal = new TestMaster_DAL(config);
    }

    public List<TestMasterEntity> GetAll() => _dal.GetAll();
  public TestMasterEntity GetById(int id)
{
    return _dal.GetById(id);
}
    public int Save(TestMasterEntity m)
    {
        if (string.IsNullOrEmpty(m.TestName))
            throw new Exception("Test Name required");

        if (m.TestId == 0)
            return _dal.Insert(m);
        else
        {
            _dal.Update(m);
            return m.TestId;
        }
    }

    public void Deactivate(int id, int userId)
    {
        _dal.Deactivate(id, userId);
    }
    public List<MasterEntity> GetSpecimen()
{
    return _dal.GetSpecimen();
}

public List<MasterEntity> GetTestType()
{
    return _dal.GetTestType();
}

public List<MasterEntity> GetOrgan()
{
    return _dal.GetOrgan();
}

public List<MasterEntity> GetDepartment()
{
    return _dal.GetDepartment();
}
public void ChangeStatus(int id, int status)
{
    _dal.ChangeStatus(id, status);
}
public (List<TestMasterEntity>, int)
GetPaged(
    string search,
    int page,
    int pageSize)
{
    return _dal.GetPaged(
        search,
        page,
        pageSize
    );
}
}
}