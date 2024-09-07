using Data.Infrastructure;
using Data.Repositories;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface ICommonService
    {
        SystemConfig GetSystemConfig(string code);
    }
    public class CommonService : ICommonService
    {
        IUnitOfWork _unitOfWork;
        ISystemConfigRepository _systemConfigRepository;
        public CommonService(IUnitOfWork unitOfWork, ISystemConfigRepository systemConfigRepository)
        {
            _unitOfWork = unitOfWork;
            _systemConfigRepository = systemConfigRepository;
        }
        public SystemConfig GetSystemConfig(string code)
        {
            return _systemConfigRepository.GetSingleByCondition(x => x.Code == code);
        }


    }
}
