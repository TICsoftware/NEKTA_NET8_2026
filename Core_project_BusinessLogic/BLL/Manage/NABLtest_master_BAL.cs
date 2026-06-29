using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Core_project_BusinessLogic.DAL;
using Core_project_BusinessLogic.Entity;
namespace Core_project_BusinessLogic.BAL
{
    public class NABLtest_Master_BAL
    {
        private readonly NABLtest_Master_DAL _dal;
        private readonly IConfiguration _config;
        public NABLtest_Master_BAL(IConfiguration config)
        {
             _config = config;
            _dal = new NABLtest_Master_DAL(config);
            
        }


        public (List<MasterEntity>, int) GetPaged(MasterEntity entity)
        {
            return _dal.GetPaged(
                entity.MasterType,
                entity.SearchText,
                entity.PageNumber,
                entity.PageSize
            );
        }

        public MasterEntity GetById(int id, string type)
        {
            return _dal.GetById(id, type);
        }


          public int Save(MasterEntity entity)
        {
            if (string.IsNullOrEmpty(entity.Name))
                throw new Exception("Name is required");

            if (entity.ID == 0)
                return _dal.Insert(entity);
            else
            {
                _dal.Update(entity);
                return entity.ID;
            }
        }

        // 🔹 DEACTIVATE
        public void Deactivate(int id, string type)
        {
            _dal.Deactivate(id, type);
        }

        public void ChangeStatus(int id, string type, int status)
        {
            _dal.ChangeStatus(id, type, status);
        }

        // 🔹 UPDATE SEQUENCE (DRAG DROP)
        public void UpdateSequence(List<MasterEntity> list, string type)
        {
            _dal.UpdateSequence(list, type);
        }
    }

}