﻿/*
 * Copyright 2018 Capnode AB
 * 
 * Licensed under the Apache License, Version 2.0 (the "License"); 
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using Algoloop.Model;
using Algoloop.ViewSupport;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Algoloop.ViewModel
{
    public class MarketsViewModel : ViewModelBase
    {
        private readonly SettingsModel _settingsModel;
        private ITreeViewModel _selectedItem;
        private bool _isBusy;

        public MarketsViewModel(MarketsModel model, SettingsModel settingsModel)
        {
            Model = model;
            _settingsModel = settingsModel;

            AddCommand = new RelayCommand(() => AddMarket(), () => !IsBusy);
            SelectedChangedCommand = new RelayCommand<ITreeViewModel>((vm) => OnSelectedChanged(vm), (vm) => vm != null);
            Messenger.Default.Register<NotificationMessageAction<List<MarketModel>>>(this, (message) => OnNotificationMessage(message));
            DataFromModel();
        }

        public RelayCommand<ITreeViewModel> SelectedChangedCommand { get; }
        public RelayCommand AddCommand { get; }

        public MarketsModel Model { get; }
        public SyncObservableCollection<MarketViewModel> Markets { get; } = new SyncObservableCollection<MarketViewModel>();

        /// <summary>
        /// Mark ongoing operation
        /// </summary>
        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                RaisePropertyChanged("IsBusy");
            }
        }

        public ITreeViewModel SelectedItem
        {
            get => _selectedItem;
            set
            {
                Set(ref _selectedItem, value);
            }
        }

        internal bool DeleteMarket(MarketViewModel market)
        {
            Debug.Assert(market != null);
            SelectedItem = null;
            return Markets.Remove(market);
        }

        internal bool Read(string fileName)
        {
            if (!File.Exists(fileName))
                return false;

            try
            {
                using (StreamReader r = new StreamReader(fileName))
                {
                    string json = r.ReadToEnd();
                    Model.Copy(JsonConvert.DeserializeObject<MarketsModel>(json));
                }

                DataFromModel();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().ToString());
                return false;
            }
        }

        internal bool Save(string fileName)
        {
            try
            {
                DataToModel();

                using (StreamWriter file = File.CreateText(fileName))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(file, Model);
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().ToString());
                return false;
            }
        }

        private void OnSelectedChanged(ITreeViewModel vm)
        {
            vm.Refresh();
            SelectedItem = vm;
        }

        private void OnNotificationMessage(NotificationMessageAction<List<MarketModel>> message)
        {
            if (string.IsNullOrEmpty(message.Notification))
            {
                message.Execute(Markets.Select(m => m.Model).ToList());
            }
            else
            {
                message.Execute(Markets.Where(s => s.Model.Name.Equals(message.Notification)).Select(m => m.Model).ToList());
            }
        }

        private void AddMarket()
        {
            var loginViewModel = new MarketViewModel(this, new MarketModel(), _settingsModel);
            Markets.Add(loginViewModel);
        }

        private void DataToModel()
        {
            Model.Markets.Clear();
            foreach (MarketViewModel market in Markets)
            {
                Model.Markets.Add(market.Model);
                market.DataToModel();
            }
        }

        private void DataFromModel()
        {
            Markets.Clear();
            foreach (MarketModel market in Model.Markets)
            {
                var viewModel = new MarketViewModel(this, market, _settingsModel);
                Markets.Add(viewModel);
            }
        }

        private void StartTasks()
        {
            foreach (MarketViewModel market in Markets)
            {
                if (market.Active)
                {
                    Task task = market.StartTaskAsync();
                }
            }
        }
    }
}
