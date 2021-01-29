using Microsoft.EntityFrameworkCore;
using PressureMeasurementApplication.Model;
using PressureMeasurementApplication.Utils;
using PressureMeasurementApplication.Utils.SerialPort;
using PropertyChanged;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PressureMeasurementApplication.ViewModel
{
    [AddINotifyPropertyChangedInterface]
    public class MissionViewModel
    {
        public SQLiteDataContext dataContext;

        public ObservableCollection<MissionModel> missionModels = new ObservableCollection<MissionModel>();

        public ICommand TakeDataCommand { get; }
        public ICommand QueryCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand DisplayCommand { get; }
        public ICommand StartStreamCommand { get; }
        public ICommand StopStreamCommand { get; }

        public MissionViewModel(SQLiteDataContext dataContext)
        {
            this.dataContext = dataContext;
            FlashQuene = new BlockingCollection<Memory<Byte>>();
            StreamQuene = new BlockingCollection<Memory<Byte>>();

            TakeDataCommand = new AwaitableDelegateCommand(TakeDataAsync);
            DeleteCommand = new AwaitableDelegateCommand(DeleteAsync);
            DisplayCommand = new AwaitableDelegateCommand(DisplayAsync);
            StartStreamCommand = new AwaitableDelegateCommand(StartStreamAsync);
            StopStreamCommand = new AwaitableDelegateCommand(StopStreamAsync);

            _ = QueryAsync();
        }

        /// <summary>
        /// 在任务列表中选中的任务。
        /// </summary>
        public MissionModel SelectedMission { get; set; }

        public BlockingCollection<Memory<Byte>> FlashQuene { get; set; }

        public BlockingCollection<Memory<Byte>> StreamQuene { get; set; }

        /// <summary>
        /// 查询数据库所有任务计划并刷新前端页面显示。
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<MissionModel> MissionModels
        {
            get { return missionModels; }
        }

        /// <summary>
        /// 根据返回参数添加任务计划至数据库中。
        /// </summary>
        /// <param name="missionModel"></param>
        /// <returns></returns>
        public async Task TakeDataAsync()
        {
            await Protocol.Instance.StartFlashTransfer();

            var lastMission = await dataContext.Set<MissionModel>().MaxAsync();

            var mission = new MissionModel
            {
                Name = "测量任务" + (lastMission.Id + 1).ToString(),
                DateTime = DateTime.Now,
                SensorNumber = 0
            };

            await dataContext.AddAsync(mission);

            await QueryAsync();
        }

        /// <summary>
        /// 请求下位机传输实时预览数据进行绘图展示。
        /// </summary>
        /// <returns></returns>
        public async Task StartStreamAsync()
        {
            await Protocol.Instance.StartStreamTransfer();
        }

        /// <summary>
        /// 停止实时预览数据传输。
        /// </summary>
        /// <returns></returns>
        public async Task StopStreamAsync()
        {
            await Protocol.Instance.StopStreamTransfer();
        }

        public async Task DisplayAsync()
        {
            await GetAsync();

        }

        /// <summary>
        /// 调用此函数以持续获取串口返回数据。
        /// </summary>
        /// <returns></returns>
        public async Task GetDataAsync()
        {
            while (true)
            {
                var memory = await Manager.Instance.ReadPort();

                switch (Protocol.Instance.AnalysisData(memory, memory.Span[4]))
                {
                    case CommandType.TransferDone:
                        FlashQuene.CompleteAdding();
                        break;
                    case CommandType.CanReadFlash:
                        FlashQuene.Add(memory);
                        break;
                    case CommandType.CanStartStream:
                        StreamQuene.Add(memory);
                        break;
                    default:
                        break;
                }
                //MemoryMarshal.Cast<byte, short>(memory.Slice(5, memory.Span[4]).Span)
            }      
        }

        /// <summary>
        /// 根据选择的任务删除数据库中对应的任务计划。
        /// </summary>
        /// <returns></returns>
        public async Task DeleteAsync()
        {
            using (var tx = await dataContext.Database.BeginTransactionAsync())
            {
                if (SelectedMission is null)
                {
                    throw new InvalidOperationException("待删除的任务ID有误！");
                }

                dataContext.Remove(SelectedMission);

                await dataContext.SaveChangesAsync();
                await tx.CommitAsync();
            }

            await QueryAsync();
        }

        /// <summary>
        /// 根据选择的ID查询该选中计划与关联的整组数据合集。
        /// </summary>
        /// <returns></returns>
        public async Task<MissionModel> GetAsync()
        {
            var mission = await dataContext
                .Set<MissionModel>()
                .Include(x => x.DataModels)
                .FirstOrDefaultAsync(x => x.Id == SelectedMission.Id);

            return mission;
        }

        public async Task QueryAsync()
        {
            missionModels.Clear();
            await dataContext.MissionModel.ForEachAsync(x => missionModels.Add(x));
        }
    }
}
