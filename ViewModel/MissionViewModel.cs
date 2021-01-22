using Microsoft.EntityFrameworkCore;
using PressureMeasurementApplication.Model;
using PressureMeasurementApplication.Utils;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PressureMeasurementApplication.ViewModel
{
    [AddINotifyPropertyChangedInterface]
    public class MissionViewModel
    {
        public SQLiteDataContext dataContext;
        
        public ICommand AddCommand { get; }
        public ICommand QueryCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand GetDataCommand { get; }

        public MissionViewModel(SQLiteDataContext dataContext)
        {
            this.dataContext = dataContext;

            AddCommand = new AwaitableDelegateCommand(AddAsync);
            QueryCommand = new AwaitableDelegateCommand(QueryAsync);
            DeleteCommand = new AwaitableDelegateCommand(DeleteAsync);
            //DisplayCommand = new AwaitableDelegateCommand(DisplayAsync);
            GetDataCommand = new AwaitableDelegateCommand(SerialPortProtocol.Instance.GetData);

            _ = QueryAsync();
        }
        
        public List<MissionModel> MissionModels { get; private set; }

        /// <summary>
        /// 在任务列表中选中的任务ID。
        /// </summary>
        public ulong? SeletedId { get; set; }

        /// <summary>
        /// 发送数据至串口，通知下位机开始传输FLASH信号
        /// </summary>
        /// <returns></returns>
        public async Task GetDataFromPort()
        {
            await SerialPortProtocol.Instance.StartFlashTransfer();
        }

        public async Task QueryAsync()
        {
            MissionModels = await dataContext.MissionModel.ToListAsync();
        }

        /// <summary>
        /// 根据返回参数添加任务计划至数据库中。
        /// </summary>
        /// <param name="missionModel"></param>
        /// <returns></returns>
        public async Task AddAsync()
        {
            await GetDataFromPort();

            var lastMission = await dataContext.Set<MissionModel>().MaxAsync();

            var mission = new MissionModel
            {
                Name = "测量任务" + (lastMission.Id + 1).ToString(),
                DateTime = DateTime.Now,
                SensorNumber = 0
            };

            await dataContext.AddAsync(mission);
        }

        /// <summary>
        /// 根据选择的ID删除对应的任务计划。
        /// </summary>
        /// <param name="Id">待删除的任务计划ID。</param>
        /// <returns></returns>
        public async Task DeleteAsync()
        {
            using (var tx = await dataContext.Database.BeginTransactionAsync())
            {
                var mission = await dataContext.Set<MissionModel>()
                    .Include(x => x.DataModels)
                    .FirstOrDefaultAsync(x => x.Id == SeletedId);

                if(mission is null)
                {
                    throw new InvalidOperationException("待删除的任务ID有误！");
                }

                dataContext.Remove(mission);

                await dataContext.SaveChangesAsync();
                await tx.CommitAsync();
            }
        }

        /// <summary>
        /// 根据选择的ID查询该选中计划与关联的整组数据合集。
        /// </summary>
        /// <returns></returns>
        public async Task GetAsync()
        {
            var mission = await dataContext
                .Set<MissionModel>()
                .Include(x => x.DataModels)
                .FirstOrDefaultAsync(x=>x.Id == SeletedId);

            foreach(var data in mission.DataModels)
            {
            }
        }
    }
}
