﻿using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using QuantConnect.Lean.Monitor.Model;
using QuantConnect.Lean.Monitor.Model.Messages;

namespace QuantConnect.Lean.Monitor.ViewModel.Panels
{
    public class RuntimeStatisticsPanelViewModel : ViewModelBase
    {
        private readonly IMessenger _messenger;

        private ObservableCollection<StatisticViewModel> _statistics = new ObservableCollection<StatisticViewModel>();

        public RuntimeStatisticsPanelViewModel(IMessenger messenger)
        {
            _messenger = messenger;
            _messenger.Register<SessionUpdateMessage>(this, message => ParseResult(message.Result));
            _messenger.Register<SessionClosedMessage>(this, m => Clear());
        }

        public ObservableCollection<StatisticViewModel> Statistics
        {
            get { return _statistics; }
            set
            {
                _statistics = value;
                RaisePropertyChanged();
            }
        }

        private void Clear()
        {
            Statistics.Clear();
        }

        private void ParseResult(Result result)
        {
            Statistics = new ObservableCollection<StatisticViewModel>(result.RuntimeStatistics.Select(s => new StatisticViewModel
            {
                Name = s.Key,
                Value = s.Value
            }));
        }
    }
}
