﻿/*
 * Copyright 2019 Capnode AB
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
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;

namespace Algoloop.ViewModel
{
    public class FolderViewModel : ViewModelBase, ITreeViewModel
    {
        private MarketViewModel _market;
        private SymbolViewModel _selectedSymbol;
        private SymbolViewModel _marketSymbol;
        private ObservableCollection<DataGridColumn> _symbolColumns = new ObservableCollection<DataGridColumn>();

        public FolderViewModel(MarketViewModel market, FolderModel model)
        {
            _market = market;
            Model = model;

            DeleteCommand = new RelayCommand(() => _market?.DeleteFolder(this), () => !_market.Active);
            StartCommand = new RelayCommand(() => { }, () => false);
            StopCommand = new RelayCommand(() => { }, () => false);
            AddSymbolCommand = new RelayCommand<SymbolViewModel>(m => AddSymbol(m), m => !_market.Active && MarketSymbols.View.Cast<object>().FirstOrDefault() != null);
            RemoveSymbolsCommand = new RelayCommand<IList>(m => RemoveSymbols(m), m => !_market.Active && SelectedSymbol != null);
            MoveUpSymbolsCommand = new RelayCommand<IList>(m => MoveUpSymbols(m), m => !_market.Active && SelectedSymbol != null);
            MoveDownSymbolsCommand = new RelayCommand<IList>(m => MoveDownSymbols(m), m => !_market.Active && SelectedSymbol != null);

            DataFromModel();

            MarketSymbols.Filter += (object sender, FilterEventArgs e) =>
            {
                SymbolViewModel marketSymbol = e.Item as SymbolViewModel;
                if (marketSymbol != null)
                {
                    e.Accepted = marketSymbol.Active && !Symbols.Any(m => m.Model.Name.Equals(marketSymbol.Model.Name));
                }
            };

            MarketSymbols.Source = _market.Symbols;
        }

        public RelayCommand DeleteCommand { get; }
        public RelayCommand StartCommand { get; }
        public RelayCommand StopCommand { get; }
        public RelayCommand<SymbolViewModel> AddSymbolCommand { get; }
        public RelayCommand<IList> RemoveSymbolsCommand { get; }
        public RelayCommand<IList> MoveUpSymbolsCommand { get; }
        public RelayCommand<IList> MoveDownSymbolsCommand { get; }

        public FolderModel Model { get; }
        public SyncObservableCollection<SymbolViewModel> Symbols { get; } = new SyncObservableCollection<SymbolViewModel>();
        public CollectionViewSource MarketSymbols { get; } = new CollectionViewSource();


        public SymbolViewModel MarketSymbol
        {
            get => _marketSymbol;
            set => Set(ref _marketSymbol, value);
        }

        public SymbolViewModel SelectedSymbol
        {
            get => _selectedSymbol;
            set
            {
                Set(ref _selectedSymbol, value);
                RemoveSymbolsCommand.RaiseCanExecuteChanged();
                MoveUpSymbolsCommand.RaiseCanExecuteChanged();
                MoveDownSymbolsCommand.RaiseCanExecuteChanged();
            }
        }

        public ObservableCollection<DataGridColumn> SymbolColumns
        {
            get => _symbolColumns;
            set => Set(ref _symbolColumns, value);
        }


        public void Refresh()
        {
            Model.Refresh();
            DataFromModel();
            MarketSymbols.View.Refresh();
            AddSymbolCommand.RaiseCanExecuteChanged();
        }

        public void AddSymbol(SymbolViewModel symbol)
        {
            if (symbol == null)
                return;

            Symbols.Add(symbol);
            DataToModel();
            MarketSymbols.View.Refresh();
            AddSymbolCommand.RaiseCanExecuteChanged();
        }

        internal void DataToModel()
        {
            Model.Symbols.Clear();
            foreach (SymbolViewModel symbol in Symbols)
            {
                Model.Symbols.Add(symbol.Model.Name);
            }
        }

        internal void DataFromModel()
        {
            SymbolColumns.Clear();
            SymbolColumns.Add(new DataGridTextColumn()
            {
                Header = "Symbol",
                Binding = new Binding("Model.Name") { Mode = BindingMode.TwoWay, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged }
            });

            Symbols.Clear();
            foreach (SymbolViewModel marketSymbol in _market.Symbols)
            {
                if (marketSymbol.Active)
                {
                    string symbol = Model.Symbols.Find(m => m.Equals(marketSymbol.Model.Name));
                    if (symbol != null)
                    {
                        Symbols.Add(marketSymbol);
                    }

                    ExDataGridColumns.AddPropertyColumns(SymbolColumns, marketSymbol.Model.Properties);
                }
            }
        }

        private void RemoveSymbols(IList symbols)
        {
            if (Symbols.Count == 0 || symbols.Count == 0)
                return;

            // Create a copy of the list before remove
            List<SymbolViewModel> list = symbols.Cast<SymbolViewModel>()?.ToList();
            Debug.Assert(list != null);

            int pos = Symbols.IndexOf(list.First());
            foreach (SymbolViewModel symbol in list)
            {
                Symbols.Remove(symbol);
            }

            DataToModel();
            if (Symbols.Count > 0)
            {
                SelectedSymbol = Symbols[Math.Min(pos, Symbols.Count - 1)];
            }

            MarketSymbols.View.Refresh();
            AddSymbolCommand.RaiseCanExecuteChanged();
        }

        private void MoveUpSymbols(IList symbols)
        {
            if (Symbols.Count <= 1)
                return;

            // Create a copy of the list before move
            List<SymbolViewModel> list = symbols.Cast<SymbolViewModel>()?.ToList();
            Debug.Assert(list != null);

            for (int i = 1; i < Symbols.Count; i++)
            {
                if (list.Contains(Symbols[i]) && !list.Contains(Symbols[i - 1]))
                {
                    Symbols.Move(i, i - 1);
                }
            }
        }

        private void MoveDownSymbols(IList symbols)
        {
            if (Symbols.Count <= 1)
                return;

            // Create a copy of the list before move
            List<SymbolViewModel> list = symbols.Cast<SymbolViewModel>()?.ToList();
            Debug.Assert(list != null);

            for (int i = Symbols.Count - 2; i >= 0; i--)
            {
                if (list.Contains(Symbols[i]) && !list.Contains(Symbols[i + 1]))
                {
                    Symbols.Move(i, i + 1);
                }
            }
        }
    }
}